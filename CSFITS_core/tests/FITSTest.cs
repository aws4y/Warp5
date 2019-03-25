using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

using nom.tam.fits;

namespace CSharpFITS
{
	/// <summary>
	/// Summary description for FITSTest.
	/// </summary>
	public class FITSTest
	{
    public static bool cleanupTestFiles = true;
    public static bool trimComment = true;
    public static bool trimValue = true;

		public FITSTest(String filename)
		{
      KeyValComment.trimComment = trimComment;
      KeyValComment.trimValue = trimValue;

      _filename = filename;
      _test1Name = filename + DateTime.Now.Ticks + ".test1";
      _test2Name = filename + DateTime.Now.Ticks + ".test2";
		}

    /// <summary>Opens a FITS file and loads its data.</summary>
    /// <param name="filename">The name of the FITS file.</param>
    /// <returns>The Fits object containing the data in the FITS file.</returns>
    protected static Fits LoadFITS(String filename)
    {
      Console.Out.WriteLine("Loading " + filename);
      long t0 = DateTime.Now.Ticks;
      Fits fits = new Fits(filename);

      for(BasicHDU hdu = fits.readHDU(); hdu != null;)
      {
        Data data = hdu.Data;
        Object dataArray = data == null ? null : data.DataArray;
        hdu = fits.readHDU();
      }
      Console.Out.WriteLine(((DateTime.Now.Ticks - t0) / 1000000) + " ms elapsed.");

      return fits;
    }

    protected static void WriteFITS(Fits fits, String filename)
    {
      FileStream os = new FileStream(filename, FileMode.Create);
      Console.Out.WriteLine("Writing " + filename);

      long t0 = DateTime.Now.Ticks;
      fits.Write(os);
      Console.Out.WriteLine(((DateTime.Now.Ticks - t0) / 1000000) + " ms elapsed.");
    }

    public void Execute()
    {
      Fits originalFits = LoadFITS(_filename);
      WriteFITS(originalFits, _test1Name);
      originalFits.Stream.Close();

      Differ(_filename, _test1Name);

      if(cleanupTestFiles)
      {
        File.Delete(_test1Name);
      }
    }

    public static void Differ(String filename1, String filename2)
    {
      FileStream s1 = new FileStream(filename1, FileMode.Open, FileAccess.Read);
      FileStream s2 = new FileStream(filename2, FileMode.Open, FileAccess.Read);
      byte[] block1 = new byte[2880];
      byte[] block2 = new byte[2880];
      bool header1 = false;
      bool header2 = false;
      String line = null;

      for(long headerNum = 0, blockNum = 0,
            nRead1 = NextBlock(s1, ref block1), nRead2 = NextBlock(s2, ref block2);
          nRead1 > 0 || nRead2 > 0;
          ++blockNum)
      {
        line = "";
        header1 = header1 || IsHeader(block1);
        header2 = header2 || IsHeader(block2);

        if(header1 && header2)
        {
          KeyValComment[] h1 = ParseHeader(block1);
          KeyValComment[] h2 = ParseHeader(block2);
          KeyValCommentPair[] diffs = CompareHeaders(h1, h2);
          if(diffs.Length > 0)
          {
            line = new HeaderDifference((int)headerNum, CompareHeaders(h1, h2)) + "";
          }
          header1 = !ClosedHeader(h1);
          header2 = !ClosedHeader(h2);
          headerNum = header1 ? headerNum : headerNum + 1;
        }
        else if(header1)
        {
          line = "Block " + blockNum + " in left file is a header, in right file is not.";
          header1 = !ClosedHeader(ParseHeader(block1));
        }
        else if(header2)
        {
          line = "Block " + blockNum + " in right file is a header, in left file is not.";
          header2 = !ClosedHeader(ParseHeader(block2));
        }
        else
        {
          ByteDifference[] diffs = CompareBytes(block1, block2, blockNum * 2880);
          for(int i = 0; i < diffs.Length; ++i)
          {
            line += diffs[i] + (i < diffs.Length - 1 ? "\n" : "");
          }
        }

        if(!(line == ""))
        {
          Console.Out.WriteLine("BEGIN BLOCK " + blockNum);
          Console.Out.WriteLine(line);
          Console.Out.WriteLine("END BLOCK " + blockNum + "\n");
        }

        nRead1 = nRead1 == 2880 ? NextBlock(s1, ref block1) : 0;
        nRead2 = nRead2 == 2880 ? NextBlock(s2, ref block2) : 0;
      }

      s1.Close();
      s2.Close();
    }

    public static ByteDifference[] CompareBytes(byte[] block1, byte[] block2, long startPos)
    {
      ArrayList byteDiffs = new ArrayList();

      for(int i = 0; i < block1.Length && i < block2.Length; ++i)
      {
        if(block1[i] != block2[i])
        {
          byteDiffs.Add(new ByteDifference(startPos + i, block1[i], block2[i]));
        }
      }

      return (ByteDifference[])byteDiffs.ToArray(typeof(ByteDifference));
    }

    /// <summary>Compares two headers.</summary>
    /// <param name="h1">The first header.</param>
    /// <param name="h2">The second header.</param>
    /// <param name="ignoreCommentWS">Whether to trim comments before comparison</param>
    /// <returns>An int array containing indices which differ between the two headers.</returns>
    public static KeyValCommentPair[] CompareHeaders(KeyValComment[] h1, KeyValComment[] h2)
    {
      ArrayList result = new ArrayList();

      for(int row = 0; row < h1.Length || row < h2.Length; ++row)
      {
        KeyValComment row1 = row < h1.Length ? h1[row] : null;
        KeyValComment row2 = row < h2.Length ? h2[row] : null;

        if(!(row1 == null && row2 == null) && !(row1 != null && row1.Equals(row2)))
        {
          result.Add(new KeyValCommentPair(row1, row2));
        }
      }

      return (KeyValCommentPair[])result.ToArray(typeof(KeyValCommentPair));
    }

    public static bool IsHeader(byte[] block)
    {
      char[] strBuf = new char[8];

      if(block != null)
      {
        Array.Copy(block, 0, strBuf, 0, 8);
        String blockMagic = new String(strBuf);

        return "SIMPLE  ".Equals(blockMagic) || "XTENSION".Equals(blockMagic);
      }

      return false;
    }

    public static bool ClosedHeader(KeyValComment[] header)
    {
      bool result = false;

      for(int i = 0; i < header.Length; ++i)
      {
        if(header[i].Key.TrimEnd(spaceArray) == "END")
        {
          result = true;
        }
      }

      return result;
    }

    public static int NextBlock(Stream s, ref byte[] buf)
    {
      if(buf == null || buf.Length < 2880)
      {
        buf = new byte[2880];
      }

      return s.Read(buf, 0, 2880);
    }

    public static KeyValComment[] ParseHeader(byte[] block)
    {
      ArrayList header = new ArrayList();
      char[] lineBuf = new char[80];
      char[] keyBuf = new char[8];
      char[] valueIndicatorBuf = new char[2];
      char[] valueCommentBuf = new char[70];

      if(block != null && block.Length == 2880)
      {
        for(int i = 0; i < 36; ++i)
        {
          Array.Copy(block, i * 80, lineBuf, 0, 80);
          header.Add(ParseLine(lineBuf, ref keyBuf, ref valueIndicatorBuf, ref valueCommentBuf));
        }
      }
      else
      {
        throw new Exception("Invalid header block.");
      }

      return (KeyValComment[])header.ToArray(typeof(KeyValComment));
    }

    protected static KeyValComment ParseLine(char[] line, ref char[] keyBuf,
      ref char[] valueIndicatorBuf, ref char[] valueCommentBuf)
    {
      Array.Copy(line, 0, keyBuf, 0, 8);
      Array.Copy(line, 8, valueIndicatorBuf, 0, 2);
      Array.Copy(line, 10, valueCommentBuf, 0, 70);

      String lineStr = new String(line);
      String key = new String(keyBuf);
      String valueIndicator = new String(valueIndicatorBuf);
      String valueComment = new String(valueCommentBuf);
      String val = null;
      String comment = null;

      if("COMMENT ".Equals(key) ||
        "HISTORY ".Equals(key) ||
        "        ".Equals(key) ||
        !("= ".Equals(valueIndicator)))
      {
        val = valueIndicator + valueComment;
      }
      else
      {
        String elevenTo30 = lineStr.Substring(10, 20);

        // TODO: should add free logical, free integer, free float,
        // complex integer, complex float
        if(fixedLogical.IsMatch(elevenTo30) ||
           fixedInt.IsMatch(elevenTo30) ||
           fixedFloat.IsMatch(elevenTo30)) // fixed logical/integer/float
        {
          val = elevenTo30;
          String[] valComment = lineStr.Substring(30, 50).Split(commentDelimArray, 2);
          if(valComment.Length > 1)
          {
            comment = valComment[1];
          }
        }
        else // everything else
        {
          /// TODO: HACK
          /// This works because FITS defines characters as ASCII only.
          /// If FITS ever allows unicode, this will break.
          String textTest = valueComment.Replace("''", quoteStandin);
          if(fixedString.IsMatch(textTest))
          {
            textTest = textTest.Substring(1, textTest.Length - 1);
            String[] valComment = textTest.Split(quoteArray, 2);
            val = valComment[0];
            if(valComment.Length > 1 && valComment[1] != null)
            {
              valComment = valComment[1].Split(commentDelimArray, 2);
              if(valComment.Length > 1 && valComment[1] != null)
              {
                comment = valComment[1];
              }
            }
          }
          else if(freeString.IsMatch(textTest))
          {
            int startIndex = textTest.IndexOf("'");
            textTest = textTest.Substring(startIndex + 1, textTest.Length - startIndex - 1);
            String[] valComment = textTest.Split(quoteArray, 2);
            val = valComment[0];
            if(valComment.Length > 1 && valComment[1] != null)
            {
              valComment = valComment[1].Split(commentDelimArray, 2);
              if(valComment.Length > 1 && valComment[1] != null)
              {
                comment = valComment[1];
              }
            }
          }
        }
      }

      return new KeyValComment(key, val, comment);
    }


    protected static readonly char[] spaceArray = new char[]{' '};
    protected static readonly char[] commentDelimArray = new char[]{'/'};
    protected static readonly char[] quoteArray = new char[]{'\''};
    protected static readonly String quoteStandin = new String(new char[]{(char)257, (char)257});
    protected static Regex fixedLogical = new Regex("\\A                   [TF]\\z");
    protected static Regex fixedInt = new Regex("\\A *[+-]?[0-9]+\\z");
    protected static Regex fixedFloat =
      new Regex("\\A *[+-]?([0-9]+[\\.]?[0-9]*|[0-9]*[\\.][0-9]+)([DE][0-9]+\\z|\\z)");
    #region fixedFloatRegexTest
//    bool dude01 = fixedFloat1.IsMatch("     -123.45E10");
//    bool dude02 = fixedFloat1.IsMatch("     +123.45E10");
//    bool dude03 = fixedFloat1.IsMatch("      123.45E10");
//    bool dude04 = fixedFloat1.IsMatch("        -.45E10");
//    bool dude05 = fixedFloat1.IsMatch("        +.45E10");
//    bool dude06 = fixedFloat1.IsMatch("         .45E10");
//    bool dude07 = fixedFloat1.IsMatch("        -123E10");
//    bool dude08 = fixedFloat1.IsMatch("        +123E10");
//    bool dude09 = fixedFloat1.IsMatch("         123E10");
//    bool dude10 = fixedFloat1.IsMatch("       -123.E10");
//    bool dude11 = fixedFloat1.IsMatch("       +123.E10");
//    bool dude12 = fixedFloat1.IsMatch("        123.E10");
//    bool dude13 = fixedFloat1.IsMatch("        -123.45");
//    bool dude14 = fixedFloat1.IsMatch("        +123.45");
//    bool dude15 = fixedFloat1.IsMatch("         123.45");
//    bool dude16 = fixedFloat1.IsMatch("          -123.");
//    bool dude17 = fixedFloat1.IsMatch("          +123.");
//    bool dude18 = fixedFloat1.IsMatch("           123.");
//    bool dude19 = fixedFloat1.IsMatch("           -123");
//    bool dude20 = fixedFloat1.IsMatch("           +123");
//    bool dude21 = fixedFloat1.IsMatch("            123");
//    bool dude22 = fixedFloat1.IsMatch("           -.45");
//    bool dude23 = fixedFloat1.IsMatch("           +.45");
//    bool dude24 = fixedFloat1.IsMatch("            .45");
//    bool dude25 = fixedFloat1.IsMatch("            .45.45");
//    bool dude26 = fixedFloat1.IsMatch("            4545");
    #endregion
    protected static Regex fixedString = new Regex("\\A'[^']{9,68}'");
    protected static Regex freeString = new Regex("\\A[ ]*'[^']*'");

    protected String _filename;
    protected String _test1Name;
    protected String _test2Name;
	}

  public class ByteDifference
  {
    public ByteDifference(long position, byte left, byte right)
    {
      this.position = position;
      this.left = left;
      this.right = right;
    }

    public override String ToString()
    {
      return "POSITION: " + position + " (L/R) = " + left + " " + right;
    }

    public long position;
    public byte left;
    public byte right;
  }
  public class HeaderDifference
  {
    public HeaderDifference(int headerNumber, KeyValCommentPair[] differingPairs)
    {
      this.headerNumber = headerNumber;
      this.differingPairs = differingPairs;
    }

    public override String ToString()
    {
      String result = "HEADER " + headerNumber + ":\n";

      for(int i = 0; i < differingPairs.Length; ++i)
      {
        result += differingPairs[i] + "\n";
      }

      return result;
    }

    public int headerNumber;
    public KeyValCommentPair[] differingPairs;
  }

  public class KeyValCommentPair
  {
    public KeyValCommentPair(KeyValComment left, KeyValComment right)
    {
      this.left = left;
      this.right = right;
    }

    public override String ToString()
    {
      return "L: " + left + "\nR: " + right;
    }

    public KeyValComment left;
    public KeyValComment right;
  }
  public class KeyValComment
  {
    public String Key
    {
      get
      {
        return key;
      }
      
      set
      {
        key = value;
      }
    }

    public String Value
    {
      get
      {
        if(trimValue && val != null)
        {
          return val.TrimEnd(new char[]{' '});
          //return tailTrimmer.Replace(headTrimmer.Replace(val, ""), "");
        }
        else
        {
          return val;
        }
      }

      set
      {
        val = value;
      }
    }

    public String Comment
    {
      get
      {
        if(comment == null)
        {
          return "";
        }
        else if(trimComment)
        {
          return comment.Trim();
        }
        else
        {
          return comment;
        }
      }

      set
      {
        comment = value;
      }
    }

    public KeyValComment(String key, String val, String comment)
    {
      Key = key;
      Value = val;
      Comment = comment;
    }


    public override int GetHashCode()
    {
      int h1 = Key == null ? 0 : Key.GetHashCode();
      int h2 = Value == null ? 0 : Value.GetHashCode();
      int h3 = Comment == null ? 0 : Comment.GetHashCode();
      int result = h1 + h2 + h3;

      return result == 0 ? base.GetHashCode() : result;
    }

    public override bool Equals(object obj)
    {
      return obj is KeyValComment &&
        ((KeyValComment)obj).Key == Key &&
        ((KeyValComment)obj).Value == Value &&
        ((((KeyValComment)obj).Comment == null && Comment == null) ||
          ((KeyValComment)obj).Comment == Comment);
    }

    public override String ToString()
    {
      return "KEY = '" + Key + "' VALUE = '" + Value + "' COMMENT = '" + Comment + "'";
    }

    protected String key;
    protected String val;
    protected String comment;
    public static bool trimComment = true;
    public static bool trimValue = true;
    public static bool nullCommentIsEmptyString = true;
    protected static Regex headTrimmer = new Regex("^[' ]*");
    protected static Regex tailTrimmer = new Regex("[' ]*$");
  }
}

using System;
using System.Collections;
using System.IO;


namespace CSharpFITS
{
  using nom.tam.fits;
  using nom.tam.util;
  using nom.tam.util.test;

  /// <summary>Summary description for cSharpFITSgood.</summary>
  public class CSharpFITSTest
  {
    public CSharpFITSTest()
    {
    }

    public static void Main(String[] args)
    {
      FitsDump(new Uri(args[0]));
      //FitsDump(args[0], Int32.Parse(args[1]), Int32.Parse(args[2]), Int32.Parse(args[3]), Int32.Parse(args[4]));
      //DiffTest(args);
      //BinaryWriteTest(args);
      //StreamBinaryWriteTest(args);
      //BinaryTableTest.StreamByteBinaryTable(args[0]);
      //NullWriteTest(args[0]);
      //BinaryTableTest.StreamedBinaryTable(args[0]);
      //BinaryTableTest.TestDataReader(args[0], Int32.Parse(args[1]));
      //new BinaryTableTest().TestTenRows();
      //new BinaryTableTest().FiveHundredThousandTestAllWrites();
      //new BinaryTableTest().SpecObjTestAllWrites();
    }

    public static void NullWriteTest(String filename)
    {
      int mb = (int)Math.Pow(2, 20);
      int nbytes = 150 * mb;
      int bufsize = 4 * mb;
      byte[] buf = new byte[bufsize];
      BufferedStream s = new BufferedStream(new FileStream(filename, FileMode.Create));
      for(int i = 0; i < bufsize; ++i)
      {
        buf[i] = 0;
      }

      for(int bytesWritten = 0; bytesWritten < nbytes; bytesWritten += bufsize)
      {
        s.Write(buf, 0, bufsize);
      }
      s.Flush();
      s.Close();
    }

    public static void DiffTest(String[] args)
    {
      FITSTest fitsTest = new FITSTest(args[0]);
      FITSTest.cleanupTestFiles = false;
      fitsTest.Execute();
    }

    public static void FitsDump(Uri uri)
    {
      try
      {
        Fits fits = new Fits(uri);
        CSharpFITS.Dump(fits);
      }
      catch (Exception e)
      {
        Console.Error.WriteLine(e);
      }
    }

	  public static void FitsDump(String filename, int x, int y, int w, int h)
	  {
		  try
		  {
			  Fits fits = new Fits(filename);
			  FitsDump(fits, x, y, w, h);
		  }
		  catch(Exception e)
		  {
			  Console.Error.WriteLine(e);
		  }
	  }

    public static void FitsDump(Fits fits, int x, int y, int w, int h)
    {
      try
      {
        int i = 0;

        for(BasicHDU hdu = fits.readHDU(); hdu != null; ++i)
        {
          Console.Out.WriteLine("*** BEGIN HDU " + i + " ***");
          Dump((ImageHDU)hdu, x, y, w, h);
          Console.Out.WriteLine("*** END HDU " + i + " ***");
          hdu = fits.readHDU();
        }
      }
      catch(Exception e)
      {
        Console.Error.WriteLine(e);
      }
    }

    public static void DumpCards(BasicHDU hdu)
    {
      Cursor c = hdu.Header.GetCursor();
      for(c.MoveNext(); c.Current != null;)
      {
        DictionaryEntry entry = (DictionaryEntry)c.Current;
        HeaderCard hc = (HeaderCard)entry.Value;
        Console.Out.WriteLine("key = '" + hc.Key + "' value = '" + hc.Value +
          "' comment = '" + hc.Comment + "'");
        c.MoveNext();
      }
    }

    public static void Dump(BasicHDU hdu)
    {
      Console.Out.WriteLine("*** BEGIN HEADER CARDS ***");
      DumpCards(hdu);
      Console.Out.WriteLine("*** END HEADER CARDS ***");

      Console.Out.WriteLine("*** BEGIN DATA ***");
      if(hdu is ImageHDU)
      {
        Dump((ImageHDU)hdu);
      }
      else if(hdu is BinaryTableHDU)
      {
        Dump((BinaryTableHDU)hdu);
      }
      else if(hdu is AsciiTableHDU)
      {
        Dump((AsciiTableHDU)hdu);
      }
      Console.Out.WriteLine("*** END DATA ***");
    }

    public static void Dump(BinaryTableHDU hdu)
    {
      try
      {
        ColumnTable columns = (ColumnTable)hdu.Data.DataArray;
        int nRows = columns.NRows;
        int nCols = columns.NCols;

        try
        {
          for(int rowNum = 0; rowNum < nRows; ++rowNum)
          {
            String rowStr = "*** row " + rowNum + ": ";
            Array row = (Array)columns.GetRow(rowNum);

            for(int col = 0; col < nCols; ++col)
            {
              Object el = row.GetValue(col);
              rowStr += StringValue(el) + (col < nCols - 1 ? ", " : "");
            }

            Console.Out.WriteLine(rowStr);
          }
        }
        catch(Exception e)
        {
          Console.Error.WriteLine(e);
        }
      }
      catch(Exception e)
      {
        Console.Error.WriteLine(e);
      }
    }

    public static void Dump(AsciiTableHDU hdu)
    {
      try
      {
        int nRows = ((AsciiTable)hdu.Data).NRows;
        int nCols = ((AsciiTable)hdu.Data).NCols;
        Array data = (Array)hdu.Data.DataArray;

        try
        {
          for(int row = 0; row < nRows; ++row)
          {
            String rowStr = "*** row " + row + ": ";

            for(int col = 0; col < nCols; ++col)
            {
              Object el = ((AsciiTable)hdu.Data).GetElement(row, col);
              rowStr += StringValue(el) + (col < nCols - 1 ? ", " : "");
            }

            Console.Out.WriteLine(rowStr);
          }
        }
        catch(Exception e)
        {
          Console.Error.WriteLine(e);
        }
      }
      catch(Exception e)
      {
        Console.Error.WriteLine(e);
      }
    }

    public static void Dump(ImageHDU hdu)
    {
      if (hdu == null)
      {
        return;
      }

      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      Object data = hdu.Data.DataArray;

      if (!(data is Array))
      {
        return;
      }
      else if (data is Array[])
      {
        Array[] a = (Array[])data;

        for (int i = 0; i < a.Length; ++i)
        {
          Array b = (Array)a[i];
          for (int j = 0; j < b.Length; ++j)
          {
            double val = bZero + bScale * Convert.ToDouble(b.GetValue(j));
            Console.Out.Write(val + " ");
          }
          Console.Out.WriteLine("");
        }
      }
      else
      {
        Array b = (Array)data;
        for (int i = 0; i < b.Length; ++i)
        {
          double val = bZero + bScale * Convert.ToDouble(b.GetValue(i));
          Console.Out.Write(val + " ");
        }
      }
      //      Array[] a = (Array[])data;
    }

    public static void Dump(ImageHDU hdu, int x, int y, int w, int h)
    {
      if(hdu == null)
      {
        return;
      }

      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      Object data = hdu.Data.DataArray;

      if(!(data is Array))
      {
        return;
      }
      else if(data is Array[])
      {
        Array[] a = (Array[])data;

        for(int i = y; i < y + h; ++i)
//        for (int i = x; i < x + w; ++i)
        {
          Array b = (Array)a[i];
          for(int j = x; j < x + w; ++j)
//          for (int j = y; j < y + h; ++j)
          {
            double val = bZero + bScale * Convert.ToDouble(b.GetValue(j));
            Console.Out.Write(val + " ");
          }
          Console.Out.WriteLine("");
        }
      }
      else
      {
        Array b = (Array)data;
        for(int i = x; i < x + w; ++i)
        {
          double val = bZero + bScale * Convert.ToDouble(b.GetValue(i));
          Console.Out.Write(val + " ");
        }
      }
//      Array[] a = (Array[])data;

      #region oldcrap
      /*
      switch(bitpix)
      {
        case 8:
          for(int y = 0; y < a.Length; ++y)
          {
            byte[] b = (byte[])a[y];
            for(int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
          break;
        case 16:
          for(int y = 0; y < a.Length; ++y)
          {
            short[] b = (short[])a[y];
            for(int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
          break;
        case 32:
          for(int y = 0; y < a.Length; ++y)
          {
            int[] b = (int[])a[y];
            for(int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
          break;
        case -32:
          for(int y = 0; y < a.Length; ++y)
          {
            float[] b = (float[])a[y];
            for(int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
          break;
        case -64:
          for(int y = 0; y < a.Length; ++y)
          {
            double[] b = (double[])a[y];
            for(int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              min = Math.Min(min, val);
              max = Math.Max(max, val);
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
          break;
      }
      */
      #endregion
    }

    public static String StringValue(Object el)
    {
      String result = null;

      if(el is Array)
      {
        Array a = (Array)el;
        Type c = el.GetType().GetElementType();

        result = "{";
        for(int i = 0; i < a.Length;  ++i)
        {
          result += StringValue(a.GetValue(i)) + (i < a.Length - 1 ? ", " : "}");
        }
      }
      else
      {
        result = el + "";
      }

      return result;
    }
  }
}

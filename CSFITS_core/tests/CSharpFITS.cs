using System;
using System.Collections;
using System.Drawing;

namespace CSharpFITS
{
  using nom.tam.fits;
  using nom.tam.util;
  using nom.tam.util.test;
  using System.IO;

  /// <summary>Summary description for cSharpFITSgood.</summary>
  public class CSharpFITS
  {
    public CSharpFITS()
    {
    }

	  /*
    public static void Main(String[] args)
    {
      //ImageCopy(args[0], args[1]);
      //TableDump(args[0]);
      //FitsTest(args[0], args[1]);
      //FitsDump(args[0]);
      //ByteFormatParseTester.Test(args);
      //ArrayFuncsTester.Test(args);
      //BufferedFileTester.Test(args);
      //HashedListTester.Test(args);
      //Sandbox();
    }
    */

    public static void ImageCopy(String inputFilename, String outputFilename)
    {
      Fits fits = new Fits(inputFilename);
      //((BinaryTableHDU)fits.getHDU(1)).GetElement(0, 0);
      Console.Error.WriteLine("first long val " + ((BinaryTableHDU)fits.getHDU(1)).GetElement(0, 0).GetType().FullName);
      FileStream output = new FileStream(outputFilename, FileMode.Create);
      fits.Write(output);
    }

    public static void Sandbox()
    {
      //ArrayFuncTest();
      //ArrayFuncTest2();
    }

    public static void TableDump(String inputFilename)
    {
      Fits fits = new Fits(inputFilename);
      BinaryTableHDU hdu = (BinaryTableHDU)fits.getHDU(1);

      for(int i = 0; i < hdu.NRows; ++i)
      //for(int i = 0; i < 10; ++i)
      {
        for(int k = 0; k < hdu.NCols; ++k)
        //for(int k = 0; k < 1; ++k)
        {
          Object el = hdu.GetElement(i, k);
          //String str = stringValue(el);
          //Console.Out.Write(stringValue(el) + " ");
          Console.Out.Write(el + " ");
        }
        Console.Out.WriteLine("");
      }
    }

    public static String StringValue(Object el)
    {
      if(el == null)
      {
        return null;
      }

      if(el is Array)
      {
        Type c = el.GetType().GetElementType();

        if(c == typeof(byte))
        {
          return new String(ToCharArray((Array)el));
        }
        else if(c == typeof(char))
        {
          return new String((char[])el);
        }
        else if(c == typeof(short))
        {
          return ((short[])el)[0] + "";
        }
        else if(c == typeof(int))
        {
          return ((int[])el)[0] + "";
        }
        else if(c == typeof(long))
        {
          return ((long[])el)[0] + "";
        }
        else if(c == typeof(float))
        {
          return ((float[])el)[0] + "";
        }
        else if(c == typeof(double))
        {
          return ((double[])el)[0] + "";
        }
      }
      else
      {
        return "Some value of type " + el.GetType().FullName;
      }

      return null;
    }

    public static char[] ToCharArray(Array a)
    {
      char[] result = new char[a.Length];

      for(int i = 0; i < a.Length; ++i)
      {
        result[i] = (char)((byte)a.GetValue(i));
      }

      return result;
    }

    public static void FitsTest(String inputFilename, String outputFilename)
    {
      Fits fits = new Fits(inputFilename);
      ImageHDU hdu = GetImageHDU(fits);
      int bitpix = hdu.BitPix;
      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      double min = hdu.MinimumValue;
      double max = hdu.MaximumValue;
      double[,] a = GetImageData(hdu);

      for(int x = 0; x < a.GetLength(0); ++x)
      {
        for(int y = 0; y < a.GetLength(1); ++y)
        {
          min = Math.Min(min, a[x, y]);
          max = Math.Max(max, a[x, y]);
        }
      }

      Console.Out.WriteLine("Bitpix = " + bitpix + " bzero = " + bZero + " bscale = " + bScale +
                            " min = " + min + " max = " + max);
      Bitmap bmp = new Bitmap(a.GetLength(0), a.GetLength(1));

      double nBins = Math.Pow(2.0, 16.0) - 1.0;
      double linearScaleFactor = nBins / (max - min);
      double logScaleFactor = nBins / Math.Log10(nBins);
      double byteScaleFactor = 255.0 / nBins;
      double val = Double.NaN;

      for(int x = 0; x < a.GetLength(0); ++x)
      {
        for(int y = 0; y < a.GetLength(1); ++y)
        {
          val = a[x, y] - min;
          val = Math.Max(0.0, Math.Min(val, max - min));
          val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
          val *= byteScaleFactor;
          bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
        }
      }

      bmp.Save(outputFilename, System.Drawing.Imaging.ImageFormat.Jpeg);

      #region old crap
      /*
      switch(bitpix)
      {
        case 8:
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              val = offset + bScale * (double)((byte[])(a[y]))[x];
              //val = a[x, y] - min;
              val = Math.Max(0.0, Math.Min(val, max - min));
              val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
              val *= byteScaleFactor;
              bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
            }
          }
          break;
        case 16:
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              val = offset + bScale * (double)((short[])(a[y]))[x];
              //val = a[x, y] - min;
              val = Math.Max(0.0, Math.Min(val, max - min));
              val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
              val *= byteScaleFactor;
              bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
            }
          }
          break;
        case 32:
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              val = offset + bScale * (double)((int[])(a[y]))[x];
              //val = a[x, y] - min;
              val = Math.Max(0.0, Math.Min(val, max - min));
              val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
              val *= byteScaleFactor;
              bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
            }
          }
          break;
        case -32:
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              val = offset + bScale * (double)((float[])(a[y]))[x];
              //val = a[x, y] - min;
              val = Math.Max(0.0, Math.Min(val, max - min));
              val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
              val *= byteScaleFactor;
              bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
            }
          }
          break;
        case -64:
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              val = offset + bScale * (double)((double[])(a[y]))[x];
              //val = a[x, y] - min;
              val = Math.Max(0.0, Math.Min(val, max - min));
              val = val <= 0.0 ? 0.0 : (Math.Log10(val * linearScaleFactor) * logScaleFactor);
              val *= byteScaleFactor;
              bmp.SetPixel(x, y, Color.FromArgb((int)val, (int)val, (int)val));
            }
          }
          break;
        default:
          throw new Exception("Data type not supported.");
      }
      */
      #endregion
    }

    public static void FitsDump(String filename)
    {
      Dump(new Fits(filename));
    }

    public static void Dump(Fits fits, int x, int y, int w, int h)
    {
      ImageHDU hdu = GetImageHDU(fits);

      if (hdu == null)
      {
        throw new Exception("No image data available.");
      }

      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      Object data = hdu.Data.DataArray;

      Console.Error.WriteLine("bZero = " + bZero + " bScale = " + bScale);
      if(!(data is Array))
      {
        throw new Exception("Image dimensions not supported.");
      }

      Array[] a = (Array[])data;

      for(int i = y; i < y + h; ++i)
      {
        Object[] b = (Object[])a[i];
        for(int j = x; j < x + w; ++j)
        {
          double val = bZero + bScale * Convert.ToInt64(b[j]);
          Console.Out.Write(val + " ");
        }
        Console.Out.WriteLine("");
      }
#region oldcrap
/*
      switch (bitpix)
      {
        case 8:
          for (int y = 0; y < a.Length; ++y)
          {
            byte[] b = (byte[])a[y];
            for (int x = 0; x < b.Length; ++x)
            {
              double val = bZero + bScale * (double)b[x];
              Console.Out.Write(val + " ");
            }
            Console.Out.WriteLine("");
          }
            break;
              case 16:
                  for (int y = 0; y < a.Length; ++y)
                  {
                      short[] b = (short[])a[y];
                      for (int x = 0; x < b.Length; ++x)
                      {
                          double val = bZero + bScale * (double)b[x];
                          Console.Out.Write(val + " ");
                      }
                      Console.Out.WriteLine("");
                  }
                  break;
              case 32:
                  for (int y = 0; y < a.Length; ++y)
                  {
                      int[] b = (int[])a[y];
                      for (int x = 0; x < b.Length; ++x)
                      {
                          double val = bZero + bScale * (double)b[x];
                          Console.Out.Write(val + " ");
                      }
                      Console.Out.WriteLine("");
                  }
                  break;
              case -32:
                  for (int y = 0; y < a.Length; ++y)
                  {
                      float[] b = (float[])a[y];
                      for (int x = 0; x < b.Length; ++x)
                      {
                          double val = bZero + bScale * (double)b[x];
                          Console.Out.Write(val + " ");
                      }
                      Console.Out.WriteLine("");
                  }
                  break;
              case -64:
                  for (int y = 0; y < a.Length; ++y)
                  {
                      double[] b = (double[])a[y];
                      for (int x = 0; x < b.Length; ++x)
                      {
                          double val = bZero + bScale * (double)b[x];
                          min = Math.Min(min, val);
                          max = Math.Max(max, val);
                          Console.Out.Write(val + " ");
                      }
                      Console.Out.WriteLine("");
                  }
                  Console.Error.WriteLine("min = " + min + " max = " + max);
                  break;
          }
*/
#endregion
      }

    public static void Dump(Fits fits)
    {
      ImageHDU hdu = GetImageHDU(fits);

      if(hdu == null)
      {
        throw new Exception("No image data available.");
      }

      double min = Double.MaxValue;
      double max = Double.MinValue;
      int bitpix = hdu.BitPix;
      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      Object data = hdu.Data.DataArray;

      Console.Error.WriteLine("bZero = " + bZero + " bScale = " + bScale);

      if(!(data is Array))
      {
        throw new Exception("Image dimensions not supported.");
      }

      Array[] a = (Array[])data;

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
          Console.Error.WriteLine("min = " + min + " max = " + max);
          break;
      }
    }

    public static double[,] GetImageData(ImageHDU hdu)
    {
      double[,] result = new double[hdu.Axes[1], hdu.Axes[0]];
      double bZero = hdu.BZero;
      double bScale = hdu.BScale;
      double min = hdu.MinimumValue;
      double max = hdu.MaximumValue;
      Console.Out.WriteLine("Starting image read at " + System.DateTime.Now);
      Array[] a = (Array[])hdu.Data.DataArray;
      Console.Out.WriteLine("Done image read at " + System.DateTime.Now);

      switch(hdu.BitPix)
      {
        case 8:
        {
          byte[] b = null;
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            b = (byte[])a[y];
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              result[x, y] = bZero + bScale * (double)b[x];
            }
          }
        }
          break;
        case 16:
        {
          short[] b = null;
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            b = (short[])a[y];
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              result[x, y] = bZero + bScale * (double)b[x];
            }
          }
        }
          break;
        case 32:
        {
          int[] b = null;
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            b = (int[])a[y];
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              result[x, y] = bZero + bScale * (double)b[x];
            }
          }
        }
          break;
        case -32:
        {
          float[] b = null;
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            b = (float[])a[y];
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              result[x, y] = bZero + bScale * (double)b[x];
            }
          }
        }
          break;
        case -64:
        {
          double[] b = null;
          for(int y = 0; y < hdu.Axes[0]; ++y)
          {
            b = (double[])a[y];
            for(int x = 0; x < hdu.Axes[1]; ++x)
            {
              result[x, y] = bZero + bScale * (double)b[x];
            }
          }
        }
          break;
        default:
          throw new Exception("Data type not supported.");
      }

      return result;
    }

    public static ImageHDU GetImageHDU(Fits fits)
    {
      int i = 0;

      for(BasicHDU hdu = fits.getHDU(i); hdu != null; ++i)
      {
        if(hdu is ImageHDU)
        {
          return (ImageHDU)hdu;
        }
      }

      return null;
    }

    public static void ArrayFuncTest2()
    {
      Object[] dude = new Object[3];
      int num = 0;
      for(int i = 0; i < dude.Length; ++i)
      {
        dude[i] = new Object[3];
        for(int j = 0; j < ((Array)dude[i]).Length; ++j)
        {
          ((Object[])dude[i])[j] = new int[]{num++, num++, num++};
        }
      }
      int[] flatDude = (int[])ArrayFuncs.Flatten(dude);
      Console.Out.WriteLine("dude: " + PrintArray(dude));
      Console.Out.WriteLine("flatDude: " + PrintArray(flatDude));
      Console.Out.WriteLine("dimensions of dude: " + PrintArray(ArrayFuncs.GetDimensions(dude)));

      Array curledFlatDude = ArrayFuncs.Curl(flatDude, ArrayFuncs.GetDimensions(dude));
      Console.Out.WriteLine("dimensions of curledFlatDude: " + PrintArray(ArrayFuncs.GetDimensions(curledFlatDude)));
      Console.Out.WriteLine("curledFlatDude: " + PrintArray(curledFlatDude));
    }

    public static void ArrayFuncTest()
    {
      int num = 0;
      String[,,,] strDude = new String[2,2,2,2];
      int[,,,] intDude = new int[2,2,2,2];
      Object[] dude2 = new Object[10];

      for(int i = 0; i < dude2.Length; ++i)
      {
        dude2[i] = new int[]{0, 1, 2, 3, 4};
      }
      for(int i = 0; i < intDude.GetLength(0); ++i)
      {
        for(int j = 0; j < intDude.GetLength(1); ++j)
        {
          for(int k = 0; k < intDude.GetLength(2); ++k)
          {
            for(int l = 0; l < intDude.GetLength(3); ++l)
            {
              intDude[i,j,k,l] = num++;
              strDude[i,j,k,l] = intDude[i,j,k,l] + "";
            }
          }
        }
      }

      /*
      Object[] dude2Clone = (Object[])ArrayFuncs.DeepClone(dude2);
      Console.Out.WriteLine("before dude2[0][0] = " + ((int[])dude2[0])[0]);
      ((int[])dude2Clone[0])[0] = 1 + ((int[])dude2[0])[0];
      Console.Out.WriteLine("after dude2[0][0] = " + ((int[])dude2[0])[0]);
      for(int i = 0; i < dude2.Length; ++i)
      {
        for(int j = 0; j < ((int[])dude2[i]).Length; ++j)
        {
          if(((int[])dude2Clone[i])[j] != ((int[])dude2[i])[j])
          {
            Console.Out.WriteLine("dude2[" + i + "][" + j + "] = " + ((int[])dude2[i])[j] +
              " dude2Clone[" + i + "][" + j + "] = " + ((int[])dude2Clone[i])[j]);
          }
        }
      }
*/
      Console.Out.WriteLine("intDude IsArrayOfArrays: " + ArrayFuncs.IsArrayOfArrays(intDude) + "\n" +
                            "dimensions in intDude: " + ArrayFuncs.CountDimensions(intDude) + "\n" +
                            "nElements in intDude: " + ArrayFuncs.CountElements(intDude) + "\n" +
                            "size of intDude: " + ArrayFuncs.ComputeSize(intDude) + "\n" +
                            "dimensions of intDude: " + PrintArray(ArrayFuncs.GetDimensions(intDude)) + "\n" +
                            "type of intDude: " + ArrayFuncs.GetBaseClass(intDude) + "\n" +
                            "\n");
      Console.Out.WriteLine("strDude IsArrayOfArrays: " + ArrayFuncs.IsArrayOfArrays(strDude) + "\n" +
                            "dimensions in strDude: " + ArrayFuncs.CountDimensions(strDude) + "\n" +
                            "nElements in strDude: " + ArrayFuncs.CountElements(strDude) + "\n" +
                            "size of strDude: " + ArrayFuncs.ComputeSize(strDude) + "\n" +
                            "dimensions of strDude: " + PrintArray(ArrayFuncs.GetDimensions(strDude)) + "\n" +
                            "type of strDude: " + ArrayFuncs.GetBaseClass(strDude) + "\n" +
                            "\n");
      Console.Out.WriteLine("dude2 IsArrayOfArrays: " + ArrayFuncs.IsArrayOfArrays(dude2) + "\n" +
                            "dimensions in dude2: " + ArrayFuncs.CountDimensions(dude2) + "\n" +
                            "nElements in dude2: " + ArrayFuncs.CountElements(dude2) + "\n" +
                            "size of dude2: " + ArrayFuncs.ComputeSize(dude2) + "\n" +
                            "dimensions of dude2: " + PrintArray(ArrayFuncs.GetDimensions(dude2)) + "\n" +
                            "type of dude2: " + ArrayFuncs.GetBaseClass(dude2) + "\n" +
                            "\n");
      int[,,,] intDudeClone = (int[,,,])ArrayFuncs.DeepClone(intDude);
      String[,,,] strDudeClone = (String[,,,])ArrayFuncs.DeepClone(strDude);

      Console.Out.WriteLine("before intDude[0,0,0,0] = " + intDude[0,0,0,0]);
      intDudeClone[0, 0, 0, 0] = -432;
      Console.Out.WriteLine("after intDude[0,0,0,0] = " + intDude[0,0,0,0] + "\n\n");

      Console.Out.WriteLine("before strDude[0,0,0,0] = " + strDude[0,0,0,0]);
      strDudeClone[0, 0, 0, 0] = "NOT ORIGINAL STRING";
      Console.Out.WriteLine("after strDude[0,0,0,0] = " + strDude[0,0,0,0] + "\n\n");

      for(int i = 0; i < intDude.GetLength(0); ++i)
      {
        for(int j = 0; j < intDude.GetLength(1); ++j)
        {
          for(int k = 0; k < intDude.GetLength(2); ++k)
          {
            for(int l = 0; l < intDude.GetLength(3); ++l)
            {
              if(!intDude[i,j,k,l].Equals(intDudeClone[i,j,k,l]))
              {
                Console.Out.WriteLine("intDude[" + i + "," + j + "," + k + "," + l + "] = " + intDude[i,j,k,l] +
                                      " intDudeClone[" + i + "," + j + "," + k + "," + l + "] = " + intDudeClone[i,j,k,l]);
              }
              if(!strDude[i,j,k,l].Equals(strDudeClone[i,j,k,l]))
              {
                Console.Out.WriteLine("strDude[" + i + "," + j + "," + k + "," + l + "] = " + strDude[i,j,k,l] +
                  " strDudeClone[" + i + "," + j + "," + k + "," + l + "] = " + strDudeClone[i,j,k,l]);
              }
            }
          }
        }
      }
    }

    public static String PrintArray(Array a)
    {
      String result = "[";
      bool started = false;
      for(IEnumerator i = a.GetEnumerator(); i.MoveNext();)
      {
        result += (started ? ", " : "") + (i.Current is Array ? PrintArray((Array)i.Current) : i.Current);
        started = true;
      }
      return result + "]";
    }
  }
}

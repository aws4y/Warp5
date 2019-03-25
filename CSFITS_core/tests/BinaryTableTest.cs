using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;

using nom.tam.fits;
using nom.tam.util;

namespace nom.tam.util
{

	/// <summary></summary>
	[TestFixture]
	public class BinaryTableTest
	{
    protected String _filename = "binaryTableTest.fits";

    #region CONNECTION DATA
    protected String _catalog = "bestdr1";
    protected int _timeout = 160;
    protected String _dataSource = "sdssad6.pha.jhu.edu";
    protected String _userID = "test";
    protected String _password = "riuzg54";
    #endregion

    #region QUERY DATA
    protected int _nrows = 10;
    protected String _query = "";
    protected int _bufferSize = 4096;
    #endregion

    public BinaryTableTest()
		{
		}

    #region TEST METHODS
    [Test]
    public void ThousandTestAllWrites()
    {
      BigTestAllWrites(1000);
    }

    [Test]
    public void TenThousandTestAllWrites()
    {
      BigTestAllWrites(10000);
    }

    [Test]
    public void HundredThousandTestAllWrites()
    {
      BigTestAllWrites(100000);
    }

    [Test]
    public void FiveHundredThousandTestAllWrites()
    {
      BigTestAllWrites(500000);
    }

    public void BigTestAllWrites(int nRows)
    {
      bool[] haveStrings = new bool[]{true, false};
      bool[] seekable = new bool[]{true, false};
      StreamedBinaryTableHDU.StringWriteMode[] mode = new StreamedBinaryTableHDU.StringWriteMode[]
      {
        StreamedBinaryTableHDU.StringWriteMode.TRUNCATE,
        StreamedBinaryTableHDU.StringWriteMode.HEAP,
        StreamedBinaryTableHDU.StringWriteMode.PAD
      };

      _nrows = nRows;
      String strQuery = "select top " + _nrows + " b.*, a.* from photoobjall a, phototype b where a.type = b.value";
      //String strQuery = "select top " + _nrows + " objid, name from photoobj a, phototype b where a.type = b.value";
      String noStrQuery = "select top " + _nrows + " * from photoobjall order by objid";

      for(int i = 0; i < haveStrings.Length; ++i)
      {
        for(int j = 0; j < seekable.Length; ++j)
        {
          for(int k = 0; k < mode.Length; ++k)
          {
            _query = haveStrings[i] ? strQuery : noStrQuery;
            String filename = "big" + haveStrings[i] + "" + seekable[j] + "" + mode[k] + ".fits";
            String message = "Running big test with haveNRows = false " +
              " haveStrings = " + haveStrings[i] + " seekable = " + seekable[j] + " mode = " + mode[k];
            SQLTest(_query, filename, message, seekable[j], mode[k]);
          }
        }
      }
    }

    [Test]
    public void SpecObjTestAllWrites()
    {
      bool[] seekable = new bool[]{true, false};
      StreamedBinaryTableHDU.StringWriteMode[] mode = new StreamedBinaryTableHDU.StringWriteMode[]
      {
        StreamedBinaryTableHDU.StringWriteMode.TRUNCATE,
        StreamedBinaryTableHDU.StringWriteMode.HEAP,
        StreamedBinaryTableHDU.StringWriteMode.PAD
      };

      _query = "select * from  SpecObj where " +
        "(ra> 247.38478 and ra< 251.04872 ) and " +
        "( dec > 39.766070   and dec<    42.325380)";

      for(int j = 0; j < seekable.Length; ++j)
      {
        for(int k = 0; k < mode.Length; ++k)
        {
          String filename = "specObj" + seekable[j] + "" + mode[k] + ".fits";
          String message = "Running specObj test with haveNRows = false " +
            " seekable = " + seekable[j] + " mode = " + mode[k];
          SQLTest(_query, filename, message, seekable[j], mode[k]);
        }
      }
    }

    [Test]
    public void QuickTestAllWrites()
    {
      bool[] haveStrings = new bool[]{true, false};
      bool[] seekable = new bool[]{true, false};
      StreamedBinaryTableHDU.StringWriteMode[] mode = new StreamedBinaryTableHDU.StringWriteMode[]
      {
        StreamedBinaryTableHDU.StringWriteMode.TRUNCATE,
        StreamedBinaryTableHDU.StringWriteMode.HEAP,
        StreamedBinaryTableHDU.StringWriteMode.PAD
      };

      String strQuery = "select top " + _nrows + " * from coordtype";
      String noStrQuery = "select top " + _nrows + " * from photoobjall order by objid";
      _nrows = 10;

      for(int i = 0; i < haveStrings.Length; ++i)
      {
        for(int j = 0; j < seekable.Length; ++j)
        {
          for(int k = 0; k < mode.Length; ++k)
          {
            _query = haveStrings[i] ? strQuery : noStrQuery;
            String filename = "quick" + haveStrings[i] + "" + seekable[j] + "" + mode[k] + ".fits";
            String message = "Running quick test with haveNRows = false " +
              " haveStrings = " + haveStrings[i] + " seekable = " + seekable[j] + " mode = " + mode[k];
            SQLTest(_query, filename, message, seekable[j], mode[k]);
          }
        }
      }
    }

    #region old tests
    /*
    //[Test]
    public void TestBreak()
    {
      _nrows = 10;
      _query = "select top 10 ra from photoobjall";
//      _query = "select top " + _nrows +
//        " p.objid, p.ra, p.dec, p.flags, p.primtarget, p.sectarget, p.colc, " +
//        "p.petrorad_r, p.petroR90_r, p.petroR50_r, p.extinction_u, p.extinction_g, " +
//        "p.extinction_r, p.extinction_i, p.extinction_z, p.sky_u, p.sky_g, p.sky_r, " +
//        "p.sky_i, p.sky_z, p.petromag_u, p.petromag_g, p.petromag_r, p.petromag_i, " +
//        "p.petromag_z, p.petromagerr_u, p.petromagerr_g, p.petromagerr_r, p.petromagerr_i, " +
//        "p.petromagerr_z, p.modelmag_u, p.modelmag_g, p.modelmag_r, p.modelmag_i, " +
//        "p.modelmag_z, p.modelmagerr_u, p.modelmagerr_g, p.modelmagerr_r, p.modelmagerr_i, " +
//        "p.modelmagerr_z, s.mjd, s.plate, s.fiberid, s.z, s.zerr, s.zconf, s.zwarning, s.specclass, " +
//        "s.scienceprimary from  Galaxy as p left outer join SpecObjAll as s " +
//        "on p.objID = s.BestObjID where p.r > 17.8 and p.r < 17.85";
      DataReaderAdapterTest(_query, "break.fits", _bufferSize, "Running break");
    }

    //[Test]
    public void TestStringDateTime()
    {
      _nrows = 10;
      //_query = "select top 10 * from dataconstants";
      _query = "select top 1 GETDATE(), ra from photoobjall";
      DataReaderAdapterTest(_query, "stringDateTime.fits", _bufferSize, "Running TestStringDateTime");
    }

    //[Test]
    public void TestTenRows()
    {
      _nrows = 10;
      //_query = "select top " + _nrows + " * from photoobjall order by objid";
      _query = "select top " + _nrows + " * from coordtype";
      DataReaderAdapterTest(_query, "tenRows.fits", _bufferSize, "Running TestTenRows");
    }

    //[Test]
    public void TestThousandRows()
    {
      _nrows = 1000;
      //_query = "select top " + _nrows + " * from photoobjall order by objid";
      _query = "select top " + _nrows + " " +
        "a.objid, b.text, a.objid, b.text, a.objid, b.text, a.objid, b.text, " +
        "a.objid, b.text, a.objid, b.text, a.objid, b.text, a.objid, b.text, " +
        "a.objid, b.text from photoobjall a, dbobjects b";
      DataReaderAdapterTest(_query, "thousandRows.fits", _bufferSize, "Running TestThousandRows");
    }

    //[Test]
    public void TestHundredThousandRows()
    {
      _nrows = 100000;
      //_query = "select top " + _nrows + " * from photoobjall order by objid";
      _query = "select top " + _nrows + " " +
        "a.objid, b.text, a.objid, b.text, a.objid, b.text, a.objid, b.text, " +
        "a.objid, b.text, a.objid, b.text, a.objid, b.text, a.objid, b.text, " +
        "a.objid, b.text from photoobjall a, dbobjects b";
      DataReaderAdapterTest(_query, "hundredThousandRows.fits", _bufferSize, "Running TestHundredThousandRows");
    }

    //[Test]
    public void OneKBTest()
    {
      RowSourceTest("oneKB.fits", _bufferSize, 1.0 / 1024.0, "Running OneMBTest");
    }

    //[Test]
    public void OneMBTest()
    {
      RowSourceTest("oneMB.fits", _bufferSize, 1, "Running OneMBTest");
    }

    //[Test]
    public void TenMBTest()
    {
      RowSourceTest("tenMB.fits", _bufferSize, 10, "Running TenMBTest");
    }

    //[Test]
    public void HundredMBTest()
    {
      RowSourceTest("hundredMB.fits", _bufferSize, 100, "Running HundredMBTest");
    }
    */
    #endregion
    #endregion

    public void SQLTest(String cmdText, String filename, String message,
      bool seekable, StreamedBinaryTableHDU.StringWriteMode mode)
    {
      Console.Out.WriteLine(message);
      Console.Out.Flush();

      long ticks = DateTime.Now.Ticks;

      SqlConnection conn = new SqlConnection("Initial Catalog=" + _catalog + "; " +
        "Connect Timeout =" + _timeout + "; " +
        "Data Source=" + _dataSource + "; " +
        "User ID=" + _userID + "; " +
        "Password=" + _password);
      conn.Open();
      SqlCommand cmd = new SqlCommand(cmdText,conn);
      SqlDataReader reader = cmd.ExecuteReader();

//      WriteFITS(reader, filename, seekable, mode);
      Fits.Write(reader, filename, mode, 128, true, ' ');
      conn.Close();

      Console.Out.WriteLine("Wrote " + new FileInfo(filename).Length + " bytes in " +
        new TimeSpan(DateTime.Now.Ticks - ticks));
      Console.Out.Flush();
    }

    public void RowSourceTest(String filename, int bufferSize, double approxSizeInMB, String message)
    {
      Console.Out.WriteLine(message);
      Console.Out.Flush();
      long ticks = DateTime.Now.Ticks;

      int nRows = (int)(approxSizeInMB * 1024.0); // each row is 1k

            Header header = new Header
            {
                Simple = true,
                Bitpix = 8,
                Naxes = 0
            };
            Cursor c = header.GetCursor();
      for(c.MoveNext(); c.MoveNext(););
      c.Add("EXTEND", new HeaderCard("EXTEND", true, null));
      ImageHDU hdu1 = new ImageHDU(header, null);

      StreamedBinaryTableHDU hdu2 = new StreamedBinaryTableHDU(new DummyRowSource(nRows), bufferSize);
      Stream s = new FileStream(filename, FileMode.Create);
      Fits fits = new Fits();
      fits.addHDU(hdu1);
      fits.addHDU(hdu2);
      fits.Write(s);

      Console.Out.WriteLine("Wrote " + new FileInfo(filename).Length + " bytes in " +
        new TimeSpan(DateTime.Now.Ticks - ticks));
      Console.Out.Flush();
    }

    public void TamasTest()
    {
      int bufferSize = 4096;
      String message = "Running TamasTest";
      String filename = "tamas.fits";

      Console.Out.WriteLine(message);
      Console.Out.Flush();

      long ticks = DateTime.Now.Ticks;

      SqlConnection conn = new SqlConnection("Initial Catalog=" + _catalog + "; " +
        "Connect Timeout =" + _timeout + "; " +
        "Data Source=" + _dataSource + "; " +
        "User ID=" + _userID + "; " +
        "Password=" + _password);
      conn.Open();
      SqlCommand cmd = new SqlCommand("select top 10 ra from star", conn);
      SqlDataReader reader = cmd.ExecuteReader();

            Header header = new Header
            {
                Simple = true,
                Bitpix = 8,
                Naxes = 0
            };

            ImageHDU hdu1 = new ImageHDU(header, null);

      StreamedBinaryTableHDU hdu2 =
        new StreamedBinaryTableHDU(new DataReaderAdapter(reader), bufferSize);

      Cursor c = hdu2.Header.GetCursor();
      while(c.MoveNext());
      c.Add("PIXTYPE",new HeaderCard("PIXTYPE", "HEALPIX", "HEALPIX Pixelisation"));
      c.MoveNext();
      c.Add("ORDERING",new HeaderCard("ORDERING", "NESTED", "Pixel ordering scheme, either RING or NESTED"));
      c.MoveNext();
      c.Add("NSIDE",new HeaderCard("NSIDE", "Some crap", "Resolution parameter for HEALPIX"));
      c.MoveNext();
      c.Add("FIRSTPIX",new HeaderCard("FIRSTPIX", "0", "First pixel # (0 based)"));
      c.MoveNext();
      c.Add("LASTPIX",new HeaderCard("LASTPIX", "Some other crap", "Last pixel # (0 based)"));
      c.MoveNext();

      Stream s = new FileStream(filename, FileMode.Create);
      Fits fits = new Fits();
      fits.addHDU(hdu1);
      fits.addHDU(hdu2);
      fits.Write(s);

      conn.Close();

      Console.Out.WriteLine("Wrote " + new FileInfo(filename).Length + " bytes in " +
        new TimeSpan(DateTime.Now.Ticks - ticks));
      Console.Out.Flush();
    }

    public void DataReaderAdapterTest(String cmdText, String filename, int bufferSize, String message)
    {
      Console.Out.WriteLine(message);
      Console.Out.Flush();

      long ticks = DateTime.Now.Ticks;

      SqlConnection conn = new SqlConnection("Initial Catalog=" + _catalog + "; " +
                                             "Connect Timeout =" + _timeout + "; " +
                                             "Data Source=" + _dataSource + "; " +
                                             "User ID=" + _userID + "; " +
                                             "Password=" + _password);
      conn.Open();
      SqlCommand cmd = new SqlCommand(cmdText,conn);
      SqlDataReader reader = cmd.ExecuteReader();

      Fits.Write(reader, filename);
      /*
      Header header = new Header();
      header.Simple = true;
      header.Bitpix = 8;
      header.Naxes = 0;

      Cursor c = header.GetCursor();
      // move to the end of the header cards
      for(c.MoveNext(); c.MoveNext(););
      // we know EXTEND isn't there yet
      c.Add("EXTEND", new HeaderCard("EXTEND", true, null));

      ImageHDU hdu1 = new ImageHDU(header, null);

      StreamedBinaryTableHDU hdu2 =
        new StreamedBinaryTableHDU(new DataReaderAdapter(reader), bufferSize);

      Stream s = new FileStream(filename, FileMode.Create);
      Fits fits = new Fits();
      fits.addHDU(hdu1);
      fits.addHDU(hdu2);
      fits.Write(s);
*/
      conn.Close();

      Console.Out.WriteLine("Wrote " + new FileInfo(filename).Length + " bytes in " +
                            new TimeSpan(DateTime.Now.Ticks - ticks));
      Console.Out.Flush();
    }

    public void WriteFITS(IDataReader reader, String filename,
      bool seekable, StreamedBinaryTableHDU.StringWriteMode mode)
    {
      WriteFITS(new DataReaderAdapter(reader), filename, seekable, mode);
    }

    public void WriteFITS(RowSource rs, String filename,
      bool seekable, StreamedBinaryTableHDU.StringWriteMode mode)
    {
            Header header = new Header
            {
                Simple = true,
                Bitpix = 8,
                Naxes = 0
            };

            Cursor c = header.GetCursor();
      // move to the end of the header cards
      for(c.MoveNext(); c.MoveNext(););
      // we know EXTEND isn't there yet
      c.Add("EXTEND", new HeaderCard("EXTEND", true, null));

      ImageHDU hdu1 = new ImageHDU(header, null);

      StreamedBinaryTableHDU hdu2 = new StreamedBinaryTableHDU(rs, 4096, mode, 128, true, ' ');

      ConfigStream s = new ConfigStream(new FileStream(filename, FileMode.Create));
      s.SetCanSeek(seekable);

      Fits fits = new Fits();
      fits.addHDU(hdu1);
      fits.addHDU(hdu2);
      fits.Write(s);
    }

    public class DummyRowSource : RowSource
    {
      public DummyRowSource(int nrows) : this(nrows, 1024)
      {
      }

      public DummyRowSource(int nrows, int rowWidthInBytes)
      {
        _nrows = nrows;
        int i = 0;
        int curWidth = 0;

        for(i = 0, curWidth = 0; curWidth < rowWidthInBytes; ++i)
        {
          curWidth += _sizes[i % _sizes.Length];
        }

        _rowLength = i;
        _columnNames = new String[i];
        _modelRow = new Array[i];
        _tnull = new Object[i];
        for(i = 0; i < _rowLength; ++i)
        {
          _columnNames[i] = "Column" + i;
          _modelRow[i] = Array.CreateInstance(_types[i % _types.Length], 1);
          if(_types[i % _types.Length] == typeof(String))
          {
            _modelRow[i].SetValue(new String(' ', _sizes[i % _sizes.Length]), 0);
          }
          _tnull[i] = _nullValues[i % _nullValues.Length];
        }
      }

      #region RowSource Members
      public override int NRows
      {
        get
        {
          return _nrows;
        }
      }

      public override String[] ColumnNames
      {
        get
        {
          return _columnNames;
        }
      }

      public override object[] TNULL
      {
        get
        {
          return _tnull;
        }
      }

      public override Array[] ModelRow
      {
        get
        {
          return _modelRow;
        }
      }

      public override Array[] GetNextRow(ref Array[] row)
      {
        if(_done)
        {
          return null;
        }

        if(row == null || row.Length != _rowLength)
        {
          Type t = null;
          row = new Array[_rowLength];
          for(int i = 0; i < row.Length; ++i)
          {
            t = _types[i % _types.Length];
            t = t == typeof(bool) ? typeof(Troolean) : t;
            row[i] = Array.CreateInstance(t, 1);
            if(t == typeof(Troolean))
            {
              ((Array)row[i]).SetValue(new Troolean(), 0);
            }
          }
        }

        Object val = null;
        for(int i = 0; i < row.Length; ++i)
        {
          switch(i % _types.Length)
          {
            case 0:
              val = (byte)_curRow;
              break;
            case 1:
              val = ((Array)row[i]).GetValue(0);
              ((Troolean)val).Val = _curRow % 2 == 0;
              ((Troolean)val).IsNull = _curRow % 3 == 0;
              //val = _curRow % 2 == 0;
              //val = _curRow % 3 == 0 ? null : val;
              break;
            case 2:
              val = 'a';
              break;
            case 3:
              val = (short)_curRow;
              break;
            case 4:
            case 5:
            case 6:
            case 7:
              val = _curRow;
              break;
            case 8:
              //val = _curRow % 2 == 0 ? "dude" : "longer";
              val = _curRow % 2 == 0 ? "longer" : "dude";
              break;
            default:
              val = (byte)_curRow;
              break;
          }
          ((Array)row[i]).SetValue(val, 0);
        }

        _done = ++_curRow == _nrows;

        return row;
      }
      #endregion

      #region INSTANCE VARIABLES
      protected bool _done = false;
      int _rowLength = 0;
      protected String[] _columnNames;
      protected Array[] _modelRow;
      protected int _curRow = 0;
      protected int _nrows = 0;
      Object[] _nullValues = new Object[]
        {
          0, new Troolean(false, true), '\0', (short)-99, -99, float.NaN, (long)-99, double.NaN, "      "
        };
      Object[] _tnull;
      protected Type[] _types = new Type[]
        {
          typeof(byte),
          typeof(Troolean),
          typeof(char),
          typeof(short),
          typeof(int),
          typeof(float),
          typeof(long),
          typeof(double),
          typeof(String)
        };
      protected int[] _sizes = new int[]
        {
          1, 1, 1, 2, 4, 4, 8, 8, 6
        };
      #endregion
    }

    public static void ReverseAndStuff(ref byte[] bytes, ref byte[] buf, ref int pos)
    {
      Array.Reverse(bytes);
      Array.Copy(bytes, 0, buf, pos, bytes.Length);
      pos += bytes.Length;
    }
	}
}

using System;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.SqlClient;

using NUnit.Framework;


namespace CSharpFITS
{
  using nom.tam.fits;
  using nom.tam.util;
  
  /// <summary>
  /// Summary description for HeaderTest.
  /// </summary>
  [TestFixture]
  public class HeaderTest
  {
    public HeaderTest()
    {
    }

/*
    public static void Main(String[] args)
    {
    }
*/
    #region TEST METHODS
    [Test]
    public void TestAdd()
    {
      Header h = ImageHDU.ManufactureHeader(new ImageData((short[][])null));
      h.AddCard(new HeaderCard("DUDE", 10, "Test AddCard"));
      h.AddComment("Test AddComment");
      h.AddHistory("Test AddHistory");
      h.AddValue("T1", "strval", "Test String AddValue");
      h.AddValue("T2", true, "Test bool AddValue");
      h.AddValue("T3", 1, "Test int AddValue");
      h.AddValue("T4", 1.5f, "Test float AddValue");
      h.AddValue("T5", Int64.MaxValue, "Test long AddValue");
      h.AddValue("T6", 1.9, "Test double AddValue");
      h.Write(new BufferedFile(_filename, FileAccess.ReadWrite, 4096));
    }

    [Test]
    public void TestInsert()
    {
      Header h = ImageHDU.ManufactureHeader(new ImageData((short[][])null));
      h.AddCard(new HeaderCard("DUDE", 10, "Test AddCard"));
      h.InsertCard(new HeaderCard("DUDE2", 11, "Test InsertCard by key"), "DUDE");
      h.Write(new BufferedFile(_filename, FileAccess.ReadWrite, 4096));
    }

    [Test]
    public void TestRemove()
    {
      Header h = ImageHDU.ManufactureHeader(new ImageData((short[][])null));
      h.AddCard(new HeaderCard("DUDE", 10, "Test AddCard"));
      h.AddComment("Test AddComment");
      h.AddHistory("Test AddHistory");
      h.AddValue("T1", "strval", "Test String AddValue");
      h.AddValue("T2", true, "Test bool AddValue");
      h.AddValue("T3", 1, "Test int AddValue");
      h.AddValue("T4", 1.5f, "Test float AddValue");
      h.AddValue("T5", Int64.MaxValue, "Test long AddValue");
      h.AddValue("T6", 1.9, "Test double AddValue");
      h.Write(new BufferedFile(_filename + ".addedValues", FileAccess.ReadWrite, 4096));

      h.RemoveCard("DUDE");
      h.RemoveCard(h.FindCard("COMMENT"));
      h.RemoveCard("HISTORY");
      ArrayList rest = new ArrayList();
      int i = 0;
      int startIndex = 0;
      IEnumerator ie = h.GetEnumerator();
      for(ie.MoveNext(); ie.Current != null; ++i)
      {
        HeaderCard c = (HeaderCard)((DictionaryEntry)ie.Current).Value;
        if("T1".Equals(c.Key))
        {
          startIndex = i;
        }
        ie.MoveNext();
      }
      for(i = 0; i < 6; ++i)
      {
        // not (startIndex + i) because the indices change with each removal
        // just remove at startIndex 6 times
        h.RemoveCard(startIndex);
      }
      h.Write(new BufferedFile(_filename + ".noAddedValues", FileAccess.ReadWrite, 4096));
    }
    #endregion

    protected static String _filename = "test.header";
  }
}

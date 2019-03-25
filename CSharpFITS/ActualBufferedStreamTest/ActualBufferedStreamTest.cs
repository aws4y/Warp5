using System;
using System.IO;

using nom.tam.util;
using NUnit.Framework;

namespace ActualBufferedStreamTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
  [TestFixture]
	class ActualBufferedStreamTest
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
      new ActualBufferedStreamTest().BufferedVsBufferedVsUnbuffered100MB();
		}

    [Test]
    public void BufferedVsBufferedVsUnbuffered100MB()
    {
      BufferedVsBufferedVsUnbuffered(100);
    }

    public void BufferedVsBufferedVsUnbuffered(int megs)
    {
      WriteBuffered(megs, "buffered.dat");
      WriteMSBuffered(megs, "msbuffered.dat");
      WriteUnbuffered(megs, "unbuffered.dat");
    }

    protected void WriteBuffered(int megs, String filename)
    {
      Console.Error.WriteLine("Writing " + megs + "MB buffered.");
      long ticks = DateTime.Now.Ticks;
      Write(megs * (int)Math.Pow(2, 20), new ActualBufferedStream(new FileStream(filename, FileMode.Create)));
      Console.Error.WriteLine("Wrote " + megs + "MB in " + ((DateTime.Now.Ticks - ticks) / 10000000) + "s");
    }

    protected void WriteMSBuffered(int megs, String filename)
    {
      Console.Error.WriteLine("Writing " + megs + "MB buffered using BufferedStream.");
      long ticks = DateTime.Now.Ticks;
      Write(megs * (int)Math.Pow(2, 20), new BufferedStream(new FileStream(filename, FileMode.Create), 4096));
      Console.Error.WriteLine("Wrote " + megs + "MB in " + ((DateTime.Now.Ticks - ticks) / 10000000) + "s");
    }

    protected void WriteUnbuffered(int megs, String filename)
    {
      Console.Error.WriteLine("Writing " + megs + "MB unbuffered.");
      long ticks = DateTime.Now.Ticks;
      Write(megs * (int)Math.Pow(2, 20), new FileStream(filename, FileMode.Create));
      Console.Error.WriteLine("Wrote " + megs + "MB in " + ((DateTime.Now.Ticks - ticks) / 10000000) + "s");
    }

    protected void Write(int nBytes, Stream s)
    {
      for(int i = 0; i < (nBytes / 4); ++i)
      {
        byte[] buf = BitConverter.GetBytes(i);
        s.Write(buf, 0, buf.Length);
      }
      s.Flush();
      s.Close();
    }
	}
}

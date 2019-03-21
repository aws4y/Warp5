using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    public class WarpImage8:WarpImageGeneric<byte>
    {
        public WarpImage8() : base()
        { }
        public WarpImage8(uint uWidth, uint uHeight, DTYPE uType) : base(uWidth, uHeight, uType)
        { }
        public WarpImage8(uint uWidth, uint uHeight) : base(uWidth, uHeight)
        { }
        public WarpImage8(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC, byte[,] uData) :
            base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC, uData)
        { }
        public WarpImage8(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC) :
         base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC)

        { }
        public WarpImage8(WarpImage8 uImage) : base(uImage)
        { }
        public static WarpImage8 operator +(WarpImage8 a, WarpImage8 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            byte[,] nData;
            bool aMajor = true;
            if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = a.Width;
                nHeight = a.Height;
                lWidth = b.Width;
                lHeight = b.Height;
                aMajor = true;
            }
            else if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = b.Width;
                nHeight = b.Height;
                lWidth = a.Width;
                lHeight = a.Height;
                aMajor = false;
            }
            else
            {
                throw new ArithmeticException("Error: Dim Missmatch");
            }
            nData = new byte[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage8(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImage8 operator -(WarpImage8 a, WarpImage8 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            byte[,] nData;
            bool aMajor = true;
            if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = a.Width;
                nHeight = a.Height;
                lWidth = b.Width;
                lHeight = b.Height;
                aMajor = true;
            }
            else if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = b.Width;
                nHeight = b.Height;
                lWidth = a.Width;
                lHeight = a.Height;
                aMajor = false;
            }
            else
            {
                throw new ArithmeticException("Error: Dim Missmatch");
            }
            nData = new byte[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage8(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImage8 operator *(WarpImage8 a, WarpImage8 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            byte[,] nData;
            bool aMajor = true;
            if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = a.Width;
                nHeight = a.Height;
                lWidth = b.Width;
                lHeight = b.Height;
                aMajor = true;
            }
            else if (a.Width >= b.Width && a.Height >= b.Height)
            {
                nWidth = b.Width;
                nHeight = b.Height;
                lWidth = a.Width;
                lHeight = a.Height;
                aMajor = false;
            }
            else
            {
                throw new ArithmeticException("Error: Dim Missmatch");
            }
            nData = new byte[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (byte)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage8(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF64 operator /(WarpImage8 a, WarpImage8 b)
        {
            double[,] nData;
            if (a.Height != b.Height || a.Width != b.Width)
            {
                throw new ArithmeticException("Error: Dim Missmatch");
            }
            else
            {
                nData = new double[a.Height, a.Width];
                for (uint i = 0; i < a.Height; i++)
                {
                    for (uint j = 0; j < a.Width; j++)
                    {
                        nData[i, j] = (double)a.GetData(i, j) / (double)b.GetData(i, j);
                    }
                }
            }
            return new WarpImageF64(a.Width, a.Height, DTYPE.DOUBLE, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
    }
}

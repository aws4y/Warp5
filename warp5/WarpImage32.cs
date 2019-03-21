using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
     public class WarpImage32:WarpImageGeneric<uint>
    {
        public WarpImage32() : base()
        { }
        public WarpImage32(uint uWidth, uint uHeight, DTYPE uType) : base(uWidth, uHeight, uType)
        { }
        public WarpImage32(uint uWidth, uint uHeight) : base(uWidth, uHeight)
        { }
        public WarpImage32(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC, uint[,] uData) :
            base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC, uData)
        { }
        public WarpImage32(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC) :
         base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC)

        { }
        public WarpImage32(WarpImage32 uImage) : base(uImage)
        { }
        public static WarpImage32 operator +(WarpImage32 a, WarpImage32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            uint[,] nData;
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
            nData = new uint[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImage32 operator -(WarpImage32 a, WarpImage32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            uint[,] nData;
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
            nData = new uint[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImage32 operator *(WarpImage32 a, WarpImage32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            uint[,] nData;
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
            nData = new uint[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (uint)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF64 operator /(WarpImage32 a, WarpImage32 b)
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

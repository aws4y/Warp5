using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    class WarpImageF64:WarpImageGeneric<double>
    {
        public WarpImageF64() : base()
        { }
        public WarpImageF64(uint uWidth, uint uHeight, DTYPE uType) : base(uWidth, uHeight, uType)
        { }
        public WarpImageF64(uint uWidth, uint uHeight) : base(uWidth, uHeight)
        { }
        public WarpImageF64(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC, double[,] uData) :
            base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC, uData)
        { }
        public WarpImageF64(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC) :
         base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC)

        { }
        public WarpImageF64(WarpImageF64 uImage) : base(uImage)
        { }
        public static WarpImageF64 operator +(WarpImageF64 a, WarpImageF64 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            double[,] nData;
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
            nData = new double[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF64(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF64 operator -(WarpImageF64 a, WarpImageF64 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            double[,] nData;
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
            nData = new double[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF64(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF64 operator *(WarpImageF64 a, WarpImageF64 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            double[,] nData;
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
            nData = new double[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (double)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF64(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
    }
}

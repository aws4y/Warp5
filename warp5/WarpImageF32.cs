using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    class WarpImageF32: WarpImageGeneric<float>
    {
        public WarpImageF32() : base()
        { }
        public WarpImageF32(uint uWidth, uint uHeight, DTYPE uType) : base(uWidth, uHeight, uType)
        { }
        public WarpImageF32(uint uWidth, uint uHeight) : base(uWidth, uHeight)
        { }
        public WarpImageF32(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC, float[,] uData) :
            base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC, uData)
        { }
        public WarpImageF32(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC) :
         base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC)

        { }
        public WarpImageF32(WarpImageF32 uImage) : base(uImage)
        { }
        public static WarpImageF32 operator +(WarpImageF32 a, WarpImageF32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            float[,] nData;
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
            nData = new float[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF32 operator -(WarpImageF32 a, WarpImageF32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            float[,] nData;
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
            nData = new float[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImageF32 operator *(WarpImageF32 a, WarpImageF32 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            float[,] nData;
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
            nData = new float[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (float)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImageF32(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
    }
}

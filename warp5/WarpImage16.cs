using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    /// <summary>
    /// WarpImage16 is the class for 16 bit warp images and there arithmetic it extends WarpImageGenaric<>
    /// </summary>
    public class WarpImage16 : WarpImageGeneric<ushort>
    {
        public WarpImage16() : base()
        { }
        public WarpImage16(uint uWidth, uint uHeight, DTYPE uType) : base(uWidth, uHeight, uType)
        { }
        public WarpImage16(uint uWidth, uint uHeight) : base(uWidth, uHeight)
        { }
        public WarpImage16(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC, ushort[,] uData): 
            base(uWidth,uHeight,uType,uOname,uNotes,uRA,uDEC,uData)
        { }
        public WarpImage16(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC) :
         base(uWidth, uHeight, uType, uOname, uNotes, uRA, uDEC)
   
        { }
        public WarpImage16(WarpImage16 uImage) : base(uImage)
        { }
        public static WarpImage16 operator +(WarpImage16 a, WarpImage16 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            ushort[,] nData;
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
            nData = new ushort[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] =(ushort)( a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (ushort)(a.GetData(i, j) + b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage16(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes,a.Ra,a.Dec, nData);
        }
        public static WarpImage16 operator -(WarpImage16 a, WarpImage16 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            ushort[,] nData;
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
            nData = new ushort[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (ushort)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (ushort)(a.GetData(i, j) - b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage16(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
        public static WarpImage16 operator *(WarpImage16 a, WarpImage16 b)
        {
            uint nWidth;
            uint nHeight;
            uint lWidth;
            uint lHeight;
            ushort[,] nData;
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
            nData = new ushort[nWidth, nHeight];
            for (uint i = 0; i < nHeight; i++)
                for (uint j = 0; j < nWidth; j++)
                {
                    if (aMajor)
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (ushort)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = a.GetData(i, j);
                    }
                    else
                    {
                        if (i < lHeight && j < lWidth)
                            nData[i, j] = (ushort)(a.GetData(i, j) * b.GetData(i, j));
                        else
                            nData[i, j] = b.GetData(i, j);
                    }
                }
            return new WarpImage16(nWidth, nHeight, DTYPE.INT16, a.OName, a.Notes, a.Ra, a.Dec, nData);
        }
       
    }
}

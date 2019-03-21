/*****************************************************************
 * Author: Ron Smith
 *(C) 2019 
 * All software is distributed under the LGPL as is with no warranty.
 * ****************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    /// <summary>
    ///The WarpImageGenaric class uses C# genarics to implement an abstract image class 
    ///Arithmetic is not possible for WarpImageGenaric types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    abstract public class WarpImageGenaric<T>
    {
        private Coord ra;       // object RA (Right Ascention)
        private Coord dec;      //object Dec (declination)
        private DTYPE idType;   //FITS data type field of class
        private uint width;     //image width
        private uint height;    //image height
        private string oName;   //object name
        private string notes;   //fits notes
        private T[,] data;      //image data
        
        //create an empty warp image 
        public WarpImageGenaric()
        {
            ra = new Coord();
            dec = new Coord();
            width = 0;
            height = 0;
            oName = "";
            notes = "";
            if (typeof(T) == typeof(double))
                idType = DTYPE.DOUBLE;
            else if (typeof(T) == typeof(float))
                idType = DTYPE.FLOAT;
            else if (typeof(T) == typeof(byte))
                idType = DTYPE.BYTE;
            else if (typeof(T) == typeof(ushort))
                idType = DTYPE.INT16;
            else if (typeof(T) == typeof(uint))
                idType = DTYPE.INT32;
            else
                throw new InvalidOperationException("Data type not a valid numeric type.");

            data = null;
        }
        //create a WarpImage of type DTYPE 
        public WarpImageGenaric(uint uWidth, uint uHeight, DTYPE uType)
        {
           
            switch(uType)
            {
                case DTYPE.DOUBLE:
                    {
                        if(typeof(T)!=typeof(double))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected "+typeof(double)+" got "+typeof(T));
                        }
                        break;
                    }
                case DTYPE.FLOAT:
                    {
                        if(typeof(T)!=typeof(float))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(float) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.BYTE:
                    {
                        if (typeof(T) != typeof(byte))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(byte) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT16:
                    {
                        if(typeof(T)!=typeof(ushort))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(ushort) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT32:
                    {
                        if (typeof(T) != typeof(uint))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(uint) + " got " + typeof(T));
                        }
                        break;
                    }
            }
            ra = new Coord();
            dec = new Coord();
            width = uWidth;
            height = uHeight;
            oName = "";
            notes = "";
            idType = uType;
            data = new T[height, width];
        }
        //create a warp image without specifying dtype
        public WarpImageGenaric(uint uWidth, uint uHeight)
        {
            ra = new Coord();
            dec = new Coord();
            width = uWidth;
            height = uHeight;
            oName = "";
            notes = "";
            if (typeof(T) == typeof(double))
                idType = DTYPE.DOUBLE;
            else if (typeof(T) == typeof(float))
                idType = DTYPE.FLOAT;
            else if (typeof(T) == typeof(byte))
                idType = DTYPE.BYTE;
            else if (typeof(T) == typeof(ushort))
                idType = DTYPE.INT16;
            else if (typeof(T) == typeof(uint))
                idType = DTYPE.INT32;
            else
                throw new InvalidOperationException("Data type not a valid numeric type.");

            data = new T[height,width];      
        }
        //specify all fields of a complete warp image
        public WarpImageGenaric(uint uWidth, uint uHeight, DTYPE uType,string uOname, string uNotes, Coord uRA, Coord uDEC, T[,] uData)
        {
          
          
            switch (uType)
            {
                case DTYPE.DOUBLE:
                    {
                        if (typeof(T) != typeof(double))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(double) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.FLOAT:
                    {
                        if (typeof(T) != typeof(float))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(float) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.BYTE:
                    {
                        if (typeof(T) != typeof(byte))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(byte) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT16:
                    {
                        if (typeof(T) != typeof(ushort))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(ushort) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT32:
                    {
                        if (typeof(T) != typeof(uint))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(uint) + " got " + typeof(T));
                        }
                        break;
                    }
            }
            width = uWidth;
            height = uHeight;
            idType = uType;
            oName = uOname;
            notes = uNotes;
            ra = uRA;
            dec = uDEC;
            copyData(ref data, uData,width,height);
           }
        //create a new warp image with an empty data field
        public WarpImageGenaric(uint uWidth, uint uHeight, DTYPE uType, string uOname, string uNotes, Coord uRA, Coord uDEC)
        {


            switch (uType)
            {
                case DTYPE.DOUBLE:
                    {
                        if (typeof(T) != typeof(double))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(double) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.FLOAT:
                    {
                        if (typeof(T) != typeof(float))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(float) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.BYTE:
                    {
                        if (typeof(T) != typeof(byte))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(byte) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT16:
                    {
                        if (typeof(T) != typeof(ushort))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(ushort) + " got " + typeof(T));
                        }
                        break;
                    }
                case DTYPE.INT32:
                    {
                        if (typeof(T) != typeof(uint))
                        {
                            throw new InvalidOperationException("Invlid Image Data type, expected " + typeof(uint) + " got " + typeof(T));
                        }
                        break;
                    }
            }
            width = uWidth;
            height = uHeight;
            idType = uType;
            oName = uOname;
            notes = uNotes;
            ra = uRA;
            dec = uDEC;
            data = new T[height, width];
        }
        public Coord Ra
        {
            get
            {
                return ra;
            }
            set
            {
                ra = value;
            }
        }
        public Coord Dec
        {
            get
            {
                return dec;
            }
            set
            {
                dec = value;
            }
        }
        public uint Width
        {
            get
            {
                return width;
            }
        }
        public uint Height
        {
            get
            {
                return height;
            }
        }
        public string OName
        {
            get
            {
                return oName;
            }
            set
            {
                oName = value;
            }
        }
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
            }
        }
        //copy data2 into data1, data1 null otherwise this is a private helper function for creating a new image from a previous one
        private static void copyData(ref T[,] data1, T[,] data2, uint w, uint h)
        {
            uint i, j;
            if(w==0 || h==0)
            {
                data1 = null;
            }
            for (i = 0; i < h; i++)
                for (j = 0; j < w; j++)
                    data1[i, j] = data2[i, j];
                
        }
        public WarpImageGenaric( WarpImageGenaric<T> uImage)
        {
          idType = uImage.idType;
          ra = uImage.ra;
          dec = uImage.dec;
          oName = uImage.oName;
          notes = uImage.notes;
          width = uImage.width;
          height = uImage.height;
          data = new T[height, width];
          copyData(ref data, uImage.data,width,height);
        }
       
        public T getData(uint i, uint j)
        {
            return (T)data[i, j];
        }
       
    }
    
}

using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
    class WarpImage<T>
    {
        private Coord RA;       // object RA (Right Ascention)
        private Coord DEC;      //object Dec (declination)
        private DTYPE idType;
        private uint width;     //image width
        private uint height;    //image height
        private string oName;   //object name
        private string notes;   //fits notes
        private T[,] data;      //image data
        

        public WarpImage()
        {
            RA = new Coord();
            DEC = new Coord();
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
        public WarpImage(uint uWidth, uint uHeight, DTYPE uType)
        {
            RA = new Coord();
            DEC = new Coord();
            width = uWidth;
            height = uHeight;
            oName = "";
            notes = "";
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
            idType = uType;
            data = new T[width, height];
        }
        //create a warp image without specifying dtype
        public WarpImage(uint uWidth, uint uHeight)
        {
            RA = new Coord();
            DEC = new Coord();
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

            data = new T[width,height];      
        }
    }
}

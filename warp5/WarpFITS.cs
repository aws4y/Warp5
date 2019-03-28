/********************************************************
 * WarpFITS.cs 
 * Fuctions for reading and writing fits images into the Warp IAF
 * (c) 2019 Ron Smith 
 * *******************************************************************/
using System;
using warp5;
using nom.tam.fits;

namespace warp5
{
    public class WarpFITS
    {
        public static WarpImage16 Warp16FitsRead(string fname)
        {
            Fits imFit;
            imFit = new Fits(fname);

            return new WarpImage16();
        }
        public static void Warp16FitsWrite(WarpImage16 image)
        {

        }
    }
   
   
}

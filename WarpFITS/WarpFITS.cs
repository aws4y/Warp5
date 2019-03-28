using System;
using warp5;
using nom.tam.fits;

namespace WarpFITS
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

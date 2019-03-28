/*****************************************************************
 * Author: Ron Smith
 *(C) 2019 
 * All software is distributed under the LGPL as is with no warranty.
 * ****************************************************************/

using System;
using System.Collections.Generic;


namespace warp5
{
    public enum DTYPE
    { 
        DOUBLE=-64,
        FLOAT=-32,
        BYTE=8,
        INT16=16,
        INT32=32

    }
   
    public class Warp
    {
        private List<WarpImage8>  stack8;
        private List<WarpImage16> stack16;
        private List<WarpImage32> stack32;
        private List<WarpImageF32> stackF32;
        private List<WarpImageF64> stackF64;
        public Warp()
        {
            stack8 = new List<WarpImage8>();
            stack16 = new List<WarpImage16>();
            stack32 = new List<WarpImage32>();
            stackF32 = new List<WarpImageF32>();
            stackF64 = new List<WarpImageF64>();
        }
        public void LoadFITSImage(string fname)
        {
            stack16.Add(WarpFITS.Warp16FitsRead(fname));
        }

       
    }
}

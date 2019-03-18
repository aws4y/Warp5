using System;

namespace warp5
{
    enum DTYPE
    { 
        DOUBLE=-64,
        FLOAT=-32,
        BYTE=8,
        INT16=16,
        INT32=32

    }
    public class Warp<T>
    {
        WarpImage<T>[] stack;

    }
}

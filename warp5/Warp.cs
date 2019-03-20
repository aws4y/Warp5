﻿/*****************************************************************
 * Author: Ron Smith
 *(C) 2019 
 * All software is distributed under the LGPL as is with no warranty.
 * ****************************************************************/

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
    public class Warp
    {
        WarpImage<ushort>[] stack;

    }
}

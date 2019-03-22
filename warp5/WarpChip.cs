using System;
using System.Collections.Generic;
using System.Text;

namespace warp5
{
     public class WarpChip
    {
        private uint height;
        private uint width;
        private uint wellDepth;
        private uint pixWidth;
        private uint pixHeight;
        private uint readNoise;

        public WarpChip()
        {
            height = 0;
            width = 0;
            wellDepth = 0;
            pixWidth = 0;
            pixHeight = 0;
            readNoise = 0;
        }
        public WarpChip(uint uWidth, uint uHeight, uint uWellDepth, uint uPixWidth, uint uPixHeight, uint uReadNoise)
        {
            width = uWidth;
            height = uHeight;
            wellDepth = uWellDepth;
            pixWidth = uPixWidth;
            pixHeight = uPixHeight;
            readNoise = uReadNoise;
        }
        public uint Height
        {
            get
            {
                return height;
            }
        }
        public uint Width
        {
            get
            {
                return width;
            }
        }
        public uint WellDepth
        {
            get
            {
                return wellDepth;
            }
        }
        public uint PixWidth
        {
            get
            {
                return pixWidth;
            }
        }
        public uint PixHeight
        {
            get
            {
                return pixHeight;
            }
        }
        public uint ReadNoise
        {
            get
            {
                return readNoise;
            }
        }
    }
}

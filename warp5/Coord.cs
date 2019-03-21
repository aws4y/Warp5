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
    public class Coord
    {
        private int hr;
        private int deg;
        private int min;
        private double sec;
        bool raType;              //raType true means use hrs instead of degrees
        //create a cordinate set to 0:0:0.00
        public Coord()
        {
            hr = 0;
            deg = 0;
            min = 0;
            sec = 0.0;
            raType = false;
        }
        //create coordinate with user input deg/hr and min sec set to 0.0
        public Coord(int uDeg, int uMin, bool uRaType)
        {
            if(uRaType)
            {
                hr = uDeg;
            }
            else
            {
                deg = uDeg;
            }
            min = uMin;
            raType = uRaType;
            sec = 0.0;
        }
        public Coord(int uDeg, int uMin, double uSec ,bool uRaType)
        {
            if (uRaType)
            {
                hr = uDeg;
            }
            else
            {
                deg = uDeg;
            }
            min = uMin;
            raType = uRaType;
            sec = uSec;
        }
        public int Hr
        {
            get
            {
                return hr;
            }
            set
            {
                if (!raType)
                    throw new System.InvalidOperationException("Error: Coordinate not RA cordinate");
                else
                {
                    if (value < 0 || value > 24)
                    {
                        throw new System.InvalidOperationException("Error: Hour out of bounds");
                    }
                    else
                        hr = value;
                }
            }
        }
         public int Deg
         {
            get
            {
                return deg;
            }
            set
            {
                if (raType)
                    throw new System.InvalidOperationException("Error: Coordinate not DEC cordinate");
                else
                {
                    if (value < -90 || value > 90)
                    {
                        throw new System.InvalidOperationException("Error: DEC cordinate out of range");
                    }
                    else
                        deg = value;
                }
            }
         }
        public int Min
        {
            get
            {
                return min;
            }
            set
            {
                if (value < 0 || value > 60)
                    throw new System.InvalidOperationException("Error: Invalid minute value");
                else
                    min = value;
            }
        }
        public double Sec
        {
            get
            {
                return sec;
            }
            set
            {
                if (value < 0.0 || value > 60.0)
                    throw new System.InvalidOperationException("Error: Invalid second value");
                else
                    sec = value;
            }
        }

    }
}

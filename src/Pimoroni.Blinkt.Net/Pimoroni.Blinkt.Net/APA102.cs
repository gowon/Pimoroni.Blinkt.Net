using System;
using System.Collections.Generic;
using System.Text;

namespace Pimoroni.Blinkt.Net
{
    public class APA102 : Pixel
    {
        public APA102()
        {
            Red = 0;
            Green = 0;
            Blue = 0;
            SetBrightness(0.2);
        }
        public APA102(byte r, byte g, byte b, float brightness)
        {
            Red = r;
            Green = g;
            Blue = b;
            SetBrightness(brightness);
        }
    }
}

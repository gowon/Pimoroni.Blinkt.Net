using System;

namespace Pimoroni.Blinkt.Net
{
    public abstract class Pixel
    {
        private int _red;
        public int Red
        {
            get => _red;
            set => _red = value & 0xff;
        }

        private int _green;
        public int Green
        {
            get => _green;
            set => _green = value & 0xff;
        }

        private int _blue;
        public int Blue
        {
            get => _blue;
            set => _blue = value & 0xff;
        }

        public int Brightness { get; set; }

        public void SetBrightness(double brightness)
        {
            if (brightness < 0.0 || brightness > 1.0)
                throw new Exception("Brightness should be between 0.0 and 1.0");

            Brightness = (int)(31.0 * brightness) & 0b11111;
        }
    }
}

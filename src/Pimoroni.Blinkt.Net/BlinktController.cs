using System;
using System.Device.Gpio;
using System.Threading;

namespace Pimoroni.Blinkt.Net
{
    public class BlinktController
    {
        private const int DAT = 23;
        private const int CLK = 24;

        private APA102[] pixels;
        private GpioController gpio;
        private TimeSpan delay = TimeSpan.FromTicks(10);
        private bool clear;


        public BlinktController(bool clearOnDestruction = true)
        {
            pixels = new APA102[]
            {   new APA102(), new APA102(), new APA102(), new APA102(),
                new APA102(), new APA102(), new APA102(), new APA102()
            };
            gpio = new GpioController();

            clear = clearOnDestruction;

            SetupGpio();
        }

        ~BlinktController()
        {
            if (clear)
                Clear();
            gpio.ClosePin(CLK);
            gpio.ClosePin(DAT);
        }

        public void Show()
        {
            StartTransfer();

            foreach (var pixel in pixels)
            {
                WriteByte(0b11100000 | pixel.Brightness);
                WriteByte(pixel.Blue);
                WriteByte(pixel.Green);
                WriteByte(pixel.Red);
            }

            StopTransfer();
        }

        public void SetAll(int r, int g, int b, double brightness = 0.2f)
        {
            foreach (var pixel in pixels)
            {
                pixel.Red = r;
                pixel.Green = g;
                pixel.Blue = b;
                pixel.SetBrightness(brightness);
            }
        }

        public void SetPixel(int x, int r, int g, int b, double brigthness = 0.2)
        {
            pixels[x].Red = r;
            pixels[x].Green = g;
            pixels[x].Blue = b;
            pixels[x].SetBrightness(brigthness);
        }

        public Pixel GetPixel(int x)
        {
            return pixels[x];
        }

        public void Clear()
        {
            foreach (var pixel in pixels)
            {
                pixel.Red = 0;
                pixel.Green = 0;
                pixel.Blue = 0;
                pixel.SetBrightness(0.2);
            }
            Show();
        }

        private void SetupGpio()
        {
            gpio.OpenPin(DAT);
            gpio.OpenPin(CLK);
            gpio.SetPinMode(DAT, PinMode.Output);
            gpio.SetPinMode(CLK, PinMode.Output);
        }

        private void StartTransfer()
        {
            gpio.Write(DAT, PinValue.Low);
            for (int i = 0; i < 36; i++)
            {
                gpio.Write(CLK, 1);
                Thread.Sleep(delay);
                gpio.Write(CLK, 0);
                Thread.Sleep(delay);
            }
        }

        private void StopTransfer()
        {
            gpio.Write(DAT, PinValue.Low);
            for (int i = 0; i < 32; i++)
            {
                gpio.Write(CLK, 1);
                Thread.Sleep(delay);
                gpio.Write(CLK, 0);
                Thread.Sleep(delay);
            }
        }

        private void WriteByte(int d)
        {
            byte data = (byte)d;
            for (int i = 0; i < 8; i++)
            {
                var output = data & 0b10000000;
                gpio.Write(DAT, output);
                gpio.Write(CLK, 1);
                Thread.Sleep(delay);
                data <<= 1;
                gpio.Write(CLK, 0);
                Thread.Sleep(delay);
            }
        }
    }
}

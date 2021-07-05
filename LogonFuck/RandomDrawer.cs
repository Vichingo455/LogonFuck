using System;
using System.Drawing;

namespace LogonFuck
{
    class RandomDrawer : Drawer
    {
        public CopyPixelOperation dwRop;

        public RandomDrawer(int delay, CopyPixelOperation dwRop) : base(delay)
        {
            this.dwRop = dwRop;
        }

        protected override void Draw(IntPtr hdc)
        {
            int xDest = random.Next(0, screenWidth);
            int yDest = random.Next(0, screenHeight);
            int xSrc = random.Next(0, screenWidth);
            int ySrc = random.Next(0, screenHeight);
            int width = random.Next(0, screenWidth);
            int height = random.Next(0, screenHeight);
            BitBlt(hdc, xDest, yDest, width, height, hdc, xSrc, ySrc, dwRop);
        }
    }
}

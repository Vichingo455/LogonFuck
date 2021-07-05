using System;
using System.Drawing;
using System.Windows.Forms;

namespace LogonFuck
{
    class ShakeDrawer : Drawer
    {
        public bool clockwise;
        public int strenght;

        public ShakeDrawer(int delay, int strenght, bool clockwise) : base(delay)
        {
            this.strenght = strenght;
            this.clockwise = clockwise;
        }

        private double rads = 0;
        private int c = 0;
        protected override void Draw(IntPtr hdc)
        {
            int xAdd = (int)(Math.Cos(rads) * strenght);
            int yAdd = (int)(Math.Sin(rads) * strenght);
            BitBlt(hdc, xAdd, yAdd, screenWidth, screenHeight, hdc, 0, 0, CopyPixelOperation.SourceCopy);
            if (clockwise)
            {
                rads += Math.PI / 8;
            }
            else {
                rads -= Math.PI / 8;
            }
            if (rads > 2 * Math.PI) rads -= 2 * Math.PI;
            else if (rads < 0) rads += 2 * Math.PI;
            int r = random.Next(0, 256);
            int g = random.Next(0, 256);
            int b = random.Next(0, 256);
            if (c == 0) {
                IntPtr brush = CreateSolidBrush((uint)(r + g*256 + b*256*256));
                SelectObject(hdc, brush);
                PatBlt(hdc, 0, 0, screenWidth, screenHeight, CopyPixelOperation.PatInvert);
                DeleteObject(brush);
            }
            c++;
            if (c >= 10)
            {
                c -= 10;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LogonFuck
{
    class InverseDrawer : Drawer
    {
        public int strenght;
        public CopyPixelOperation dwRop;
        private Point[] lpPoints;

        public InverseDrawer(int delay, int strenght, CopyPixelOperation dwRop) : base(delay)
        {
            this.strenght = strenght;
            this.dwRop = dwRop;
            lpPoints = new Point[3];
            lpPoints[0] = new Point(strenght*2, strenght);
            lpPoints[1] = new Point(screenWidth-strenght, strenght*2);
            lpPoints[2] = new Point(strenght, screenHeight-strenght*2);
        }

        int i = 1;
        protected override void Draw(IntPtr hdc)
        {
            PlgBlt(hdc, lpPoints, hdc, 0, 0, screenWidth, screenHeight, IntPtr.Zero, 0, 0);
            BitBlt(hdc, i, 0, screenWidth, screenHeight, hdc, 0, 0, dwRop);
            i *= -1;
        }
    }
}

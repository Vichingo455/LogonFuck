using System;
using System.Drawing;

namespace LogonFuck
{
    class MeltDrawer : Drawer
    {
        public int strenght;

        public MeltDrawer(int strenght) : base(0)
        {
            this.strenght = strenght;
        }

        protected override void Draw(IntPtr hdc)
        {
            int xDest = random.Next(-strenght, strenght+1);
            int row = random.Next(0, screenHeight);
            BitBlt(hdc, xDest, row, screenWidth, 300, hdc, 0, row, CopyPixelOperation.SourceCopy);
        }
    }
}

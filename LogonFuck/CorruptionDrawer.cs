using System;
using System.Drawing;

namespace LogonFuck
{
    class CorruptionDrawer : Drawer
    {
        public CorruptionDrawer(int delay) : base(delay)
        {
            this.delay = delay;
        }

        protected int blockWidth = 5;
        protected int blockHeight = 5;
        protected int move = 2;
        protected int row = 0;
        protected int column = 0;
        protected override void Draw(IntPtr hdc)
        {
            int xDest = random.Next(-move, move + 1);
            int yDest = random.Next(-move, move + 1);
            BitBlt(hdc, xDest, row, screenWidth, blockHeight, hdc, 0, row, CopyPixelOperation.SourceCopy);
            BitBlt(hdc, column, yDest, blockWidth, screenHeight, hdc, column, 0, CopyPixelOperation.SourceCopy);
            row += blockHeight;
            column += blockWidth;
            if (row >= screenHeight)
            {
                row = 0;
            }
            if (column >= screenWidth)
            {
                column = 0;
            }
        }
    }
}

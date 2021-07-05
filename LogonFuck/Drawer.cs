using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace LogonFuck
{
    abstract class Drawer
    {
        protected static int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        protected static int screenHeight = Screen.PrimaryScreen.Bounds.Height;
        protected static Random random = new Random();
        protected Thread thread;
        public int delay;

        [DllImport("User32.dll")]
        protected static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("User32.dll")]
        protected static extern void ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("gdi32.dll")]
        protected static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll")]
        protected static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        protected static extern bool PatBlt(IntPtr hdc, int nXLeft, int nYLeft, int nWidth, int nHeight, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll")]
        protected static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

        [DllImport("gdi32.dll")]
        protected static extern bool DeleteObject([In] IntPtr hObject);

        [DllImport("gdi32.dll")]
        protected static extern bool PlgBlt(IntPtr hdcDest, Point[] lpPoint, IntPtr hdcSrc, int nXSrc, int nYSrc, int nWidth, int nHeight, IntPtr hbmMask, int xMask, int yMask);

        public Drawer(int delay)
        {
            this.delay = delay;
        }

        public void Start()
        {
            if (thread == null)
            {
                thread = new Thread(new ThreadStart(StartDraw));
                thread.Start();
            }
        }

        private void StartDraw()
        {
            while (true)
            {
                try
                {
                    IntPtr desktop = GetDC(IntPtr.Zero);
                    try { Draw(desktop); } catch { }
                    ReleaseDC(IntPtr.Zero, desktop);
                }
                catch { }
                Thread.Sleep(delay);
            }
        }

        public void Stop()
        {
            thread.Abort();
            thread = null;
        }

        protected abstract void Draw(IntPtr hdc);
    }
}

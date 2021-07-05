using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogonFuck
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void noBt_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void yesBt_Click(object sender, EventArgs e)
        {
            Close();
            Program.StartDestruction();
        }
    }
}

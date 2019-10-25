using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Hav3Fun
{
    internal class WinAPI
    {
        // Edit
        [DllImport("kernel32")]
        internal static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, ref int lpNumberOfBytesRead);

        [DllImport("kernel32")]
        internal static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, out int lpNumberOfBytesWritten);

        [DllImport("kernel32")]
        internal static extern bool CloseHandle(int hObject);

        [DllImport("kernel32")]
        internal static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        // Mouse
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        internal static extern void mouse_event(uint dwFlags, int dx, int dy, uint cButtons, uint dwExtraInfo);
        internal const int MOUSEEVENTF_LEFTDOWN = 0x02;
        internal const int MOUSEEVENTF_LEFTUP = 0x04;

        // Keyboard
        [DllImport("user32.dll")]
        internal static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        // Work with ini files (cfg)
        [DllImport("kernel32")]
        internal static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        internal static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        // KeyHooks
        [DllImport("kernel32.dll")]
        internal static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        internal static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool UnhookWindowsHookEx(IntPtr hInstance);
        internal delegate IntPtr KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);

        //AnimateWindow
        //[DllImport("user32")]
        //public static extern bool AnimateWindow(IntPtr hwnd, int time, FormClass.AnimateWindowFlags flags);

        // ActiveWindow checker
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        //WindowText
        //[DllImport("user32.dll")]
        //internal static extern int SetWindowText(IntPtr hWnd, string text);
    }
}
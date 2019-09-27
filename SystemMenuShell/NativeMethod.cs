﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SystemMenuShell {

    static class NativeMethod {

        // http://chokuto.ifdef.jp/urawaza/struct/MENUITEMINFO.html

        [StructLayout(LayoutKind.Sequential)]
        public struct MENUITEMINFO {
            public uint cbSize;
            public uint fMask;
            public uint fType;
            public uint fState;
            public uint wID;
            public IntPtr hSubMenu;
            public IntPtr hbmpChecked;
            public IntPtr hbmpUnchecked;
            public IntPtr dwItemData;
            public string dwTypeData;
            public uint cch;
            public IntPtr hbmpItem;

            public static uint sizeOf {
                get { return (uint)Marshal.SizeOf(typeof(MENUITEMINFO)); }
            }
        }

        // Menu

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll")]
        public static extern bool InsertMenuItem(IntPtr hMenu, uint uItem, bool fByPosition, [In] ref MENUITEMINFO lpmii);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateMenu();

        [DllImport("user32.dll")]
        public static extern uint GetMenuState(IntPtr hmenu, uint uItem, uint uFlags);

        [DllImport("user32.dll")]
        public static extern uint CheckMenuItem(IntPtr hmenu, uint uItem, uint uCheck);

        [DllImport("user32.dll")]
        public static extern bool CheckMenuRadioItem(IntPtr hMenu, uint idFirst, uint idLast, uint idCheck, uint uFlags);

        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uItem, uint uEnable);

        // Hook & Message

        // [DllImport("user32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        // public static extern int RegisterShellHookWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SendNotifyMessage(int hWnd, uint Msg, int wParam, int lParam);

    }
}

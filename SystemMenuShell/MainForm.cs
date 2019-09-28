﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace SystemMenuShell {

    public partial class MainForm : Form {

        // wID
        private const uint MENU_ID_01 = 0x0001;
        private const uint MENU_ID_02 = 0x0002;
        private const uint MENU_ID_03 = 0x0003;
        private const uint MENU_ID_04 = 0x0004;
        private const uint MENU_ID_05 = 0x0005;
        private const uint MENU_ID_06 = 0x0006;
        
        // MSG
        private uint MY_MESSAGE = 0;
        private const int HWND_BROADCAST = 0xffff;

        private IntPtr hSysMenu;

        public MainForm() {
            InitializeComponent();

            hSysMenu = NativeMethod.GetSystemMenu(this.Handle, false);

            // Split
            var splititem = new NativeMethod.MENUITEMINFO();
            splititem.cbSize = (uint) Marshal.SizeOf(splititem);
            splititem.fMask = NativeConstant.MIIM_FTYPE;
            splititem.fType = NativeConstant.MFT_SEPARATOR;

            // Check
            var testitem1 = new NativeMethod.MENUITEMINFO();
            testitem1.cbSize = (uint) Marshal.SizeOf(testitem1);
            testitem1.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            testitem1.dwTypeData = "MFS_CHECKED";
            testitem1.wID = MENU_ID_01;
            testitem1.fState = NativeConstant.MFS_CHECKED;

            // String
            var testitem2 = new NativeMethod.MENUITEMINFO();
            testitem2.cbSize = (uint) Marshal.SizeOf(testitem2);
            testitem2.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID;
            testitem2.dwTypeData = "MIIM_STRING";
            testitem2.wID = MENU_ID_02;

            // Disable
            var testitem3 = new NativeMethod.MENUITEMINFO();
            testitem3.cbSize = (uint) Marshal.SizeOf(testitem3);
            testitem3.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_STATE;
            testitem3.dwTypeData = "MFS_GLAYED";
            testitem3.wID = MENU_ID_03;
            testitem3.fState = NativeConstant.MFS_GLAYED;

            // Radio
            uint radioFMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_ID | NativeConstant.MIIM_FTYPE | NativeConstant.MIIM_CHECKMARKS | NativeConstant.MIIM_STATE;

            var testitem4 = new NativeMethod.MENUITEMINFO();
            testitem4.cbSize = (uint) Marshal.SizeOf(testitem4);
            testitem4.fMask = radioFMask;
            testitem4.dwTypeData = "MFT_RADIOCHECK_1";
            testitem4.wID = MENU_ID_04;
            testitem4.fType = NativeConstant.MFT_RADIOCHECK;
            testitem4.hbmpChecked = IntPtr.Zero;
            testitem4.fState = NativeConstant.MFS_CHECKED;

            var testitem5 = new NativeMethod.MENUITEMINFO();
            testitem5.cbSize = (uint) Marshal.SizeOf(testitem5);
            testitem5.fMask = radioFMask;
            testitem5.dwTypeData = "MFT_RADIOCHECK_2";
            testitem5.wID = MENU_ID_05;
            testitem5.fType = NativeConstant.MFT_RADIOCHECK;
            testitem5.hbmpChecked = IntPtr.Zero;
            testitem5.fState = NativeConstant.MFS_UNCHECKED;

            var testitem6 = new NativeMethod.MENUITEMINFO();
            testitem6.cbSize = (uint) Marshal.SizeOf(testitem6);
            testitem6.fMask = radioFMask;
            testitem6.dwTypeData = "MFT_RADIOCHECK_3";
            testitem6.wID = MENU_ID_06;
            testitem6.fType = NativeConstant.MFT_RADIOCHECK;
            testitem6.hbmpChecked = IntPtr.Zero;
            testitem6.fState = NativeConstant.MFS_UNCHECKED;

            // SubMenu
            IntPtr hSubMenu = NativeMethod.CreateMenu();

            var testitem7 = new NativeMethod.MENUITEMINFO();
            testitem7.cbSize = (uint) Marshal.SizeOf(testitem7);
            testitem7.fMask = NativeConstant.MIIM_STRING | NativeConstant.MIIM_SUBMENU;
            testitem7.dwTypeData = "MIIM_SUBMENU";
            testitem7.hSubMenu = hSubMenu;

            NativeMethod.InsertMenuItem(hSysMenu, 5, true, ref splititem);
            NativeMethod.InsertMenuItem(hSysMenu, 6, true, ref testitem1);
            NativeMethod.InsertMenuItem(hSysMenu, 7, true, ref testitem2);
            NativeMethod.InsertMenuItem(hSysMenu, 8, true, ref testitem3);
            NativeMethod.InsertMenuItem(hSysMenu, 9, true, ref testitem7);

            NativeMethod.InsertMenuItem(hSubMenu, 0, true, ref testitem4);
            NativeMethod.InsertMenuItem(hSubMenu, 1, true, ref testitem5);
            NativeMethod.InsertMenuItem(hSubMenu, 2, true, ref testitem6);
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            // System menu event test:

            if (m.Msg == NativeConstant.WM_SYSCOMMAND) {
                uint menuid = (uint)(m.WParam.ToInt32() & 0xffff);
                uint state;

                switch (menuid) {
                    case MENU_ID_01: // 6
                        state = NativeMethod.GetMenuState(hSysMenu, MENU_ID_01, NativeConstant.MF_BYCOMMAND);
                        if ((state & NativeConstant.MFS_CHECKED) != 0x0)
                            NativeMethod.CheckMenuItem(hSysMenu, MENU_ID_01, NativeConstant.MF_BYCOMMAND | NativeConstant.MFS_UNCHECKED);
                        else
                            NativeMethod.CheckMenuItem(hSysMenu, MENU_ID_01, NativeConstant.MF_BYCOMMAND | NativeConstant.MFS_CHECKED);
                        break;
                    case MENU_ID_02: // 7
                        state = NativeMethod.GetMenuState(hSysMenu, MENU_ID_03, NativeConstant.MF_BYCOMMAND);
                        if ((state & NativeConstant.MFS_DISABLED) != 0x0)
                            NativeMethod.EnableMenuItem(hSysMenu, MENU_ID_03, NativeConstant.MF_BYCOMMAND | NativeConstant.MFS_ENABLED);
                        else
                            NativeMethod.EnableMenuItem(hSysMenu, MENU_ID_03, NativeConstant.MF_BYCOMMAND | NativeConstant.MFS_DISABLED);
                        break;
                    case MENU_ID_03: // 8
                        // MessageBox.Show("MFS_GLAYED が選択されました。");
                        MY_MESSAGE = NativeMethod.RegisterWindowMessage("MY_MESSAGE");
                        if (MY_MESSAGE != 0)
                            NativeMethod.SendNotifyMessage(HWND_BROADCAST, MY_MESSAGE, 0, 0);
                        break;
                    case MENU_ID_04: // 9
                    case MENU_ID_05: // 10
                    case MENU_ID_06: // 11
                        NativeMethod.CheckMenuRadioItem(hSysMenu, MENU_ID_04, MENU_ID_06, menuid, NativeConstant.MF_BYCOMMAND);
                        break;
                }
            } else if (m.Msg == MY_MESSAGE) {
                MessageBox.Show(m.Msg.ToString() + " From WndProc MY_MESSAGE");
            }

            // Init Hook:
            // 間違ったフォーマットのプログラムを読み込もうとしました。

            if (m.Msg == NativeConstant.WM_CREATE)
                onStartHook();
            else if (m.Msg == NativeConstant.WM_DESTROY)
                onStopHook();
            
            // Proc Shell Hook Msg:

            if (m.Msg == HookMessage.MSG_HSHELL_WINDOWCREATED ||
                m.Msg == HookMessage.MSG_HCBT_CREATEWND)
                onWindowCreated(m.WParam);
            else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWDESTROYED ||
                m.Msg == HookMessage.MSG_HCBT_DESTROYWND)
                onWindowDestroyed(m.WParam);
            else if (m.Msg == HookMessage.MSG_HSHELL_WINDOWACTIVATED
                || m.Msg == HookMessage.MSG_HCBT_ACTIVATE
                || m.Msg == HookMessage.MSG_HCBT_SETFOCUS)
                onWindowActivated(m.WParam);

            // Proc GetMsg Hook Msg:

            if (m.Msg == HookMessage.MSG_HOOK_GETMSG) {
                addToList(m.WParam, "GetMsg");
                WinUtil.cacheHandle = m.WParam;
                WinUtil.cacheMessage = m.LParam;
            } else if (m.Msg == HookMessage.MSG_HOOK_GETMSG_PARAMS) {
                if (WinUtil.cacheHandle != IntPtr.Zero && WinUtil.cacheMessage != IntPtr.Zero) {
                    addToList(WinUtil.cacheHandle, "GetMsgParam");
                    onWinProcMsg(WinUtil.cacheHandle, WinUtil.cacheMessage, m.WParam, m.LParam);
                    WinUtil.cacheHandle = IntPtr.Zero;
                    WinUtil.cacheMessage = IntPtr.Zero;
                }
            }
        }

        private void addToList(IntPtr hwnd, string token) {
            string title = WinUtil.GetWindowTitle(hwnd);
            if (title != "") listBox1.Items.Add(token + ": 0x" + hwnd.ToInt64().ToString("X6") + " " + title);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Handle events
        List<IntPtr> WinList;

        private void onStartHook() {
            WinList = WinUtil.GetAllWindows();
            foreach (var hwnd in WinList) {
                addToList(hwnd, "Exist");
            }

            HookMessage.RegisterMsg();

            HookMethod.InitGetMsgHook(0, Handle);
            HookMethod.InitShellHook(0, Handle);
            HookMethod.InitCbtHook(0, Handle);
        }

        private void onStopHook() {
            foreach (var hwnd in WinList) {
                // WinUtil.RemoveSystemMenu(hwnd);
            }

            HookMethod.UnInitGetMsgHook();
            HookMethod.UnInitShellHook();
            HookMethod.UnInitCbtHook();
        }

        private void onWindowCreated(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Cre " + newList.Count.ToString();
            foreach (var newHwnd in newList.Except(WinList)) {
                addToList(newHwnd, "Create");
            }
            WinList = newList;
        }

        private void onWindowDestroyed(IntPtr hwnd) {
            var newList = WinUtil.GetAllWindows();
            button1.Text = "Des " + newList.Count.ToString();
            foreach (var oldHwnd in WinList.Except(newList)) {
                addToList(oldHwnd, "Delete");
            }
            WinList = newList;
        }

        private void onWindowActivated(IntPtr hwnd) {

            var newList = WinUtil.GetAllWindows();
            button1.Text = "Act " + newList.Count.ToString();
            if (newList.Count > WinList.Count) {
                foreach (var newHwnd in newList.Except(WinList)) {
                    addToList(newHwnd, "Create(Act)");
                }
            } else {
                foreach (var newHwnd in WinList.Except(newList)) {
                    addToList(newHwnd, "Delete(Act)");
                }
            }
            WinList = newList;
        }

        private void onWinProcMsg(IntPtr hwnd, IntPtr message, IntPtr WParam, IntPtr LParam) {
            if (message.ToInt64() == NativeConstant.WM_SYSCOMMAND) {
                string title = WinUtil.GetWindowTitle(hwnd);

                uint menuid = (uint)(WParam.ToInt32() & 0xffff);
                if (menuid == WinUtil.MENU_ID_TOPMOST) {
                    MessageBox.Show("MENU_ID_TOPMOST: " + title);
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Others

        private void MainForm_Load(object sender, EventArgs e) {

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
            
        }

        private void button1_Click(object sender, EventArgs e) {
            Form form = new Form();
            form.Text = "Test";
            form.Show();
        }
    }
}

#include "pch.h"
#include "hook.h"

HINSTANCE hModuleInstance = NULL;

#pragma data_seg(".Shared")

HWND hwndMain = NULL;
HHOOK hookShell = NULL;
HHOOK hookCbt = NULL;
HHOOK hookGetMessage = NULL;
HHOOK hookCallWndProc = NULL;

#pragma data_seg()
#pragma comment(linker, "/section:.Shared,rws")

static LRESULT CALLBACK ShellHookCallback(int, WPARAM, LPARAM);
static LRESULT CALLBACK CbtHookCallback(int, WPARAM, LPARAM);
static LRESULT CALLBACK GetMessageHookCallback(int, WPARAM, LPARAM);
static LRESULT CALLBACK CallWndProcHookCallback(int, WPARAM, LPARAM);

// WH_SHELL

DLLEXPORT bool __stdcall InitializeShellHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndMain != NULL) {
        UINT msg = RegisterWindowMessage(WcsShellReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndMain, msg, 0, 0);
        }
    }

    hwndMain = destination;
    hookShell = SetWindowsHookEx(WH_SHELL, (HOOKPROC) ShellHookCallback, hModuleInstance, threadId);
    return hookShell != NULL;
}

DLLEXPORT void __stdcall UninitializeShellHook() {
    if (hookShell != NULL) {
        UnhookWindowsHookEx(hookShell);
    }

    hookShell = NULL;
}

static LRESULT CALLBACK ShellHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (code >= 0) {
        LPCWSTR wcs;
        switch (code) {
        case HSHELL_WINDOWCREATED:
            wcs = WcsHShellWindowCreated;
        case HSHELL_WINDOWDESTROYED:
            wcs = WcsHShellWindowDestroyed;
        case HSHELL_ACTIVATESHELLWINDOW:
            wcs = WcsHShellActivateShellWindow;
        case HSHELL_WINDOWACTIVATED:
            wcs = WcsHShellWindowActivated;
        case HSHELL_GETMINRECT:
            wcs = WcsHShellGetMinRect;
        case HSHELL_REDRAW:
            wcs = WcsHShellRedraw;
        case HSHELL_TASKMAN:
            wcs = WcsHShellTaskman;
        case HSHELL_LANGUAGE:
            wcs = WcsHShellLanguage;
        case HSHELL_ACCESSIBILITYSTATE:
            wcs = WcsHShellAccessibilityState;
        case HSHELL_APPCOMMAND:
            wcs = WcsHShellAppCommand;
        case HSHELL_WINDOWREPLACED:
            wcs = WcsHShellWindowReplaced;
        default:
            wcs = NULL;
        }

        if (wcs != NULL) {
            UINT msg = RegisterWindowMessage(wcs);
            if (msg != 0) {
                SendNotifyMessage(hwndMain, msg, wparam, lparam);
            }
        }
    }

    return CallNextHookEx(hookShell, code, wparam, lparam);
}

// WH_CBT

DLLEXPORT bool __stdcall InitializeCbtHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndMain != NULL) {
        UINT msg = RegisterWindowMessage(WcsCbtReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndMain, msg, 0, 0);
        }
    }

    hwndMain = destination;
    hookCbt = SetWindowsHookEx(WH_CBT, (HOOKPROC) CbtHookCallback, hModuleInstance, threadId);
    return hookCbt != NULL;
}

DLLEXPORT void __stdcall UninitializeCbtHook() {
    if (hookCbt != NULL) {
        UnhookWindowsHookEx(hookCbt);
    }
    hookCbt = NULL;
}

static LRESULT CALLBACK CbtHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (code < 0) {
        return CallNextHookEx(hookCbt, code, wparam, lparam);
    }
}

// WH_GETMESSAGE

DLLEXPORT bool __stdcall InitializeGetMessageHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndMain != NULL) {
        UINT msg = RegisterWindowMessage(WcsGetMessageReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndMain, msg, 0, 0);
        }
    }

    hwndMain = destination;
    hookGetMessage = SetWindowsHookEx(WH_GETMESSAGE, (HOOKPROC) GetMessageHookCallback, hModuleInstance, threadId);
    return hookGetMessage != NULL;
}

DLLEXPORT void __stdcall UninitializeGetMessageHook() {
    if (hookGetMessage != NULL) {
        UnhookWindowsHookEx(hookGetMessage);
    }
    hookGetMessage = NULL;
}

static LRESULT CALLBACK GetMessageHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (code < 0) {
        return CallNextHookEx(hookGetMessage, code, wparam, lparam);
    }
}

// WH_CALLWNDPROC

DLLEXPORT bool __stdcall InitializeCallWndProcHook(int threadId, HWND destination) {
    if (hModuleInstance == NULL) {
        return false;
    }

    if (hwndMain != NULL) {
        UINT msg = RegisterWindowMessage(WcsCallWndProcReplaced);
        if (msg != 0) {
            SendNotifyMessage(hwndMain, msg, 0, 0);
        }
    }

    hwndMain = destination;
    hookCallWndProc = SetWindowsHookEx(WH_CALLWNDPROC, (HOOKPROC) CallWndProcHookCallback, hModuleInstance, threadId);
    return hookCallWndProc != NULL;
}

DLLEXPORT void __stdcall UninitializeCallWndProcHook() {
    if (hookCallWndProc != NULL) {
        UnhookWindowsHookEx(hookCallWndProc);
    }
    hookCallWndProc = NULL;
}

static LRESULT CALLBACK CallWndProcHookCallback(int code, WPARAM wparam, LPARAM lparam) {
    if (code < 0) {
        return CallNextHookEx(hookCallWndProc, code, wparam, lparam);
    }
}

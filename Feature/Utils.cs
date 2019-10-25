using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Threading;

namespace Hav3Fun
{
    class Utils
    {
        internal static bool IsWindowFocused;
        internal static Structure.Entity LocalPlayer = new Structure.Entity();
        internal static Structure.ClientState ClientState = new Structure.ClientState();
        internal static int CurrentWeapon, GlowObj, GlowObjCount;
        internal const int MOUSEEVENTF_LEFTDOWN = 0x02;
        internal const int MOUSEEVENTF_LEFTUP = 0x04;
        internal static void Run()
        {
            while (true)
            {
                CurrentWeapon = GetCurrentWeapon();
                ClientState.GameState = InGame();
                GlowObj = Memory.Read<int>(Structure.Base.Client + Offset.dwGlowObjectManager);
                GlowObjCount = Memory.Read<int>(Structure.Base.Client + Offset.dwGlowObjectManager + 0x4);

                LocalPlayer.Base = Memory.Read<int>(Structure.Base.Client + Offset.dwLocalPlayer);
                LocalPlayer.Team = GetTeam(LocalPlayer.Base);
                LocalPlayer.CrosshairID = GetCrosshairID(LocalPlayer.Base);
                LocalPlayer.Kills = Memory.Read<int>(LocalPlayer.Base + Offset.m_iNumRoundKills);
                //LocalPlayer.FlashDuration = Memory.Read<float>(LocalPlayer.Base + Offset.m_flFlashDuration);

                IsWindowFocused = IsWindowFocues(Memory.CurrentProcess);
                Thread.Sleep(13);
            }
        }

        internal static int GetTeam(object entityCrossID)
        {
            throw new NotImplementedException();
        }

        internal static List<int> WeaponRifle = new List<int>() { 7, 10, 39, 262151, 262160, 262154, 262183, 8, 60, 13, 262152, 262204, 262157, 16 };
        internal static List<int> WeaponPistols = new List<int>() { 61, 36, 32, 2, 262205, 262180, 262176, 262207, 4, 30, 3, 63, 262146, 262148, 262174, 262207 };
        internal static List<int> WeaponSMGs = new List<int>() { 33, 26, 24, 262161, 262177, 262170, 262168, 34, 262167, 19, 262178, 786455, 262163, 17 };

        private static int GetCurrentWeapon()
        {
            int getHandleWeap = Memory.Read<int>(LocalPlayer.Base + Offset.m_hActiveWeapon);
            int getWeapEnt = getHandleWeap &= 0xFFF;
            int dwWeapon = Memory.Read<int>(Structure.Base.Client + Offset.dwEntityList + (getWeapEnt - 1) * 0x10);
            return Memory.Read<int>(dwWeapon + Offset.m_iItemDefinitionIndex);
        }

        internal static bool IsWindowFocues(Process proc)
        {
            IntPtr activatedHandle = WinAPI.GetForegroundWindow();

            if (activatedHandle == IntPtr.Zero)
                return false;

            WinAPI.GetWindowThreadProcessId(activatedHandle, out int activeProcId);
            return activeProcId == proc.Id;
        }

        private static int InGame()
        {
            int ClientState = Memory.Read<int>(Structure.Base.Engine + Offset.dwClientState);
            int InGameNum = Memory.Read<int>(ClientState + Offset.dwClientState_State);
            return InGameNum;
        }

        internal static int GetViewAngles(int EnginePointer) =>
            Memory.Read<int>(EnginePointer + Offset.dwClientState_ViewAngles);

        internal static Vector3 GetVecView(int EntityBase) =>
            Memory.Read<Vector3>(EntityBase + Offset.m_vecViewOffset);

        internal static int GetCrosshairID(int Entity) =>
            Memory.Read<int>(Entity + Offset.m_iCrosshairId);

        internal static Vector3 GetAimPunch(int EntityBase) =>
            Memory.Read<Vector3>(EntityBase + Offset.m_aimPunchAngle);

        internal static Vector3 GetPosition(int EntityBase) =>
            Memory.Read<Vector3>(EntityBase + Offset.m_vecOrigin);

        internal static int GetClassID(int id) =>
            Memory.Read<int>(Memory.Read<int>(Memory.Read<int>(Memory.Read<int>(id + 0x8) + 2 * 0x4) + 1) + 20);

        internal static int GetInt(byte[] bytes, int offset) =>
            (bytes[offset] | bytes[++offset] << 8 | bytes[++offset] << 16 | bytes[++offset] << 24);

        internal static int GetHP(int Entity) =>
            Memory.Read<int>(Entity + Offset.m_iHealth);

        internal static int GetTeam(int Entity) =>
            Memory.Read<int>(Entity + Offset.m_iTeamNum);

        internal static int GetFlags(int Entity) =>
            Memory.Read<int>(Entity + Offset.m_fFlags);
    }
}
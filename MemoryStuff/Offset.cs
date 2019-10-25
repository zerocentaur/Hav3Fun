using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Hav3Fun
{
    partial class Offset
    {
        internal static Int32 dwGlowObjectManager;
        internal static Int32 dwEntityList;
        internal static Int32 dwClientState;
        internal static Int32 dwLocalPlayer;
        internal static Int32 dwModelAmbient;
        internal static Int32 dwClientState_State;
        internal static Int32 dwClientState_ViewAngles;
        //internal static Int32 dwbSendPackets = 864154;
        private static Int32 dwGetAllClasses;
        internal static Int32 m_iTeamNum;
        internal static Int32 m_iGlowIndex;
        internal static Int32 m_iHealth;
        internal static Int32 m_dwBoneMatrix;
        internal static Int32 m_vecOrigin;
        internal static Int32 m_vecViewOffset;
        internal static Int32 m_aimPunchAngle;
        internal static Int32 m_bSpottedByMask;
        internal static Int32 m_hActiveWeapon;
        internal static Int32 m_iItemDefinitionIndex;
        internal static Int32 m_iCrosshairId;
        internal static Int32 m_fFlags;
        internal static Int32 m_iNumRoundKills;
        //internal static Int32 m_flFlashDuration;
    }

    partial class Offset
    {
        internal static void GetOffsets()
        {
            dwGlowObjectManager = ScanPattern(Structure.Base.Client, "A1 ? ? ? ? A8 01 75 4B", 4, 1, true);
            dwEntityList = ScanPattern(Structure.Base.Client, "BB ? ? ? ? 83 FF 01 0F 8C ? ? ? ? 3B F8", 0, 1, true);
            dwLocalPlayer = ScanPattern(Structure.Base.Client, "8D 34 85 ? ? ? ? 89 15 ? ? ? ? 8B 41 08 8B 48 04 83 F9 FF", 4, 3, true);
            dwClientState = ScanPattern(Structure.Base.Engine, "A1 ? ? ? ? 33 D2 6A 00 6A 00 33 C9 89 B0", 0, 1, true);
            dwModelAmbient = ScanPattern(Structure.Base.Engine, "F3 0F 10 0D ? ? ? ? F3 0F 11 4C 24 ? 8B 44 24 20 35 ? ? ? ? 89 44 24 0C", 0, 4, true);
            dwClientState_State = ScanPattern(Structure.Base.Engine, "83 B8 ? ? ? ? ? 0F 94 C0 C3", 0, 2, false);
            dwClientState_ViewAngles = ScanPattern(Structure.Base.Engine, "F3 0F 11 80 ? ? ? ? D9 46 04 D9 05", 0, 4, false);
            dwGetAllClasses = Memory.Read<int>(ScanPattern(Structure.Base.Client, "A1 ? ? ? ? C3 CC CC CC CC CC CC CC CC CC CC A1 ? ? ? ? B9", 0, 1, false));

            GetClasses();

            //m_flFlashDuration = Value["DT_CSPlayer"]["m_flFlashDuration"];
            m_iNumRoundKills = Value["DT_CSPlayer"]["m_iNumRoundKills"];
            m_vecOrigin = Value["DT_BaseEntity"]["m_vecOrigin"];
            m_iHealth = Value["DT_CSPlayer"]["m_iHealth"];
            m_iGlowIndex = Value["DT_CSPlayer"]["m_flFlashDuration"] + 0x18;
            m_iTeamNum = Value["DT_BaseEntity"]["m_iTeamNum"];
            m_vecViewOffset = Value["DT_CSPlayer"]["m_vecViewOffset[0]"];
            m_dwBoneMatrix = Value["DT_BaseAnimating"]["m_nForceBone"] + 28;
            m_bSpottedByMask = Value["DT_BaseEntity"]["m_bSpottedByMask"];
            m_iItemDefinitionIndex = Value["DT_BaseCombatWeapon"]["m_iItemDefinitionIndex"];
            m_aimPunchAngle = Value["DT_BasePlayer"]["m_aimPunchAngle"];
            m_fFlags = Value["DT_CSPlayer"]["m_fFlags"];
            m_hActiveWeapon = Value["DT_CSPlayer"]["m_hActiveWeapon"];
            m_iCrosshairId = Value["DT_CSPlayer"]["m_bHasDefuser"] + 92;
        }

        internal static int ScanPattern(int dll, string pattern, int extra, int offset, bool modeSubtract)
        {
            int tempOffset = BitConverter.ToInt32(Memory.ReadMemory((AobScan(dll, 0x1800000, pattern, 0) + offset), 4), 0) + extra;

            if (modeSubtract)
                tempOffset -= dll;

            return tempOffset;
        }

        internal static int ScanPattern1(int dll, string pattern, int extra, int offset, bool modeSubtract)
        {
            int tempOffset = BitConverter.ToInt32(Memory.ReadMemory((AobScan(dll, 0x1800000, pattern, 0) + offset), 4), 0) + extra;

            if (modeSubtract)
                tempOffset -= dll;

            return tempOffset;
        }

        private static int AobScan(int dll, int range, string signature, int instance)
        {
            if (signature == string.Empty)
                return -1;

            int count = -1;
            string tempSignature = Regex.Replace(signature.Replace("?", "3F"), "[^a-fA-F0-9]", "");
            byte[] searchRange = new byte[(tempSignature.Length / 2)];
            byte[] readMemory = Memory.ReadMemory(dll, range);
            int[] sBytes = new int[0x100];

            for (int i = 0; i <= searchRange.Length - 1; i++)
                searchRange[i] = byte.Parse(tempSignature.Substring(i * 2, 2), NumberStyles.HexNumber);

            int temp1 = 0;
            int iEnd = searchRange.Length < 0x20 ? searchRange.Length : 0x20;

            for (int j = 0; j <= iEnd - 1; j++)
                if ((searchRange[j] == 0x3f))
                    temp1 = (temp1 | (Convert.ToInt32(1) << ((iEnd - j) - 1)));

            if ((temp1 != 0))
                for (int k = 0; k <= sBytes.Length - 1; k++)
                    sBytes[k] = temp1;

            temp1 = 1;

            int index = (iEnd - 1);
            while ((index >= 0))
            {
                sBytes[searchRange[index]] = (sBytes[searchRange[index]] | temp1);
                index -= 1;
                temp1 = (temp1 << 1);
            }

            int temp2 = 0;

            while ((temp2 <= (readMemory.Length - searchRange.Length)))
            {
                int last = searchRange.Length;
                temp1 = (searchRange.Length - 1);

                int temp3 = -1;
                while ((temp3 != 0))
                {
                    temp3 = (temp3 & sBytes[readMemory[temp2 + temp1]]);

                    if ((temp3 != 0))
                    {
                        if ((temp1 == 0))
                        {
                            count += 1;
                            if (count == instance)
                                return dll + temp2;
                            temp2 += 2;
                        }
                        last = temp1;
                    }
                    temp1 -= 1;
                    temp3 = (temp3 << 1);
                }
                temp2 += last;
            }
            return -1;
        }

        static public Dictionary<string, Dictionary<string, int>> Value = new Dictionary<string, Dictionary<string, int>>();
        static void ScanTable(int table, int level, int offset, string name)
        {
            int count = Memory.Read<int>(table + 0x4);
            for (var i = 0; i < count; ++i)
            {
                int propID = Memory.Read<int>(table) + i * 0x3C;
                string propName = Memory.ReadTextFromProcess(Memory.Read<int>(propID), 64);
                bool isBaseClass = propName.IndexOf("baseclass") == 0;
                int propOffset = offset + Memory.Read<int>(propID + 0x2C);
                if (!isBaseClass)
                {
                    if (!Value.ContainsKey(name))
                        Value.Add(name, new Dictionary<string, int>());
                    if (!Value[name].ContainsKey(propName))
                        Value[name].Add(propName, propOffset);
                }

                int child = Memory.Read<int>(propID + 0x28);
                if (child == 0)
                    continue;

                if (isBaseClass)
                    --level;

                ScanTable(child, ++level, propOffset, name);
            }
        }
        static private void GetClasses()
        {
            do
            {
                int Table = Memory.Read<int>(dwGetAllClasses + 0xC);
                if (Table != 0)
                {
                    string TableName = Memory.ReadTextFromProcess(Memory.Read<int>(Table + 0xC), 32);
                    ScanTable(Table, 0, 0, TableName);
                }
                dwGetAllClasses = Memory.Read<int>(dwGetAllClasses + 0x10);
            } while (dwGetAllClasses != 0);
        }
    }
}
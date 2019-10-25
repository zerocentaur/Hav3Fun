using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Hav3Fun
{
    class Settings
    {
        private static readonly string ConfigPath = Application.StartupPath + @"\config.ini";
        private static void Write(string Section, string Key, string Value) =>
            WinAPI.WritePrivateProfileString(Section, Key, Value, ConfigPath);

        private static string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            WinAPI.GetPrivateProfileString(Section, Key, "", temp, 255, ConfigPath);
            return temp.ToString();
        }

        internal static void GetSettings()
        {
            if (!File.Exists("config.ini"))
            {
                Write("\t\tGlow\t\t", "SwitchGlow", "true" + Environment.NewLine);


                Write("\t\tAim\t\t", "SwitchAim", "true");
                Write("Aim", "AimRecoilHelper", "true" + Environment.NewLine);

                Write("Aim", "KillDelay", "250");
                Write("Aim", "AimKey", "1" + Environment.NewLine);

                Write("Aim", "Rifles_FOV", "2,0");
                Write("Aim", "Rifles_Smooth", "14,5");
                Write("Aim", "Rifles_Bone", "8");
                Write("Aim", "Rifles_YawRecoil", "2,2");
                Write("Aim", "Rifles_PitchRecoil", "2,2" + Environment.NewLine);

                Write("Aim", "SMGs_FOV", "2,5");
                Write("Aim", "SMGs_Smooth", "12,5");
                Write("Aim", "SMGs_Bone", "8");
                Write("Aim", "SMGs_YawRecoil", "2,2");
                Write("Aim", "SMGs_PitchRecoil", "2,2" + Environment.NewLine);

                Write("Aim", "Pistols_FOV", "1,75");
                Write("Aim", "Pistols_Smooth", "9,5");
                Write("Aim", "Pistols_Bone", "7");
                Write("Aim", "Pistols_YawRecoil", "0,0");
                Write("Aim", "Pistols_PitchRecoil", "0,0" + Environment.NewLine);


                Write("\t\tTrigger\t\t", "SwitchTrigger", "true");
                Write("Trigger", "TriggerDelay", "10");
                Write("Trigger", "TriggerKey", "18");
            }

            if (File.Exists("config.ini"))
            {
                SwitchGlow = Convert.ToBoolean(Read("Glow", "SwitchGlow"));


                SwitchAim = Convert.ToBoolean(Read("Aim", "SwitchAim"));
                AimRecoilHelper = Convert.ToBoolean(Read("Aim", "AimRecoilHelper"));

                KillDelay = Convert.ToInt32(Read("Aim", "KillDelay"));
                AimKey = Convert.ToInt32(Read("Aim", "AimKey"));

                Rifles_FOV = Convert.ToSingle(Read("Aim", "Rifles_FOV"));
                Rifles_Smooth = Convert.ToSingle(Read("Aim", "Rifles_Smooth"));
                Rifles_Bone = Convert.ToInt32(Read("Aim", "Rifles_Bone"));
                Rifles_YawRecoil = Convert.ToSingle(Read("Aim", "Rifles_YawRecoil"));
                Rifles_PitchRecoil = Convert.ToSingle(Read("Aim", "Rifles_PitchRecoil"));

                SMGs_FOV = Convert.ToSingle(Read("Aim", "SMGs_FOV"));
                SMGs_Smooth = Convert.ToSingle(Read("Aim", "SMGs_Smooth"));
                SMGs_Bone = Convert.ToInt32(Read("Aim", "SMGs_Bone"));
                SMGs_YawRecoil = Convert.ToSingle(Read("Aim", "SMGs_YawRecoil"));
                SMGs_PitchRecoil = Convert.ToSingle(Read("Aim", "SMGs_PitchRecoil"));

                Pistols_FOV = Convert.ToSingle(Read("Aim", "Pistols_FOV"));
                Pistols_Smooth = Convert.ToSingle(Read("Aim", "Pistols_Smooth"));
                Pistols_Bone = Convert.ToInt32(Read("Aim", "Pistols_Bone"));
                Pistols_YawRecoil = Convert.ToSingle(Read("Aim", "Pistols_YawRecoil"));
                Pistols_PitchRecoil = Convert.ToSingle(Read("Aim", "Pistols_PitchRecoil"));


                SwitchTrigger = Convert.ToBoolean(Read("Trigger", "SwitchTrigger"));
                TriggerDelay = Convert.ToInt32(Read("Trigger", "TriggerDelay"));
                TriggerKey = Convert.ToInt32(Read("Trigger", "TriggerKey"));
            }
        }

        internal static bool SwitchGlow { get; set; }

        internal static bool SwitchAim { get; set; }
        internal static bool AimRecoilHelper { get; set; }

        internal static int KillDelay { get; set; }
        internal static int AimKey { get; set; }

        internal static float Rifles_FOV { get; set; }
        internal static float Rifles_Smooth { get; set; }
        internal static int Rifles_Bone { get; set; }
        internal static float Rifles_YawRecoil { get; set; }
        internal static float Rifles_PitchRecoil { get; set; }

        internal static float SMGs_FOV { get; set; }
        internal static float SMGs_Smooth { get; set; }
        internal static int SMGs_Bone { get; set; }
        internal static float SMGs_YawRecoil { get; set; }
        internal static float SMGs_PitchRecoil { get; set; }

        internal static float Pistols_FOV { get; set; }
        internal static float Pistols_Smooth { get; set; }
        internal static int Pistols_Bone { get; set; }
        internal static float Pistols_YawRecoil { get; set; }
        internal static float Pistols_PitchRecoil { get; set; }


        internal static bool SwitchTrigger { get; set; }
        internal static int TriggerDelay { get; set; }
        internal static int TriggerKey { get; set; }
    }
}

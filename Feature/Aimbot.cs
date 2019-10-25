using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;

namespace Hav3Fun
{
    class Aimbot
    {
        internal static void Run()
        {
            Structure.Entity Enemy = new Structure.Entity();
            int Kills = 0;
            var Aim = (FOV: 0f, Smooth: 0f, AimBone: 0, YawRecoil: 0f, PitchRecoil: 0f);

            void SetConfig(float Fov, float Smooth, int Bone, float yRec, float pRec)
            {
                Aim.FOV = Fov;
                Aim.Smooth = Smooth;
                Aim.AimBone = Bone;
                Aim.YawRecoil = yRec;
                Aim.PitchRecoil = pRec;
            }

            for (; ; )
            {
                if (Utils.ClientState.GameState != 6 || !Settings.SwitchAim || !Utils.IsWindowFocused)
                {
                    Thread.Sleep(2000);
                    continue;
                }

                if (Utils.LocalPlayer.Kills != Kills)
                {
                    Thread.Sleep(Settings.KillDelay);
                    Kills = Utils.LocalPlayer.Kills;
                }

                Thread.Sleep(8);

                if (WinAPI.GetAsyncKeyState(Settings.AimKey) == -32768)
                {
                    byte[] Entities = Memory.ReadMemory((Structure.Base.Client + Offset.dwEntityList), 32 * 0x10);
                    Dictionary<float, Vector3> PossibleTargets = new Dictionary<float, Vector3> { };

                    if (Utils.WeaponRifle.Contains(Utils.CurrentWeapon))
                        SetConfig(Settings.Rifles_FOV, Settings.Rifles_Smooth,
                                  Settings.Rifles_Bone,
                                  Settings.Rifles_YawRecoil, Settings.Rifles_PitchRecoil);
                    else if (Utils.WeaponPistols.Contains(Utils.CurrentWeapon))
                        SetConfig(Settings.Pistols_FOV, Settings.Pistols_Smooth,
                                  Settings.Pistols_Bone,
                                  Settings.Pistols_YawRecoil, Settings.Pistols_PitchRecoil);
                    else if (Utils.WeaponSMGs.Contains(Utils.CurrentWeapon))
                        SetConfig(Settings.SMGs_FOV, Settings.SMGs_Smooth,
                                  Settings.SMGs_Bone,
                                  Settings.SMGs_YawRecoil, Settings.SMGs_PitchRecoil);
                    else continue;

                    for (int EntityCounter = 1; EntityCounter < 32; EntityCounter++)
                    {
                        Enemy.Base = Utils.GetInt(Entities, EntityCounter * 0x10);
                        Enemy.Health = Utils.GetHP(Enemy.Base);
                        Enemy.Flags = Utils.GetFlags(Enemy.Base);

                        if (Enemy.Flags == 774 || Enemy.Health <= 0 || !EntIsVisible(Enemy.Base, Utils.LocalPlayer.Base)) continue;

                        Vector3 bonePosition = Calculations.GetBonePos(Enemy.Base, Aim.AimBone);
                        if (bonePosition == Vector3.Zero) continue;

                        Utils.LocalPlayer.Position = Utils.GetPosition(Utils.LocalPlayer.Base);
                        Utils.LocalPlayer.AimPunch = Utils.GetAimPunch(Utils.LocalPlayer.Base);
                        Utils.LocalPlayer.VecView = Utils.GetVecView(Utils.LocalPlayer.Base);

                        Vector3 destination = Settings.AimRecoilHelper
                            ? Calculations.CalcAngle(Utils.LocalPlayer.Position, bonePosition, Utils.LocalPlayer.AimPunch, Utils.LocalPlayer.VecView, Aim.YawRecoil, Aim.PitchRecoil)
                            : Calculations.CalcAngle(Utils.LocalPlayer.Position, bonePosition, Utils.LocalPlayer.AimPunch, Utils.LocalPlayer.VecView, 0f, 0f);

                        if (destination == Vector3.Zero) continue;

                        Utils.ClientState.Base = Memory.Read<int>(Structure.Base.Engine + Offset.dwClientState);
                        Utils.ClientState.ViewAngles = Memory.Read<Vector3>(Utils.ClientState.Base + Offset.dwClientState_ViewAngles);

                        float distance = Calculations.GetDistance3D(destination, Utils.ClientState.ViewAngles);

                        if (!(distance <= Aim.FOV)) continue;
                        PossibleTargets.Add(distance, destination);
                    }

                    if (!PossibleTargets.Any()) continue;

                    Vector3 aimAngle = PossibleTargets.OrderByDescending(x => x.Key).LastOrDefault().Value;

                    aimAngle = Calculations.NormalizeAngle(aimAngle);
                    aimAngle = Calculations.ClampAngle(aimAngle);

                    Memory.Write<Vector3>(Utils.ClientState.Base + Offset.dwClientState_ViewAngles, Aim.Smooth == 0f ? aimAngle : Calculations.SmoothAim(Utils.ClientState.ViewAngles, aimAngle, Aim.Smooth));

                }
            }
        }

        static bool EntIsVisible(int ent, int Base)
        {
            int mask = Memory.Read<int>(ent + Offset.m_bSpottedByMask);
            int PBASE = Memory.Read<int>(Base + 0x64) - 1;
            return (mask & (1 << PBASE)) > 0;
        }
    }
}
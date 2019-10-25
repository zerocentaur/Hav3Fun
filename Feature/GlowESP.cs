using System.Threading;

namespace Hav3Fun
{
    class GlowESP
    {
        internal static void Run()
        {
            Structure.Entity Enemy = new Structure.Entity();

            for (; ; )
            {
                if (Utils.ClientState.GameState != 6 || !Settings.SwitchGlow || !Utils.IsWindowFocused)
                {
                    Thread.Sleep(2000);
                    continue;
                }

                Thread.Sleep(8);

                byte[] Entities = Memory.ReadMemory(Utils.GlowObj, Utils.GlowObjCount * 0x38);

                for (int EntityCounter = 0; EntityCounter < Utils.GlowObjCount; EntityCounter++)
                {
                    int CurrentEntity = Utils.GetInt(Entities, EntityCounter * 0x38);
                    if (CurrentEntity == 0) continue;

                    int classID = Utils.GetClassID(CurrentEntity);
                    if (classID < 0) continue;

                    switch (classID)
                    {
                        case 40: //ppl
                            Enemy.Team = Utils.GetTeam(CurrentEntity);
                            Enemy.Health = Utils.GetHP(CurrentEntity);

                            if (Enemy.Team != Utils.LocalPlayer.Team)
                                Write(Utils.GlowObj, EntityCounter, 255.0f - Enemy.Health * 2.55f, Enemy.Health * 2.55f, 0f, 65f);
                            break;
                        case 1: //ak
                            Write(Utils.GlowObj, EntityCounter, 0f, 255f, 0f, 65f);
                            break;
                        case 248: //m4
                            Write(Utils.GlowObj, EntityCounter, 200f, 200f, 110f, 65f);
                            break;
                        case 231: //aug
                            Write(Utils.GlowObj, EntityCounter, 0f, 255f, 255f, 65f);
                            break;
                        case 232: //awp
                            Write(Utils.GlowObj, EntityCounter, 255f, 0f, 255f, 65f);
                            break;
                        case 264: //sg553
                            Write(Utils.GlowObj, EntityCounter, 0f, 255f, 155f, 65f);
                            break;
                        case 266: //ssg08
                            Write(Utils.GlowObj, EntityCounter, 200f, 165f, 155f, 65f);
                            break;
                        case 34: //c4
                            Write(Utils.GlowObj, EntityCounter, 255f, 255f, 155f, 75f);
                            break;
                        case 128: //c4planted
                            Write(Utils.GlowObj, EntityCounter, 0f, 55f, 255f, 75f);
                            break;
                    }
                }
            }
        }
        static void Write(int GlowObj, int EntityCounter, float R, float G, float B, float A)
        {
            Structure.Glow_t currGlowObject = Memory.Read<Structure.Glow_t>((GlowObj + (EntityCounter * 0x38) + 0x4));
            currGlowObject.r = R / 100f;
            currGlowObject.g = G / 100f;
            currGlowObject.b = B / 100f;
            currGlowObject.a = A / 255f;
            currGlowObject.m_bRenderWhenOccluded = true;
            currGlowObject.m_bRenderWhenUnoccluded = false;
            //currGlowObject.m_bFullBloom = FullBloom;

            Memory.Write<Structure.Glow_t>((GlowObj + (EntityCounter * 0x38) + 0x4), currGlowObject);
        }
    }
}
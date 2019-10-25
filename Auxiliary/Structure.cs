using System.Numerics;
using System.Runtime.InteropServices;

namespace Hav3Fun
{
    class Structure
    {
        internal struct Base
        {
            public static int Client { get; set; }
            public static int ClientSize { get; set; }
            public static int Engine { get; set; }
            public static int EngineSize { get; set; }
            //public static int Server { get; set; }
        }

        internal struct Entity
        {
            internal int Base { get; set; }
            internal int Health { get; set; }
            internal int Team { get; set; }
            internal int Flags { get; set; }
            internal int Kills { get; set; }
            internal float FlashDuration { get; set; }
            internal Vector3 Position { get; set; }
            internal Vector3 AimPunch { get; set; }
            internal Vector3 VecView { get; set; }
            //internal int GlowIndex { get; set; }
            internal int CrosshairID { get; set; }
            //internal bool Spotted { get; set; }
        }

        internal struct ClientState
        {
            internal int Base { get; set; }
            internal int GameState { get; set; }
            internal Vector3 ViewAngles { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct Glow_t
        {
            internal float r; //0x4
            internal float g; //0x8
            internal float b; //0xC
            internal float a; //0x10

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            internal byte[] junk1;

            internal bool m_bRenderWhenOccluded;      //0x24
            internal bool m_bRenderWhenUnoccluded;    //0x25
            //internal bool m_bFullBloom;               //0x26
        }
    }
}
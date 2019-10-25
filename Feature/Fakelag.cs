//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Hav3FunGlowESP
//{
//    class Fakelag
//    {
//        internal static void Run()
//        {
//            char dontsend = '0'; //byte
//            char sendpack = '1';
//            for (; ; )
//            {
//                Memory.Write<IntPtr>(Structure.Base.Engine + Offset.dwbSendPackets, dontsend);
//                Console.WriteLine(Memory.Read<byte>(Structure.Base.Engine + Offset.dwbSendPackets));
//                Thread.Sleep(150);
//                Memory.Write<IntPtr>(Structure.Base.Engine + Offset.dwbSendPackets, sendpack);

//                Thread.Sleep(100);
//            }
//        }
//    }
//}

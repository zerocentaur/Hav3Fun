using System.Threading;

namespace Hav3Fun
{
    class Program
    {
        static void Main(string[] args)
        {
            Memory.Attach();
            Offset.GetOffsets();
            Settings.GetSettings();

            new Thread(Utils.Run).Start();
            Thread.Sleep(10);
            new Thread(GlowESP.Run).Start();
            Thread.Sleep(10);
            new Thread(Aimbot.Run).Start();
            Thread.Sleep(10);
            new Thread(Triggerbot.Run).Start();
            //Thread.Sleep(10);
            //new Thread(Fakelag.Run).Start();
        }
    }
}
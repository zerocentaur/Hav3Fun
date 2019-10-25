using System.Threading;

namespace Hav3Fun
{
    class Triggerbot
    {
        internal static void Run()
        {
            Structure.Entity Entity = new Structure.Entity();

            for (; ; )
            {
                if (Utils.ClientState.GameState != 6 || !Settings.SwitchTrigger || !Utils.IsWindowFocused)
                {
                    Thread.Sleep(2000);
                    continue;
                }

                Thread.Sleep(2);

                if (Utils.LocalPlayer.CrosshairID > 0 && Utils.LocalPlayer.CrosshairID <= 32)
                {
                    Entity.Base = Memory.Read<int>(Structure.Base.Client + Offset.dwEntityList + (Utils.LocalPlayer.CrosshairID - 1) * 0x10);
                    Entity.Team = Utils.GetTeam(Entity.Base);

                    if (Entity.Team != Utils.LocalPlayer.Team && WinAPI.GetAsyncKeyState(Settings.TriggerKey) == -32768 && WinAPI.GetAsyncKeyState(1) == 0)
                    {
                        Thread.Sleep(Settings.TriggerDelay);
                        WinAPI.mouse_event(Utils.MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        Thread.Sleep(1);
                        WinAPI.mouse_event(Utils.MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                    }
                }
            }
        }
    }
}
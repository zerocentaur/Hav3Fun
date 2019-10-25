using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Hav3Fun
{
    internal class Memory : WinAPI
    {
        internal static Process CurrentProcess;
        internal static int ProcessHandle;
        private static int m_iNumberOfBytesRead;
        private static int m_iNumberOfBytesWritten;
        private static int Timer = 60;
        const int AccessCodes = 0x10 | 0x20 | 0x8;

        internal static T Read<T>(int address) where T : struct
        {
            int ByteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[ByteSize];
            ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, ref m_iNumberOfBytesRead);
            return ByteArrayToStructure<T>(buffer);
        }

        public static string ReadTextFromProcess(int address, int size)
        {
            byte[] data;
            if (size != -1)
                data = ReadMemory(address, size);
            else
            {
                List<byte> name = new List<byte>();
                int offset = 0;
                byte curbyte;
                do
                {
                    curbyte = ReadMemory(address + offset, 1)[0];
                    name.Add(curbyte);
                    ++offset;
                } while (curbyte != 0x00);
                size = offset;
                data = name.ToArray();
            }
            Encoding encoding = Encoding.Default;
            string result = encoding.GetString(data, 0, size);
            result = result.Substring(0, result.IndexOf('\0'));
            return result;
        }

        internal static byte[] ReadMemory(int offset, int size)
        {
            byte[] buffer = new byte[size];
            ReadProcessMemory(ProcessHandle, (int)offset, buffer, size, ref m_iNumberOfBytesRead);
            return buffer;
        }

        internal static float[] ReadMatrix<T>(int address, int MatrixSize) where T : struct
        {
            int ByteSize = Marshal.SizeOf(typeof(T));
            byte[] buffer = new byte[ByteSize * MatrixSize];
            ReadProcessMemory(ProcessHandle, address, buffer, buffer.Length, ref m_iNumberOfBytesRead);
            return ConvertToFloatArray(buffer);
        }

        internal static void Write<T>(int address, object Value) where T : struct
        {
            byte[] buffer = StructureToByteArray(Value);
            WriteProcessMemory(ProcessHandle, address, buffer, buffer.Length, out m_iNumberOfBytesWritten);
        }

        private static T ByteArrayToStructure<T>(byte[] bytes) where T : struct
        {
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            try
            {
                return (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            }
            finally
            {
                handle.Free();
            }
        }

        internal static float[] ConvertToFloatArray(byte[] bytes)
        {
            if (bytes.Length % 4 != 0) throw new ArgumentException();
            float[] floats = new float[bytes.Length / 4];
            for (int i = 0; i < floats.Length; i++) floats[i] = BitConverter.ToSingle(bytes, i * 4);
            return floats;
        }

        private static byte[] StructureToByteArray(object obj)
        {
            int length = Marshal.SizeOf(obj);
            byte[] array = new byte[length];
            IntPtr pointer = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(obj, pointer, true);
            Marshal.Copy(pointer, array, 0, length);
            Marshal.FreeHGlobal(pointer);
            return array;
        }

        internal static void Attach()
        {
            try
            {
                CurrentProcess = Process.GetProcessesByName("csgo")[0];
                if (CurrentProcess.Id != 0)
                {
                    Console.Clear();
                    Console.WriteLine("CS was found!");
                    bool isAttached = false;
                    while (!isAttached)
                    {
                        if (Utils.IsWindowFocues(CurrentProcess))
                        {
                            Thread.Sleep(500);
                            foreach (ProcessModule module in CurrentProcess.Modules)
                                if (module.ModuleName == "client_panorama.dll")
                                {
                                    Structure.Base.Client = (int)module.BaseAddress;
                                    Structure.Base.ClientSize = module.ModuleMemorySize;
                                }
                                else if (module.ModuleName == "engine.dll")
                                {
                                    Structure.Base.Engine = (int)module.BaseAddress;
                                    Structure.Base.EngineSize = module.ModuleMemorySize;
                                }

                            if (Structure.Base.Client == 0 || Structure.Base.Engine == 0)
                            {
                                Attach();
                                return;
                            }

                            isAttached = true;
                        }
                    }
                    ProcessHandle = OpenProcess(AccessCodes, false, CurrentProcess.Id);
                    Console.Clear();
                    Console.WriteLine("Successfully attached to the game!");
                }
            }
            catch (Exception CurrentException)
            {
                if (CurrentException.HResult == -2146233080)
                {
                    Console.Clear();
                    Console.WriteLine($"Waiting for CS... Time left: {Timer--}");
                    Thread.Sleep(1000);

                    if (Timer == 0) Environment.Exit(0);

                    Attach();
                }
                else
                    Console.WriteLine($"Unknown error #{CurrentException.HResult}...");
            }
        }
    }
}
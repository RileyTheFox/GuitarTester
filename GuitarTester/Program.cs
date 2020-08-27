using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;

using SharpDX.DirectInput;


namespace GuitarTester
{
    class Program
    {

        private static long lastTick = System.Environment.TickCount;

        static void Main()
        {
            Config config;
            Console.CursorVisible = false;

            if (!File.Exists("config.json"))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("config.json not found, creating...");
                config = new Config();
                config.SaveToFile("config.json");
                Console.WriteLine("config.json created, edit controller_name field if it doesn't match the controller name on your computer");
                Console.WriteLine();
                Console.ResetColor();
                Thread.Sleep(1000);
            } 
            else
            {
                config = Config.LoadFromFile("config.json");
            }

            Joystick controller = null;

            using (DirectInput directInput = new DirectInput())
            {
                var devices = directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);

                bool found = false;

                foreach (var d in devices)
                {
                    if (d.InstanceName.Contains(config.ControllerName))
                    {
                        controller = new Joystick(directInput, d.InstanceGuid);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Couldn't find controller, is it connected?");
                    Thread.Sleep(2000);
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(1000);
                    Console.ResetColor();
                    Environment.Exit(0);
                } 

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Found controller ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(controller.Properties.InstanceName);
                Console.ForegroundColor = ConsoleColor.Green;

                Console.ResetColor();

                controller.Acquire();

                //controller.SetCooperativeLevel(IntPtr.Zero, CooperativeLevel.NonExclusive);

                DateTime lastWhammyTime = DateTime.Now;
                int lastWhammyValue = 0;

                List<double> times = new List<double>();

                double secondDelta = 0;

                Console.WriteLine();

                TEST:

                Console.WriteLine("Beginning the test in 3");
                Thread.Sleep(1000);
                Console.WriteLine("Beginning the test in 2");
                Thread.Sleep(1000);
                Console.WriteLine("Beginning the test in 1");
                Thread.Sleep(1000);
                Console.WriteLine("Start.\n");

                while (times.Count < 101)
                {
                    var currentTick = System.Environment.TickCount;
                    double delta = (currentTick - lastTick);
                    secondDelta += delta / 1000f;

                    lastTick = currentTick;

                    Console.WriteLine("Device changes counted: " + times.Count);

                    DateTime currentTime = DateTime.Now;

                    int z = controller.GetCurrentState().Z;

                    if(lastWhammyValue != controller.GetCurrentState().Z)
                    {
                        times.Add((currentTime - lastWhammyTime).TotalMilliseconds);

                        lastWhammyTime = currentTime;
                        lastWhammyValue = controller.GetCurrentState().Z;
                    }

                    Console.SetCursorPosition(0, 5);
                }

                Console.SetCursorPosition(0, 7);

                if (times.Count > 0)
                {
                    double average = 0;

                    foreach (double value in times)
                    {
                        average += value;
                    }

                    average /= times.Count;

                    Console.WriteLine($"Average time between axis changes per second: {average}ms");

                    times.Clear();
                }
                else
                {
                    Console.WriteLine($"No axis changes were found.");
                }

                Console.WriteLine("\nPress R to restart the test or close the application if finished");

                if(Console.ReadKey(true).Key == ConsoleKey.R)
                {
                    times.Clear();
                    lastWhammyTime = DateTime.Now;
                    lastWhammyValue = 0;

                    Console.SetCursorPosition(0, 2);

                    for(int i = 2; i < 20; i++)
                    {
                        Console.SetCursorPosition(0, i);

                        Console.Write(new string(' ', Console.WindowWidth));
                    }

                    Console.SetCursorPosition(0, 2);

                    goto TEST;
                }
            }
        }

    }
}

using System;
using System.IO;
using System.Threading;

using SharpDX.DirectInput;


namespace GuitarTester
{
    class Program
    {
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

                Console.WriteLine("Use some flat object to press all buttons at the same time. Start to Exit.");
                Console.WriteLine("Recommended object: Roll of wipers, and press it QUICKLY and release QUICKLY for best results");
                Console.WriteLine();
                Console.WriteLine();
                Console.ResetColor();

                controller.Acquire();

                //controller.SetCooperativeLevel(IntPtr.Zero, CooperativeLevel.NonExclusive);

                bool[] buttons;

                bool gp = false;
                bool rp = false;
                bool yp = false;
                bool bp = false;
                bool op = false;

                bool buttonsPressed = false;

                DateTime comp = DateTime.Now;
                DateTime g = DateTime.Now;
                DateTime r = DateTime.Now;
                DateTime y = DateTime.Now;
                DateTime b = DateTime.Now;
                DateTime o = DateTime.Now;

                bool first = true;

                while (true)
                {
                    buttons = controller.GetCurrentState().Buttons;

                    if (buttons[6])
                    {
                        Environment.Exit(0);
                    }

                    if (!buttonsPressed)
                    {
                        if (buttons[0] && !gp) // green
                        {
                            g = DateTime.Now;
                            gp = true;

                            if (first)
                            {
                                comp = g;
                                first = false;
                            }
                        }
                        if (buttons[1] && !rp) // red
                        {
                            r = DateTime.Now;
                            rp = true;

                            if (first)
                            {
                                comp = r;
                                first = false;
                            }
                        }
                        if (buttons[2] && !yp) // yellow
                        {
                            y = DateTime.Now;
                            yp = true;

                            if (first)
                            {
                                comp = y;
                                first = false;
                            }
                        }
                        if (buttons[3] && !bp) // blue
                        {
                            b = DateTime.Now;
                            bp = true;

                            if (first)
                            {
                                comp = b;
                                first = false;
                            }
                        }
                        if (buttons[4] && !op) // yellow
                        {
                            o = DateTime.Now;
                            op = true;

                            if (first)
                            {
                                comp = o;
                                first = false;
                            }
                        }

                        if (gp && rp && yp && bp && op)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("On:  " + (g - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("On:  " + (r - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("On:  " + (y - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("On:  " + (b - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("On:  " + (o - comp).TotalMilliseconds + "ms              ");

                            buttonsPressed = true;

                            gp = false;
                            rp = false;
                            yp = false;
                            bp = false;
                            op = false;

                            first = true;

                            Thread.Sleep(100);
                        }
                    } 
                    else
                    {
                        if (!buttons[0] && !gp) // green
                        {
                            g = DateTime.Now;
                            gp = true;

                            if (first)
                            {
                                comp = g;
                                first = false;
                            }
                        }
                        if (!buttons[1] && !rp) // red
                        {
                            r = DateTime.Now;
                            rp = true;

                            if (first)
                            {
                                comp = r;
                                first = false;
                            }
                        }
                        if (!buttons[2] && !yp) // yellow
                        {
                            y = DateTime.Now;
                            yp = true;

                            if (first)
                            {
                                comp = y;
                                first = false;
                            }
                        }
                        if (!buttons[3] && !bp) // blue
                        {
                            b = DateTime.Now;
                            bp = true;

                            if (first)
                            {
                                comp = b;
                                first = false;
                            }
                        }
                        if (!buttons[4] && !op) // yellow
                        {
                            o = DateTime.Now;
                            op = true;

                            if (first)
                            {
                                comp = o;
                                first = false;
                            }
                        }

                        if (gp && rp && yp && bp && op)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Off: " + (g - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Off: " + (r - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Off: " + (y - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine("Off: " + (b - comp).TotalMilliseconds + "ms              ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine("Off: " + (o - comp).TotalMilliseconds + "ms              ");

                            buttonsPressed = false;

                            gp = false;
                            rp = false;
                            yp = false;
                            bp = false;
                            op = false;

                            first = true;

                            Thread.Sleep(100);
                        }
                    }

                    Console.SetCursorPosition(0, 5);
                }
            }
        }
    }
}

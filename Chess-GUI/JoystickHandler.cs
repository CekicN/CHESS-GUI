using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Chess_GUI
{
    public class JoystickHandler
    {
        private DirectInput directInput;
        private Joystick joystick;
        private bool isRunning;
        private Rectangle rectangle;
        public int X { get; set; }
        public int Y { get; set; }

        private MainWindow _mainWindow;

        public JoystickHandler(MainWindow mainWindow)
        {
            isRunning = true;
            X = 0; Y = 0;
            _mainWindow = mainWindow;
        }

        public  async Task StartTracking()
        {
            await Task.Run(() =>
            {
                directInput = new DirectInput();

                var joystickGuid = Guid.Empty;

                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                            DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

                if (joystickGuid == Guid.Empty)
                    foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                            DeviceEnumerationFlags.AllDevices))
                        joystickGuid = deviceInstance.InstanceGuid;

                if (joystickGuid == Guid.Empty)
                {
                    Console.WriteLine("No joystick/Gamepad found.");
                    return;
                }

                var joystick = new Joystick(directInput, joystickGuid);
                var allEffects = joystick.GetEffects();
                foreach (var effectInfo in allEffects)
                    Console.WriteLine("Effect available {0}", effectInfo.Name);

                joystick.Properties.BufferSize = 128;

                joystick.Acquire();
                while (isRunning)
                {
                    joystick.Poll();
                    var datas = joystick.GetBufferedData();
                    foreach (var state in datas)
                    {

                        if (state.Offset == JoystickOffset.X)
                            if (state.Value > 32511 && X < 7)
                                X++;
                            else if (state.Value < 32511 && X > 0)
                                X--;
                        
                        if(state.Offset == JoystickOffset.Y)
                            if (state.Value > 32511 && Y < 7)
                                Y++;
                            else if (state.Value < 32511 && Y > 0)
                                Y--;

                        if(state.Offset == JoystickOffset.Buttons2)
                        {
                            _mainWindow.Dispatcher.InvokeAsync(() =>
                            {
                                _mainWindow.odigraj(new Position(Y, X));
                            });
                        }
                        Application.Current.Dispatcher.InvokeAsync(() =>
                        {
                            DrawRectangle();
                            
                        });
                    }

                    Task.Delay(50).Wait();
                }
            });
        }
        private void DrawRectangle()
        {
            if (rectangle != null)
            {
                _mainWindow.canvas.Children.Remove(rectangle);
            }

            int squareSize = 100;
            int startX = X * squareSize;
            int startY = Y * squareSize;

            rectangle = new Rectangle
            {
                Width = squareSize,
                Height = squareSize,
                Stroke = Brushes.Red,
                StrokeThickness = 4,
                Fill = Brushes.Transparent 
            };

            Canvas.SetLeft(rectangle, startX);
            Canvas.SetTop(rectangle, startY);

            _mainWindow.canvas.Children.Add(rectangle);
        }

        public void StopTracking()
        {
            isRunning = false;
        }
    }
}

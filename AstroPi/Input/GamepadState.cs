using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace AstroPi.Input
{
    public class GamepadState
    {
        public bool Connected { get; set; }
        public GamepadReading Reading { get; set; }
        public string StateLog { get; set; }

        public string StatusText
        {
            get
            {
                return Connected ? "Gamepad connected!" : "No gamepad connected!";
            }
        }

        public string LeftJoystickCoordinates
        {
            get
            {
                if(Connected)
                {
                    return GetCoordinates(Reading.LeftThumbstickX, Reading.LeftThumbstickY);
                }

                return "0, 0";
            }
        }

        public string RightJoystickCoordinates
        {
            get
            {
                if (Connected)
                {
                    return GetCoordinates(Reading.RightThumbstickX, Reading.RightThumbstickY);
                }

                return "0, 0";
            }
        }

        private string GetCoordinates(double x, double y)
        {
            x = Math.Round(x, 1);
            y = Math.Round(y, 1);

            return $"{x}, {y}";
        }
    }
}

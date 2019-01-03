using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace AstroPi
{
    public class GamepadManager
    {
        public delegate void InputRecievedEvent(GamepadState gamepadState);

        private Gamepad _gamepad;
        private InputRecievedEvent _inputRecievedEvent;

        public GamepadManager(InputRecievedEvent inputRecievedEvent)
        {
            _inputRecievedEvent = inputRecievedEvent;

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            Task.Factory.StartNew(() => ReadInput());
        }

        private async void ReadInput()
        {
            while (true)
            {
                await Task.Delay(32);

                _inputRecievedEvent.Invoke(GetState());
            }
        }

        private GamepadState GetState()
        {
            if (_gamepad is null)
            {
                return new GamepadState
                {
                    Connected = false
                };
            }

            var reading = _gamepad.GetCurrentReading();

            var stateLog = GetStateLog(reading);

            return new GamepadState
            {
                Connected = true,
                Reading = reading,
                StateLog = stateLog
            };
        }

        private string GetStateLog(GamepadReading reading)
        {
            var stateLog = $"Left Joystick: {reading.LeftThumbstickX.ToString()}, {reading.LeftThumbstickY}\n";

            stateLog += $"Right Joystick: {reading.RightThumbstickX}, {reading.RightThumbstickY}\n";
            stateLog += $"Left Trigger: {reading.LeftTrigger}\n";
            stateLog += $"Right Trigger: {reading.RightTrigger}\n";
            stateLog += $"Buttons: {reading.Buttons}";

            return stateLog;
        }

        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            _gamepad = null;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _gamepad = e;
        }
    }
}

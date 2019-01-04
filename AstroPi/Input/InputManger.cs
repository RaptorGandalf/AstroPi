using AstroPi.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Gaming.Input;

namespace AstroPi.Input
{
    public class InputManger
    {
        public delegate void InputRecievedEvent(GamepadState gamepadState);

        public InputRecievedEvent OnInputRecieved { get; set; }

        private Gamepad _gamepad;

        private AudioManager _audioManager;

        public InputManger()
        {
            _audioManager = new AudioManager();

            _audioManager.Configure().Wait();

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;

            Task.Factory.StartNew(() => ReadInput());
        }
        
        private async void ReadInput()
        {
            while (true)
            {
                await Task.Delay(32);

                var state = GetState();

                OnInputRecieved?.Invoke(state);

                var reading = state.Reading;

                ProcessThumbsticks(reading.LeftThumbstickX, reading.LeftThumbstickY, reading.RightThumbstickX, reading.RightThumbstickY);

                await ProcessButtons(reading.Buttons);
            }
        }

        private void ProcessThumbsticks(double leftX, double leftY, double rightX, double rightY)
        {

        }

        #region button handlers
        private async Task ProcessButtons(GamepadButtons buttons)
        {
            switch (buttons)
            {
                case GamepadButtons.A: await OnAButton(); break;
                case GamepadButtons.B: await OnBButton(); break;
                case GamepadButtons.X: OnXButton(); break;
                case GamepadButtons.Y: OnYButton(); break;
                case GamepadButtons.LeftShoulder: OnLeftShoulderButton(); break;
                case GamepadButtons.RightShoulder: OnRightShoulderButton(); break;
                case GamepadButtons.DPadUp: OnDPadUpButton(); break;
                case GamepadButtons.DPadDown: OnDPadDownButton(); break;
                case GamepadButtons.DPadLeft: OnDPadLeftButton(); break;
                case GamepadButtons.DPadRight: OnDPadRightButton(); break;
                case GamepadButtons.Menu: OnMenuButton(); break;
                case GamepadButtons.LeftThumbstick: OnLeftThumbstickButton(); break;
                case GamepadButtons.RightThumbstick: OnRightThumbstickButton(); break;
            }
        }

        private async Task OnAButton()
        {
            await _audioManager.PlayRandomAstromechSound();
        }

        private async Task OnBButton()
        {
            await _audioManager.PlayCantinaSong();
        }

        private void OnXButton()
        {

        }

        private void OnYButton()
        {

        }

        private void OnLeftShoulderButton()
        {

        }

        private void OnRightShoulderButton()
        {

        }

        private void OnDPadUpButton()
        {

        }

        private void OnDPadDownButton()
        {

        }
        private void OnDPadLeftButton()
        {

        }

        private void OnDPadRightButton()
        {

        }

        private void OnMenuButton()
        {

        }

        private void OnLeftThumbstickButton()
        {

        }

        private void OnRightThumbstickButton()
        {

        }

        #endregion

        #region state
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
        #endregion

        #region gamepad events
        private void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            _gamepad = null;
        }

        private void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            _gamepad = e;
        }
        #endregion
    }
}

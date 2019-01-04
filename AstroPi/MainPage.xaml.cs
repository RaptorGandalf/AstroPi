using AstroPi.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace AstroPi
{
    public sealed partial class MainPage : Page
    {
        public GamepadState GamepadState;

        private InputManger _inputManager;

        public MainPage()
        {
            this.InitializeComponent();

            GamepadState = new GamepadState
            {
                Connected = false
            };

            _inputManager = new InputManger();

            _inputManager.OnInputRecieved = InputRecieved;
        }

        private async void InputRecieved(GamepadState state)
        {
            GamepadState = state;

            await UpdateInputUI();
        }

        private async Task UpdateInputUI()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                GamepadStatusText.Text = GamepadState.StatusText;
                LtsText.Text = GamepadState.LeftJoystickCoordinates;
                RtsText.Text = GamepadState.RightJoystickCoordinates;
                ButtonsText.Text = GamepadState.Reading.Buttons.ToString();
            });
        }
    }
}

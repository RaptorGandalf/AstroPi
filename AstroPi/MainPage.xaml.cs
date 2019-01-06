using AstroPi.Input;
using AstroPi.Logging;
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
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
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

            Logger.LogEvent = ConsoleLog;

            GamepadState = new GamepadState
            {
                Connected = false
            };

            Task.Factory.StartNew(() => Setup());
        }

        private void Setup()
        {
            Logger.Log("Welcome to AstroPi!");


            Logger.Log("Configuring input manager...");

            _inputManager = new InputManger
            {
                OnInputRecieved = InputRecieved
            };

            Logger.Log("Done.");

            Logger.Log("Setup complete!");
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

        private async void ConsoleLog(string text)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                ConsoleBox.IsReadOnly = false;

                ConsoleBox.Document.GetText(TextGetOptions.None, out string consoleText);

                var lines = consoleText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                if(lines.Length > 100)
                {
                    lines = lines.Reverse().Take(100).Reverse().ToArray();

                    consoleText = "";

                    foreach(var line in lines)
                    {
                        consoleText += $"\n{line}"; 
                    }
                }

                consoleText += $"\n{text}";

                ConsoleBox.Document.SetText(TextSetOptions.None, consoleText);

                ConsoleBox.IsReadOnly = true;
            });
        }
    }
}

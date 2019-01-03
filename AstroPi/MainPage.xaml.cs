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

        private GamepadManager _gamepadManager;

        public MainPage()
        {
            this.InitializeComponent();

            _gamepadManager = new GamepadManager(InputRecieved);

            GamepadState = new GamepadState
            {
                Connected = false
            };

        }

        private async void InputRecieved(GamepadState state)
        {
            GamepadState = state;

            //just putting this here to try audio
            if(state.Reading.Buttons == Windows.Gaming.Input.GamepadButtons.A)
            {
                var soundDirectory = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\Audio\Astromech-Sounds");

                var files = await soundDirectory.GetFilesAsync();

                var generator = new Random();

                var fileIndex = generator.Next(0, files.Count - 1);

                var file = files[fileIndex];

                var mediaPlayer = new MediaPlayer
                {
                    Source = MediaSource.CreateFromStorageFile(file),
                };

                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //await Task.Delay(1);
                    mediaPlayer.Play();

                  //  Task.Delay(5000);
                });

                //mediaPlayer.Dispose();
            }

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

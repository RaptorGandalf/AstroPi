using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;

namespace AstroPi.Audio
{
    public class AudioManager
    {
        private readonly string ASTROMECH_SOUNDS = @"Assets\Audio\Astromech-Sounds";
        private readonly string SONGS = @"Assets\Audio\Songs";
        private readonly string CANTINA_SONG = "CANTINA.wav";

        private MediaPlayer _mediaPlayer;

        private StorageFolder _astromechSoundFolder;
        private StorageFolder _songFolder;

        private List<string> _astromechSoundFiles;

        public AudioManager()
        {
            _mediaPlayer = new MediaPlayer();
        }

        ~AudioManager()
        {
            _mediaPlayer.Dispose();
        }

        public async Task Configure()
        {
            _astromechSoundFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(ASTROMECH_SOUNDS);
            _songFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(SONGS);

            _astromechSoundFiles = new List<string>();

            var astromechFiles = await _astromechSoundFolder.GetFilesAsync();

            foreach(var file in astromechFiles)
            {
                _astromechSoundFiles.Add(file.Name);
            }
        }

        public async Task PlayRandomAstromechSound()
        {
            var generator = new Random();

            var fileIndex = generator.Next(0, _astromechSoundFiles.Count - 1);

            var file = await _astromechSoundFolder.GetFileAsync(_astromechSoundFiles[fileIndex]);

            PlayFile(file);
        }

        public async Task PlayCantinaSong()
        {
            var file = await _songFolder.GetFileAsync(CANTINA_SONG);

            PlayFile(file);
        }

        private void PlayFile(StorageFile file)
        {
            _mediaPlayer.Source = MediaSource.CreateFromStorageFile(file);

            _mediaPlayer.Play();
        }
    }
}

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
        }

        public async Task PlayRandomAstromechSound()
        {
            var files = await _astromechSoundFolder.GetFilesAsync();

            var generator = new Random();

            var fileIndex = generator.Next(0, files.Count - 1);
            
            PlayFile(files[fileIndex]);
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

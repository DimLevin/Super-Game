using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGame
{
    class Music : Sound
    {
        // for singleton
        private static Music _music = null;

        // files
        private string _musicPath = @"Media\Music\";
        public enum MusicLibrary : byte { intro, end, win, pause, level1, level2, boss1, boss2 };
        private string[] _tracks;

        //for state save/recover
        private string _currentFile;
        private double _currentPosition;
        private bool _isPauseMode = false;

        // CONSTR.
        private Music()
        {
            this._mplayer.settings.setMode("loop", true);      // set to play in loop
            _GenerateMusicFilePaths();  // generate music files paths

            // set flag
            _isPauseMode = false;
        }

        // Singleton initiator
        public static Music Init()
        {
            if (Music._music == null)
            {
                Music._music = new Music();
            }

            return Music._music;
        }

        // METHODS

        // Generate music file paths
        private void _GenerateMusicFilePaths()
        {
            // enum to filenames
            string[] tracks = Enum.GetNames(typeof(MusicLibrary));

            int total_size = tracks.Length;

            string[] tmp_list = new string[total_size];

            // save media elements
            for (int i = 0; i < total_size; i++)
            {
                tmp_list[i] = _musicPath + tracks[i] + Sound._fileType;
            }

            _tracks = tmp_list; // save to array
        }

        #region Play Control
        // Play music
        public void Play(MusicLibrary track)
        {
            //statuses: Undefined, Stopped, Paused, Playing

            // if player is not playing or the track is different
            if (!this._mplayer.status.Contains("Playing") || !_mplayer.URL.Equals(System.Environment.CurrentDirectory + @"\" + _tracks[(int)track]))
            {
                this._mplayer.URL = _tracks[(int)track];    // set track
            }
        }

        // Pause mode
        public void PauseModeToggle()
        {
            // pause mode
            if (!_isPauseMode)
            {
                // save state
                _currentFile = this._mplayer.URL;
                _currentPosition = this._mplayer.controls.currentPosition;

                // load pause mode track
                this._mplayer.URL = _tracks[(int)MusicLibrary.pause];

                // change flag
                _isPauseMode = true;           
            }
            else
            {
                //recover state
                this._mplayer.URL = _currentFile;
                this._mplayer.controls.pause();
                this._mplayer.controls.currentPosition = _currentPosition;
                this._mplayer.controls.play();
               
                // change flag
                _isPauseMode = false;
            }
        }
        #endregion
    }
}

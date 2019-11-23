using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//for audio
using WMPLib;

namespace SuperGame
{
    abstract class Sound
    {
        protected WMPLib.WindowsMediaPlayer _mplayer;
        protected static string _fileType = ".mp3";

        // function pointers
        public delegate void FuncPointer();
        public FuncPointer UpdateVolumeBar;

        //settings
        private int _volume, _volumeStep = 5;

        // Constr.
        public Sound()
        {
            _mplayer = new WMPLib.WindowsMediaPlayer();

            //set volume
            _volume = 50;
            _mplayer.settings.volume = _volume;
        }


        // PROPERTIES
        public int CurrentVolume
        {
            get => _mplayer.settings.volume;

            set
            {
                if (value >= 0 && value <=100)
                {
                    _volume = value;
                    _mplayer.settings.volume = value;
                }
                else if (value < 0)
                {
                    _volume = 0;
                    _mplayer.settings.volume = 0;
                }
                else
                {
                    _volume = 100;
                    _mplayer.settings.volume = 100;
                }

                // updade volume bar
                _UpadeVolBar();
            }
        }


        // METHODS


        // Stop playing
        public void Stop()
        {
            _mplayer.controls.stop();
        }

        // Pause Toggle
        public void PauseToggle()
        {
            if (_mplayer.status.Equals("Paused"))
            {
                _mplayer.controls.play();
            }
            else
            {
                _mplayer.controls.pause();
            }
            
        }

        // Update the volume bar
        protected void _UpadeVolBar()
        {
            if (UpdateVolumeBar != null)
            {
                // updade volume bar
                UpdateVolumeBar();
            }
        }

        #region Volume Control
        // Volume Up
        public void VolumeUp()
        {
            if (_volume < 100)
            {
                _volume += _volumeStep;

                if (_volume > 100)
                {
                    _volume = 100;
                }

                // set the new volume
                _mplayer.settings.volume = _volume;

                // updade volume bar
                _UpadeVolBar();
            }
        }

        // Volume Down
        public void VolumeDown()
        {
            if (_volume > 0)
            {
                _volume -= _volumeStep;

                if (_volume < 0)
                {
                    _volume = 0;
                }

                //set the new volume
                _mplayer.settings.volume = _volume;

                // updade volume bar
                _UpadeVolBar();
            }
        }

        // Volume Mute
        public void VolumeMute()
        {
            if (_mplayer.settings.volume != 0)
            {
                _mplayer.settings.volume = 0;
            }
            else
            {
                _mplayer.settings.volume = _volume;
            }

            // updade volume bar
            _UpadeVolBar();
        }
        #endregion
    }
}

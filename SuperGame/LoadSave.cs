using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGame
{
    // Load and save functionality
    class LoadSave
    {
        // data save format: "mVol 50 sVol 50"

        // files
        private static string _settingsFile = "settings.ini";
        private static string[] _saveSlot = { "slot1.sav", "slot2.sav", "slot3.sav", "slot4.sav" };

        // media instances
        private Music _music;
        private SfxSound _sound;
        //private GameMenu _gameMenu;

        // for singleton
        private static LoadSave _instance = null;

        // settings
        private int _musicVolume;
        private int _soundVolume;

        // level
        private int _curLevel;
        private int _lives;


        // CONSTR.
        private LoadSave()
        {
            // default levels
            _musicVolume = 50;
            _soundVolume = 50;

            // get media instances
            _music = Music.Init();
            _sound = SfxSound.Init();
            
            // load settings from a file
            LoadSettings();
        }

        // Singleton initiator
        public static LoadSave Init()
        {
            if (LoadSave._instance == null)
            {
                LoadSave._instance = new LoadSave();
            }

            return LoadSave._instance;
        }


        //METHODS

        // Load settings
        public void LoadSettings()
        {
            string res = string.Empty;

            res = FileHandler.ReadFromFile(_settingsFile);

            if (res.Length > 0)
            {
                // retrieve values
                string[] rawData = res.Split();

                // music volume
                int.TryParse(rawData[1], out _musicVolume);

                // sound volume
                int.TryParse(rawData[3], out _soundVolume);

                // update settings
                _UpdateSettings();
            }
        }

        // Update settings
        private void _UpdateSettings()
        {
            _music.CurrentVolume = _musicVolume;
            _sound.CurrentVolume = _soundVolume;
        }

        // Save settings
        public void SaveSettings()
        {
            // data to write
            string dataToWrite = "mVol " + _music.CurrentVolume.ToString() + " sVol " + _sound.CurrentVolume.ToString();

            FileHandler.WriteToFile(_settingsFile, dataToWrite);
        }

    }
}

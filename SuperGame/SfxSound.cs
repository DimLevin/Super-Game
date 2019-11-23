using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGame
{
    class SfxSound : Sound
    {
        // for singleton
        private static SfxSound _sfxSound = null;

        // files
        private static string _libPath = @"Media\Sounds\";

        // sound types
        public enum SoundTypes { menu, effect };
        /*
        // characters sounds
        public enum CharacterSounds : byte { shoot, die, reload_shoot2, jump, fall, getItem };
        private string[] _enemy1Sounds;
        private string[] _enemy2Sounds;
        private string[] _enemy3Sounds;
        private string[] _boss1Sounds;
        private string[] _boss2Sounds;
        private string[] _playerSounds;
        */
        // j.array of all sounds
        private string[][] _sounds;

        // other sounds
        public enum MenuSounds : byte { choose, change, show_menu, intro_effect, test_sound };
        private string[] _menuSounds;

        public enum EffectSounds : byte { swtch, door1_opens, door1_closes, door_closed, door2_opens,
                                            door2_closes, terminal, port_in, port_out, armor_up, health_up };
        //private string[] _effectSounds;

        // CONSTR.
        private SfxSound()
        {
            // set j.array size
            _sounds = new string[2][];

            _GenerateMenuSoundsPaths(); // generate menu sounds files paths
        }

        // Singleton initiator
        public static SfxSound Init()
        {
            if (SfxSound._sfxSound == null)
            {
                SfxSound._sfxSound = new SfxSound();
            }

            return SfxSound._sfxSound;
        }

        // METHODS

        // Generate menu sounds filenames paths
        private void _GenerateMenuSoundsPaths()
        {
            // enum to filenames
            string[] sounds = Enum.GetNames(typeof(MenuSounds));

            int total_size = sounds.Length;

            string[] tmp_list = new string[total_size];

            // save media elements
            for (int i = 0; i < total_size; i++)
            {
                tmp_list[i] = SfxSound._libPath + @"Menu\" + sounds[i] + Sound._fileType;
            }

            _menuSounds = tmp_list;                         // save to array
            _sounds[(int)SoundTypes.menu] = _menuSounds;    // j.array of all sounds
        }

        // Play sound -in library
        public void PlaySound(SoundTypes type, Enum index)
        {
            this._mplayer.controls.stop();
            this._mplayer.URL = _sounds[(int)type][Convert.ToInt32(index)];
        }

        // Play sound -by path
        public void PlaySound(string path)
        {
            // path not empty
            if (!path.Equals(string.Empty))
            {
                this._mplayer.controls.stop();
                this._mplayer.URL = path;
            }
        }
    }
}

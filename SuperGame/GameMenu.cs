using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperGame
{
    // Game menu
    class GameMenu
    {
        // for singleton
        private static GameMenu _instance = null;

        // forms
        private Form _mainForm;
        private Form _levelForm;

        // components
        private Music _music;
        private SfxSound _sound;
        private Pointer _pointer;
        private LoadSave _loadSave;
        private Level _level;

        // menu elements
        private PictureBox _pBoxMenuOptions;
        private PictureBox _pBoxLoadSave;
        private PictureBox _pBoxSettings;
        private PictureBox _pBoxExitPrompt;

        // labels
        private Label _labelMusicVol;
        private Label _labelSoundVol;

        // in game menu elements
        private PictureBox _pBoxInGameMenu;
        private PictureBox _pBoxPaused;

        // flags
        private bool _inGame;

        private string[] _currentOptions;
        private PictureBox _pBoxCurrent;
        //private PictureBox _pBoxPrevious;

        // function pointers
        private delegate void FuncPointer();
        FuncPointer[] _menuFunctions;

        // CONSTR.
        private GameMenu(PictureBox pointerPbox)
        {
            // init. components
            _music = Music.Init();
            _sound = SfxSound.Init();
            _pointer = Pointer.Init(pointerPbox);
            _loadSave = LoadSave.Init();

            // max. possible amount of options in the menu
            _menuFunctions = new FuncPointer[5];

            // updade volume bars pointer
            _music.UpdateVolumeBar = UpdateMusicVolumeLevel;
            _sound.UpdateVolumeBar = UpdateSoundVolumeLevel;

            // set flags
            _inGame = false;
        }

        // Singleton initiator
        public static GameMenu Init(PictureBox pointerPbox)
        {
            if (GameMenu._instance == null)
            {
                GameMenu._instance = new GameMenu(pointerPbox);
            }

            return GameMenu._instance;
        }

        public static GameMenu Init()
        {
            return GameMenu._instance;
        }


        // PROPERTIES

        // picture boxes
        public PictureBox PbMenuOptions { set => _pBoxMenuOptions = value; }
        public PictureBox PbInGameMenu { set => _pBoxInGameMenu = value; }
        public PictureBox PbLoadSave { set => _pBoxLoadSave = value; }
        public PictureBox PbSettings { set => _pBoxSettings = value; }
        public PictureBox PbExitPrompt { set => _pBoxExitPrompt = value; }
        public PictureBox PbPaused { set => _pBoxPaused = value; }

        // labels
        public Label LbMusicVol { set => _labelMusicVol = value; }
        public Label LbSoundVol { set => _labelSoundVol = value; }

        // forms
        public Form MainForm { set => _mainForm = value; }
        public Form LevelForm { set => _levelForm = value; }

        // METHODS

        #region Keyboard inputs
        // Key is pressed
        public void KeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Up:
                    _MoveUp();
                    break;
                case Keys.Down:
                    _MoveDown();
                    break;
                case Keys.Left:
                    _LeftArrow();
                    break;
                case Keys.Right:
                    _RightArrow();
                    break;
                case Keys.Escape:
                    _EscapeKey();
                    break;
                case Keys.Space:
                    _ChooseOption();
                    break;
                case Keys.Enter:
                    _ChooseOption();
                    break;
            }
        }

        // Move up
        private void _MoveUp()
        {
            _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.change);
            _pointer.MoveUp();
        }

        // Move down
        private void _MoveDown()
        {
            _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.change);
            _pointer.MoveDown();
        }

        // Left arrow
        private void _LeftArrow()
        {
            // if current menu is settings
            if (_pBoxCurrent == _pBoxSettings)
            {
                if (_pointer.Position == 1)
                {
                    _music.VolumeDown();
                }
                else if (_pointer.Position == 2)
                {
                    _sound.VolumeDown();
                }

            }
        }

        // Right arrow
        private void _RightArrow()
        {
            // if current menu is settings
            if (_pBoxCurrent == _pBoxSettings)
            {
                if (_pointer.Position == 1)
                {
                    _music.VolumeUp();
                }
                else if (_pointer.Position == 2)
                {
                    _sound.VolumeUp();
                }
            }
        }

        // Escape key
        private void _EscapeKey()
        {
            // in game
            if (_inGame)
            {
                _BackToGame();
            }
        }
        #endregion

        #region Sound
        // Volume control
        private void _VolumeControl()
        {
            // music level
            if (_pointer.Position == 1)
            {
                _music.VolumeMute();
            }
            // sound level
            else if (_pointer.Position == 2)
            {
                _sound.VolumeMute();
            }

            _Settings();
        }

        // Update music volume level
        public void UpdateMusicVolumeLevel()
        {
            // set value
            if (_music.CurrentVolume > 0)
            {
                _labelMusicVol.Text = _music.CurrentVolume.ToString();
            }
            else
            {
                _labelMusicVol.Text = "muted";
            }
        }

        // Update sound volume levels
        public void UpdateSoundVolumeLevel()
        {
            // play sound if bar is visible
            if (_labelSoundVol.Visible)
            {
                _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.test_sound);
            }

            // set value
            if (_sound.CurrentVolume > 0)
            {
                _labelSoundVol.Text = _sound.CurrentVolume.ToString();
            }
            else
            {
                _labelSoundVol.Text = "muted";
            }
        }
        #endregion

        #region Menu (Main and Game)
        // Main menu
        private void _MainMenu()
        {
            // functions pointers
            _menuFunctions[0] = _NewGame;      // start a new game
            //_menuFunctions[1] = LoadGame;     // load a saved game 
            _menuFunctions[2] = _Settings;       // show settings
            //_menuFunctions[3] = About;        // show about
            _menuFunctions[4] = _ExitPrompt;         // exit the game

            // set current
            _pBoxCurrent = _pBoxMenuOptions;
            _pBoxCurrent.Visible = true;

            // menu open sound
            _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.show_menu);

            //show options
            _pBoxCurrent.Visible = true;

            // pointer setup
            _pointer.LeftInitVal = _pBoxCurrent.Left - _pointer.Width - 20;
            _pointer.TopInitVal = _pBoxCurrent.Top + 47;
            _pointer.Offset = 62;
            _pointer.TotalElements = 5;
            _pointer.Visible = true;

            // play music theme
            _music.Play(Music.MusicLibrary.intro);
        }

        // Game menu
        private void _GameMenu()
        {
            // functions pointers
            _menuFunctions[0] = _NewGame;      // start a new game
            //_menuFunctions[1] = SaveGame;    // save a game 
            _menuFunctions[2] = _Settings;     // show settings
            _menuFunctions[3] = _ExitPrompt;   // exit the current game

            // set current
            _pBoxCurrent = _pBoxInGameMenu;
            _pBoxCurrent.Visible = true;

            // menu open sound
            _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.show_menu);

            //show options
            _pBoxCurrent.Visible = true;

            // pointer setup
            _pointer.LeftInitVal = _pBoxCurrent.Left - _pointer.Width - 20;
            _pointer.TopInitVal = _pBoxCurrent.Top + 80;
            _pointer.Offset = 63;
            _pointer.TotalElements = 4;
            _pointer.Visible = true;
        }
        #endregion

        #region Menus items
        // New game
        private void _NewGame()
        {
            // stop music
            _music.Stop();

            // hide main form
            _mainForm.Visible = false;

            _inGame = true;     // set flag

            // init level
            _level = Level.Init();
            _level.LoadLevel(1, true);
        }

        // Settings
        private void _Settings()
        {
            // if current menu is not settings
            if (_pBoxCurrent != _pBoxSettings)
            {
                // functions pointers
                _menuFunctions[0] = _VolumeControl;  // mute music
                _menuFunctions[1] = _VolumeControl;  // mute sound
                _menuFunctions[2] = _Back;           // back to the previous menu

                // pointer setup
                _pointer.LeftInitVal = 32;
                _pointer.TopInitVal = _pBoxCurrent.Top + 88;
                _pointer.Offset = 80;
                _pointer.TotalElements = 3;
            }

            // show element
            _pBoxCurrent = _pBoxSettings;
            _pBoxCurrent.Visible = true;

            _pointer.Visible = true;

            // set labels
            UpdateMusicVolumeLevel();
            UpdateSoundVolumeLevel();
            _labelMusicVol.Visible = true;
            _labelSoundVol.Visible = true;
        }

        // Back
        private void _Back()
        {
            ShowMenu();
        }

        // Exit prompt
        private void _ExitPrompt()
        {
            // functions pointers
            _menuFunctions[0] = _Exit;   // exit the game
            _menuFunctions[1] = _Back;   // back to the previous menu

            // show element
            _pBoxCurrent = _pBoxExitPrompt;
            _pBoxCurrent.Visible = true;

            // pointer setup
            _pointer.LeftInitVal = _pBoxCurrent.Left + 170;
            _pointer.TopInitVal = _pBoxCurrent.Top + 65;
            _pointer.Offset = 60;
            _pointer.TotalElements = 2;
            _pointer.Visible = true;
        }

        // Exit
        private void _Exit()
        {
            if (_inGame)
            {
                _ExitCurrentGame();
            }
            else
            {
                // save current settings
                _loadSave.SaveSettings();

                // exit
                System.Windows.Forms.Application.Exit();
            }
        }
        #endregion

        // Launch menu
        public void ShowMenu()
        {
            // in game menu
            if (_inGame)
            {
                _levelForm.Visible = false;     // hide game form
                _mainForm.Visible = true;       // show menu form
                _GameMenu();
            }
            else
            {
                _MainMenu();
            }
        }

        // Back to game
        private void _BackToGame()
        {
            _mainForm.Visible = false;  // hide menu form
            _levelForm.Visible = true;  // show game form
            _level.PauseGameToggle();
        }

        // Choose option
        private void _ChooseOption()
        {
            // play sound
            _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.choose);

            // hide elements
            _pBoxCurrent.Visible = false;
            _pointer.Visible = false;

            // hide labels
            _labelMusicVol.Visible = false;
            _labelSoundVol.Visible = false;

            // execute func.
            _menuFunctions[_pointer.Position - 1]();
        }

        // Exit current game
        private void _ExitCurrentGame()
        {
            _level.Close();
            _inGame = false;
            ShowMenu();
        }
    }
}

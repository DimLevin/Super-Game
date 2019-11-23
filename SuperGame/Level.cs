using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SuperGame
{
    class Level
    {

        /* Level data */
        private Music.MusicLibrary _musicTrack;
        private int _levelNumber;
        private bool _directionIsRight;

        // timers
        private Timer _gameTimer;

        // for singleton
        private static Level _instance = null;

        // picture boxes
        private PictureBox _pBoxLevelBackground;
        private PictureBox _pBoxLevelTile;

        // labels
        private Label _inGameInfoLbl;

        // characters
        private Character[] _enemies;
        private Player _player;

        // media instances
        private Music _music;
        private SfxSound _sound;

        // components
        private Form _currentLevel;
        private GameMenu _gameMenu;

        // flags
        private bool _isPaused;
        //private bool _levelInitialized;


        // CONSTR.
        private Level()
        {
            // get media instances
            _music = Music.Init();
            _sound = SfxSound.Init();

            _gameMenu = GameMenu.Init();

            // set flags
            _isPaused = false;
            //_levelInitialized = false;

            // set default level number
            //_levelNumber = 1;




        }

        // Singleton initiator
        public static Level Init()
        {
            if (Level._instance == null)
            {
                Level._instance = new Level();
            }

            return Level._instance;
        }

        // PROPERTIES
        public Label lblInGameInfo { set => _inGameInfoLbl = value; }
        public PictureBox PbLevelBackground { set => _pBoxLevelBackground = value; }
        public PictureBox PbLevelTile { set => _pBoxLevelTile = value; }
        public bool Pause { get => _isPaused; }
        //public bool Initialized { set => _levelInitialized = value; get => _levelInitialized; }
        public bool DirectionIsRight { set => _directionIsRight = value; }
        public Music.MusicLibrary MusicTrack { set => _musicTrack = value; }
        public Timer GameTimer { set => _gameTimer = value; }
        public Player PlayerCharacter { set => _player = value; }
        public Enemie[] Enemies { set => _enemies = value; }


        // METHODS

        #region Keyboard inputs
        // Key is pressed
        public void KeyPressed(Keys key)
        {
            // game mode
            if (!_isPaused)
            {
                // pass key to the player class
                _player.KeyDown(key);
            }

            // P button -pause
            if (key == Keys.P)
            {
                PauseGameToggle();
            }
            // Escape button
            else if (key == Keys.Escape)
            {
                if (_isPaused)
                {
                    PauseGameToggle();
                }
                else
                {
                    PauseGameToggle();         // toggle pause
                    _gameMenu.ShowMenu();       // show menu
                }
            }

        }

        // Key is released
        public void KeyUp(Keys key)
        {
            // game mode
            if (!_isPaused)
            {
                // pass key to the player class
                _player.KeyUp(key);
            }
        }
        #endregion

        // Load level
        public void LoadLevel(int levelNum, bool defaultSettings)
        {
            // dispose current level
            _currentLevel?.Dispose();

            // set level number
            _levelNumber = levelNum;

            // create new level
            _currentLevel = _CreateLevelForm(levelNum);

            // set form to game menu
            _gameMenu.LevelForm = _currentLevel;

            // play music
            _music.Play(_musicTrack);

            // show the form
            _currentLevel.ShowDialog();

            // add form to collision class
            CollisionSystem.form = _currentLevel;
        }

        // Create new level
        private static Form _CreateLevelForm(int level)
        {
            Form frm = null;

            switch (level)
            {
                case 1:
                    frm = new FormLevel1();
                    break;
                case 2:
                    frm = new FormLevel2();
                    break;
            }

            return (Form)frm;
        }

        // Close level -resume
        public void Close()
        {
            _currentLevel.Close();      // close current form
        }

        // Pause the game
        public void PauseGameToggle()
        {
            if (_isPaused)
            {
                _gameTimer.Enabled = true;      // start timer
                _inGameInfoLbl.Visible = false;    // in game label
                _isPaused = false;              // change flag
            }
            else
            {
                _gameTimer.Enabled = false;     // stop timer
                _isPaused = true;               // change flag

                // In game label
                ShowOnScreen("PAUSED");
            }
            
            // sound and music pause toggle
            _sound.PauseToggle();
            _music.PauseModeToggle();
        }

        // Timer tick
        public void TimerTick()
        {
            _player.MyCharacter.Update();
        }

        // Center control on screen
        private void _CenterControl(Control control)
        {
            // calc. the center of screen (x axis)
            int center = _currentLevel.Width / 2 - (_pBoxLevelBackground.Left + _pBoxLevelTile.Left);
            int halfControl = control.Width / 2;

            // set new x axis to control
            control.Left = center - halfControl;
        }

        // Show message on screen
        public void ShowOnScreen(string message)
        {
            _inGameInfoLbl.Text = message;     // set text
            _CenterControl(_inGameInfoLbl);    // center on screen
            _inGameInfoLbl.Visible = true;     // make visible
        }



    }
}

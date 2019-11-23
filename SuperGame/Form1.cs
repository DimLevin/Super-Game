using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperGame
{
    public partial class FrmMainMenu : Form
    {
        private Music _music;
        private SfxSound _sound;
        private GameMenu _gameMenu;

        // Intro
        PictureBox[] _bullets;   // bullets pBoxes
        private int _counter;
        private bool _introActive;

        // Main form
        public FrmMainMenu()
        {
            InitializeComponent();

            _music = Music.Init();
            _sound = SfxSound.Init();

            // load settings
            LoadSave ls = LoadSave.Init();
            ls.LoadSettings();
            
            // Intro

            // set bullets pBoxes array
            _bullets = new PictureBox[10];
            _bullets[0] = this.pBoxBullet1;
            _bullets[1] = this.pBoxBullet2;
            _bullets[2] = this.pBoxBullet3;
            _bullets[3] = this.pBoxBullet4;
            _bullets[4] = this.pBoxBullet5;
            _bullets[5] = this.pBoxBullet6;
            _bullets[6] = this.pBoxBullet7;
            _bullets[7] = this.pBoxBullet8;
            _bullets[8] = this.pBoxBullet9;
            _bullets[9] = this.pBoxBullet10;

            _counter = 0;
            _introActive = true;
        }

        // When forms load
        private void Form1_Load(object sender, EventArgs e)
        {
            Intro();
        }

        // Intro timer
        private void tmrMainIntro_Tick(object sender, EventArgs e)
        {
            //bullets order: 1,2,3,4 pause 5,6 pause 7,8,9,10

            // show bullets
            if (_counter > 0 && _counter <= 4)
            {
                //play sound
                if (_counter == 1)
                {
                    // play shooting sound
                    _sound.PlaySound(SfxSound.SoundTypes.menu, SfxSound.MenuSounds.intro_effect);
                }

                _bullets[_counter - 1].Visible = true;    //show bullet
            }
            else if (_counter >= 7 && _counter <= 8)
            {
                _bullets[_counter - 3].Visible = true;    //show bullet
            }
            else if (_counter >= 11 && _counter <= 14)
            {
                _bullets[_counter - 5].Visible = true;    //show bullet
            }

            // move title
            if (this.pBoxMainTitle.Top < 40)
            {
                this.pBoxMainTitle.Top += 8;
            }
            else
            {
                // intro finished
                _counter = 100;
                tmrMainIntro.Interval = 300;
                _introActive = false;
            }

            // show prompt
            if (_counter >= 100 && _counter % 10 == 0)
            {
                if (this.pBoxPrompt.Visible)
                {
                    this.pBoxPrompt.Visible = false;
                }
                else
                {
                    this.pBoxPrompt.Visible = true;
                }
            }

            _counter++;  //advance counter
        }

        // Intro
        private void Intro()
        {
            // start intro
            tmrMainIntro.Enabled = true;
        }

        // Key down event
        private void FrmMainMenu_KeyDown(object sender, KeyEventArgs e)
        {
            // if intro inactive
            if (!_introActive)
            {
                // space key pressed and menu not started
                if (e.KeyCode == Keys.Space && tmrMainIntro.Enabled)
                {
                    tmrMainIntro.Enabled = false;   //stop the intro timer

                    // remove intro items
                    pBoxPrompt.Dispose();   // prompt

                    // bullets
                    for (int i = 0; i < _bullets.Length; i++)
                    {
                        _bullets[i].Dispose();
                    }

                    // set game menu elements
                    _gameMenu = GameMenu.Init(pBoxMenuPointer);

                    // this form
                    _gameMenu.MainForm = this;

                    // picture boxes
                    _gameMenu.PbMenuOptions = pBoxMenuOptions;
                    _gameMenu.PbInGameMenu = pBoxInGameMenu;
                    _gameMenu.PbLoadSave = pBoxMenuLoadSave;
                    _gameMenu.PbSettings = pBoxMenuSettings;
                    _gameMenu.PbExitPrompt = pBoxMenuExitPrompt;

                    // labels
                    _gameMenu.LbMusicVol = lblMusicVolume;
                    _gameMenu.LbSoundVol = lblSoundVolume;

                    // show the menu
                    _gameMenu.ShowMenu();
                }
                // main menu is init.
                else if(_gameMenu != null)
                {
                    // pass. the key to the level
                    _gameMenu.KeyPressed(e.KeyCode);
                }
                
            }
        }
    }
}

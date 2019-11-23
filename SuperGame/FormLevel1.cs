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
    public partial class FormLevel1 : Form
    {
        private static string _libPathGraphics = @"Media\Graphics\Levels\Level1\";
        private static string _libPathSounds = @"Media\Sounds\Levels\Level1\";

        // Components
        private Level _level;
        private SfxSound _sound = SfxSound.Init();

        /* Cinematic */

        // Tile and background
        private int _tileMovementStep = 5;
        private int _tileMovement;

        // Car
        private int _carMovementSideStep = 4;
        private int _carMovementDownStep = 1;
        private int _carMovementSide = 130;
        private int _carMovementDown = 8;
        private int _carMovementHover = 55;
        private int _carHoverLevel = 2;
        private string _carSoundMusic = _libPathSounds + "car_sound.mp3";
        private string _carSoundBrake = _libPathSounds + "car_brake.mp3";
        private string _carSoundLand = _libPathSounds + "car_land.mp3";
        private bool _playCarMusic = true;
        private bool _playCarBrake = true;
        private bool _playCarLand = true;

        // Player & etc
        private Point _playerLocation = new Point(2360, 210);
        private bool _setPlayer = true;
        private string _emptyCarImage = "car_empty.png";

        // intro message
        private int _messageLevelInfoTime = 4;
        private string _messageLevelInfo = "Level 1 -City Apocalypse";

        /* Level triggers */
        private int _chickenTime = 50;
        private int _chickenCurrentCnt;

        public FormLevel1()
        {
            InitializeComponent();

            // init level
            _level = Level.Init();

            // link form to collision system
            CollisionSystem.form = this;

            // create and link characters
            _level.PlayerCharacter = new Player(new Character1(pBoxPlayer), Character.State.fall, Character.Direction.left);

            // set p.boxes
            _level.PbLevelBackground = pBoxGameBackground;
            _level.PbLevelTile = pBoxGameTile;

            // labels
            _level.lblInGameInfo = lblInGameInfo;

            // set timer
            _level.GameTimer = tmrCinematic;

            // set music
            _level.MusicTrack = Music.MusicLibrary.level1;

            // other settings
            _level.DirectionIsRight = false;

            // screen move init values
            ScreenMove.background = pBoxGameBackground;
            ScreenMove.tile = pBoxGameTile;
            ScreenMove.tilePhantom = pBoxGameTilePhantom;
        }

        // Form loaded
        private void FormLevel_Load(object sender, EventArgs e)
        {
            // flip car
            pBoxCar.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            //change game tile parent to background
            pBoxGameTile.Parent = pBoxGameBackground;

            // change elements parent to game tile
            pBoxCar.Parent = pBoxGameTile;
            pBoxStopSign.Parent = pBoxGameTile;
            pBoxComicSay.Parent = pBoxGameTile;
            pBoxPlayer.Parent = pBoxGameTile;
            lblInGameInfo.Parent = pBoxGameTile;

            // tile and background int. offset
            int offset = ScreenMove.backGroundTileOffset();
            
            // cinematic movement of background + small offset
            pBoxGameBackground.Left -= 1 + offset;

            // cinematic movement
            _tileMovement = pBoxGameTile.Width - this.Width + offset;

            // tile offset. acc. to background
            pBoxGameTile.Left += offset;
            pBoxGameTilePhantom.Left += offset;
        }

        // Button pressed
        private void FormLevel_KeyDown(object sender, KeyEventArgs e)
        {
            // pass. the key to the level
            _level.KeyPressed(e.KeyCode);
        }

        // Button released
        private void FormLevel1_KeyUp(object sender, KeyEventArgs e)
        {
            _level.KeyUp(e.KeyCode);
        }


        /* TIMERS */

        // Cinematic timer
        private void tmrCinematic_Tick(object sender, EventArgs e)
        {
            InitLevel();
        }

        // Game timer
        private void tmrGame_Tick(object sender, EventArgs e)
        {
            _level.TimerTick();
            Console.WriteLine("Player loc: [{0}.{1}]", pBoxPlayer.Location.X, pBoxPlayer.Location.Y);
        }

        // Level brief info timer
        private void tmrLevelBrief_Tick(object sender, EventArgs e)
        {
            _messageLevelInfoTime--;                 // reduce counter
            _level.ShowOnScreen(_messageLevelInfo);  // show level name message

            // time ended
            if (_messageLevelInfoTime == 0)
            {
                tmrLevelBrief.Enabled = false;  // disable timer
                lblInGameInfo.Visible = false;  // hide label
            }
            
        }

        // Chicken timer
        private void tmrChicken_Tick(object sender, EventArgs e)
        {
            // chicken time ended
            if (_chickenCurrentCnt == 0)
            {
                tmrChicken.Enabled = false;
            }
        }

        // Init. level (cinematic move)
        public void InitLevel()
        {
            // 1. Move the tile and background
            if (_tileMovement > 0)
            {
                _CinematicMove();
            }
            // 2. Move the car to side
            else if (_carMovementSide > 0)
            {
                _MoveCarSide();
            }
            // 4. Hover the car
            else if (_carMovementHover > 0)
            {
                _HoverCar();
                //_level.ShowOnScreen(_message);  // show level name message
            }
            // 4. Move the car down
            else if (_carMovementDown > 0)
            {
                _MoveCarDown();
                tmrLevelBrief.Enabled = true;  // show level name message
            }
            // 5. Replace car and place player
            else if (_setPlayer)
            {
                _SetPlayer();
            }
            else
            {
                // set flags
                tmrCinematic.Enabled = false;   // disable cinematic timer
                tmrGame.Enabled = true;         // enable game timer

                _level.GameTimer = tmrGame;     // start game timer
            }
        }

        // Set player
        private void _SetPlayer()
        {
            // car
            pBoxCar.Image = Image.FromFile(_libPathGraphics + _emptyCarImage);  // empty car picture
            pBoxCar.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);           // flip car

            // player
            //pBoxPlayer.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);    // flip player image
            pBoxPlayer.Location = _playerLocation;  // move to location
            pBoxPlayer.Visible = true;              // set visible
            _setPlayer = false;                     // change flag
        }

        // Cinematic move
        private void _CinematicMove()
        {
            // "move" screen right
            ScreenMove.MoveTileLeft(_tileMovementStep);

            _tileMovement -= _tileMovementStep;     //dec. steps
        }

        // Move car side
        private void _MoveCarSide()
        {
            // car music sound
            if (_playCarMusic)
            {
                _sound.PlaySound(_carSoundMusic);
                _playCarMusic = false;
            }

            pBoxCar.Left -= _carMovementSideStep;
            _carMovementSide -= 1;
        }

        // Hover car
        private void _HoverCar()
        {
            // brake sound
            if (_playCarBrake)
            {
                _sound.PlaySound(_carSoundBrake);
                _playCarBrake = false;
                pBoxComicSay.Visible = true;
            }
            else if (_carMovementHover < 25 && _playCarLand)
            {
                _sound.PlaySound(_carSoundLand);
                _playCarLand = false;
            }

            pBoxCar.Top += _carHoverLevel;
            _carHoverLevel *= -1;
            _carMovementHover -= 1;
        }

        // Move car down
        private void _MoveCarDown()
        {
            pBoxComicSay.Visible = false;
            pBoxCar.Top += _carMovementDownStep;
            _carMovementDown -= 1;
        }

        /* Level Triggers*/

        // Chicken
        public void Chicken()
        {
            if (!tmrChicken.Enabled)
            {
                //play sound

                // set comic say
                pBoxComicSay.Location = new Point(1143, 395);
                pBoxComicSay.Visible = true;

                tmrChicken.Enabled = true;  // enable timer

                _chickenCurrentCnt = _chickenTime;
            }

            _chickenCurrentCnt--;
        }

        
    }
}

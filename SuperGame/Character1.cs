using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SuperGame
{
    class Character1 : Character
    {
        /* Files */
        // Sounds
        private static string _libPathSounds = @"Media\Sounds\Characters\Character1\";
        private static string _soundsFileType = ".mp3";
        private static string _soundsHealthUp = _libPathSounds + "health_up" + _soundsFileType;
        private static string _soundsArmorUp = _libPathSounds + "armor_up" + _soundsFileType;
        // Graphics
        private static string _libPathGraphics = @"Media\Graphics\Characters\Character1\";
        private static string _graphicsFileType = ".png";
        

        // CONSTR.
        public Character1(PictureBox characterPb) : base(characterPb)
        {
            #region Set sounds
            // Set sounds
            _soundHitHealth = _libPathSounds + "hit_health" + _soundsFileType;
            _soundHitArmor = _libPathSounds + "hit_armor" + _soundsFileType;
            _soundDie = _libPathSounds + "die" + _soundsFileType;
            _soundDieFall = _libPathSounds + "die_fall" + _soundsFileType;
            _soundJumpUp = _libPathSounds + "jump_up" + _soundsFileType;
            _soundFall = _libPathSounds + "fall_scream" + _soundsFileType;
            _soundGetItem = _libPathSounds + "get_item" + _soundsFileType;
            //_soundCollectItem = _libPathSounds + "collect_item" + _soundsFileType;
            #endregion
            #region Set graphics
            /* Set graphics */
            // Aim
            _animationAimUp = new Image[] { Image.FromFile(_libPathGraphics + @"Aim\up" + _graphicsFileType) };
            _animationAimDown = new Image[] { Image.FromFile(_libPathGraphics + @"Aim\down" + _graphicsFileType) };
            _animationAimSide = new Image[] { Image.FromFile(_libPathGraphics + @"Aim\side" + _graphicsFileType) };
            _animationAimCrouch = new Image[] { Image.FromFile(_libPathGraphics + @"Aim\crouch" + _graphicsFileType) };
            // Die
            _animationDie = new Image[] { Image.FromFile(_libPathGraphics + @"Die\die1" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Die\die2" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Die\die3" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Die\die4" + _graphicsFileType)};
            // Jump
            _animationJumpUp = new Image[] { Image.FromFile(_libPathGraphics + @"Jump\up" + _graphicsFileType) };
            _animationJumpDown = new Image[] { Image.FromFile(_libPathGraphics + @"Jump\down" + _graphicsFileType) };
            _animationJumpSide = new Image[] { Image.FromFile(_libPathGraphics + @"Jump\side" + _graphicsFileType) };
            // Run
            _animationRunSide = new Image[] { Image.FromFile(_libPathGraphics + @"Run\run1" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Run\run2" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Run\run3" + _graphicsFileType)};
            // Shoot
            _animationShootUp = new Image[] { Image.FromFile(_libPathGraphics + @"Shoot\up" + _graphicsFileType) };
            _animationShootDown = new Image[] { Image.FromFile(_libPathGraphics + @"Shoot\down" + _graphicsFileType) };
            _animationShootSide = new Image[] { Image.FromFile(_libPathGraphics + @"Shoot\side" + _graphicsFileType) };
            _animationShootCrouch = new Image[] { Image.FromFile(_libPathGraphics + @"Shoot\crouch" + _graphicsFileType) };
            // Walk
            _animationWalkSide = new Image[] { Image.FromFile(_libPathGraphics + @"Walk\walk1" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Walk\walk2" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Walk\walk3" + _graphicsFileType),
                                            Image.FromFile(_libPathGraphics + @"Walk\walk4" + _graphicsFileType)};
            #endregion
            _defaultState = State.aim;
            _defaulttDirection = Direction.left;
        }
    }
}

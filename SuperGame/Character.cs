using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperGame
{
    // Character class
    abstract class Character
    {
        #region Character class parameters
        // components
        protected SfxSound _sound;

        /* Enums */

        // direction of a character
        public enum Direction : byte { up, down, left, right };

        // action of a character
        //public enum Action : byte { stand, crouch, look, aim, walk, run, jump, fall, shoot, die, appear, dissapear };
        
        // state of a character
        public enum State : byte { stand, crouch, look, aim, walk, run, jump, fall, shoot, die, appear, dissapear };
        //public enum State : byte { walking, runing };

        // Sounds
        protected string _soundHitHealth, _soundHitArmor, _soundDie, _soundDieFall,
                         _soundJumpUp, _soundJumpDown, _soundFall, _soundGetItem, _soundCollectItem;

        /* Bitmaps  (all side bitmaps facing right) */
        //private Bitmap[][] _animationsUp;// = { _animationStand, _animationLook, 
        //_animationAim, _animationWalk, _animationRun, _animationJump, _animationFall, _animationShoot,_animationDie, _animationAppear, _animationDissapear};

        // Stand animation
        protected Image[] _animationStand;

        // Look animation
        protected Image[] _animationLookUp;
        protected Image[] _animationLookDown;
        protected Image[] _animationLookSide;

        // Aim animation
        protected Image[] _animationAimUp;
        protected Image[] _animationAimDown;
        protected Image[] _animationAimSide;
        protected Image[] _animationAimCrouch;

        // Walk animation
        protected Image[] _animationWalkUp;
        protected Image[] _animationWalkDown;
        protected Image[] _animationWalkSide;

        // Run animation
        protected Image[] _animationRunUp;
        protected Image[] _animationRunDown;
        protected Image[] _animationRunSide;

        // Jump animation
        protected Image[] _animationJumpUp;
        protected Image[] _animationJumpDown;
        protected Image[] _animationJumpSide;

        // Fall animation
        protected Image[] _animationFallUp;
        protected Image[] _animationFallDown;
        protected Image[] _animationFallSide;

        // Shoot animation
        protected Image[] _animationShootUp;
        protected Image[] _animationShootDown;
        protected Image[] _animationShootSide;
        protected Image[] _animationShootCrouch;

        // Die animation
        protected Image[] _animationDie;

        // Appear animation
        protected Image[] _animationAppear;

        // Dissapear animation
        protected Image[] _animationDissapear;

        /* Movement specs */

        // Speed
        protected int _currentSpeed;
        protected int _speedWalk = 5;
        protected int _speedRun = 10;
        protected int _speedJump = 20;
        protected int _speedFall = 15;

        // Other
        protected Direction _currentDirection;
        protected Direction _newDirection;
        protected Direction _defaulttDirection;
        protected State _currentState;
        protected State _newState;
        protected State _defaultState;
        protected int _jumpPowerWalk;
        protected int _jumpPowerRun;
        protected int _holdOnDie;
        protected int _holdOnHit;
        protected int _blinkOnDie;
        protected int _blinkOnHit;

        /* Other */
        protected PictureBox _currentCharacter;
        protected Weapon[] _weapon;
        protected int _currentHealth;
        protected int _currentArmor;
        protected int _maxHealth;
        protected int _maxArmor;
        protected int _currentHit;
        protected int _fallResistance;

        /* Counters */
        protected int _currentDirectionCount;
        protected int _currentStateCount;
        protected int _currentFallCount;
        protected int _currentHoldCount;
        protected int _currentBlinkCount;
        protected int _currentHitCount;

        /* Flags */
        protected bool _doMove;
        protected bool _canWalk;
        protected bool _canRun;
        protected bool _canJump;
        protected bool _canMove;
        protected bool _canAim;
        protected bool _canLook;
        protected bool _canShoot;
        protected bool _canChangeDirection;
        protected bool _canChangeState;
        protected bool _isHit;
        protected bool _isHold;
        protected bool _isDying;
        protected bool _isDiedFalling;
        protected bool _isDead;
        #endregion
        // CONSTR.
        public Character()
        {
            // components
            _sound = SfxSound.Init();
        }
        public Character(PictureBox characterPb)
        {
            _currentCharacter = characterPb;
        }


        // PROPERTIES
        public PictureBox CharacterPbox { get => _currentCharacter; }
        public State CurrentState { get => _currentState; set => _currentState = value; }
        public State DefaultState { get => _defaultState; set => _defaultState = value; }
        public Direction CurrentDirection { get => _currentDirection; set => _currentDirection = value; }
        public Direction DefaultDirection { get => _defaulttDirection; set => _defaulttDirection = value; }
        public int CurrentSpeed { get => _currentSpeed; set => _currentSpeed = value; }
        
        // Flags
        public bool CanRun { get => _canRun; set => _canRun = value; }
        public bool CanMove { get => _canMove; set => _canMove = value; }
        public bool CanJump { get => _canJump; set => _canJump = value; }
        public bool CanWalk { get => _canWalk; set => _canWalk = value; }
        public bool CanAim { get => _canAim; set => _canAim = value; }
        public bool CanLook { get => _canLook; set => _canLook = value; }
        public bool CanShoot { get => _canShoot; set => _canShoot = value; }
        public bool CanChangeDirection { get => _canChangeDirection; set => _canChangeDirection = value; }
        public bool CanChangeState { get => _canRun; set => _canRun = value; }
        public bool IsHit { get => _isHit; set => _isHit = value; }
        public bool IsHold { get => _isHold; set => _isHold = value; }
        public bool IsDaying { get => _isDying; set => _isDying = value; }
        public bool IsDiedFalling { get => _isDiedFalling; set => _isDiedFalling = value; }
        public bool IsDead { get => _isDead; set => _isDead = value; }
        public bool DoMove { get => _doMove; set => _doMove = value; }

        // METHODS

        // Loads a character image
        protected void _CharacterImageLoad(Image[] pictArr)
        {
            if (pictArr != null)
            {
                _currentCharacter.Image = pictArr[_currentDirectionCount % pictArr.Length];
            }
        }

        // Updade image based on current state anddirection
        protected void _UpdateImage()
        {
            switch (_currentState)
            {
                case State.stand:
                        _CharacterImageLoad(_animationStand);
                    break;
                case State.look:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationLookUp);
                    }
                    else if(_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationLookDown);
                        _currentCharacter.Image = _animationLookDown[_currentDirectionCount % _animationLookDown.Length];
                    }
                    else
                    {
                        _CharacterImageLoad(_animationLookSide);
                    }
                    break;
                case State.aim:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationAimUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationAimDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationAimSide);
                    }
                    break;
                case State.walk:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationWalkUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationWalkDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationWalkSide);
                    }
                    break;
                case State.run:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationRunUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationRunDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationRunSide);
                    }
                    break;
                case State.jump:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationJumpUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationJumpDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationJumpSide);
                    }
                    break;
                case State.fall:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationFallUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationFallDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationFallSide);
                    }
                    break;
                case State.shoot:
                    if (_currentDirection == Direction.up)
                    {
                        _CharacterImageLoad(_animationShootUp);
                    }
                    else if (_currentDirection == Direction.down)
                    {
                        _CharacterImageLoad(_animationShootDown);
                    }
                    else
                    {
                        _CharacterImageLoad(_animationShootSide);
                    }
                    break;
                case State.die:
                    _CharacterImageLoad(_animationDie);
                    break;
                case State.appear:
                    _CharacterImageLoad(_animationAppear);
                    break;
                case State.dissapear:
                    _CharacterImageLoad(_animationDissapear);
                    break;
            }
                // flip right to left
                if (_currentDirection == Direction.left)
                {
                    _currentCharacter.Image?.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }
        }

        // Move -moves a character
        public void Move()
        {
            // state counter
            if (_currentState == _newState)
            {
                _currentStateCount++;
            }
            else
            {
                _currentStateCount = 0;
            }

            // direction counter
            if (_currentDirection == _newDirection)
            {
                _currentDirectionCount++;
            }
            else
            {
                _currentDirectionCount = 0;
            }

            // if no action
            if (!_doMove)
            {
                _currentState = _newState = _defaultState;
            }

            _UpdateSpeed();     //update character speed

            // can move
            if (_canMove && (_doMove || _currentState == State.jump))
            {
                switch (_currentDirection)
                {
                    case Direction.up:
                        _currentCharacter.Top -= _currentSpeed;
                        break;
                    case Direction.down:
                        _currentCharacter.Top += _currentSpeed;
                        break;
                    case Direction.left:
                        _currentCharacter.Left -= _currentSpeed;
                        break;
                    case Direction.right:
                        _currentCharacter.Left += _currentSpeed;
                        break;
                }
            }
            else if (_isDying || _currentState == State.fall)
            {
                _currentCharacter.Top += _speedFall;
            }

        }

        // Upate Speed -updates the speed of player
        private void _UpdateSpeed()
        {
            switch (_currentState)
            {
                case State.stand:
                case State.crouch:
                case State.look:
                case State.aim:
                case State.shoot:
                case State.appear:
                case State.dissapear:
                case State.walk:
                    _currentSpeed = _speedWalk;
                    break;
                case State.run:
                    _currentSpeed = _speedRun;
                    break;
                case State.jump:
                    _currentSpeed = _speedJump;
                    break;
                case State.fall:
                    _currentSpeed = _speedFall;
                    break;
            }
        }

        // Jump
        public void Jump()
        {
            // jump up
            if (_currentDirection != Direction.down)
            {
                // play sound
                _sound.PlaySound(_soundJumpUp);

                switch (_currentState)
                {
                    case State.stand:
                    case State.look:
                    case State.aim:
                    case State.walk:
                        _currentCharacter.Top += _jumpPowerWalk;
                        break;
                    case State.run:
                        _currentCharacter.Top += _jumpPowerRun;
                        break;
                }

                _ChangeDirection(Direction.up);     // change direction
            }
            else
            {
                // play sound
                _sound.PlaySound(_soundJumpDown);

                switch (_currentState)
                {
                    case State.stand:
                    case State.look:
                    case State.aim:
                    case State.walk:
                        _currentCharacter.Top -= _jumpPowerWalk;
                        break;
                }

                _ChangeDirection(Direction.down);     // change direction
            }
            
            _ChangeState(State.jump);   // change state
        }

        // Change state -changes character state
        protected void _ChangeState(State state)
        {
            _currentState = state;      // change state
            _currentStateCount = 0;     // reset counter
        }

        // Change direction -changes character direction
        protected void _ChangeDirection(Direction direction)
        {
            _currentDirection = direction;  // change direction
            _currentDirectionCount = 0;     // reset counter
        }

        // Die
        protected void _Die()
        {
            if (!_isDying)
            {
                // play die sound
                if (_isDiedFalling)
                {
                    _sound.PlaySound(_soundDieFall);
                }
                else
                {
                    _sound.PlaySound(_soundDie);
                }

                _isDying = true;
                _currentBlinkCount = _blinkOnDie;
            }

            _Blink();

            if (_currentBlinkCount == 0)
            {
                _isDead = true;
            }
        }

        // Blink the character
        protected void _Blink()
        {
            // visible
            if (_currentCharacter.Visible)
            {
                _currentCharacter.Visible = false;
            }

            _currentCharacter.Visible = true;

            _currentBlinkCount--;   // decrease counter
        }

        // Character is get hit
        protected void _GetHit()
        {
            _isHit = false;

            if (_currentArmor > 0)
            {
                // play hit sound
                _sound.PlaySound(_soundHitArmor);

                _currentArmor -= _currentHit;

                if (_currentArmor < 0)
                {
                    _currentHit -= _currentArmor;
                    _currentArmor = 0;
                }
            }

            if (_currentHit > 0)
            {
                _isHold = true;
                _currentHoldCount = _blinkOnHit;

                _currentHealth -= _currentHit;

                if (_currentHealth > 0)
                {
                    // play hit sound
                    _sound.PlaySound(_soundHitHealth);
                }
                else
                {
                    _Die();
                }
            }
        }

        // Fall & die -character is die falling
        protected void _FallDie()
        {
            // change flags
            _canMove = false;
            _isDiedFalling = true;

            // play sound
            _sound.PlaySound(_soundFall);
        }

        // Reset -reset a character
        public void ResetCharacter(bool withArmor)
        {
            // flags
            _isHit = false;
            _isHold = false;
            _isDying = false;
            _isDiedFalling = false;
            _isDead = false;

            // other
            _currentSpeed = _speedWalk;
            _currentHealth = _maxHealth;
            _currentArmor = (withArmor) ? _maxArmor : 0;

            // counters 
            _currentDirectionCount = 0;
            _currentStateCount = 0;
            _currentFallCount = 0;
            _currentHoldCount = 0;
            _currentBlinkCount = 0;
            _currentHitCount = 0;
        }

        // Update -updates a character
        public void Update()
        {
            _CheckCollisions();
            Move();
            _UpdateImage();
        }

        // Check collisions
        private void _CheckCollisions()
        {
            Control[] controls;

            // get collisions
            controls = CollisionSystem.CollisionCheck(_currentCharacter);

            // controls found
            if (controls != null)
            {
                // check collisions
                foreach (Control control in controls)
                {
                    if (control.Tag.Equals("floor") && _currentCharacter.Bounds.IntersectsWith(control.Bounds))
                    {
                        _currentCharacter.Top = control.Top - _currentCharacter.Height;
                    }
                
                }
            }
            else
            {
                Fall();
            }
        }

        // Fall -the character is failling
        public void Fall()
        {
            if (_currentState != State.fall)
            {
                _currentState = State.fall;
                _currentDirection = Direction.down;
                _currentSpeed = _speedFall;

                // counters
                _currentDirectionCount = 0;
                _currentStateCount = 0;
            }
        }
    }
}

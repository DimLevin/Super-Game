using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SuperGame
{
    class Player
    {
        Character _currentCharacter;

        // CONSTR.
        public Player(Character character, Character.State state, Character.Direction direction)
        {
            _currentCharacter = character;

            // set direction and state
            _currentCharacter.CurrentState = state;
            _currentCharacter.CurrentDirection = direction;

            // set flags
            _currentCharacter.CanWalk = true;
            _currentCharacter.CanMove = true;
            _currentCharacter.CanLook = true;
            _currentCharacter.CanChangeDirection = true;
            _currentCharacter.CanChangeState = true;
            _currentCharacter.CanAim = true;
            _currentCharacter.CanShoot = true;
            //_currentCharacter.CanRun = true;

            // speed settings
            //_currentCharacter.

        }

        // PROPERTIES
        public Character MyCharacter { get => _currentCharacter; }

        // METHODS
        #region Keyboard inputs
        // Key is pressed
        public void KeyDown(Keys key)
        {
            // up or down keys   
            if (key == Keys.Up)
            {
                _currentCharacter.CurrentDirection = Character.Direction.up;
            }
            else if (key == Keys.Down)
            {
                _currentCharacter.CurrentDirection = Character.Direction.down;
            }

            // left or right keys
            if (key == Keys.Left)
            {
                _currentCharacter.CurrentDirection = Character.Direction.left;
                _currentCharacter.DoMove = true;

                // move tile
                ScreenMove.MoveTileRight(10);   //CHANGE!
            }
            else if (key == Keys.Right)
            {
                _currentCharacter.CurrentDirection = Character.Direction.right;
                _currentCharacter.DoMove = true;

                // move tile
                ScreenMove.MoveTileRight(10);   //CHANGE!
            }

            // jump
            if (key == Keys.LShiftKey)
            {
                _currentCharacter.CurrentState = Character.State.jump;
                _currentCharacter.DoMove = true;
            }

            // shoot
            if (key == Keys.ControlKey)
            {
                _currentCharacter.CurrentState = Character.State.shoot;
            }

            // interract
            if (key == Keys.Enter)
            {
                //_currentCharacter.CurrentState = Character.State.shoot;
            }

        }

        // Key is released
        public void KeyUp(Keys key)
        {
            _currentCharacter.DoMove = false;

            /*
            
            if (key == Keys.Left)
            {
                goLeft = false;
            }


            if (key == Keys.Right)
            {
                goRight = false;
            }


            if (jumping)
            {
                jumping = false;
            }*/
        }
        #endregion

    }
}

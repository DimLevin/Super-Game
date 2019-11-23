using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperGame
{
    // Menu Pointer
    class Pointer
    {
        // for singleton
        private static Pointer _instance = null;

        PictureBox _pointerPbox;
        private int _topInitVal;
        private int _leftInitVal;
        private int _offset;
        private int _totalElements;
        private int _currentPos;


        // CONSTR.
        private Pointer(PictureBox pointerPbox)
        {
            this._pointerPbox = pointerPbox;
            _currentPos = 1;
        }

        // Singleton initiator
        public static Pointer Init(PictureBox pointerPbox)
        {
            if (Pointer._instance == null)
            {
                Pointer._instance = new Pointer(pointerPbox);
            }

            return Pointer._instance;
        }

        public static Pointer Init()
        {
            return Pointer._instance;
        }


        // PROPERTIES

        public int TopInitVal
        {
            set
            {
                _topInitVal = (value >= 0) ? value : 0;
                _pointerPbox.Top = _topInitVal;
            }
        }

        public int LeftInitVal
        {
            set
            {
                _leftInitVal = (value >= 0) ? value : 0;
                _pointerPbox.Left = _leftInitVal;
            }
        }

        public int Offset { get => _offset; set => _offset = value; }
        public int TotalElements
        {
            get => _totalElements;

            set
            {
                _totalElements = value;
                _currentPos = 1;
            }
        }

        public bool Visible { get => _pointerPbox.Visible; set => _pointerPbox.Visible = value; }
        public int Width { get => _pointerPbox.Width; }
        public int Position { get => _currentPos; }


        // METHODS

        // Move up
        public void MoveUp()
        {
            // end not reached
            if (_currentPos > 1)
            {
                _pointerPbox.Top -= _offset;
                _currentPos--;
            }
                
        }

        // Move down
        public void MoveDown()
        {
            // end not reached
            if (_currentPos < _totalElements)
            {
                _pointerPbox.Top += _offset;
                _currentPos++;
            }
        }


    }
}

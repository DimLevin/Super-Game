using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperGame
{
    static class ScreenMove
    {
        private static int tileToBackgroundDelta = 5;
        static public PictureBox background;
        static public PictureBox tile;
        static public PictureBox tilePhantom;


        // METHODS

        // Calculate background-tile offset
        public static int backGroundTileOffset()
        {
            return (tile.Width - tilePhantom.Parent.Width) / (tileToBackgroundDelta - 1);
        }

        /* Move screen */

        // Left
        public static bool MoveTileLeft(int tileStep)
        {
            bool moved = false;

            // step can be made
            if (ScreenMove.tilePhantom?.Right > tilePhantom.Parent.Width)//ScreenMove.tilePhantom.Right > ScreenMove.tilePhantom.Right - tilePhantom.Parent.Width
            {
                // full step
                if (tileStep <= ScreenMove.tilePhantom.Right - tilePhantom.Parent.Width)
                {
                    int backGroundStep = (tileStep / tileToBackgroundDelta > 1) ? tileStep / tileToBackgroundDelta : 1;

                    tilePhantom.Left -= tileStep;       // move phantom
                    background.Left += backGroundStep;  // move background
                    tile.Left -= tileStep;              // move tile
                }
                // partial step
                else
                {
                    int partMove = ScreenMove.tilePhantom.Right - tilePhantom.Parent.Width;
                    int backGroundStep = (partMove / tileToBackgroundDelta > 1) ? partMove / tileToBackgroundDelta : 1;

                    tilePhantom.Left -= partMove;       // move phantom
                    background.Left += backGroundStep;  // move background
                    tile.Left -= partMove;              // move tile
                }

                moved = true;   //change flag
            }

            return moved;
        }

        // Right
        public static bool MoveTileRight(int tileStep)
        {
            bool moved = false;

            // step can be made
            if (ScreenMove.tilePhantom?.Location.X > 0)
            {
                // full step
                if (ScreenMove.tilePhantom.Location.X + tileStep >= 0)
                {
                    tile.Left += tileStep;
                    tilePhantom.Left += tileStep;
                    background.Left -= tileStep / tileToBackgroundDelta;
                }
                // partial step
                else
                {
                    int partMove = -1 * (ScreenMove.tilePhantom.Location.X - tileStep);

                    tile.Left += partMove;
                    tilePhantom.Left += partMove;
                    background.Left -= partMove / tileToBackgroundDelta;
                }

                moved = true;   //change flag
            }

            return moved;
            //Console.WriteLine("phantom tile right: " + ScreenMove.tilePhantom.Right);
            //Console.WriteLine("parent width: " + tilePhantom.Parent.Width);
        }

    }
}

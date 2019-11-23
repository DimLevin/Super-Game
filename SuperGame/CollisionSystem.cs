using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Collections.Generic;

namespace SuperGame
{
    static class CollisionSystem
    {
        public static Form form;

        // METHODS

        // Check collision for certain pBox
        public static Control[] CollisionCheck(PictureBox client)
        {
            List<Control> tList = null;

            if (form != null)
            {
                tList = new List<Control>();

                foreach (Control control in form.Controls)
                {
                    // picture box with tag
                    if (control is PictureBox && control.Tag != null)
                    {
                        // 
                        if (client.Bounds.IntersectsWith(control.Bounds))
                        {
                            tList.Add(control);
                        }
                    }
                
                }
            }

            return tList?.ToArray();
        }

    }
}

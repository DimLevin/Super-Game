using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace SuperGame
{
    abstract class Weapon
    {
        /* Ammo */

        // Ammo animation
        private Bitmap[] _animationAmmo;
        // Stand animation
        private PictureBox[] _firedAmmo;

        // Weapon specs
        private int _damage;
        private int _cooldown;
        private int _fireRate;

        // Ammo
        private int _ammoAccuracy;
        private int _ammoAmount;
        private int _ammoSpeed;

        // Counters
        private int _currentCooldownCount;
        private int _currentFireRateCount;
        private int _currentAmmoCount;

        // Flags
        private bool _isAutoAim;
        private bool _isAutoNavigate;


        // CONSTR.
        public Weapon()
        {

        }





    }
}

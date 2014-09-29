using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarelTheRobot
{
    public class KarelTheTest : BaseUI
    {
        public KarelTheTest() : base() { }

        protected override void run()
        {
            karel.move();
            karel.turnLeft();
            karel.turnLeft();
            karel.turnLeft();
            karel.move();
            karel.turnLeft();
            karel.turnLeft();
            karel.turnLeft();

            if (!karel.leftIsClear()) MessageBox.Show("Right is blocked!");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            karel.pickBeeper();
            karel.pickBeeper();
            karel.turnLeft();
            karel.turnLeft();
            karel.turnLeft();
            karel.move();
        }
    }
}

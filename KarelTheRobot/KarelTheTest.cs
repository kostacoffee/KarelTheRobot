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
            while (!karel.facingSouth()) karel.turnLeft();
        }
    }
}

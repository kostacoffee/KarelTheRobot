using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarelTheRobot
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        public override string ToString()
        {
            return row.ToString() + ", " + col.ToString();
        }
    }

    class KarelException : Exception
    {
        public KarelException(string message) : base(message) { }
    }

}

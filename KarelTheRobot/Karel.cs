using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarelTheRobot
{
    public class Karel
    {
        //Constants
        private const string KAREL_PIC_LOCATION = "Resources/karel.png";
        //End Consants
        //Private fields
        internal double rot;
        //End Private fields
        internal Position pos;
        internal int numBeepers { get; set; }
        internal PictureBox karelPic { get; set; }
        private KarelBoard board;
        internal int waitTime;

        public Karel(int row, int col, int numBeepers, KarelBoard board)
        {
            this.pos = new Position(row, col);
            this.numBeepers = numBeepers;
            this.karelPic = new PictureBox { ImageLocation = KAREL_PIC_LOCATION, SizeMode = PictureBoxSizeMode.StretchImage };
            this.rot = 0;
            this.board = board;
            this.waitTime = 500;
        }

        /// <summary>
        /// Asks Karel to move forward one block in the current direction.
        /// Karel will crash if there is a wall blocking its way.
        /// </summary>
        public void move()
        {
            Position nextPos = pos;
            nextPos.col += (int)Math.Round(Math.Cos(rot), MidpointRounding.AwayFromZero); // cosine moves Karel left/right
            nextPos.row += (int)Math.Round(-Math.Sin(rot), MidpointRounding.AwayFromZero); // sine move Karel up/down
            board.checkWalls(pos, nextPos); // throws KarelException if check doeesn't pass
            pos = nextPos;
            board.moveKarel();
            board.Refresh();
            System.Threading.Thread.Sleep(waitTime);

        }

        /// <summary>
        /// Asks Karel to rotate 90 degrees to the left (counterclockwise).
        /// </summary>
        public void turnLeft() 
        {
            rot += Math.PI / 2;
            karelPic.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
            board.Refresh();
            System.Threading.Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Asks Karel to pick up one beeper from the current cell and stores the beeper in its beeper bag.
        /// Karel will crash unless there is a beeper on the current cell.
        /// </summary>
        public void pickBeeper()
        {
            board.takeBeeper(pos);
            this.numBeepers++;
            board.Refresh();
            System.Threading.Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Asks Karel to take a beeper from its beeper bag and put it down on the current cell.
        /// Karel will crash unless there are beepers in its beeper bag.
        /// </summary>
        public void placeBeeper()
        {
            if (numBeepers == 0) throw new KarelException("No beepers in this cell!");
            numBeepers--;
            board.putBeeper(pos);
            board.Refresh();
            System.Threading.Thread.Sleep(waitTime);
        }
    }
}

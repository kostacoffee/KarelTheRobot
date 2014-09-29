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
            if (!frontIsClear()) throw new KarelException("Crashed into wall!");
            makeMove();
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
        /// <summary>
        /// Is there a wall in front of Karel?
        /// </summary>
        /// <returns></returns>
        public bool frontIsClear()
        {
            Position prevPos = pos;
            makeMove();
            bool isClear = !board.wallBlocks(pos, prevPos);
            pos = prevPos;
            return isClear;
        }

        /// <summary>
        /// Is there a wall to Karel’s left?
        /// </summary>
        /// <returns></returns>
        public bool leftIsClear()
        {
            rot += Math.PI / 2;
            bool isClear = frontIsClear();
            rot -= Math.PI / 2;
            return isClear;
        }

        /// <summary>
        /// Is there a wall to Karel’s right?
        /// </summary>
        /// <returns></returns>
        public bool rightIsClear()
        {
            rot += 3 * (Math.PI / 2);
            bool isClear = frontIsClear();
            rot -= 3 * (Math.PI / 2);
            return isClear;
        }

        /// <summary>
        /// Are there beepers in this cell?
        /// </summary>
        /// <returns></returns>
        public bool beepersPresent()
        {
            return board.getBeeper(pos).numBeepers > 0;
        }

        /// <summary>
        /// Any there beepers in Karel’s bag?
        /// </summary>
        /// <returns></returns>
        public bool beepersInBag()
        {
            return numBeepers > 0;
        }

        /// <summary>
        /// Is Karel facing north?
        /// </summary>
        /// <returns></returns>
        public bool facingNorth()
        {
            return rot == Math.PI / 2;
        }

        /// <summary>
        /// Is Karel facing east?
        /// </summary>
        /// <returns></returns>
        public bool facingEast()
        {
            return rot == 0;
        }

        /// <summary>
        /// Is Karel facing south?
        /// </summary>
        /// <returns></returns>
        public bool facingSouth()
        {
            return rot == 3 * (Math.PI / 2);
        }

        /// <summary>
        /// Is Karel facing west?
        /// </summary>
        /// <returns></returns>
        public bool facingWest()
        {
            return rot == Math.PI;
        }

        private void makeMove()
        {
            pos.col += (int)Math.Round(Math.Cos(rot), MidpointRounding.AwayFromZero); // cosine moves Karel left/right
            pos.row += (int)Math.Round(-Math.Sin(rot), MidpointRounding.AwayFromZero); // sine move Karel up/down
        }
    }
}

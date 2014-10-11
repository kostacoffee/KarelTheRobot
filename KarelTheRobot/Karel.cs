using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace KarelTheRobot
{
    public abstract class Karel
    {
        private const string KAREL_PIC_LOCATION = "Resources/karel.png";
        internal double rot;
        internal int cellSize;
        internal Position pos;
        internal int numBeepers { get; set; }
        internal PictureBox karelPic { get; set; }
        private KarelBoard board;
        internal int waitTime;
        internal Thread karelThread;

        public Karel(int row, int col, int numBeepers, int cellSize)
        {
            this.cellSize = cellSize;
            this.pos = new Position(row, col);
            this.numBeepers = numBeepers;
            this.karelPic = new PictureBox { ImageLocation = KAREL_PIC_LOCATION, SizeMode = PictureBoxSizeMode.StretchImage };
            this.rot = 0;
            this.waitTime = 100;
        }

        internal void Start()
        {
            try
            {
                run();
            }
            catch (KarelException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        protected virtual void run() { } // Filled by extended classes.

        /// <summary>
        /// Asks Karel to move forward one block in the current direction.
        /// Karel will crash if there is a wall blocking its way.
        /// </summary>
        protected void move()
        {
            if (!frontIsClear()) throw new KarelException("Crashed into wall!");
            Position nextPos = makeMove();
            moveKarelPic(pos, nextPos);
            pos = nextPos;
            Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Asks Karel to rotate 90 degrees to the left (counterclockwise).
        /// </summary>
        protected void turnLeft() 
        {
            rot += Math.PI / 2;
            karelPic.Invoke((MethodInvoker)delegate
            {
                karelPic.Image.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                board.Refresh();
            });
            Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Asks Karel to pick up one beeper from the current cell and stores the beeper in its beeper bag.
        /// Karel will crash unless there is a beeper on the current cell.
        /// </summary>
        protected void pickBeeper()
        {
            board.Invoke((MethodInvoker)delegate
            {
                board.takeBeeper(pos);
                board.Refresh();
            });
            this.numBeepers++;
            Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Asks Karel to take a beeper from its beeper bag and put it down on the current cell.
        /// Karel will crash unless there are beepers in its beeper bag.
        /// </summary>
        protected void placeBeeper()
        {
            if (numBeepers == 0) throw new KarelException("No beepers in bag!");
            numBeepers--;
            board.Invoke((MethodInvoker)delegate
            {
                board.putBeeper(pos);
                board.Refresh();
            });
            Thread.Sleep(waitTime);
        }

        /// <summary>
        /// Is there a wall in front of Karel?
        /// </summary>
        /// <returns></returns>
        protected bool frontIsClear()
        {
            Position nextPos = makeMove();
            bool isClear = !board.wallBlocks(nextPos, pos);
            return isClear;
        }

        /// <summary>
        /// Is there a wall to Karel’s left?
        /// </summary>
        /// <returns></returns>
        protected bool leftIsClear()
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
        protected bool rightIsClear()
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
        protected bool beepersPresent()
        {
            return board.getBeeper(pos).numBeepers > 0;
        }

        /// <summary>
        /// Any there beepers in Karel’s bag?
        /// </summary>
        /// <returns></returns>
        protected bool beepersInBag()
        {
            return numBeepers > 0;
        }

        /// <summary>
        /// Is Karel facing north?
        /// </summary>
        /// <returns></returns>
        protected bool facingNorth()
        {
            return rot == Math.PI / 2;
        }

        /// <summary>
        /// Is Karel facing east?
        /// </summary>
        /// <returns></returns>
        protected bool facingEast()
        {
            return rot == 0;
        }

        /// <summary>
        /// Is Karel facing south?
        /// </summary>
        /// <returns></returns>
        protected bool facingSouth()
        {
            return rot == 3 * (Math.PI / 2);
        }

        /// <summary>
        /// Is Karel facing west?
        /// </summary>
        /// <returns></returns>
        protected bool facingWest()
        {
            return rot == Math.PI;
        }

        private Position makeMove()
        {
            Position newPos = pos;
            newPos.col += (int)Math.Round(Math.Cos(rot), MidpointRounding.AwayFromZero); // cosine moves Karel left/right
            newPos.row += (int)Math.Round(-Math.Sin(rot), MidpointRounding.AwayFromZero); // sine move Karel up/down
            return newPos;
        }

        private void moveKarelPic(Position p1, Position p2)
        {
            int colChange = p2.col - p1.col;
            int rowChange = p2.row - p1.row;
            karelPic.Invoke((MethodInvoker)delegate
            {
                karelPic.Left += colChange * cellSize;
                karelPic.Top += rowChange * cellSize;
                board.Refresh();
            });

        }

        protected internal void setBoard(KarelBoard board)
        {
            this.board = board;
        }
    }
}

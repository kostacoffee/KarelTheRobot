using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarelTheRobot
{
    public partial class  BaseUI : Form
    {
        protected KarelBoard board;
        protected ExtendedKarel karel;
        private const int BOARD_SIZE = 603;
        public BaseUI()
        {
            InitializeComponent();
            constructWorld("Worlds/test.w");
            
            this.board.Left = 200;
            this.board.Top = 10;
            this.Controls.Add(board);
        }

        private void constructWorld(string worldPath)
        {
            //Reading Board Properties
            StreamReader f = new StreamReader(worldPath);
            string[] boardProps = f.ReadLine().Split(' ');
            int numRows = Convert.ToInt32(boardProps[0]);
            int numCols = Convert.ToInt32(boardProps[1]);
            int cellSize = cellSize = BOARD_SIZE / (numCols > numRows ? numCols : numRows);

            //Reading Karel Properties
            string[] karelParams = f.ReadLine().Split(' ');
            int row = Convert.ToInt32(karelParams[0]);
            int col = Convert.ToInt32(karelParams[1]);
            int beeperBag = Convert.ToInt32(karelParams[2]);
            if (beeperBag == -1) beeperBag = int.MaxValue;

            //Instantiating Karel and the Board.
            karel = new ExtendedKarel(row, col, beeperBag, cellSize);
            this.board = new KarelBoard(numRows, numCols, BOARD_SIZE, cellSize, karel, worldPath);
            karel.setBoard(board);
            this.Controls.Add(board);
        }

        private void resetUI()
        {
            btnStart.Enabled = true;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            Task karelRun = new Task(karel.Start);
            karelRun.Start();
            karelRun.GetAwaiter().OnCompleted(resetUI);
        }
    }

    // Declaration for ExtendedKarel Class. (So it compiles)
    public partial class ExtendedKarel : Karel
    {
        public ExtendedKarel(int row, int col, int numBeepers, int cellSize) : base(row, col, numBeepers, cellSize) { }
    }
}

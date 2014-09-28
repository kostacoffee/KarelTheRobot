using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KarelTheRobot
{
    public class KarelBoard : UserControl
    {
        // Max row/cols = 30
        private int numRows;
        private int numCols;
        private Dictionary<Position, Position> walls;
        private Beeper[,] beepers;
        private Karel karel;
        private int cellSize;
        private const int BOARD_SIZE = 603;
        private const int LABEL_SIZE = 30;
        private const int LABEL_OFFSET = 20;
        private const int WALL_WIDTH = 3;
        private const int CELL_MARKER_SIZE = 3;
        private const double KAREL_SIZE_MULT = 0.9;
        private const double KAREL_OFFSET = 0.05;
        public KarelBoard(string worldFile)
        {
            this.walls = new Dictionary<Position, Position>();
            initBoard(worldFile);
            visualiseBoard();
        }

        private void visualiseBoard() {
            
            this.Width = BOARD_SIZE + LABEL_SIZE + LABEL_OFFSET;
            this.Height = BOARD_SIZE + LABEL_SIZE + LABEL_OFFSET;
            this.cellSize = BOARD_SIZE / (this.numCols > this.numRows ? this.numCols : this.numRows);
            defineBoard();
            placeLabels(cellSize);
            placeCells(cellSize);
            placeBeepers(cellSize);
            placeWalls(cellSize);
            placeKarel(cellSize);
        }

        private void defineBoard()
        {
            PictureBox boardBG = new PictureBox
            {
                Width = BOARD_SIZE,
                Height = BOARD_SIZE,
                Left = LABEL_SIZE + LABEL_OFFSET,
                Top = 0,
                BackColor = System.Drawing.Color.White
            };
            this.Controls.Add(boardBG);
            boardBG.SendToBack();
        }

        private void placeCells(int cellSize)
        {
            for (int row = 0; row < numRows; row++) {
                for (int col = 0; col < numCols; col++){
                    PictureBox cellMarker = new PictureBox
                    {
                        Width = CELL_MARKER_SIZE,
                        Height = CELL_MARKER_SIZE,
                        BackColor = System.Drawing.Color.Black,
                        Left = col * cellSize + cellSize/2 + LABEL_SIZE + LABEL_OFFSET,
                        Top = row*cellSize + cellSize/2
                    };
                    this.Controls.Add(cellMarker);
                    cellMarker.BringToFront();
                }
            }
        }

        private void placeKarel(int cellSize)
        {
            int karelOffset = (int)(cellSize * KAREL_OFFSET);
            karel.karelPic.Width = (int)(cellSize*KAREL_SIZE_MULT);
            karel.karelPic.Height = (int)(cellSize*KAREL_SIZE_MULT);
            karel.karelPic.Left = karel.pos.col * cellSize + LABEL_OFFSET + LABEL_SIZE + karelOffset;
            karel.karelPic.Top = karel.pos.row * cellSize + karelOffset;
            this.Controls.Add(karel.karelPic);
            karel.karelPic.BringToFront();
        }

        private void placeLabels(int cellSize)
        {

            for (int row = 0; row < numRows; row++)
            {
                Label rowNum = new Label
                {
                    Left = LABEL_SIZE/2,
                    Text = (row + 1).ToString(),
                    Width = LABEL_SIZE,
                    Height = LABEL_SIZE,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                    };
                rowNum.Top = row * cellSize  + rowNum.Height/2;
                this.Controls.Add(rowNum);
                rowNum.BringToFront();
            }

            for (int col = 0; col < numCols; col++)
            {
                Label colNum = new Label
                {
                    Text = (col + 1).ToString(),
                    Top = BOARD_SIZE,
                    Width = LABEL_SIZE,
                    Height = LABEL_SIZE,
                    TextAlign = System.Drawing.ContentAlignment.MiddleCenter
                };
                colNum.Left = col * cellSize + LABEL_SIZE + LABEL_OFFSET + colNum.Width/2;
                this.Controls.Add(colNum);
                colNum.BringToFront();
            }
        }

        private void placeBeepers(int cellSize)
        {
            int beeperSize = (int)(cellSize * 0.9);
            int beeperOffset = (int)(cellSize * 0.05);
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    beepers[row, col].setSize(beeperSize);
                    beepers[row, col].Top = cellSize * row;
                    beepers[row, col].Left = cellSize*col + LABEL_SIZE + LABEL_OFFSET;
                    this.Controls.Add(beepers[row, col]);
                    beepers[row, col].BringToFront();
                };
            }
        }

        private void placeWalls(int cellSize)
        {
            foreach (KeyValuePair<Position, Position> wall in walls)
            {
                if (wall.Key.col == wall.Value.col) placeHorizontalWall(wall.Key, wall.Value, cellSize);
                else placeVerticalWall(wall.Key, wall.Value, cellSize);
            }
        }

        private void placeVerticalWall(Position p1, Position p2, int cellSize)
        {
            PictureBox wall = new PictureBox
            {
                BackColor = System.Drawing.Color.Black,
                Height = cellSize,
                Width = WALL_WIDTH,
                Top = cellSize*p1.row,
                Left = cellSize*(p1.col > p2.col ? p1.col : p2.col) + LABEL_SIZE + LABEL_OFFSET
            };
            this.Controls.Add(wall);
            wall.BringToFront();
        }

        private void placeHorizontalWall(Position p1, Position p2, int cellSize)
        {
            PictureBox wall = new PictureBox
            {
                BackColor = System.Drawing.Color.Black,
                Width = cellSize,
                Height = WALL_WIDTH,
                Top = cellSize*(p1.row > p2.row ? p1.row : p2.row),
                Left = cellSize*p1.col + LABEL_OFFSET + LABEL_SIZE
            };
            this.Controls.Add(wall);
            wall.BringToFront();
        }

        internal void addWall(Position p1, Position p2)
        {
            if (walls.Keys.Contains(p1)) walls.Add(p2, p1);
            else walls.Add(p1, p2);
        }

        private void initBoard(string worldFile)
        {
            string[] worldProps = File.ReadLines(worldFile).ToArray();
            initDims(worldProps[0].Split(' '));
            initKarel(worldProps[1].Split(' '));
            initBeepers();
            foreach (string line in worldProps.Skip(2))
            {
                string[] worldParams = line.Split(' ');
                if (worldParams[0].Equals("w")) addWall(worldParams);
                else if (worldParams[0].Equals("b")) addBeeper(worldParams);
            }
            addBorderWalls();
        }

        private void initBeepers()
        {
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    beepers[row, col] = new Beeper(0);
                }
            }
        }

        private void addBorderWalls()
        {
            for (int row = 0; row < numRows; row++)
            {
                addWall(new Position(row, 0), new Position(row, -1));
                addWall(new Position(row, numCols - 1), new Position(row, numCols));
            }
            for (int col = 0; col < numCols; col++)
            {
                addWall(new Position(-1, col), new Position(0, col));
                addWall(new Position(numRows, col), new Position(numRows - 1, col));
            }
        }
        
        private void initDims(string[] dims)
        {
            this.numRows = Convert.ToInt32(dims[0]);
            this.numCols = Convert.ToInt32(dims[1]);
            this.beepers = new Beeper[numRows, numCols];
        }

        private void initKarel(string[] karelParams)
        {
            int row = Convert.ToInt32(karelParams[0]);
            int col = Convert.ToInt32(karelParams[1]);
            int beeperBag = Convert.ToInt32(karelParams[2]);
            if (beeperBag == -1) beeperBag = int.MaxValue;
            this.karel = new Karel(row, col, beeperBag, this);
        }

        private void addWall(string[] wallProps)
        {
            int row1 = Convert.ToInt32(wallProps[1]);
            int col1 = Convert.ToInt32(wallProps[2]);
            int row2 = Convert.ToInt32(wallProps[3]);
            int col2 = Convert.ToInt32(wallProps[4]);
            addWall(new Position(row1, col1), new Position(row2, col2));
        }

        private void addBeeper(string[] beeperParams)
        {
            int row = Convert.ToInt32(beeperParams[1]);
            int col = Convert.ToInt32(beeperParams[2]);
            int numBeepers = Convert.ToInt32(beeperParams[3]);
            beepers[row, col].editBeeperCount(numBeepers);
        }

        internal void putBeeper(Position pos)
        {
            beepers[pos.row, pos.col].editBeeperCount(1);
        }

        internal void takeBeeper(Position pos)
        {
            beepers[pos.row, pos.col].editBeeperCount(-1);
        }

        internal void checkWalls(Position pos, Position nextPos)
        {
            if ((walls.ContainsKey(pos) && walls[pos].Equals(nextPos)) || (walls.ContainsKey(nextPos) && walls[nextPos].Equals(pos)))
                throw new KarelException("Crashed into wall!");
        }

        public Karel getKarel()
        {
            return karel;
        }

        internal void moveKarel()
        {
            int karelOffset = (int)(cellSize * KAREL_OFFSET);
            karel.karelPic.Left = karel.pos.col * cellSize + LABEL_OFFSET + LABEL_SIZE + karelOffset;
            karel.karelPic.Top = karel.pos.row * cellSize + karelOffset;
        }
    }
}

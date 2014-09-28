using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace KarelTheRobot
{
    class Beeper : PictureBox
    {
        private Label beeperLabel;
        private const double LABEL_SIZE_MULTIPLIER = 0.3;
        public int numBeepers { get; private set; }
        private const string BEEPER_IMG_LOC = "Resources/beeper.png";

        public Beeper(int numBeepers)
        {
            this.numBeepers = numBeepers;
            this.ImageLocation = BEEPER_IMG_LOC;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.beeperLabel = new Label { Text = numBeepers.ToString(), TextAlign = System.Drawing.ContentAlignment.MiddleCenter, BackColor = System.Drawing.Color.Transparent };
            this.Visible = false;
            this.Controls.Add(beeperLabel);
            beeperLabel.BringToFront();
        }


        public void setSize(int size)
        {
            this.Width = size;
            this.Height = size;
            beeperLabel.Width = this.Width;
            beeperLabel.Height = this.Height;
        }
        public void editBeeperCount(int change){
            numBeepers += change;
            if (numBeepers > 0) this.Visible = true;
            else if (numBeepers == 0) this.Visible = false;
            else throw new KarelException("Not enough beepers in cell");
            this.beeperLabel.Text = numBeepers.ToString();
        }
    }
}

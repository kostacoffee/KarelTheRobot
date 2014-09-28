using System;
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
        protected Karel karel;
        public BaseUI()
        {
            InitializeComponent();
            this.board = new KarelBoard("Worlds/test.w");
            this.board.Left = 200;
            this.board.Top = 10;
            this.Controls.Add(board);
            karel = board.getKarel();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                run();
            }
            catch (KarelException ex){
                MessageBox.Show(ex.Message);
            }
        }

        protected virtual void run()
        {

        }
    }
}

using System;
using System.Windows.Forms;

namespace Othello
{
    public partial class Othello : Form
    {
        readonly ClsOthello cls = new ClsOthello();

        public Othello()
        {
            InitializeComponent();
            dataGridView.RowCount = 8;
            cls.Display(dataGridView);
        }

        public void DataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (cls.ITEMS[e.ColumnIndex, e.RowIndex] == "P")
            {
                cls.CIndex = e.ColumnIndex;
                cls.TurnLabelChanger(Turn_Label);
                cls.RIndex = e.RowIndex;
                cls.CellClick();
                cls.Display(dataGridView);
                cls.GameOver(dataGridView);
                cls.CounterLabelChanger(WhiteCounter_Label, BlackCounter_Label);
            }
        }

        public void BtnReset_Click(object sender, EventArgs e)
        {
            cls.Reset(dataGridView);
            cls.CounterLabelChanger(WhiteCounter_Label, BlackCounter_Label);
            Turn_Label.Text = "White's Turn";
        }
    }
}
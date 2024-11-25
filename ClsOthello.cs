//---------------------------------------//
//-----------© by Amin Mirzaei-----------//
//---------------------------------------//

using Othello.Properties;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Othello
{
    sealed class ClsOthello : IOthello
    {
        #region Game Methods

        private int cindex;
        private int rindex;
        private int BlackCounter = 2;
        private int WhiteCounter = 2;
        private string[,] items = new string[8, 8];
        private string opponentColor = "B";
        private string myColor = "W";

        public string[,] ITEMS { get { return items; } set { items = value; } }
        public bool CLICKFLAG { get; set; }
        public int CIndex { get => cindex; set => cindex = value; }
        public int RIndex { get => rindex; set => rindex = value; }

        public ClsOthello()
        {
            Initialize();
        }

        public void Initialize()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    items[i, j] = "";

            items[3, 3] = "W";
            items[3, 4] = "B";
            items[4, 3] = "B";
            items[4, 4] = "W";

            CLICKFLAG = false;
            BlackCounter = 2;
            WhiteCounter = 2;
            BoardState();
            CheckPossibleMoves();
        }

        public void Display(DataGridView dg)
        {
            //
            // This Method changes the color of the cells
            //
            BlackCounter = 0;
            WhiteCounter = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    dg[i, j].Value = items[i, j];
                    switch (dg[i, j].Value)
                    {
                        case "W":
                            dg.Rows[j].Cells[i].Style.BackColor = Color.White;
                            WhiteCounter++;
                            break;
                        case "B":
                            dg.Rows[j].Cells[i].Style.BackColor = Color.Black;
                            BlackCounter++;
                            break;
                        case "P":
                            dg.Rows[j].Cells[i].Style.BackColor = Color.LimeGreen;
                            break;
                        default:
                            dg.Rows[j].Cells[i].Style.BackColor = Color.Green;
                            break;
                    }
                    dg.Rows[j].Cells[i].Style.SelectionBackColor = dg.Rows[j].Cells[i].Style.BackColor;
                }
            }
        }

        public void CellClick()
        {
            //
            // This methos places black or white on the cell
            //
            PlaySound();
            ClickRules();
            items[CIndex, RIndex] = ChangeFlag();
            CLICKFLAG = !CLICKFLAG;

            //
            // this for clears previous possible moves
            //
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (ITEMS[i, j] == "P")
                        ITEMS[i, j] = "";

            BoardState();
            CheckPossibleMoves();
            CheckIsPossibleToMove();
        }

        public string ChangeFlag()
        {
            //
            // this method changes turn when we click on a cell
            //
            return CLICKFLAG ? "B" : "W";
        }

        public void TurnLabelChanger(Label Turn_Label) =>
            //
            // this method changes the text of Turn_Label
            // 
            Turn_Label.Text = CLICKFLAG ? "White's Turn" : "Black's Turn";

        public void CounterLabelChanger(Label WhiteCounter_Label, Label BlackCounter_Label)
        {
            WhiteCounter_Label.Text = WhiteCounter.ToString();
            BlackCounter_Label.Text = BlackCounter.ToString();
        }

        public void PlaySound()
        {
            //
            // this method plays sound when clicking
            //
            SoundPlayer clickSound = new SoundPlayer(Resources.MouseClick);
            clickSound.Play();
        }

        public void Reset(DataGridView dg)
        {
            //
            // this method Resets the game data and starts a new game
            //
            Initialize();
            Display(dg);
        }

        public void GameOver(DataGridView dg)
        {
            //
            // this method checks the game state and shows a message box when its finished
            //
            if (BlackCounter + WhiteCounter == 64 ||
                BlackCounter == 0 ||
                WhiteCounter == 0 ||
                CheckIsPossibleToMove() == false)
            {
                int winnerPoint;
                string winner;
                winnerPoint = (BlackCounter > WhiteCounter) ? BlackCounter : WhiteCounter;
                winner = (BlackCounter > WhiteCounter) ? "Black" : "White";
                MessageBox.Show($"Winner is {winner} Player With {winnerPoint} Points.",
                                "Game Over",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                Reset(dg);
            }
        }

        public void BoardState()
        {
            //
            // this method defines myColor and opponentColor
            //
            opponentColor = CLICKFLAG ? "W" : "B";
            myColor = CLICKFLAG ? "B" : "W";
        }

        #endregion

        #region Game Rules

        public void Up()
        {
            if (RIndex < 6 &&
                ITEMS[CIndex, RIndex + 2] != "" &&
                ITEMS[CIndex, RIndex + 2] != "P" &&
                ITEMS[CIndex, RIndex + 1] == opponentColor)
            {
                for (int i = RIndex + 1; i < 7; i++)
                {
                    if (ITEMS[CIndex, i] == opponentColor &&
                        ITEMS[CIndex, i + 1] == myColor)
                    {
                        for (int t = i; t > RIndex; t--)
                        {
                            ITEMS[CIndex, t] = myColor;
                        }
                        break;
                    }
                }
            }
        }

        public void UpRight()
        {
            if (CIndex > 1 &&
                RIndex < 6 &&
                ITEMS[CIndex - 2, RIndex + 2] != "" &&
                ITEMS[CIndex - 2, RIndex + 2] != "P" &&
                ITEMS[CIndex - 1, RIndex + 1] == opponentColor)
            {
                for (int j = CIndex - 1; j > 0; j--)
                {
                    for (int i = RIndex + 1; i < 7; i++)
                    {
                        if (i - RIndex == -1 * (j - CIndex) &&
                            ITEMS[j, i] == opponentColor &&
                            ITEMS[j - 1, i + 1] == myColor)
                        {
                            for (int tj = j; tj < CIndex; tj++)
                            {
                                for (int ti = i; ti > RIndex; ti--)
                                {
                                    if (ti - RIndex == -1 * (tj - CIndex))
                                    {
                                        ITEMS[tj, ti] = myColor;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void Right()
        {
            if (CIndex > 1 &&
                ITEMS[CIndex - 2, RIndex] != "" &&
                ITEMS[CIndex - 2, RIndex] != "P" &&
                ITEMS[CIndex - 1, RIndex] == opponentColor)
            {
                for (int i = CIndex - 1; i > 0; i--)
                {
                    if (ITEMS[i, RIndex] == opponentColor &&
                        ITEMS[i - 1, RIndex] == myColor)
                    {
                        for (int t = i; t < CIndex; t++)
                        {
                            ITEMS[t, RIndex] = myColor;
                        }
                        break;
                    }
                }
            }
        }

        public void DownRight()
        {
            if (CIndex > 1 &&
                RIndex > 1 &&
                ITEMS[CIndex - 2, RIndex - 2] != "" &&
                ITEMS[CIndex - 2, RIndex - 2] != "P" &&
                ITEMS[CIndex - 1, RIndex - 1] == opponentColor)
            {
                for (int i = RIndex - 1; i > 0; i--)
                {
                    for (int j = CIndex - 1; j > 0; j--)
                    {
                        if (i - RIndex == j - CIndex &&
                            ITEMS[j, i] == opponentColor &&
                            ITEMS[j - 1, i - 1] == myColor)
                        {
                            for (int ti = i; ti < RIndex; ti++)
                            {
                                for (int tj = j; tj < CIndex; tj++)
                                {
                                    if (ti - RIndex == tj - CIndex)
                                    {
                                        ITEMS[tj, ti] = myColor;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void Down()
        {
            if (RIndex > 1 &&
                ITEMS[CIndex, RIndex - 2] != "" &&
                ITEMS[CIndex, RIndex - 2] != "P" &&
                ITEMS[CIndex, RIndex - 1] == opponentColor)
            {
                for (int i = RIndex - 1; i > 0; i--)
                {
                    if (ITEMS[CIndex, i] == opponentColor &&
                        ITEMS[CIndex, i - 1] == myColor)
                    {
                        for (int t = i; t < RIndex; t++)
                        {
                            ITEMS[CIndex, t] = myColor;
                        }
                        break;
                    }
                }
            }
        }

        public void DownLeft()
        {
            if (CIndex < 6 &&
                RIndex > 1 &&
                ITEMS[CIndex + 2, RIndex - 2] != "" &&
                ITEMS[CIndex + 2, RIndex - 2] != "P" &&
                ITEMS[CIndex + 1, RIndex - 1] == opponentColor)
            {
                for (int i = RIndex - 1; i > 0; i--)
                {
                    for (int j = CIndex + 1; j < 7; j++)
                    {
                        if (i - RIndex == -1 * (j - CIndex) &&
                            ITEMS[j, i] == opponentColor &&
                            ITEMS[j + 1, i - 1] == myColor)
                        {
                            for (int ti = i; ti < RIndex; ti++)
                            {
                                for (int tj = j; tj > CIndex; tj--)
                                {
                                    if (ti - RIndex == -1 * (tj - CIndex))
                                    {
                                        ITEMS[tj, ti] = myColor;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void Left()
        {
            if (CIndex < 6 &&
                ITEMS[CIndex + 2, RIndex] != "" &&
                ITEMS[CIndex + 2, RIndex] != "P" &&
                ITEMS[CIndex + 1, RIndex] == opponentColor)
            {
                for (int i = CIndex + 1; i < 7; i++)
                {
                    if (ITEMS[i, RIndex] == opponentColor &&
                        ITEMS[i + 1, RIndex] == myColor)
                    {
                        for (int t = i; t > CIndex; t--)
                        {
                            ITEMS[t, RIndex] = myColor;
                        }
                        break;
                    }
                }
            }
        }

        public void UpLeft()
        {
            if (CIndex < 6 &&
                RIndex < 6 &&
                ITEMS[CIndex + 2, RIndex + 2] != "" &&
                ITEMS[CIndex + 2, RIndex + 2] != "P" &&
                ITEMS[CIndex + 1, RIndex + 1] == opponentColor)
            {
                for (int j = CIndex + 1; j < 7; j++)
                {
                    for (int i = RIndex + 1; i < 7; i++)
                    {
                        if (i - RIndex == j - CIndex &&
                            ITEMS[j, i] == opponentColor &&
                            ITEMS[j + 1, i + 1] == myColor)
                        {
                            for (int tj = j, ti = i; tj > CIndex && ti > RIndex; tj--, ti--)
                            {
                                if (ti - RIndex == tj - CIndex)
                                {
                                    ITEMS[tj, ti] = myColor;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }

        public void ClickRules()
        {
            Up();
            UpRight();
            Right();
            DownRight();
            Down();
            DownLeft();
            Left();
            UpLeft();
        }

        #endregion

        #region Check Possible Moves

        public void CheckPossibleUp()
        {
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 6; row++)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col, row + 2] != "" && ITEMS[col, row + 2] != "P" &&
                        ITEMS[col, row + 1] == opponentColor)
                    {
                        for (int i = row + 1; i < 7; i++)
                        {
                            if (ITEMS[col, i] == opponentColor &&
                                ITEMS[col, i + 1] == myColor)
                            {
                                ITEMS[col, row] = "P";

                            }
                        }
                    }
                }
            }
        }


        public void CheckPossibleUpRight()
        {
            for (int col = 7; col > 1; col--)
            {
                for (int row = 0; row < 6; row++)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col - 2, row + 2] != "" &&
                        ITEMS[col - 2, row + 2] != "P" &&
                        ITEMS[col - 1, row + 1] == opponentColor)
                    {
                        for (int j = col - 1; j > 0; j--)
                        {
                            for (int i = row + 1; i < 7; i++)
                            {
                                if (i - row == -1 * (j - col) &&
                                    ITEMS[j, i] == opponentColor &&
                                    ITEMS[j - 1, i + 1] == myColor)
                                {
                                    for (int tj = j; tj < col; tj++)
                                    {
                                        for (int ti = i; ti > row; ti--)
                                        {
                                            if (ti - row == -1 * (tj - col))
                                                ITEMS[col, row] = "P";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleRight()
        {
            for (int col = 7; col > 1; col--)
            {
                for (int row = 0; row < 8; row++)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col - 2, row] != "" && ITEMS[col - 2, row] != "P" &&
                        ITEMS[col - 1, row] == opponentColor)
                    {
                        for (int i = col - 1; i > 0; i--)
                        {
                            if (ITEMS[i, row] == opponentColor &&
                                ITEMS[i - 1, row] == myColor)
                            {
                                ITEMS[col, row] = "P";
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleDownRight()
        {
            for (int col = 7; col > 1; col--)
            {
                for (int row = 7; row > 1; row--)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col - 2, row - 2] != "" &&
                        ITEMS[col - 2, row - 2] != "P" &&
                        ITEMS[col - 1, row - 1] == opponentColor)
                    {
                        for (int j = col - 1; j > 0; j--)
                        {
                            for (int i = row - 1; i > 0; i--)
                            {
                                if (i - row == j - col &&
                                    ITEMS[j, i] == opponentColor &&
                                    ITEMS[j - 1, i - 1] == myColor)
                                {
                                    for (int tj = j; tj < col; tj++)
                                    {
                                        for (int ti = i; ti < row; ti++)
                                        {
                                            if (ti - row == tj - col)
                                                ITEMS[col, row] = "P";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleDown()
        {
            for (int col = 0; col < 8; col++)
            {
                for (int row = 7; row > 1; row--)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col, row - 2] != "" &&
                        ITEMS[col, row - 2] != "P" &&
                        ITEMS[col, row - 1] == opponentColor)
                    {
                        for (int i = row - 1; i > 0; i--)
                        {
                            if (ITEMS[col, i] == opponentColor &&
                                ITEMS[col, i - 1] == myColor)
                            {
                                ITEMS[col, row] = "P";
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleDownLeft()
        {
            for (int col = 0; col < 6; col++)
            {
                for (int row = 7; row > 1; row--)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col + 2, row - 2] != "" &&
                        ITEMS[col + 2, row - 2] != "P" &&
                        ITEMS[col + 1, row - 1] == opponentColor)
                    {
                        for (int j = col + 1; j < 7; j++)
                        {
                            for (int i = row - 1; i > 0; i--)
                            {
                                if (i - row == -1 * (j - col) &&
                                    ITEMS[j, i] == opponentColor &&
                                    ITEMS[j + 1, i - 1] == myColor)
                                {
                                    for (int tj = j; tj > col; tj--)
                                    {
                                        for (int ti = i; ti < row; ti++)
                                        {
                                            if (ti - row == -1 * (tj - col))
                                                ITEMS[col, row] = "P";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleLeft()
        {
            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col + 2, row] != "" &&
                        ITEMS[col + 2, row] != "P" &&
                        ITEMS[col + 1, row] == opponentColor)
                    {
                        for (int i = col + 1; i < 7; i++)
                        {
                            if (ITEMS[i, row] == opponentColor &&
                                ITEMS[i + 1, row] == myColor)
                            {
                                ITEMS[col, row] = "P";
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleUpLeft()
        {
            for (int col = 0; col < 6; col++)
            {
                for (int row = 0; row < 6; row++)
                {
                    if (ITEMS[col, row] == "" &&
                        ITEMS[col + 2, row + 2] != "" &&
                        ITEMS[col + 2, row + 2] != "P" &&
                        ITEMS[col + 1, row + 1] == opponentColor)
                    {
                        for (int j = col + 1; j < 7; j++)
                        {
                            for (int i = row + 1; i < 7; i++)
                            {
                                if (i - row == j - col &&
                                    ITEMS[j, i] == opponentColor &&
                                    ITEMS[j + 1, i + 1] == myColor)
                                {
                                    for (int tj = j; tj > col; tj--)
                                    {
                                        for (int ti = i; ti > row; ti--)
                                        {
                                            if (ti - row == tj - col)
                                                ITEMS[col, row] = "P";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckPossibleMoves()
        {
            CheckPossibleUp();
            CheckPossibleUpRight();
            CheckPossibleRight();
            CheckPossibleDownRight();
            CheckPossibleDown();
            CheckPossibleDownLeft();
            CheckPossibleLeft();
            CheckPossibleUpLeft();
        }

        public bool CheckIsPossibleToMove()
        {
            for (int col = 0; col < 8; col++)
            {
                for (int row = 0; row < 8; row++)
                {
                    if (ITEMS[col, row] == "P")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
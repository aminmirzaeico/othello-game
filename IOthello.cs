using System.Windows.Forms;

namespace Othello
{
    internal interface IOthello
    {
        string[,] ITEMS { get; set; }
        bool CLICKFLAG { get; set; }
        int CIndex { get; set; }
        int RIndex { get; set; }

        void Display(DataGridView dg);
        void CellClick();
        void TurnLabelChanger(Label Turn_Label);
        void CounterLabelChanger(Label WhiteCounter_Label, Label BlackCounter_Label);
        void PlaySound();
        void Reset(DataGridView dg);
        void GameOver(DataGridView dg);
        void BoardState();

        void Up();
        void UpRight();
        void Right();
        void DownRight();
        void Down();
        void DownLeft();
        void Left();
        void UpLeft();
        void ClickRules();

        void CheckPossibleUp();
        void CheckPossibleUpRight();
        void CheckPossibleRight();
        void CheckPossibleDownRight();
        void CheckPossibleDown();
        void CheckPossibleDownLeft();
        void CheckPossibleLeft();
        void CheckPossibleUpLeft();

        void CheckPossibleMoves();
        bool CheckIsPossibleToMove();
    }
}

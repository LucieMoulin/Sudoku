///ETML
///Auteur : Lucie Moulin
///Date : 12.09.2019
///Description : Reporésentation d'une case de sudoku

using System;
using System.Drawing;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;

namespace SudokuGame
{
    /// <summary>
    /// Reporésentation d'une case de sudoku
    /// </summary>
    public partial class SudokuCellView : UserControl
    {
        private SudokuCell cell;
        private string mainNumber
        {
            set
            {
                labelNumber.Text = value;
                if (value != "")
                {
                    if (Parent != null)
                    {
                        ((SudokuView)Parent).CheckVictory();
                    }
                    labelSmallNumbers1.Visible = false;
                    labelSmallNumbers2.Visible = false;
                }
                else
                {
                    labelSmallNumbers1.Visible = true;
                    labelSmallNumbers2.Visible = true;
                }
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="cell"></param>
        public SudokuCellView(SudokuCell cell)
        {
            InitializeComponent();

            this.cell = cell;

            if (cell.Number != 0)
            {
                mainNumber = cell.Number.ToString();
            }

            UpdateSmallNumbersDisplay();
        }

        /// <summary>
        /// Met à jour l'affichage des petits chiffres
        /// </summary>
        private void UpdateSmallNumbersDisplay()
        {
            labelSmallNumbers1.Text = "";
            labelSmallNumbers2.Text = "";

            if (!cell.IsFixed)
            {
                for (int i = 0; i < cell.SmallNumbers.Count; i++)
                {
                    if (i < 5)
                    {
                        labelSmallNumbers1.Text += " " + cell.SmallNumbers[i];
                    }
                    else
                    {
                        labelSmallNumbers2.Text += " " + cell.SmallNumbers[i];
                    }

                    if (i == 6)
                    {
                        labelSmallNumbers2.Text += "\n";
                    }
                }
            }
        }

        /// <summary>
        /// Est exécuté lorsque l'utilisateur appuye sur une touche du clavier
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //Affichage de l'aide
            if (!cell.IsFixed && Focused)
            {
                switch (keyData)
                {
                    case Keys.D0 | Keys.Shift:
                    case Keys.Back:
                    case Keys.Delete:
                        cell.EditNumber(0);
                        mainNumber = "";
                        break;
                    case Keys.D1 | Keys.Shift:
                        cell.EditNumber(1);
                        mainNumber = "1";
                        break;
                    case Keys.D2 | Keys.Shift:
                        cell.EditNumber(2);
                        mainNumber = "2";
                        break;
                    case Keys.D3 | Keys.Shift:
                        cell.EditNumber(3);
                        mainNumber = "3";
                        break;
                    case Keys.D4 | Keys.Shift:
                        cell.EditNumber(4);
                        mainNumber = "4";
                        break;
                    case Keys.D5 | Keys.Shift:
                        cell.EditNumber(5);
                        mainNumber = "5";
                        break;
                    case Keys.D6 | Keys.Shift:
                        cell.EditNumber(6);
                        mainNumber = "6";
                        break;
                    case Keys.D7 | Keys.Shift:
                        cell.EditNumber(7);
                        mainNumber = "7";
                        break;
                    case Keys.D8 | Keys.Shift:
                        cell.EditNumber(8);
                        mainNumber = "8";
                        break;
                    case Keys.D9 | Keys.Shift:
                        cell.EditNumber(9);
                        mainNumber = "9";
                        break;
                    case Keys.D1:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(1))
                            {
                                cell.AddSmallNumber(1);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(1);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D2:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(2))
                            {
                                cell.AddSmallNumber(2);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(2);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D3:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(3))
                            {
                                cell.AddSmallNumber(3);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(3);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D4:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(4))
                            {
                                cell.AddSmallNumber(4);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(4);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D5:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(5))
                            {
                                cell.AddSmallNumber(5);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(5);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D6:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(6))
                            {
                                cell.AddSmallNumber(6);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(6);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D7:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(7))
                            {
                                cell.AddSmallNumber(7);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(7);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D8:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(8))
                            {
                                cell.AddSmallNumber(8);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(8);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.D9:
                        if (cell.Number == 0)
                        {
                            if (!cell.SmallNumbers.Contains(9))
                            {
                                cell.AddSmallNumber(9);
                            }
                            else
                            {
                                cell.RemoveSmallNumber(9);
                            }
                            UpdateSmallNumbersDisplay();
                        }
                        break;
                    case Keys.Up:
                        if (cell.Y > 0)
                        {
                            ((SudokuView)Parent).CellGrid[cell.Y - 1, cell.X].Focus();
                        }
                        return true;
                    case Keys.Down:
                        if (cell.Y < ((SudokuView)Parent).CellGrid.GetLength(0) - 1)
                        {
                            ((SudokuView)Parent).CellGrid[cell.Y + 1, cell.X].Focus();
                        }
                        return true;
                    case Keys.Left:
                        if (cell.X > 0)
                        {
                            ((SudokuView)Parent).CellGrid[cell.Y, cell.X - 1].Focus();
                        }
                        return true;
                    case Keys.Right:
                        if (cell.X < ((SudokuView)Parent).CellGrid.GetLength(0) - 1)
                        {
                            ((SudokuView)Parent).CellGrid[cell.Y, cell.X + 1].Focus();
                        }
                        return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SudokuCellView_Enter(object sender, EventArgs e)
        {
            BackColor = SystemColors.ControlDark;
        }

        private void SudokuCellView_Leave(object sender, EventArgs e)
        {
            BackColor = SystemColors.Control;
        }

        private void LabelSmallNumbers2_Click(object sender, EventArgs e)
        {
            Focus();
        }

        private void LabelSmallNumbers1_Click(object sender, EventArgs e)
        {
            Focus();
        }

        private void LabelNumber_Click(object sender, EventArgs e)
        {
            Focus();
        }

        private void SudokuCellView_Click(object sender, EventArgs e)
        {
            Focus();
        }
    }
}

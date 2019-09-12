///ETML
///Auteur : Lucie Moulin
///Date : 29.08.2019
///Description : Vue du Sudoku

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;

namespace SudokuGame
{
    /// <summary>
    /// Vue du Sudoku
    /// </summary>
    public partial class SudokuView : Form
    {
        private Sudoku sudoku;
        private SudokuCellView[,] cellGrid;

        /// <summary>
        /// Grille de cellules
        /// </summary>
        public SudokuCellView[,] CellGrid { get => cellGrid; set => cellGrid = value; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public SudokuView()
        {
            InitializeComponent();

            sudoku = new Sudoku();
            switch (sudoku.Type)
            {
                default:
                case SudokuType.Numeric9:
                    cellGrid = new SudokuCellView[9, 9];
                    break;
                case SudokuType.Numeric4:
                    cellGrid = new SudokuCellView[4, 4];
                    break;
            }

            DisplaySudoku();
        }

        /// <summary>
        /// Affiche le sudoku
        /// </summary>
        private void DisplaySudoku()
        {
            Controls.Clear();

            Width = sudoku.Grid.GetLength(0) * 50 + 16;
            Height = sudoku.Grid.GetLength(0) * 50 + 38;

            //Parcours de la grille du sudoku, et affichage de chaque case.
            for (int y = 0; y < sudoku.Grid.GetLength(0); y++)
            {
                for (int x = 0; x < sudoku.Grid.GetLength(0); x++)
                {
                    cellGrid[y,x] = new SudokuCellView(sudoku.Grid[y, x]);
                    int intX = x * cellGrid[y, x].Width - 1;
                    int intY = y * cellGrid[y, x].Height - 1;
                    if (sudoku.Type == SudokuType.Numeric9)
                    {
                        if (x > 2)
                        {
                            intX++;
                            if (x > 5)
                            {
                                intX++;
                            }
                        }
                        if (y > 2)
                        {
                            intY++;
                            if (y > 5)
                            {
                                intY++;
                            }
                        }
                    }
                    else
                    {
                        intX++;
                        intY++;
                    }
                    cellGrid[y, x].Location = new Point(intX, intY);
                    cellGrid[y, x].BackColor = SystemColors.Control;
                    Controls.Add(cellGrid[y, x]);
                }
            }
        }

        /// <summary>
        /// Vérifie si le sudoku est terminé, et affiche un message de victoire le cas échéant.
        /// </summary>
        public void CheckVictory()
        {
            if (sudoku.IsCompleted())
            {
                MessageBox.Show("Vous avez terminé votre sudoku !", "Bravo !");
            }
        }
    }
}

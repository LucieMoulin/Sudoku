///ETML
///Auteur : Lucie Moulin
///Date : 29.08.2019
///Description : Vue du Sudoku

using System.Drawing;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;
using SudokuGame.Solver;
using SudokuGame.Generator;
using System.Threading;

namespace SudokuGame
{
    /// <summary>
    /// Vue du Sudoku
    /// </summary>
    public partial class SudokuView : Form
    {
        private Sudoku sudoku;
        private SudokuCellView[,] cellGrid;
        private SudokuGenerator generator;
        private SudokuSolver solver;
        private int progress = 0;
        private Thread solveThread;

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

            generator = new SudokuGenerator();
            sudoku = generator.NewRandomSudoku();
            solver = new SudokuSolver(sudoku, this);
            solveThread = new Thread(() => { solver.SolveSudoku(); });

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
            InitializeComponent();

            int length = sudoku.Grid.GetLength(0);

            Width = length * 50 + 16;
            Height = length * 50 + 62;

            //Parcours de la grille du sudoku, et affichage de chaque case.
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    cellGrid[y, x] = new SudokuCellView(sudoku.Grid[y, x]);
                    int intX = x * cellGrid[y, x].Width - 1;
                    int intY = y * cellGrid[y, x].Height + 24;
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
        /// Met à jour le sudoku
        /// </summary>
        public void UpdateSudoku()
        {
            int length = sudoku.Grid.GetLength(0);

            Width = length * 50 + 16;
            Height = length * 50 + 62;

            //Parcours de la grille du sudoku, et affichage de chaque case.
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    if(sudoku.Grid[y, x].Number != cellGrid[y, x].DisplayedNumber || sudoku.Grid[y, x].SmallNumbers.Count != cellGrid[y, x].DisplayedSmallNumbers.Count)
                    {
                        cellGrid[y, x].UpdateDisplay();
                    }
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

        private void SolveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            solveThread.Start();
        }

        private void SudokuView_FormClosing(object sender, FormClosingEventArgs e)
        {
            solveThread.Abort();
        }
    }
}

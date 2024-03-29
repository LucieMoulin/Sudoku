﻿///ETML
///Auteur : Lucie Moulin
///Date : 29.08.2019
///Description : Vue du Sudoku

using System.Drawing;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;
using SudokuGame.Solver;
using SudokuGame.Generator;
using System.Threading;
using System;

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
        private Thread solveThread;
        private Thread generationThread;

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

            cellGrid = new SudokuCellView[sudoku.Length, sudoku.Length];

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

                    cellGrid[y, x].Location = new Point(intX, intY);
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
                    if (sudoku.Grid[y, x].Number != cellGrid[y, x].DisplayedNumber || sudoku.Grid[y, x].SmallNumbers.Count != cellGrid[y, x].DisplayedSmallNumbers.Count)
                    {
                        cellGrid[y, x].UpdateDisplay();
                    }
                }
            }

            CheckVictory();
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

        /// <summary>
        /// Événement de click du menu "Résoudre"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SolveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //Parcours de la grille du sudoku, et affichage de chaque case.
            for (int y = 0; y < sudoku.Length; y++)
            {
                for (int x = 0; x < sudoku.Length; x++)
                {
                    cellGrid[y, x].UpdateDisplay();
                }
            }

            if (solveThread is null || solveThread.ThreadState != ThreadState.Unstarted)
            {
                solveThread = new Thread(() =>
                {
                    solver = new SudokuSolver(sudoku, this);
                    solver.SolveSudoku();
                });
            }

            solveThread.Start();

            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }

            if (!SudokuSolver.DISPLAY_REAL_TIME)
            {
                while (solveThread.IsAlive) ;
            }

            foreach (Control control in Controls)
            {
                control.Enabled = true;
            }

            UpdateSudoku();
        }

        /// <summary>
        /// Événement de click du menu "Ouvrir"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                sudoku.Load(openFileDialog.FileName);
                DisplaySudoku();
            }
        }

        /// <summary>
        /// Événement de click du menu "Sauvegarder"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                sudoku.Save(saveFileDialog.FileName);
            }
        }

        /// <summary>
        /// Événement de click du menu "Nouveau Sudoku"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSudokuToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            if (generationThread is null || generationThread.ThreadState != ThreadState.Unstarted)
            {
                generationThread = new Thread(() =>
                {
                    sudoku = generator.NewRandomSudoku();
                });
            }

            generationThread.Start();

            foreach (Control control in Controls)
            {
                control.Enabled = false;
            }

            while (generationThread.IsAlive) ;

            DisplaySudoku();
        }

        /// <summary>
        /// Fermeture de l'application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SudokuView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Générateur de sudokus

using SudokuGame.Solver;
using SudokuGame.SudokuObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace SudokuGame.Generator
{
    /// <summary>
    /// Générateur de sudokus
    /// </summary>
    public class SudokuGenerator
    {
        private Random random;
        private SudokuSolver solver;
        private const int MAX_GENERATION_TIME = 500;
        private const int MAXIMUM_FULL_CELLS_IN_SUDOKU = 26;

        /// <summary>
        /// Constructeur
        /// </summary>
        public SudokuGenerator()
        {
            random = new Random();
        }

        /// <summary>
        /// Génère un nouveau sudoku aléatoire
        /// </summary>
        /// <returns></returns>
        public Sudoku NewRandomSudoku()
        {
            Sudoku sudoku = new Sudoku();

            solver = new SudokuSolver(sudoku);
            
            //Crée une nouvelle grille complète aléatoire
            while (!FillWithRandomNumbers(sudoku))
            {
                sudoku = new Sudoku();
                solver = new SudokuSolver(sudoku);
            }

            //Cases pleines
            List<SudokuCell> fullCells = new List<SudokuCell>();
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (cell.Number != 0)
                {
                    fullCells.Add(cell);
                }
            }

            do
            {
                int lastX = 0, lastY = 0;
                byte lastNumber = 0;

                //Enlever des chiffres jusqu'a ce que le sudoku ne soit plus résolvable
                do
                {
                    int x = random.Next(sudoku.Length);
                    int y = random.Next(sudoku.Length);

                    SudokuCell currentCell = sudoku.Grid[y, x];

                    if (currentCell.Number != 0)
                    {
                        lastX = x;
                        lastY = y;
                        lastNumber = currentCell.Number;

                        currentCell.EditNumber(0);

                        fullCells.Remove(currentCell);
                    }
                } while (solver.IsSudokuSolvable());

                //Rajouter un chiffre pour qu'il le soit à nouveau
                sudoku.Grid[lastY, lastX].EditNumber(lastNumber);
                fullCells.Add(sudoku.Grid[lastY, lastX]);

            } while (fullCells.Count > MAXIMUM_FULL_CELLS_IN_SUDOKU);

            //Fixation des cases
            foreach (SudokuCell cell in fullCells)
            {
                cell.IsFixed = true;
            }

            return sudoku;
        }

        /// <summary>
        /// Remplis le sudoku (vide) avec des chiffres aléatoire
        /// </summary>
        /// <param name="sudoku"></param>
        public bool FillWithRandomNumbers(Sudoku sudoku)
        {
            //Cases à remplir
            List<SudokuCell> emptyCells = new List<SudokuCell>();
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (cell.Number == 0)
                {
                    emptyCells.Add(cell);
                }
            }

            //Remplissage de la première ligne et de la première colonne aléatoirement (accélère la suite)
            for (int index = 0; index < sudoku.Length; index++)
            {
                SudokuCell lineCell = sudoku.Grid[0, index];
                SudokuCell columnCell = sudoku.Grid[index, 0];

                //Ajout d'un chiffre aléatoire
                if (lineCell.Number == 0)
                {
                    //Récupération des possibilités
                    List<byte> possibilities = GetPossibleNumbersForCell(lineCell, sudoku);

                    //Placement d'un chiffre pour la ligne
                    if (possibilities.Count > 0)
                    {
                        byte currentNumber = possibilities[random.Next(possibilities.Count)];

                        lineCell.EditNumber(currentNumber);

                        emptyCells.Remove(lineCell);
                    }
                }
                if (columnCell.Number == 0)
                {
                    //Récupération des possibilités
                    List<byte> possibilities = GetPossibleNumbersForCell(columnCell, sudoku);

                    //Placement d'un chiffre pour la ligne
                    if (possibilities.Count > 0)
                    {
                        byte currentNumber = possibilities[random.Next(possibilities.Count)];

                        columnCell.EditNumber(currentNumber);

                        emptyCells.Remove(columnCell);
                    }
                }
            }

            //Chronomètre
            Stopwatch stopwatch = new Stopwatch();

            //Ajout aléatoire des chiffres dans la grille
            Thread generationThread = new Thread(() =>
            {
                RecursivePlaceRandomNumber(emptyCells, sudoku);
            });

            stopwatch.Start();
            generationThread.Start();

            //Limite de temps de clacul
            while (generationThread.IsAlive && stopwatch.ElapsedMilliseconds < MAX_GENERATION_TIME) ;

            stopwatch.Stop();

            //Si le temps a été dépassé mais que le thread tourne toujours, il n'a pas eu le temps de terminer
            if (generationThread.IsAlive)
            {
                //Arrêt du thread
                while (generationThread.IsAlive)
                {
                    generationThread.Abort();
                }

                //Retour du résultat
                return false;
            }

            return sudoku.IsCompleted();
        }

        /// <summary>
        /// Place récursivement des chiffres aléatoires dans le sudoku
        /// </summary>
        /// <param name="emptyCells"></param>
        /// <param name="sudoku"></param>
        /// <returns></returns>
        private bool RecursivePlaceRandomNumber(List<SudokuCell> emptyCells, Sudoku sudoku)
        {
            //Fin de la récursivité
            if (emptyCells.Count == 0)
            {
                return true;
            }

            //Sélection aléatoire de case
            SudokuCell currentCell = emptyCells[random.Next(emptyCells.Count)];

            //Récupération des possibilités
            List<byte> possibilities = GetPossibleNumbersForCell(currentCell, sudoku);

            //Si pas de possibilités, retour de false
            if (possibilities.Count == 0)
            {
                return false;
            }

            //Essai de placement de chaque chiffre
            while (possibilities.Count > 0)
            {
                byte currentNumber = possibilities[random.Next(possibilities.Count)];

                currentCell.EditNumber(currentNumber);

                //Si le sudoku est toujours possible à résoudre
                if (solver.IsSudokuSolvableWithBruteForce())
                {
                    emptyCells.Remove(currentCell);

                    //Placement du prochain chiffre
                    if (RecursivePlaceRandomNumber(emptyCells, sudoku))
                    {
                        return true;
                    }

                    emptyCells.Add(currentCell);
                }

                currentCell.EditNumber(0);
                possibilities.Remove(currentNumber);
            }

            //Si on arrive ici, aucun placement n'a réussi
            return false;
        }

        /// <summary>
        /// Regarde quels chiffres peuvent être placés dans la case
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="sudoku"></param>
        /// <returns></returns>
        public List<byte> GetPossibleNumbersForCell(SudokuCell cell, Sudoku sudoku)
        {
            List<byte> availableNumbers = new List<byte>();

            //Récupération du sommet du carré actuel
            int squareLength = (int)Math.Sqrt(sudoku.Length);
            int xMin = (int)(Math.Floor(cell.X / 3D) * squareLength);
            int yMin = (int)(Math.Floor(cell.Y / 3D) * squareLength);

            for (byte number = 1; number <= sudoku.Length; number++)
            {
                //Si le chiffre peut être placé ici
                if (CheckLine(cell.Y, number, sudoku) && CheckColumn(cell.X, number, sudoku) && CheckSquare(xMin, yMin, number, sudoku))
                {
                    //Ajout dans la liste
                    availableNumbers.Add(number);
                }
            }

            return availableNumbers;
        }

        /// <summary>
        /// Vérifie la présence d'un chiffre sur une ligne
        /// </summary>
        /// <param name="line">index de ligne</param>
        /// <param name="number">Chiffre testé</param>
        /// <returns></returns>
        private bool CheckLine(int line, byte number, Sudoku sudoku)
        {
            //Parcours de toute la ligne
            for (int x = 0; x < sudoku.Length; x++)
            {
                //Si le chiffre est déjà présent, retour de faux
                if (sudoku.Grid[line, x].Number == number)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Vérifie la présence d'un chiffre sur une colonne
        /// </summary>
        /// <param name="column">index de la colonne</param>
        /// <param name="number">Chiffre testé</param>
        /// <param name="sudoku">Sudoku</param>
        /// <returns></returns>
        private bool CheckColumn(int column, byte number, Sudoku sudoku)
        {
            //Parcours de toute la colonne
            for (int y = 0; y < sudoku.Length; y++)
            {
                //Si le chiffre est déjà présent, retour de faux
                if (sudoku.Grid[y, column].Number == number)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Vérifie la présence d'un chiffre dans un carré
        /// </summary>
        /// <param name="xMin">Coordonnée X du coin haut gauche</param>
        /// <param name="yMin">Coordonnée Y du coin haut gauche</param>
        /// <param name="number">Chiffre testé</param>
        /// <param name="sudoku">Sudoku</param>
        /// <returns></returns>
        private bool CheckSquare(int xMin, int yMin, byte number, Sudoku sudoku)
        {
            int length = (int)Math.Sqrt(sudoku.Length);

            //Parcours de tout le carré
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    //Si le chiffre est déjà présent, retour de faux
                    if (sudoku.Grid[y + yMin, x + xMin].Number == number)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}

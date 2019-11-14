///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Générateur de sudokus

using SudokuGame.Solver;
using SudokuGame.SudokuObjects;
using System;
using System.Collections.Generic;

namespace SudokuGame.Generator
{
    /// <summary>
    /// Générateur de sudokus
    /// </summary>
    public class SudokuGenerator
    {
        private Random random;
        private SudokuSolver solver;

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

            FillWithRandomNumbers(sudoku);
            /*
            int lastX = 0, lastY = 0;
            byte lastNumber = 0;

            //Enlever des chiffres jusqu'a ce que le sudoku ne soit plus résolvable
            do
            {

            } while (solver.IsSudokuSolvable());

            //Rajouter un chiffre pour qu'il le soit à nouveau
            sudoku.Grid[lastY, lastX].EditNumber(lastNumber);
            */
            return sudoku;
        }

        /// <summary>
        /// Remplis le sudoku (vide) avec des chiffres aléatoire
        /// </summary>
        /// <param name="sudoku"></param>
        public void FillWithRandomNumbers(Sudoku sudoku)
        {
            //Cases à remplir
            List<SudokuCell> emptyCells = new List<SudokuCell>();
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if(cell.Number == 0)
                {
                    emptyCells.Add(cell);
                }
            }

            RecursivePlaceRandomNumber(emptyCells, sudoku);
        }


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
            if(possibilities.Count == 0)
            {
                return false;
            }

            //Essai de placement de chaque chiffre
            while(possibilities.Count > 0)
            {
                byte currentNumber = possibilities[random.Next(possibilities.Count)];

                currentCell.EditNumber(currentNumber);
                
                //Si le sudoku est toujours possible à résoudre
                if(solver.IsSudokuSolvableWithBruteForce())
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

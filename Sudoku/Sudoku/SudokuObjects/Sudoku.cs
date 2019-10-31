///ETML
///Auteur : Lucie Moulin
///Date : 05.09.2019
///Description : Représente un sudoku

using System;
using System.Collections.Generic;

namespace SudokuGame.SudokuObjects
{
    /// <summary>
    /// Représente un sudoku
    /// </summary>
    public class Sudoku
    {
        private SudokuCell[,] grid;
        private SudokuType type;
        private int length;

        /// <summary>
        /// Grille
        /// </summary>
        public SudokuCell[,] Grid { get => grid; set => grid = value; }

        /// <summary>
        /// Type de sudoku
        /// </summary>
        public SudokuType Type { get => type; }

        public int Length { get => length; }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="type">Type de sudoku</param>
        public Sudoku(SudokuType type = SudokuType.Numeric9)
        {
            this.type = type;
            switch (type)
            {
                default:
                case SudokuType.Numeric9:
                    InitGrid(9);
                    break;

                case SudokuType.Numeric4:
                    InitGrid(4);
                    break;
            }

            length = Grid.GetLength(0);
        }

        /// <summary>
        /// Initialise la grille du sudoku
        /// </summary>
        private void InitGrid(int size)
        {
            grid = new SudokuCell[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    //Initialisation de chaque cellule
                    grid[y, x] = new SudokuCell(this, x, y);
                }
            }
        }


        /// <summary>
        /// Charge un fichier .sudoku
        /// </summary>
        /// <param name="fileName"></param>
        public void Load(string fileName)
        {
            Sudoku newSudoku = SudokuSaver.Load(fileName);
            grid = newSudoku.grid;
            type = newSudoku.type;
        }

        /// <summary>
        /// Sauvegarde la partie vers un fichier .sudoku
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool Save(string fileName)
        {
            return SudokuSaver.Save(fileName, this);
        }

        /// <summary>
        /// Vérifie que le sudoku ne soit pas terminé
        /// </summary>
        /// <returns></returns>
        public bool IsCompleted()
        {
            bool isCompleted = true;

            //Vérification des lignes
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                isCompleted = isCompleted && CheckLine(y);
            }

            if (isCompleted)
            {
                //Vérification des colonnes 
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    isCompleted = isCompleted && CheckColumn(x);
                }
            }

            if (isCompleted)
            {
                //Vérification des carrés
                isCompleted = isCompleted && CheckSquares();
            }

            return isCompleted;
        }

        /// <summary>
        /// Vérifie si une ligne est complète
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool CheckLine(int line)
        {
            //Liste de chiffres déjà utilisés
            List<byte> usedNumbers = new List<byte>();

            //Parcours de toute la ligne
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[line, x].Number != 0 && !usedNumbers.Contains(grid[line, x].Number))
                {
                    usedNumbers.Add(grid[line, x].Number);
                }
            }

            return usedNumbers.Count == grid.GetLength(0);
        }

        /// <summary>
        /// Vérifie si une colonne est complète
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool CheckColumn(int column)
        {
            //Liste de chiffres déjà utilisés
            List<byte> usedNumbers = new List<byte>();

            //Parcours de toute la colonne
            for (int y = 0; y < grid.GetLength(0); y++)
            {
                if (grid[y, column].Number != 0 && !usedNumbers.Contains(grid[y, column].Number))
                {
                    usedNumbers.Add(grid[y, column].Number);
                }
            }

            return usedNumbers.Count == grid.GetLength(0);
        }

        /// <summary>
        /// Vérifications des carrés
        /// </summary>
        /// <returns></returns>
        private bool CheckSquares()
        {
            bool areOk = true;

            //Parcours de tous les carrés
            for (int y = 0; y < grid.GetLength(0); y += (int)Math.Sqrt(grid.GetLength(0)))
            {
                for (int x = 0; x < grid.GetLength(0); x += (int)Math.Sqrt(grid.GetLength(0)))
                {
                    areOk = areOk && CheckSquare(x, y);
                }
            }

            return areOk;
        }

        /// <summary>
        /// Vérification d'un carré
        /// </summary>
        /// <param name="xMin">Coordonnée X du coin haut gauche</param>
        /// <param name="yMin">Coordonnée Y du coin haut gauche</param>
        /// <returns></returns>
        private bool CheckSquare(int xMin, int yMin)
        {
            //Liste de chiffres déjà utilisés
            List<byte> usedNumbers = new List<byte>();

            //Parcours de tout le carré
            for (int y = 0; y < Math.Sqrt(grid.GetLength(0)); y++)
            {
                for (int x = 0; x < Math.Sqrt(grid.GetLength(0)); x++)
                {
                    if (grid[y + yMin, x + xMin].Number != 0 && !usedNumbers.Contains(grid[y + yMin, x + xMin].Number))
                    {
                        usedNumbers.Add(grid[y + yMin, x + xMin].Number);
                    }
                }
            }

            return usedNumbers.Count == grid.GetLength(0);
        }
    }
}

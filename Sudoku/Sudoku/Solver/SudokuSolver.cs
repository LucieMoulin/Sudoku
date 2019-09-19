///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Résolveur de sudokus

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SudokuGame.SudokuObjects;

namespace SudokuGame.Solver
{
    /// <summary>
    /// Résolveur de sudokus
    /// </summary>
    public class SudokuSolver
    {
        private Sudoku sudoku;

        public SudokuSolver(Sudoku sudoku)
        {
            this.sudoku = sudoku;
        }

        public bool IsSudokuValid()
        {
            return false;
        }

        public void SolveSudoku()
        {
            RecursivePlaceSmallNumbers(0, 0, 1);

            //TreatSmallNumbers(0, 0);
        }

        /// <summary>
        /// Parcours récusivement le sudoku et place les petits chiffres aux endroits possibles
        /// </summary>
        /// <param name="line">ligne</param>
        /// <param name="column">colonne</param>
        /// <param name="number">chiffre</param>
        private void RecursivePlaceSmallNumbers(int line, int column, byte number)
        {
            //Récupération du sommet du carré actuel
            int length = sudoku.Grid.GetLength(0);
            int xMin = (int)(Math.Floor(column / 3D) * Math.Sqrt(length));
            int yMin = (int)(Math.Floor(line / 3D) * Math.Sqrt(length));

            //Si le chiffre peut être placé ici
            if (!sudoku.Grid[line, column].IsFixed && CheckLine(line, number) && CheckColumn(column, number) && CheckSquare(xMin, yMin, number))
            {
                //Ajout du petit chiffre
                sudoku.Grid[line, column].AddSmallNumber(number);
            }

            //Vérification dépassment vérical
            if (line < length)
            {
                //Vérification de dépassement horizontal
                if (column < length - 1)
                {
                    //Résoud à partir de la case à droite
                    RecursivePlaceSmallNumbers(line, column + 1, number);
                }
                //Si au bout de la ligne, changement de ligne
                else
                {
                    if (line == length - 1 && column == length - 1)
                    {
                        if (number < length)
                        {
                            //Passage au chiffre suivant
                            RecursivePlaceSmallNumbers(0, 0, (byte)(number + 1));
                        }
                    }
                    else
                    {
                        //Résoud à partir de la première case de la ligne en dessous
                        RecursivePlaceSmallNumbers(line + 1, 0, number);
                    }
                }
            }
        }
        
        /// <summary>
        /// Traite les petits chiffres
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        private void TreatSmallNumbers(int line, int column)
        {
            int length = sudoku.Grid.GetLength(0);

            //Si la case est modifiable
            if (!sudoku.Grid[line, column].IsFixed && sudoku.Grid[line, column].Number == 0)
            {
                //Si il y a un seul petit chiffre dans la case, placement du chiffre
                if (sudoku.Grid[line, column].SmallNumbers.Count == 1)
                {
                    if (sudoku.Grid[line, column].EditNumber(sudoku.Grid[line, column].SmallNumbers[0]))
                    {
                        //Sudoku terminé
                        return;
                    }
                    else
                    {
                        RemoveSmallNumbers(line, column, sudoku.Grid[line, column].Number);
                    }
                }
            }

            //Passage à la prochaine case
            //Vérification de dépassement vertical
            if (line < length)
            {
                //Vérification de dépassement horizontal
                if (column < length - 1)
                {
                    //Résoud à partir de la case à droite
                    TreatSmallNumbers(line, column + 1);
                }
                //Si au bout de la ligne, changement de ligne
                else
                {
                    //Si dernière case, recommence au début
                    if (line == length - 1 && column == length - 1)
                    {
                        TreatSmallNumbers(0, 0);
                    }
                    else
                    {
                        //Résoud à partir de la première case de la ligne en dessous
                        TreatSmallNumbers(line + 1, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Enlève tous les petits chiffres impossibles si placé à ligne colonne
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="number"></param>
        private void RemoveSmallNumbers(int line, int column, byte number)
        {
            //Récupération du sommet du carré actuel
            int length = sudoku.Grid.GetLength(0);
            int xMin = (int)(Math.Floor(column / 3D) * Math.Sqrt(length));
            int yMin = (int)(Math.Floor(line / 3D) * Math.Sqrt(length));

            RemoveSmallNumbersFromLine(line, number);
            RemoveSmallNumbersFromColumn(column, number);
            RemoveSmallNumbersFromSquare(xMin, yMin, number);
        }

        /// <summary>
        /// Enlève tous les petits chiffres correspondants dans la ligne
        /// </summary>
        /// <param name="line"></param>
        /// <param name="number"></param>
        private void RemoveSmallNumbersFromLine(int line, byte number)
        {
            int length = sudoku.Grid.GetLength(0);

            //Parcours de toute la ligne
            for (int x = 0; x < length; x++)
            {
                //Si le petit chiffre est présent, le retirer
                if (sudoku.Grid[line, x].SmallNumbers.Contains(number))
                {
                    sudoku.Grid[line, x].RemoveSmallNumber(number);
                }
            }
        }

        /// <summary>
        /// Enlève tous les petits chiffres correspondants dans la colonne
        /// </summary>
        /// <param name="column"></param>
        /// <param name="number"></param>
        private void RemoveSmallNumbersFromColumn(int column, byte number)
        {
            int length = sudoku.Grid.GetLength(0);

            //Parcours de toute la colonne
            for (int y = 0; y < length; y++)
            {
                //Si le petit chiffre est présent, le retirer
                if (sudoku.Grid[y, column].SmallNumbers.Contains(number))
                {
                    sudoku.Grid[y, column].RemoveSmallNumber(number);
                }
            }
        }

        /// <summary>
        /// Enlève tous les petits chiffres correspondants dans le carré
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="number"></param>
        private void RemoveSmallNumbersFromSquare(int xMin, int yMin, byte number)
        {
            int length = (int)Math.Sqrt(sudoku.Grid.GetLength(0));

            //Parcours de tout le carré
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    //Si le petit chiffre est présent, le retirer
                    if (sudoku.Grid[y + yMin, x + xMin].SmallNumbers.Contains(number))
                    {
                        sudoku.Grid[y + yMin, x + xMin].RemoveSmallNumber(number);
                    }
                }
            }
        }


        /// <summary>
        /// Vérifie la présence d'un chiffre sur une ligne
        /// </summary>
        /// <param name="line">index de ligne</param>
        /// <param name="number">Chiffre testé</param>
        /// <returns></returns>
        private bool CheckLine(int line, byte number)
        {
            int length = sudoku.Grid.GetLength(0);

            //Parcours de toute la ligne
            for (int x = 0; x < length; x++)
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
        /// <returns></returns>
        private bool CheckColumn(int column, byte number)
        {
            int length = sudoku.Grid.GetLength(0);

            //Parcours de toute la colonne
            for (int y = 0; y < length; y++)
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
        /// <returns></returns>
        private bool CheckSquare(int xMin, int yMin, byte number)
        {
            int length = (int)Math.Sqrt(sudoku.Grid.GetLength(0));

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

///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Résolveur de sudokus

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;

namespace SudokuGame.Solver
{
    /// <summary>
    /// Résolveur de sudokus
    /// </summary>
    public class SudokuSolver
    {
        /// <summary>
        /// États de résolution
        /// </summary>
        private enum SolveState
        {
            Solved,
            Solving,
            FoundNumber,
            UnableToSolve
        }

        private Sudoku sudoku;
        private SudokuView observer;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="sudoku"></param>
        /// <param name="observer"></param>
        public SudokuSolver(Sudoku sudoku, SudokuView observer = null)
        {
            this.sudoku = sudoku;
            this.observer = observer;
        }

        /// <summary>
        /// Lance la résolution du sudoku
        /// </summary>
        public void SolveSudoku()
        {
            RecursivePlaceSmallNumbers();

            BruteForceSolve();
        }


        private SolveState BruteForceSolve()
        {
            //Essai de résolution simple
            if(RecursiveMethodicalSolveSudoku() == SolveState.Solved)
            {
                return SolveState.Solved;
            }

            //Sauvegarde de l'état du sudoku
            SudokuCell[,] copy = CopySudoku();

            //Essai avec un chiffre arbitraire
            if(TryNumber() == SolveState.Solved)
            {
                return SolveState.Solved;
            }

            //Restauration de l'état avant l'essai
            RestoreCopy(copy);
        }

        private SolveState TryNumber()
        {
            return SolveState.UnableToSolve;
        }

        private void FindBestPossibility()
        {

        }

        /// <summary>
        /// Résoud le sudoku
        /// </summary>
        /// <returns></returns>
        private SolveState RecursiveMethodicalSolveSudoku()
        {
            SolveState uniqueState = TreatUniqueSmallNumbers(0, 0, SolveState.Solving);
            SolveState lineState = TreatLineSmallNumbers(0, SolveState.Solving);
            SolveState columnState = TreatColumnSmallNumbers(0, SolveState.Solving);
            SolveState squareState = TreatSquareSmallNumbers(0, 0, SolveState.Solving);

            //Si aucun n'a rien avancé, cet algorithme n'est pas capable de résoudre le sudoku
            if (uniqueState == SolveState.UnableToSolve && lineState == SolveState.UnableToSolve && columnState == SolveState.UnableToSolve && squareState == SolveState.UnableToSolve)
            {
                return SolveState.UnableToSolve;
            }
            //Sudoku terminé
            else if (uniqueState == SolveState.Solved || lineState == SolveState.Solved || columnState == SolveState.Solved || squareState == SolveState.Solved)
            {
                return SolveState.Solved;
            }
            //Sudoku en cours
            else
            {
                return RecursiveMethodicalSolveSudoku();
            }
        }

        /// <summary>
        /// Parcours récusivement le sudoku et place les petits chiffres aux endroits possibles
        /// </summary>
        /// <param name="line">ligne</param>
        /// <param name="column">colonne</param>
        /// <param name="number">chiffre</param>
        private void RecursivePlaceSmallNumbers(int line = 0, int column = 0, byte number = 1)
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

                //Mise à jour de l'observateur
                UpdateObservers();
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
        /// Valide récursivement les petits chiffres seuls
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private SolveState TreatUniqueSmallNumbers(int line, int column, SolveState state)
        {
            int length = sudoku.Grid.GetLength(0);

            SolveState nextState = SolveState.Solving;

            //Si la case est modifiable
            if (!sudoku.Grid[line, column].IsFixed && sudoku.Grid[line, column].Number == 0)
            {
                //Si il y a un seul petit chiffre dans la case, placement du chiffre
                if (sudoku.Grid[line, column].SmallNumbers.Count == 1)
                {
                    if (sudoku.Grid[line, column].EditNumber(sudoku.Grid[line, column].SmallNumbers[0]))
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        RemoveSmallNumbers(line, column, sudoku.Grid[line, column].Number);

                        state = SolveState.FoundNumber;
                    }

                    //Mise à jour de l'observateur
                    UpdateObservers();
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
                    nextState = TreatUniqueSmallNumbers(line, column + 1, state);
                }
                //Si au bout de la ligne, changement de ligne
                else
                {
                    //Si dernière case
                    if (line == length - 1 && column == length - 1)
                    {
                        if (state != SolveState.Solving)
                        {
                            nextState = TreatUniqueSmallNumbers(0, 0, SolveState.Solving);
                        }
                    }
                    else
                    {
                        //Résoud à partir de la première case de la ligne en dessous                        
                        nextState = TreatUniqueSmallNumbers(line + 1, 0, state);
                    }
                }
            }

            //Si la suite n'a rien trouvé et que rien a été trouvé cette fois, retour que rien a été trouvé
            switch (nextState)
            {
                case SolveState.FoundNumber:
                case SolveState.Solved:
                    return nextState;
                default:
                    if (state == SolveState.Solving)
                    {
                        return SolveState.UnableToSolve;
                    }
                    else
                    {
                        return state;
                    }
            }
        }

        /// <summary>
        /// Regarde si une ligne contient un seul exemplaire d'un petit chiffre
        /// </summary>
        /// <param name="line"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private SolveState TreatLineSmallNumbers(int line, SolveState state)
        {
            int length = sudoku.Grid.GetLength(0);

            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= length; i++)
            {
                byte counter = 0;
                byte index = 0;

                //Vérification de chaque case
                for (byte x = 0; x < length; x++)
                {
                    //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                    if (!sudoku.Grid[line, x].IsFixed && sudoku.Grid[line, x].Number == 0 && sudoku.Grid[line, x].SmallNumbers.Contains(i))
                    {
                        counter++;
                        index = x;
                    }
                }

                //S'il y a une seule fois un exemplaire d'un petit chiffre, le placer
                if (counter == 1)
                {
                    if (sudoku.Grid[line, index].EditNumber(i))
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        RemoveSmallNumbers(line, index, sudoku.Grid[line, index].Number);
                        state = SolveState.FoundNumber;
                    }

                    //Mise à jour de l'observateur
                    UpdateObservers();
                }
            }

            //Passage à la prochaine ligne
            //Vérification de dépassement vertical
            if (line < length)
            {
                //Si dernière ligne, recommence au début
                if (line == length - 1)
                {
                    //Si rien n'a été trouvé, ne recommence pas
                    if (state != SolveState.Solving)
                    {
                        nextState = TreatLineSmallNumbers(0, SolveState.Solving);
                    }
                }
                else
                {
                    //Résoud la prochaine ligne
                    nextState = TreatLineSmallNumbers(line + 1, state);
                }
            }

            //Retour de l'état
            switch (nextState)
            {
                case SolveState.FoundNumber:
                case SolveState.Solved:
                    return nextState;
                default:
                    if (state == SolveState.Solving)
                    {
                        return SolveState.UnableToSolve;
                    }
                    else
                    {
                        return state;
                    }
            }
        }

        /// <summary>
        /// Regarde si une colonne contient un seul exemplaire d'un petit chiffre
        /// </summary>
        /// <param name="column"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private SolveState TreatColumnSmallNumbers(int column, SolveState state)
        {
            int length = sudoku.Grid.GetLength(0);

            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= length; i++)
            {
                byte counter = 0;
                byte index = 0;

                //Vérification de chaque case
                for (byte y = 0; y < length; y++)
                {
                    //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                    if (!sudoku.Grid[y, column].IsFixed && sudoku.Grid[y, column].Number == 0 && sudoku.Grid[y, column].SmallNumbers.Contains(i))
                    {
                        counter++;
                        index = y;
                    }
                }

                //S'il y a une seule fois un exemplaire d'un petit chiffre, le placer
                if (counter == 1)
                {
                    if (sudoku.Grid[index, column].EditNumber(i))
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        RemoveSmallNumbers(index, column, sudoku.Grid[index, column].Number);
                        state = SolveState.FoundNumber;
                    }

                    //Mise à jour de l'observateur
                    UpdateObservers();
                }
            }

            //Passage à la prochaine ligne
            //Vérification de dépassement horizontal
            if (column < length)
            {
                //Si dernière colonne, recommence au début
                if (column == length - 1)
                {
                    //Si rien n'a été trouvé, ne recommence pas
                    if (state != SolveState.Solving)
                    {
                        nextState = TreatColumnSmallNumbers(0, SolveState.Solving);
                    }
                }
                else
                {
                    //Résoud la prochaine colonne
                    nextState = TreatColumnSmallNumbers(column + 1, state);
                }
            }

            //Retour de l'état
            switch (nextState)
            {
                case SolveState.FoundNumber:
                case SolveState.Solved:
                    return nextState;
                default:
                    if (state == SolveState.Solving)
                    {
                        return SolveState.UnableToSolve;
                    }
                    else
                    {
                        return state;
                    }
            }
        }

        /// <summary>
        /// Regarde si un carré contient un seul exemplaire d'un petit chiffre
        /// </summary>
        /// <param name="xMin"></param>
        /// <param name="yMin"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        private SolveState TreatSquareSmallNumbers(int xMin, int yMin, SolveState state)
        {
            int squareLength = (int)Math.Sqrt(sudoku.Grid.GetLength(0));
            int length = sudoku.Grid.GetLength(0);

            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= length; i++)
            {
                byte counter = 0;
                int indexY = 0;
                int indexX = 0;

                //Parcours de tout le carré
                for (int y = 0; y < squareLength; y++)
                {
                    for (int x = 0; x < squareLength; x++)
                    {
                        //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                        if (!sudoku.Grid[y + yMin, x + xMin].IsFixed && sudoku.Grid[y + yMin, x + xMin].Number == 0 && sudoku.Grid[y + yMin, x + xMin].SmallNumbers.Contains(i))
                        {
                            counter++;
                            indexY = y + yMin;
                            indexX = x + xMin;
                        }
                    }
                }

                //S'il y a une seule fois un exemplaire d'un petit chiffre, le placer
                if (counter == 1)
                {
                    if (sudoku.Grid[indexY, indexX].EditNumber(i))
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        RemoveSmallNumbers(indexY, indexX, sudoku.Grid[indexY, indexX].Number);
                        state = SolveState.FoundNumber;
                    }

                    //Mise à jour de l'observateur
                    UpdateObservers();
                }
            }

            //Passage à la prochaine case
            //Vérification de dépassement vertical
            if (yMin + squareLength < length)
            {
                //Vérification de dépassement horizontal
                if (xMin + squareLength < length - 1)
                {
                    //Résoud à partir de la case à droite
                    nextState = TreatSquareSmallNumbers(yMin, xMin + squareLength, state);
                }
                //Si au bout de la ligne, changement de ligne
                else
                {
                    //Si dernière case, recommence au début
                    if (yMin + squareLength == length - 1 && xMin + squareLength == length - 1)
                    {
                        //Si rien n'a été trouvé, ne recommence pas
                        if (state != SolveState.Solving)
                        {
                            nextState = TreatSquareSmallNumbers(0, 0, SolveState.Solving);
                        }
                    }
                    else
                    {
                        //Résoud à partir de la première case de la ligne en dessous
                        nextState = TreatSquareSmallNumbers(yMin + squareLength, 0, state);
                    }
                }
            }

            //Retour de l'état
            switch (nextState)
            {
                case SolveState.FoundNumber:
                case SolveState.Solved:
                    return nextState;
                default:
                    if (state == SolveState.Solving)
                    {
                        return SolveState.UnableToSolve;
                    }
                    else
                    {
                        return state;
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

        /// <summary>
        /// Mise à jour de l'observateur
        /// </summary>
        private void UpdateObservers()
        {
            //Mise à jour de l'observateur
            if (observer != null)
            {
                observer.Invoke((MethodInvoker)(() =>
                {
                    observer.UpdateSudoku();
                }));
            }
        }

        /// <summary>
        /// Copie les chiffres du sudoku dans un tablea de bytes
        /// </summary>
        /// <returns></returns>
        private SudokuCell[,] CopySudoku()
        {
            int length = sudoku.Grid.GetLength(0);
            SudokuCell[,] copy = new SudokuCell[length, length];
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    copy[x, y] = new SudokuCell(sudoku, x, y);
                    copy[x, y].EditNumber(sudoku.Grid[x, y].Number);
                    foreach (byte smallnumber in sudoku.Grid[x, y].SmallNumbers)
                    {
                        copy[x, y].AddSmallNumber(smallnumber);
                    }
                }
            }

            return copy;
        }

        /// <summary>
        /// Restaure une copie du sudoku
        /// </summary>
        /// <param name="copy"></param>
        private void RestoreCopy(SudokuCell[,] copy)
        {
            int length = sudoku.Grid.GetLength(0);
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    if (copy[x, y].Number != sudoku.Grid[x, y].Number)
                    {
                        sudoku.Grid[x, y].EditNumber(copy[x, y].Number);
                    }

                    //Retire et restaure les petits chiffres
                    byte[] numbers = new byte[sudoku.Grid[x, y].SmallNumbers.Count];
                    sudoku.Grid[x, y].SmallNumbers.CopyTo(numbers);

                    foreach (byte number in numbers)
                    {
                        sudoku.Grid[x, y].RemoveSmallNumber(number);
                    }

                    foreach (byte smallNumber in copy[x, y].SmallNumbers)
                    {
                        sudoku.Grid[x, y].AddSmallNumber(smallNumber);
                    }
                }
            }

            UpdateObservers();
        }
    }
}

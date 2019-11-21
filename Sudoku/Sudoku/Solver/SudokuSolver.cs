///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Résolveur de sudokus

using System;
using System.Collections.Generic;
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
        /// Définis si les observateurs sont mis à jour en temps réel
        /// </summary>
        private const bool DISPLAY_REAL_TIME = false;

        /// <summary>
        /// Profondeur maximale du bruteforce (plus grand, plus lent, mais résoud des sudokus plus compliqués)
        /// </summary>
        private const int MAXIMUM_BRUTEFORCE_DEPTH = 5;

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
        public bool SolveSudoku()
        {
            PlaceSmallNumbers();

            //Mise à jour de l'observateur
            if (DISPLAY_REAL_TIME)
            {
                UpdateObservers();
            }

            return BruteForceSolve() == SolveState.Solved;
        }

        /// <summary>
        /// Résoud le sudoku en ajoutant des chiffres quand il est bloqué (Peut être lent)
        /// </summary>
        /// <returns></returns>
        private SolveState BruteForceSolve(int counter = 0, bool ignoreLimit = false)
        {
            //Essaie de résoudre
            if (RecursiveMethodicalSolveSudoku() == SolveState.Solved)
            {
                return SolveState.Solved;
            }

            //Sélectionne les cases et les ordonne par nombre de petits chiffres
            List<SudokuCell> selectedCells = new List<SudokuCell>();
            int minimumSmallNumbersCount = sudoku.Length;
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (!cell.IsFixed && cell.Number == 0 && cell.SmallNumbers.Count < minimumSmallNumbersCount)
                {
                    minimumSmallNumbersCount = cell.SmallNumbers.Count;
                }
            }
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (cell.SmallNumbers.Count == minimumSmallNumbersCount && !cell.IsFixed && cell.Number == 0)
                {
                    selectedCells.Add(cell);
                }
            }

            //Essaie pour chaque case sélectionnée chaque possibilité
            foreach (SudokuCell cell in selectedCells)
            {
                //Si la case est testable
                if ((counter < MAXIMUM_BRUTEFORCE_DEPTH || ignoreLimit) && !cell.IsFixed && cell.Number == 0)
                {
                    //Essaie chaque chiffre
                    for (int index = 0; index < cell.SmallNumbers.Count; index++)
                    {
                        //Sauvegarde le sudoku
                        SudokuCell[,] copy = CopySudoku();

                        //Place le chiffre
                        cell.EditNumber(cell.SmallNumbers[index]);

                        //Mise à jour de l'observateur
                        if (DISPLAY_REAL_TIME)
                        {
                            UpdateObservers();
                        }

                        //Replace les petits chiffres
                        RemoveAllSmallNumbers();
                        PlaceSmallNumbers();

                        //Essai de résolution simple
                        if (BruteForceSolve(counter + 1, ignoreLimit) == SolveState.Solved)
                        {
                            return SolveState.Solved;
                        }

                        //Enlève le chiffre
                        cell.EditNumber(0);

                        //Restaure la copie
                        RestoreCopy(copy);

                        //Mise à jour de l'observateur
                        if (DISPLAY_REAL_TIME)
                        {
                            UpdateObservers();
                        }
                    }
                }
            }

            return SolveState.UnableToSolve;
        }

        /// <summary>
        /// Résoud le sudoku
        /// </summary>
        /// <returns></returns>
        private SolveState RecursiveMethodicalSolveSudoku()
        {
            SolveState uniqueState = TreatUniqueSmallNumbers();
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
        /// Regarde si un sudoku est résolvable (sans bruteforce)
        /// </summary>
        /// <returns></returns>
        public bool IsSudokuSolvable()
        {
            bool result = false;

            SudokuCell[,] sudoku = CopySudoku();

            result = this.sudoku.IsCompleted() ? true : RecursiveMethodicalSolveSudoku() == SolveState.Solved;

            RestoreCopy(sudoku);

            return result;
        }

        /// <summary>
        /// Regarde si un sudoku est résolvable (sans bruteforce)
        /// </summary>
        /// <returns></returns>
        public bool IsSudokuSolvableWithBruteForce()
        {
            bool result = false;

            SudokuCell[,] sudoku = CopySudoku();

            PlaceSmallNumbers();

            result = this.sudoku.IsCompleted() ? true : BruteForceSolve(0, true) == SolveState.Solved;

            RestoreCopy(sudoku);

            return result;
        }

        /// <summary>
        /// Parcours le sudoku et place les petits chiffres aux endroits possibles
        /// </summary>
        private void PlaceSmallNumbers()
        {
            int squareLength = (int)Math.Sqrt(sudoku.Length);

            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (!cell.IsFixed && cell.Number == 0)
                {
                    int column = cell.X;
                    int line = cell.Y;

                    //Récupération du sommet du carré actuel
                    int xMin = (int)(Math.Floor(column / 3D) * squareLength);
                    int yMin = (int)(Math.Floor(line / 3D) * squareLength);

                    for (byte number = 1; number <= sudoku.Length; number++)
                    {
                        //Si le chiffre peut être placé ici
                        if (CheckLine(line, number) && CheckColumn(column, number) && CheckSquare(xMin, yMin, number))
                        {
                            //Ajout du petit chiffre
                            cell.AddSmallNumber(number);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Efface tous les petits chiffres
        /// </summary>
        private void RemoveAllSmallNumbers()
        {
            foreach (SudokuCell cell in sudoku.Grid)
            {
                if (!cell.IsFixed && cell.Number == 0)
                {
                    cell.SmallNumbers.Clear();
                }
            }
        }

        /// <summary>
        /// Valide récursivement les petits chiffres seuls
        /// </summary>
        /// <returns></returns>
        private SolveState TreatUniqueSmallNumbers()
        {
            SolveState finalState = SolveState.Solving;

            foreach (SudokuCell cell in sudoku.Grid)
            {

                SolveState cellState = SolveState.Solving;

                //Si la case est modifiable
                if (!cell.IsFixed && cell.Number == 0)
                {
                    //Si il y a un seul petit chiffre dans la case, placement du chiffre
                    if (cell.SmallNumbers.Count == 1)
                    {
                        cell.EditNumber(cell.SmallNumbers[0]);
                        if (sudoku.IsCompleted())
                        {
                            //Mise à jour de l'observateur
                            UpdateObservers();

                            //Sudoku terminé
                            return SolveState.Solved;
                        }
                        else
                        {
                            RemoveSmallNumbers(cell.Y, cell.X, cell.Number);

                            //Mise à jour de l'observateur
                            if (DISPLAY_REAL_TIME)
                            {
                                UpdateObservers();
                            }

                            cellState = SolveState.FoundNumber;
                        }
                    }
                }

                //Si la suite n'a rien trouvé et que rien a été trouvé cette fois, retour que rien a été trouvé
                switch (cellState)
                {
                    case SolveState.FoundNumber:
                    case SolveState.Solved:
                        finalState = cellState;
                        break;
                }
            }
            
            if(finalState == SolveState.Solving)
            {
                return SolveState.UnableToSolve;
            }
            else
            {
                return finalState;
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
            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= sudoku.Length; i++)
            {
                byte counter = 0;
                byte index = 0;

                //Vérification de chaque case
                for (byte x = 0; x < sudoku.Length; x++)
                {
                    SudokuCell cell = sudoku.Grid[line, x];

                    //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                    if (!cell.IsFixed && cell.Number == 0 && cell.SmallNumbers.Contains(i))
                    {
                        counter++;
                        index = x;
                    }
                }

                //S'il y a une seule fois un exemplaire d'un petit chiffre, le placer
                if (counter == 1)
                {
                    sudoku.Grid[line, index].EditNumber(i);
                    if (sudoku.IsCompleted())
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        //Enlève les petits chiffres devenus impossibles
                        RemoveSmallNumbers(line, index, sudoku.Grid[line, index].Number);

                        //Mise à jour de l'observateur
                        if (DISPLAY_REAL_TIME)
                        {
                            UpdateObservers();
                        }

                        state = SolveState.FoundNumber;
                    }
                }
            }

            //Passage à la prochaine ligne
            //Vérification de dépassement vertical
            if (line < sudoku.Length)
            {
                //Si dernière ligne, recommence au début
                if (line == sudoku.Length - 1)
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
            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= sudoku.Length; i++)
            {
                byte counter = 0;
                byte index = 0;

                //Vérification de chaque case
                for (byte y = 0; y < sudoku.Length; y++)
                {
                    SudokuCell cell = sudoku.Grid[y, column];
                    //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                    if (!cell.IsFixed && cell.Number == 0 && cell.SmallNumbers.Contains(i))
                    {
                        counter++;
                        index = y;
                    }
                }

                //S'il y a une seule fois un exemplaire d'un petit chiffre, le placer
                if (counter == 1)
                {
                    sudoku.Grid[index, column].EditNumber(i);
                    if (sudoku.IsCompleted())
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        //Enlève les petits chiffres devenus impossibles
                        RemoveSmallNumbers(index, column, sudoku.Grid[index, column].Number);

                        //Mise à jour de l'observateur
                        if (DISPLAY_REAL_TIME)
                        {
                            UpdateObservers();
                        }

                        state = SolveState.FoundNumber;
                    }
                }
            }

            //Passage à la prochaine ligne
            //Vérification de dépassement horizontal
            if (column < sudoku.Length)
            {
                //Si dernière colonne, recommence au début
                if (column == sudoku.Length - 1)
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
            int squareLength = (int)Math.Sqrt(sudoku.Length);

            SolveState nextState = SolveState.Solving;

            //Pour chaque chiffre
            for (byte i = 1; i <= sudoku.Length; i++)
            {
                byte counter = 0;
                int indexY = 0;
                int indexX = 0;

                //Parcours de tout le carré
                for (int y = 0; y < squareLength; y++)
                {
                    for (int x = 0; x < squareLength; x++)
                    {
                        SudokuCell cell = sudoku.Grid[y + yMin, x + xMin];

                        //Si la case n'est pas fixe, n'a pas de chiffre placé et contient le chiffre recherché, incrémentation du compteur
                        if (!cell.IsFixed && cell.Number == 0 && cell.SmallNumbers.Contains(i))
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
                    sudoku.Grid[indexY, indexX].EditNumber(i);
                    if (sudoku.IsCompleted())
                    {
                        //Mise à jour de l'observateur
                        UpdateObservers();

                        //Sudoku terminé
                        return SolveState.Solved;
                    }
                    else
                    {
                        //Enlève les petits chiffres devenus impossibles
                        RemoveSmallNumbers(indexY, indexX, sudoku.Grid[indexY, indexX].Number);

                        //Mise à jour de l'observateur
                        if (DISPLAY_REAL_TIME)
                        {
                            UpdateObservers();
                        }

                        state = SolveState.FoundNumber;
                    }
                }
            }

            //Passage à la prochaine case
            //Vérification de dépassement vertical
            if (yMin + squareLength < sudoku.Length)
            {
                //Vérification de dépassement horizontal
                if (xMin + squareLength < sudoku.Length - 1)
                {
                    //Résoud à partir de la case à droite
                    nextState = TreatSquareSmallNumbers(yMin, xMin + squareLength, state);
                }
                //Si au bout de la ligne, changement de ligne
                else
                {
                    //Si dernière case, recommence au début
                    if (yMin + squareLength == sudoku.Length - 1 && xMin + squareLength == sudoku.Length - 1)
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
            int xMin = (int)(Math.Floor(column / 3D) * Math.Sqrt(sudoku.Length));
            int yMin = (int)(Math.Floor(line / 3D) * Math.Sqrt(sudoku.Length));

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
            //Parcours de toute la ligne
            for (int x = 0; x < sudoku.Length; x++)
            {
                SudokuCell cell = sudoku.Grid[line, x];

                //Si le petit chiffre est présent, le retirer
                if (cell.SmallNumbers.Contains(number))
                {
                    cell.RemoveSmallNumber(number);
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
            //Parcours de toute la colonne
            for (int y = 0; y < sudoku.Length; y++)
            {
                SudokuCell cell = sudoku.Grid[y, column];

                //Si le petit chiffre est présent, le retirer
                if (cell.SmallNumbers.Contains(number))
                {
                    cell.RemoveSmallNumber(number);
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
            int length = (int)Math.Sqrt(sudoku.Length);

            //Parcours de tout le carré
            for (int y = 0; y < length; y++)
            {
                for (int x = 0; x < length; x++)
                {
                    SudokuCell cell = sudoku.Grid[y + yMin, x + xMin];

                    //Si le petit chiffre est présent, le retirer
                    if (cell.SmallNumbers.Contains(number))
                    {
                        cell.RemoveSmallNumber(number);
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
        /// <returns></returns>
        private bool CheckColumn(int column, byte number)
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
        /// <returns></returns>
        private bool CheckSquare(int xMin, int yMin, byte number)
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
            //Copie du sudoku
            SudokuCell[,] copy = new SudokuCell[sudoku.Length, sudoku.Length];

            //Copie de chaque case vers la copie
            for (int x = 0; x < sudoku.Length; x++)
            {
                for (int y = 0; y < sudoku.Length; y++)
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
            for (int x = 0; x < sudoku.Length; x++)
            {
                for (int y = 0; y < sudoku.Length; y++)
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
        }
    }
}

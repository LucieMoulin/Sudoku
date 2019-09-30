///ETML
///Auteur : Lucie Moulin
///Date : 19.09.2019
///Description : Générateur de sudokus

using SudokuGame.SudokuObjects;

namespace SudokuGame.Generator
{
    /// <summary>
    /// Générateur de sudokus
    /// </summary>
    public class SudokuGenerator
    {
        public Sudoku NewRandomSudoku()
        {
            Sudoku sudoku = new Sudoku();

            //SUDOKU DE TESTS
            sudoku.Grid[0, 2].EditNumber(1);
            sudoku.Grid[0, 2].IsFixed = true;

            sudoku.Grid[0, 3].EditNumber(4);
            sudoku.Grid[0, 3].IsFixed = true;

            sudoku.Grid[0, 4].EditNumber(7);
            sudoku.Grid[0, 4].IsFixed = true;

            sudoku.Grid[0, 5].EditNumber(8);
            sudoku.Grid[0, 5].IsFixed = true;

            sudoku.Grid[0, 8].EditNumber(9);
            sudoku.Grid[0, 8].IsFixed = true;

            sudoku.Grid[1, 7].EditNumber(6);
            sudoku.Grid[1, 7].IsFixed = true;

            sudoku.Grid[1, 8].EditNumber(3);
            sudoku.Grid[1, 8].IsFixed = true;

            sudoku.Grid[2, 6].EditNumber(4);
            sudoku.Grid[2, 6].IsFixed = true;

            sudoku.Grid[3, 0].EditNumber(7);
            sudoku.Grid[3, 0].IsFixed = true;

            sudoku.Grid[3, 1].EditNumber(8);
            sudoku.Grid[3, 1].IsFixed = true;

            sudoku.Grid[3, 4].EditNumber(5);
            sudoku.Grid[3, 4].IsFixed = true;

            sudoku.Grid[3, 5].EditNumber(9);
            sudoku.Grid[3, 5].IsFixed = true;

            sudoku.Grid[4, 4].EditNumber(8);
            sudoku.Grid[4, 4].IsFixed = true;

            sudoku.Grid[4, 6].EditNumber(3);
            sudoku.Grid[4, 6].IsFixed = true;

            sudoku.Grid[5, 1].EditNumber(1);
            sudoku.Grid[5, 1].IsFixed = true;

            sudoku.Grid[5, 2].EditNumber(3);
            sudoku.Grid[5, 2].IsFixed = true;

            sudoku.Grid[6, 5].EditNumber(3);
            sudoku.Grid[6, 5].IsFixed = true;

            sudoku.Grid[6, 6].EditNumber(2);
            sudoku.Grid[6, 6].IsFixed = true;

            sudoku.Grid[6, 8].EditNumber(7);
            sudoku.Grid[6, 8].IsFixed = true;

            sudoku.Grid[7, 0].EditNumber(5);
            sudoku.Grid[7, 0].IsFixed = true;

            sudoku.Grid[7, 1].EditNumber(4);
            sudoku.Grid[7, 1].IsFixed = true;

            sudoku.Grid[8, 1].EditNumber(9);
            sudoku.Grid[8, 1].IsFixed = true;

            sudoku.Grid[8, 3].EditNumber(1);
            sudoku.Grid[8, 3].IsFixed = true;

            return sudoku;
        }
    }
}

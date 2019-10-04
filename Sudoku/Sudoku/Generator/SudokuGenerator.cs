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
            sudoku.Grid[0, 0].EditNumber(5);
            sudoku.Grid[0, 0].IsFixed = true;

            sudoku.Grid[0, 1].EditNumber(3);
            sudoku.Grid[0, 1].IsFixed = true;

            sudoku.Grid[0, 4].EditNumber(7);
            sudoku.Grid[0, 4].IsFixed = true;

            sudoku.Grid[1, 0].EditNumber(6);
            sudoku.Grid[1, 0].IsFixed = true;

            sudoku.Grid[1, 3].EditNumber(1);
            sudoku.Grid[1, 3].IsFixed = true;

            sudoku.Grid[1, 4].EditNumber(9);
            sudoku.Grid[1, 4].IsFixed = true;

            sudoku.Grid[1, 5].EditNumber(5);
            sudoku.Grid[1, 5].IsFixed = true;

            sudoku.Grid[2, 1].EditNumber(9);
            sudoku.Grid[2, 1].IsFixed = true;

            sudoku.Grid[2, 2].EditNumber(8);
            sudoku.Grid[2, 2].IsFixed = true;

            sudoku.Grid[2, 7].EditNumber(6);
            sudoku.Grid[2, 7].IsFixed = true;

            sudoku.Grid[3, 0].EditNumber(8);
            sudoku.Grid[3, 0].IsFixed = true;

            sudoku.Grid[3, 4].EditNumber(6);
            sudoku.Grid[3, 4].IsFixed = true;

            sudoku.Grid[3, 8].EditNumber(3);
            sudoku.Grid[3, 8].IsFixed = true;

            sudoku.Grid[4, 0].EditNumber(4);
            sudoku.Grid[4, 0].IsFixed = true;

            sudoku.Grid[4, 3].EditNumber(8);
            sudoku.Grid[4, 3].IsFixed = true;

            sudoku.Grid[4, 5].EditNumber(3);
            sudoku.Grid[4, 5].IsFixed = true;

            sudoku.Grid[4, 8].EditNumber(1);
            sudoku.Grid[4, 8].IsFixed = true;

            sudoku.Grid[5, 0].EditNumber(7);
            sudoku.Grid[5, 0].IsFixed = true;

            sudoku.Grid[5, 4].EditNumber(2);
            sudoku.Grid[5, 4].IsFixed = true;

            sudoku.Grid[5, 8].EditNumber(6);
            sudoku.Grid[5, 8].IsFixed = true;

            sudoku.Grid[6, 1].EditNumber(6);
            sudoku.Grid[6, 1].IsFixed = true;

            sudoku.Grid[6, 6].EditNumber(2);
            sudoku.Grid[6, 6].IsFixed = true;

            sudoku.Grid[6, 7].EditNumber(8);
            sudoku.Grid[6, 7].IsFixed = true;

            sudoku.Grid[7, 3].EditNumber(4);
            sudoku.Grid[7, 3].IsFixed = true;

            sudoku.Grid[7, 4].EditNumber(1);
            sudoku.Grid[7, 4].IsFixed = true;

            sudoku.Grid[7, 5].EditNumber(9);
            sudoku.Grid[7, 5].IsFixed = true;

            sudoku.Grid[7, 8].EditNumber(5);
            sudoku.Grid[7, 8].IsFixed = true;

            sudoku.Grid[8, 4].EditNumber(8);
            sudoku.Grid[8, 4].IsFixed = true;

            sudoku.Grid[8, 7].EditNumber(7);
            sudoku.Grid[8, 7].IsFixed = true;

            sudoku.Grid[8, 8].EditNumber(9);
            sudoku.Grid[8, 8].IsFixed = true;

            return sudoku;
        }
    }
}

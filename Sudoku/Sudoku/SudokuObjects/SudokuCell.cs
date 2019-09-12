///ETML
///Auteur : Lucie Moulin
///Date : 05.09.2019
///Description : Représente une case de la grille du sudoku

using System.Collections.Generic;

namespace SudokuGame.SudokuObjects
{
    /// <summary>
    /// Représente une case de la grille du sudoku
    /// </summary>
    public class SudokuCell
    {
        private byte number;
        private List<byte> smallNumbers;
        private Sudoku parent;
        private byte maxNumber;

        /// <summary>
        /// Chiffre principal de la cellule
        /// </summary>
        public byte Number { get => number; }

        /// <summary>
        /// Petits chiffres dans la cellule
        /// </summary>
        public List<byte> SmallNumbers { get => smallNumbers; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public SudokuCell(Sudoku parent)
        {
            this.parent = parent;
            smallNumbers = new List<byte>();
            number = 0;
            switch (parent.Type)
            {
                default:
                case SudokuType.Numeric9:
                    maxNumber = 9;
                    break;

                case SudokuType.Numeric4:
                    maxNumber = 4;
                    break;
            }
        }

        /// <summary>
        /// Entre le chiffre principal
        /// </summary>
        /// <param name="number"></param>
        public void EditNumber(byte number)
        {
            if (number <= maxNumber)
            {
                this.number = number;
            }
        }

        /// <summary>
        /// Entre un petit chiffre
        /// </summary>
        /// <param name="number"></param>
        public void AddSmallNumber(byte number)
        {
            if (number <= maxNumber && !smallNumbers.Contains(number))
            {
                smallNumbers.Add(number);
            }
        }

        /// <summary>
        /// Retire un petit chiffre
        /// </summary>
        /// <param name="number"></param>
        public void RemoveSmallNumber(byte number)
        {
            if(smallNumbers.Contains(number))
            {
                smallNumbers.Remove(number);
            }
        }
    }
}

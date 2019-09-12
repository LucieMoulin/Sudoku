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
        private bool isFixed;
        private int x, y;


        /// <summary>
        /// Chiffre principal de la cellule
        /// </summary>
        public byte Number { get => number; }

        /// <summary>
        /// Petits chiffres dans la cellule
        /// </summary>
        public List<byte> SmallNumbers { get => smallNumbers; }

        /// <summary>
        /// Définis si la case est modifiable ou pas
        /// </summary>
        public bool IsFixed { get => isFixed; set => isFixed = value; }

        /// <summary>
        /// Suméro de colonne
        /// </summary>
        public int X { get => x; }

        /// <summary>
        /// Numéro de ligne
        /// </summary>
        public int Y { get => y; }

        /// <summary>
        /// Constructeur
        /// </summary>
        public SudokuCell(Sudoku parent, int x, int y)
        {
            isFixed = false;

            this.parent = parent;
            this.x = x;
            this.y = y;
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
            if (!isFixed && number <= maxNumber)
            {
                this.number = number;
                parent.IsCompleted();
            }
        }

        /// <summary>
        /// Entre un petit chiffre
        /// </summary>
        /// <param name="number"></param>
        public void AddSmallNumber(byte number)
        {
            if (!isFixed && number <= maxNumber && !smallNumbers.Contains(number))
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
            if(!isFixed && smallNumbers.Contains(number))
            {
                smallNumbers.Remove(number);
            }
        }
    }
}

///ETML
///Auteur : Lucie Moulin
///Date : 05.09.2019
///Description : Représente une case de la grille du sudoku

using System;
using System.Collections.Generic;

namespace SudokuGame.SudokuObjects
{
    /// <summary>
    /// Représente une case de la grille du sudoku
    /// </summary>
    [Serializable]
    public class SudokuCell
    {
        private byte number;
        private List<byte> smallNumbers;
        private bool isFixed;
        private int x, y;


        /// <summary>
        /// Chiffre principal de la cellule
        /// </summary>
        public byte Number { get => number; set => number = value; }

        /// <summary>
        /// Petits chiffres dans la cellule
        /// </summary>
        public List<byte> SmallNumbers { get => smallNumbers; set => smallNumbers = value; }

        /// <summary>
        /// Définis si la case est modifiable ou pas
        /// </summary>
        public bool IsFixed { get => isFixed; set => isFixed = value; }

        /// <summary>
        /// Suméro de colonne
        /// </summary>
        public int X { get => x; set => x = value; }

        /// <summary>
        /// Numéro de ligne
        /// </summary>
        public int Y { get => y; set => y = value; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public SudokuCell()
        {

        }

        /// <summary>
        /// Constructeur
        /// </summary>
        public SudokuCell(Sudoku parent, int x, int y)
        {
            isFixed = false;
            
            this.x = x;
            this.y = y;
            smallNumbers = new List<byte>();
            number = 0;
        }

        /// <summary>
        /// Entre le chiffre principal
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Sudoku terminé</returns>
        public void EditNumber(byte number)
        {
            if (!isFixed)
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
            if (!isFixed && !smallNumbers.Contains(number))
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
            if (!isFixed && smallNumbers.Contains(number))
            {
                smallNumbers.Remove(number);
            }
        }
    }
}

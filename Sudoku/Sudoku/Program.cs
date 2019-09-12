///ETML
///Auteur : Lucie Moulin
///Date : 29.08.2019
///Description : programme principal de l'application sudoku

using System;
using System.Windows.Forms;

namespace SudokuGame
{
    /// <summary>
    /// Programme principal
    /// </summary>
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SudokuView());
        }
    }
}

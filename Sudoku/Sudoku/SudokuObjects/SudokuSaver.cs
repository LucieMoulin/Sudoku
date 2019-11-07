///ETML
///Auteur : Lucie Moulin
///Date : 05.09.2019
///Description : Sauvegardeur de partie

using Newtonsoft.Json;
using System;
using System.IO;

namespace SudokuGame.SudokuObjects
{
    /// <summary>
    /// Sauvegardeur de partie
    /// </summary>
    public static class SudokuSaver
    {
        /// <summary>
        /// Charge un sudoku sauvegardé
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static SudokuCell[,] Load(string fileName)
        {
            TextReader reader = null;

            try
            {
                reader = new StreamReader(fileName);
                string fileContents = reader.ReadToEnd();
                SudokuCell[,] grid = JsonConvert.DeserializeObject<SudokuCell[,]>(fileContents);
                return grid;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Sauvegarde un sudoku
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sudoku"></param>
        /// <returns>sauvegarde réussie</returns>
        public static bool Save(string fileName, Sudoku sudoku)
        {
            TextWriter writer = null;
            try
            {
                string contentsToWriteToFile = JsonConvert.SerializeObject(sudoku.Grid);
                writer = new StreamWriter(fileName);
                writer.Write(contentsToWriteToFile);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }
}

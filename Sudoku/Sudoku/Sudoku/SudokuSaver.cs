///ETML
///Auteur : Lucie Moulin
///Date : 05.09.2019
///Description : Sauvegardeur de partie

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Sudoku.Sudoku
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
        public static Sudoku Load(string fileName)
        {
            Sudoku sudokuOut = default(Sudoku);

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(fileName);
                string xmlString = document.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(Sudoku);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        sudokuOut = (Sudoku)serializer.Deserialize(reader);
                    }
                }
            }
            catch { }

            return sudokuOut;
        }

        /// <summary>
        /// Sauvegarde un sudoku
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sudoku"></param>
        /// <returns>sauvegarde réussie</returns>
        public static bool Save(string fileName, Sudoku sudoku)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(sudoku.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, sudoku);
                    stream.Position = 0;
                    document.Load(stream);
                    document.Save(fileName);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

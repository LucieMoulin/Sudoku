using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SudokuGame.SudokuObjects;

namespace SudokuGame
{
    public partial class SudokuView : Form
    {
        private Sudoku sudoku;


        public SudokuView()
        {
            InitializeComponent();

            sudoku = new Sudoku();
        }
    }
}

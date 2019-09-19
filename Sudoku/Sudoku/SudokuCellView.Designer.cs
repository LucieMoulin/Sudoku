namespace SudokuGame
{
    partial class SudokuCellView
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelNumber = new System.Windows.Forms.Label();
            this.labelSmallNumbers1 = new System.Windows.Forms.Label();
            this.labelSmallNumbers2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNumber
            // 
            this.labelNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Bold);
            this.labelNumber.Location = new System.Drawing.Point(3, 0);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(44, 50);
            this.labelNumber.TabIndex = 0;
            this.labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelNumber.Click += new System.EventHandler(this.LabelNumber_Click);
            // 
            // labelSmallNumbers1
            // 
            this.labelSmallNumbers1.Location = new System.Drawing.Point(-1, 0);
            this.labelSmallNumbers1.Name = "labelSmallNumbers1";
            this.labelSmallNumbers1.Size = new System.Drawing.Size(52, 15);
            this.labelSmallNumbers1.TabIndex = 1;
            this.labelSmallNumbers1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSmallNumbers1.Click += new System.EventHandler(this.LabelSmallNumbers1_Click);
            // 
            // labelSmallNumbers2
            // 
            this.labelSmallNumbers2.Location = new System.Drawing.Point(24, 14);
            this.labelSmallNumbers2.Name = "labelSmallNumbers2";
            this.labelSmallNumbers2.Size = new System.Drawing.Size(27, 30);
            this.labelSmallNumbers2.TabIndex = 2;
            this.labelSmallNumbers2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSmallNumbers2.Click += new System.EventHandler(this.LabelSmallNumbers2_Click);
            // 
            // SudokuCellView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.labelSmallNumbers2);
            this.Controls.Add(this.labelSmallNumbers1);
            this.Controls.Add(this.labelNumber);
            this.Name = "SudokuCellView";
            this.Size = new System.Drawing.Size(50, 50);
            this.Click += new System.EventHandler(this.SudokuCellView_Click);
            this.Enter += new System.EventHandler(this.SudokuCellView_Enter);
            this.Leave += new System.EventHandler(this.SudokuCellView_Leave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelNumber;
        private System.Windows.Forms.Label labelSmallNumbers1;
        private System.Windows.Forms.Label labelSmallNumbers2;
    }
}

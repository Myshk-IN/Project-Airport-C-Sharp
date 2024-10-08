namespace project_Airport
{
    partial class Arrivals
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlArrivals = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlArrivals
            // 
            this.pnlArrivals.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlArrivals.Location = new System.Drawing.Point(0, 0);
            this.pnlArrivals.Name = "pnlArrivals";
            this.pnlArrivals.Size = new System.Drawing.Size(800, 450);
            this.pnlArrivals.TabIndex = 0;
            // 
            // Arrivals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pnlArrivals);
            this.Name = "Arrivals";
            this.Text = "Arrivals";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlArrivals;
    }
}
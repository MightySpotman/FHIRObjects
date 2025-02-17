namespace WinFormsTestApp
{
    partial class CodeTester
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
            btnTest1 = new Button();
            txtTest1 = new TextBox();
            SuspendLayout();
            // 
            // btnTest1
            // 
            btnTest1.Location = new Point(57, 35);
            btnTest1.Name = "btnTest1";
            btnTest1.Size = new Size(289, 100);
            btnTest1.TabIndex = 0;
            btnTest1.Text = "button1";
            btnTest1.UseVisualStyleBackColor = true;
            btnTest1.Click += btnTest1_Click;
            // 
            // txtTest1
            // 
            txtTest1.Location = new Point(397, 39);
            txtTest1.Multiline = true;
            txtTest1.Name = "txtTest1";
            txtTest1.Size = new Size(698, 805);
            txtTest1.TabIndex = 1;
            // 
            // CodeTester
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1181, 871);
            Controls.Add(txtTest1);
            Controls.Add(btnTest1);
            Name = "CodeTester";
            Text = "CodeTester";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnTest1;
        private TextBox txtTest1;
    }
}
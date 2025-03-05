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
            txtCcd = new TextBox();
            txtFhir = new TextBox();
            btnClearTextboxes = new Button();
            btnCheckForValidFiles = new Button();
            SuspendLayout();
            // 
            // btnTest1
            // 
            btnTest1.Location = new Point(22, 39);
            btnTest1.Name = "btnTest1";
            btnTest1.Size = new Size(289, 100);
            btnTest1.TabIndex = 0;
            btnTest1.Text = "Select CCD XML && Convert";
            btnTest1.UseVisualStyleBackColor = true;
            btnTest1.Click += btnTest1_Click;
            // 
            // txtCcd
            // 
            txtCcd.Location = new Point(327, 39);
            txtCcd.Multiline = true;
            txtCcd.Name = "txtCcd";
            txtCcd.ScrollBars = ScrollBars.Both;
            txtCcd.Size = new Size(698, 805);
            txtCcd.TabIndex = 1;
            txtCcd.WordWrap = false;
            // 
            // txtFhir
            // 
            txtFhir.Location = new Point(1047, 39);
            txtFhir.Multiline = true;
            txtFhir.Name = "txtFhir";
            txtFhir.ScrollBars = ScrollBars.Both;
            txtFhir.Size = new Size(698, 805);
            txtFhir.TabIndex = 2;
            txtFhir.WordWrap = false;
            // 
            // btnClearTextboxes
            // 
            btnClearTextboxes.Location = new Point(22, 161);
            btnClearTextboxes.Name = "btnClearTextboxes";
            btnClearTextboxes.Size = new Size(289, 100);
            btnClearTextboxes.TabIndex = 3;
            btnClearTextboxes.Text = "Clear Textboxes";
            btnClearTextboxes.UseVisualStyleBackColor = true;
            btnClearTextboxes.Click += btnClearTextboxes_Click;
            // 
            // btnCheckForValidFiles
            // 
            btnCheckForValidFiles.Location = new Point(22, 370);
            btnCheckForValidFiles.Name = "btnCheckForValidFiles";
            btnCheckForValidFiles.Size = new Size(289, 100);
            btnCheckForValidFiles.TabIndex = 4;
            btnCheckForValidFiles.Text = "Check for Valid CCDA";
            btnCheckForValidFiles.UseVisualStyleBackColor = true;
            btnCheckForValidFiles.Click += button1_Click;
            // 
            // CodeTester
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1842, 871);
            Controls.Add(btnCheckForValidFiles);
            Controls.Add(btnClearTextboxes);
            Controls.Add(txtFhir);
            Controls.Add(txtCcd);
            Controls.Add(btnTest1);
            Name = "CodeTester";
            Text = "XML CCD --> JSON FHIR Bundle Conversion";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnTest1;
        private TextBox txtCcd;
        private TextBox txtFhir;
        private Button btnClearTextboxes;
        private Button btnCheckForValidFiles;
    }
}
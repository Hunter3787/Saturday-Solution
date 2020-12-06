namespace AutoBuildApp.WindowsForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.LastNameText = new System.Windows.Forms.TextBox();
            this.LastNameLabel = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.FirstNameText = new System.Windows.Forms.TextBox();
            this.FirstNameLabel = new System.Windows.Forms.Label();
            this.RoleLabel = new System.Windows.Forms.Label();
            this.RoleText = new System.Windows.Forms.TextBox();
            this.dg = new System.Windows.Forms.DataGridView();
            this.serach = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dg)).BeginInit();
            this.SuspendLayout();
            // 
            // LastNameText
            // 
            this.LastNameText.Location = new System.Drawing.Point(203, 60);
            this.LastNameText.Name = "LastNameText";
            this.LastNameText.Size = new System.Drawing.Size(157, 45);
            this.LastNameText.TabIndex = 1;
            this.LastNameText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // LastNameLabel
            // 
            this.LastNameLabel.AutoSize = true;
            this.LastNameLabel.Location = new System.Drawing.Point(219, 9);
            this.LastNameLabel.Name = "LastNameLabel";
            this.LastNameLabel.Size = new System.Drawing.Size(139, 38);
            this.LastNameLabel.TabIndex = 2;
            this.LastNameLabel.Text = "LastName";
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(699, 53);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(200, 52);
            this.SearchButton.TabIndex = 3;
            this.SearchButton.Text = "AddButton";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // FirstNameText
            // 
            this.FirstNameText.Location = new System.Drawing.Point(23, 60);
            this.FirstNameText.Name = "FirstNameText";
            this.FirstNameText.Size = new System.Drawing.Size(174, 45);
            this.FirstNameText.TabIndex = 1;
            this.FirstNameText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // FirstNameLabel
            // 
            this.FirstNameLabel.AutoSize = true;
            this.FirstNameLabel.Location = new System.Drawing.Point(23, 9);
            this.FirstNameLabel.Name = "FirstNameLabel";
            this.FirstNameLabel.Size = new System.Drawing.Size(143, 38);
            this.FirstNameLabel.TabIndex = 2;
            this.FirstNameLabel.Text = "FirstName";
            this.FirstNameLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // RoleLabel
            // 
            this.RoleLabel.AutoSize = true;
            this.RoleLabel.Location = new System.Drawing.Point(394, 9);
            this.RoleLabel.Name = "RoleLabel";
            this.RoleLabel.Size = new System.Drawing.Size(71, 38);
            this.RoleLabel.TabIndex = 2;
            this.RoleLabel.Text = "Role";
            // 
            // RoleText
            // 
            this.RoleText.Location = new System.Drawing.Point(394, 60);
            this.RoleText.Name = "RoleText";
            this.RoleText.Size = new System.Drawing.Size(137, 45);
            this.RoleText.TabIndex = 1;
            this.RoleText.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // dg
            // 
            this.dg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg.Location = new System.Drawing.Point(23, 111);
            this.dg.Name = "dg";
            this.dg.RowHeadersWidth = 62;
            this.dg.Size = new System.Drawing.Size(649, 365);
            this.dg.TabIndex = 4;
            this.dg.Text = "dataGridView1";
            // 
            // serach
            // 
            this.serach.Location = new System.Drawing.Point(699, 131);
            this.serach.Name = "serach";
            this.serach.Size = new System.Drawing.Size(200, 56);
            this.serach.TabIndex = 5;
            this.serach.Text = "search";
            this.serach.UseVisualStyleBackColor = true;
            this.serach.Click += new System.EventHandler(this.serach_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 38F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1155, 684);
            this.Controls.Add(this.serach);
            this.Controls.Add(this.dg);
            this.Controls.Add(this.RoleText);
            this.Controls.Add(this.RoleLabel);
            this.Controls.Add(this.FirstNameLabel);
            this.Controls.Add(this.FirstNameText);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.LastNameLabel);
            this.Controls.Add(this.LastNameText);
            this.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Data Access Demo";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox LastNameText;
        private System.Windows.Forms.Label LastNameLabel;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.TextBox FirstNameText;
        private System.Windows.Forms.Label FirstNameLabel;
        private System.Windows.Forms.Label RoleLabel;
        private System.Windows.Forms.TextBox RoleText;
        private System.Windows.Forms.DataGridView dg;
        private System.Windows.Forms.Button serach;
    }
}


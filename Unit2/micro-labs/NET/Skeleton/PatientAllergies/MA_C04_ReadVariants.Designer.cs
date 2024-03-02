namespace FhirIntermediateMaEx
{
    partial class MA_C04_ReadVariants
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
            this.btnVersionRead = new System.Windows.Forms.Button();
            this.btnDirectRead = new System.Windows.Forms.Button();
            this.btnURLRead = new System.Windows.Forms.Button();
            this.txtResponse = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnVersionRead
            // 
            this.btnVersionRead.Location = new System.Drawing.Point(211, 140);
            this.btnVersionRead.Name = "btnVersionRead";
            this.btnVersionRead.Size = new System.Drawing.Size(177, 88);
            this.btnVersionRead.TabIndex = 3;
            this.btnVersionRead.Text = "Version Read";
            this.btnVersionRead.UseVisualStyleBackColor = true;
            this.btnVersionRead.Click += new System.EventHandler(this.btnVersionRead_Click);
            // 
            // btnDirectRead
            // 
            this.btnDirectRead.Location = new System.Drawing.Point(208, 38);
            this.btnDirectRead.Name = "btnDirectRead";
            this.btnDirectRead.Size = new System.Drawing.Size(180, 76);
            this.btnDirectRead.TabIndex = 2;
            this.btnDirectRead.Text = "Direct Read";
            this.btnDirectRead.UseVisualStyleBackColor = true;
            this.btnDirectRead.Click += new System.EventHandler(this.btnDirectRead_Click);
            // 
            // btnURLRead
            // 
            this.btnURLRead.Location = new System.Drawing.Point(211, 249);
            this.btnURLRead.Name = "btnURLRead";
            this.btnURLRead.Size = new System.Drawing.Size(177, 88);
            this.btnURLRead.TabIndex = 4;
            this.btnURLRead.Text = "Build URL+Read";
            this.btnURLRead.UseVisualStyleBackColor = true;
            this.btnURLRead.Click += new System.EventHandler(this.btnURLRead_Click);
            // 
            // txtResponse
            // 
            this.txtResponse.Location = new System.Drawing.Point(506, 40);
            this.txtResponse.Multiline = true;
            this.txtResponse.Name = "txtResponse";
            this.txtResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResponse.Size = new System.Drawing.Size(864, 282);
            this.txtResponse.TabIndex = 5;
            // 
            // formMA_C04_ReadVariants
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 375);
            this.Controls.Add(this.txtResponse);
            this.Controls.Add(this.btnURLRead);
            this.Controls.Add(this.btnVersionRead);
            this.Controls.Add(this.btnDirectRead);
            this.Name = "formMA_C04_ReadVariants";
            this.Text = "MA_C04_ReadVariants";
            this.Load += new System.EventHandler(this.MA_C04_ReadVariants_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnVersionRead;
        private System.Windows.Forms.Button btnDirectRead;
        private System.Windows.Forms.Button btnURLRead;
        private System.Windows.Forms.TextBox txtResponse;
    }
}
namespace FhirIntermediateMaEx
{
    partial class MA_C02_Authentication
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
            this.btnBasicAuth = new System.Windows.Forms.Button();
            this.btnBearerAuth = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnBasicAuth
            // 
            this.btnBasicAuth.Location = new System.Drawing.Point(60, 50);
            this.btnBasicAuth.Name = "btnBasicAuth";
            this.btnBasicAuth.Size = new System.Drawing.Size(180, 76);
            this.btnBasicAuth.TabIndex = 0;
            this.btnBasicAuth.Text = "btnBasicAuth";
            this.btnBasicAuth.UseVisualStyleBackColor = true;
            this.btnBasicAuth.Click += new System.EventHandler(this.btnBasicAuth_Click);
            // 
            // btnBearerAuth
            // 
            this.btnBearerAuth.Location = new System.Drawing.Point(63, 152);
            this.btnBearerAuth.Name = "btnBearerAuth";
            this.btnBearerAuth.Size = new System.Drawing.Size(177, 88);
            this.btnBearerAuth.TabIndex = 1;
            this.btnBearerAuth.Text = "btnBearerAuth";
            this.btnBearerAuth.UseVisualStyleBackColor = true;
            this.btnBearerAuth.Click += new System.EventHandler(this.btnBearerAuth_Click);
            // 
            // MA_C02_Authentication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 287);
            this.Controls.Add(this.btnBearerAuth);
            this.Controls.Add(this.btnBasicAuth);
            this.Name = "MA_C02_Authentication";
            this.Text = "MA_C02_Authentication";
            this.Load += new System.EventHandler(this.MA_C02_Authentication_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBasicAuth;
        private System.Windows.Forms.Button btnBearerAuth;
    }
}
﻿namespace FhirIntermediateMaEx
{
    partial class MA_C23_ProcessBundleEntries
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
            this.btnTestClient = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnTestClient
            // 
            this.btnTestClient.Location = new System.Drawing.Point(30, 35);
            this.btnTestClient.Name = "btnTestClient";
            this.btnTestClient.Size = new System.Drawing.Size(215, 135);
            this.btnTestClient.TabIndex = 19;
            this.btnTestClient.Text = "Process Bundle Entries";
            this.btnTestClient.UseVisualStyleBackColor = true;
            this.btnTestClient.Click += new System.EventHandler(this.btnTestClient_Click);
            // 
            // MA_C23_ProcessBundleEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 205);
            this.Controls.Add(this.btnTestClient);
            this.Name = "MA_C23_ProcessBundleEntries";
            this.Text = "MA_C23_ProcessBundleEntries";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTestClient;
    }
}
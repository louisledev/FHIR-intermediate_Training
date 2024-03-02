namespace PatientAllergies
{
    partial class formPatientAllergies
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSearchPatient = new System.Windows.Forms.Button();
            this.lblFamily = new System.Windows.Forms.Label();
            this.lblGivenName = new System.Windows.Forms.Label();
            this.txtFamilyName = new System.Windows.Forms.TextBox();
            this.txtGivenName = new System.Windows.Forms.TextBox();
            this.lblEndpoint = new System.Windows.Forms.Label();
            this.txtFHIREndpoint = new System.Windows.Forms.TextBox();
            this.lblTitlePatient = new System.Windows.Forms.Label();
            this.listCandidates = new System.Windows.Forms.ListBox();
            this.btnShowAllergies = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listAllergies = new System.Windows.Forms.ListBox();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSearchPatient
            // 
            this.btnSearchPatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearchPatient.Location = new System.Drawing.Point(921, 132);
            this.btnSearchPatient.Name = "btnSearchPatient";
            this.btnSearchPatient.Size = new System.Drawing.Size(218, 114);
            this.btnSearchPatient.TabIndex = 2;
            this.btnSearchPatient.Text = "Search Patient";
            this.btnSearchPatient.UseVisualStyleBackColor = true;
            this.btnSearchPatient.Click += new System.EventHandler(this.btnSearchPatient_Click);
            // 
            // lblFamily
            // 
            this.lblFamily.AutoSize = true;
            this.lblFamily.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFamily.Location = new System.Drawing.Point(38, 132);
            this.lblFamily.Name = "lblFamily";
            this.lblFamily.Size = new System.Drawing.Size(218, 38);
            this.lblFamily.TabIndex = 1;
            this.lblFamily.Text = "Family Name:";
            // 
            // lblGivenName
            // 
            this.lblGivenName.AutoSize = true;
            this.lblGivenName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGivenName.Location = new System.Drawing.Point(48, 193);
            this.lblGivenName.Name = "lblGivenName";
            this.lblGivenName.Size = new System.Drawing.Size(208, 38);
            this.lblGivenName.TabIndex = 2;
            this.lblGivenName.Text = "Given Name:";
            // 
            // txtFamilyName
            // 
            this.txtFamilyName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFamilyName.Location = new System.Drawing.Point(252, 129);
            this.txtFamilyName.Name = "txtFamilyName";
            this.txtFamilyName.Size = new System.Drawing.Size(653, 44);
            this.txtFamilyName.TabIndex = 0;
            // 
            // txtGivenName
            // 
            this.txtGivenName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGivenName.Location = new System.Drawing.Point(252, 190);
            this.txtGivenName.Name = "txtGivenName";
            this.txtGivenName.Size = new System.Drawing.Size(653, 44);
            this.txtGivenName.TabIndex = 1;
            // 
            // lblEndpoint
            // 
            this.lblEndpoint.AutoSize = true;
            this.lblEndpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndpoint.Location = new System.Drawing.Point(34, 24);
            this.lblEndpoint.Name = "lblEndpoint";
            this.lblEndpoint.Size = new System.Drawing.Size(243, 38);
            this.lblEndpoint.TabIndex = 5;
            this.lblEndpoint.Text = "FHIR EndPoint:";
            // 
            // txtFHIREndpoint
            // 
            this.txtFHIREndpoint.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFHIREndpoint.Location = new System.Drawing.Point(322, 18);
            this.txtFHIREndpoint.Name = "txtFHIREndpoint";
            this.txtFHIREndpoint.Size = new System.Drawing.Size(583, 44);
            this.txtFHIREndpoint.TabIndex = 6;
            this.txtFHIREndpoint.Text = "http://fhir.hl7fundamentals.org/r4";
            // 
            // lblTitlePatient
            // 
            this.lblTitlePatient.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.lblTitlePatient.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblTitlePatient.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitlePatient.Location = new System.Drawing.Point(38, 74);
            this.lblTitlePatient.Name = "lblTitlePatient";
            this.lblTitlePatient.Size = new System.Drawing.Size(867, 40);
            this.lblTitlePatient.TabIndex = 7;
            this.lblTitlePatient.Text = "SEARCH AND SELECT PATIENT";
            this.lblTitlePatient.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listCandidates
            // 
            this.listCandidates.DisplayMember = "Text";
            this.listCandidates.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listCandidates.FormattingEnabled = true;
            this.listCandidates.HorizontalScrollbar = true;
            this.listCandidates.ItemHeight = 37;
            this.listCandidates.Location = new System.Drawing.Point(41, 254);
            this.listCandidates.Name = "listCandidates";
            this.listCandidates.ScrollAlwaysVisible = true;
            this.listCandidates.Size = new System.Drawing.Size(864, 226);
            this.listCandidates.TabIndex = 3;
            // 
            // btnShowAllergies
            // 
            this.btnShowAllergies.Enabled = false;
            this.btnShowAllergies.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowAllergies.Location = new System.Drawing.Point(921, 293);
            this.btnShowAllergies.Name = "btnShowAllergies";
            this.btnShowAllergies.Size = new System.Drawing.Size(218, 114);
            this.btnShowAllergies.TabIndex = 4;
            this.btnShowAllergies.Text = "Show Allergies";
            this.btnShowAllergies.UseVisualStyleBackColor = true;
            this.btnShowAllergies.Click += new System.EventHandler(this.btnShowAllergies_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 494);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(867, 40);
            this.label1.TabIndex = 10;
            this.label1.Text = "PATIENT ALLERGIES";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listAllergies
            // 
            this.listAllergies.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.06283F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listAllergies.FormattingEnabled = true;
            this.listAllergies.ItemHeight = 37;
            this.listAllergies.Location = new System.Drawing.Point(41, 551);
            this.listAllergies.Name = "listAllergies";
            this.listAllergies.Size = new System.Drawing.Size(1107, 337);
            this.listAllergies.TabIndex = 5;
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.AutoEllipsis = true;
            this.lblErrorMessage.BackColor = System.Drawing.Color.White;
            this.lblErrorMessage.ForeColor = System.Drawing.Color.Red;
            this.lblErrorMessage.Location = new System.Drawing.Point(38, 938);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(1110, 90);
            this.lblErrorMessage.TabIndex = 12;
            this.lblErrorMessage.Text = "lblErrorMessage";
            this.lblErrorMessage.Visible = false;
            // 
            // formPatientAllergies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 1033);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.listAllergies);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnShowAllergies);
            this.Controls.Add(this.listCandidates);
            this.Controls.Add(this.lblTitlePatient);
            this.Controls.Add(this.txtFHIREndpoint);
            this.Controls.Add(this.lblEndpoint);
            this.Controls.Add(this.txtGivenName);
            this.Controls.Add(this.txtFamilyName);
            this.Controls.Add(this.lblGivenName);
            this.Controls.Add(this.lblFamily);
            this.Controls.Add(this.btnSearchPatient);
            this.Name = "formPatientAllergies";
            this.Text = "HL7 FHIR Intermediate Course - Patient Allergies";
           // this.Load += new System.EventHandler(this.formPatientAllergies_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearchPatient;
        private System.Windows.Forms.Label lblFamily;
        private System.Windows.Forms.Label lblGivenName;
        private System.Windows.Forms.TextBox txtFamilyName;
        private System.Windows.Forms.TextBox txtGivenName;
        private System.Windows.Forms.Label lblEndpoint;
        private System.Windows.Forms.TextBox txtFHIREndpoint;
        private System.Windows.Forms.Label lblTitlePatient;
        private System.Windows.Forms.ListBox listCandidates;
        private System.Windows.Forms.Button btnShowAllergies;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listAllergies;
        private System.Windows.Forms.Label lblErrorMessage;
    }
}


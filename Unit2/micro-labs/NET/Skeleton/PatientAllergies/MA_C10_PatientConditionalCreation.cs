﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirIntermediateMaEx
{
    public partial class MA_C10_PatientConditionalCreation : Form
    {
        public MA_C10_PatientConditionalCreation()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            //Step 1: Create The Instance
            var MyPatient = new Patient();
            //Step 2: Populate The Instance
            MyPatient.Name.Add(new HumanName().WithGiven("Alanis").AndFamily("Smithia"));
            MyPatient.Identifier.Add(new Identifier("http://central.patient.id/ident", "123456"));
            MyPatient.Gender = AdministrativeGender.Male;
            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            SearchParams conditions = new SearchParams();
            conditions.Add("identifier", "http://central.patient.id/ident|123456" );
            try
            {
                Patient CreatedPatient = client.Create<Patient>(MyPatient,conditions);
                MessageBox.Show(CreatedPatient.Id);

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }
    }
}

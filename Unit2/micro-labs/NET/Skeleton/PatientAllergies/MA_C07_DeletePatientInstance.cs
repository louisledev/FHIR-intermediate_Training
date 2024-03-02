using System;
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
    public partial class MA_C07_DeletePatientInstance : Form
    {
        public MA_C07_DeletePatientInstance()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            //Step 1: Load the Patient from the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            { 
            String PatientLogicalId = "Patient/109";
            client.Delete(PatientLogicalId);
            
            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Message.ToString());
            }

        }
    }
}

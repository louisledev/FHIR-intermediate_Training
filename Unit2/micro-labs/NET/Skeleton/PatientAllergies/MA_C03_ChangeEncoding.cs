using System;
using System.Windows.Forms;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirIntermediateMaEx
{
    public partial class MA_C03_ChangeEncoding : Form
    {
        public MA_C03_ChangeEncoding()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            client.PreferredFormat = Hl7.Fhir.Rest.ResourceFormat.Json;
            MessageBox.Show("Format Changed to JSON");

        }

        private void MA_C03_ChangeEncoding_Load(object sender, EventArgs e)
        {

        }
    }
}

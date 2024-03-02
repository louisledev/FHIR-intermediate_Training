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
    public partial class MA_C11_TerminologyCodeLookup : Form
    {
        public MA_C11_TerminologyCodeLookup()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                Code c = new Code();
                c.Value = "73211009";
                FhirUri u = new FhirUri("http://snomed.info/sct");
                client.ValidateCode( system:u ,code: c);
            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }

        private void MA_C11_TerminologyCodeLookup_Load(object sender, EventArgs e)
        {

        }
    }
}

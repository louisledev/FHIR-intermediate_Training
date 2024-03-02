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
    public partial class MA_C12_ExpandValueSet : Form
    {
        public MA_C12_ExpandValueSet()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                // 1.	Search for all the concepts related to diabetes – 73211009- (relationship: is-a)
                FhirUri u = new FhirUri("http://snomed.info/sct?fhir_vs=isa/73211009");
                var response1=client.ExpandValueSet(identifier:u);
                // 2.  Search all the concepts in the general practice ref set / pain
                u = new FhirUri("http://snomed.info/sct?fhir_vs=ecl/450970008");
                var response2 = client.ExpandValueSet(identifier: u);

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }

        private void MA_C12_ExpandValueSet_Load(object sender, EventArgs e)
        {

        }
    }
}

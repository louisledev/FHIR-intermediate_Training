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
    public partial class EX_C40_SearchPractitioner : Form
    {
        public EX_C40_SearchPractitioner()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                //A.2 Search By Specialty using simple criteria
                Bundle bu3 = client.Search<PractitionerRole>(new string[] { "specialty=http://snomed.info/sct|408443003" });
                Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
                s.Settings.Pretty = false;

                String results = s.SerializeToString(instance: bu3, summary: SummaryType.True);
                MessageBox.Show(results);
                //A.2 Search using Query
                var q = new SearchParams()
                    .Where("specialty=http://snomed.info/sct|408443003")
                    .Include("practitioner:practitioner")
                    .Include("organization:organization")
                    .LimitTo(50)
                    .OrderBy("family", Hl7.Fhir.Rest.SortOrder.Ascending);

                q.Add("City", "New York");
            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }
    }
}


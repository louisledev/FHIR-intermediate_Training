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
    public partial class EX_C39_ProcessSearchBundleEntries : Form
    {
        public EX_C39_ProcessSearchBundleEntries()
        {
            InitializeComponent();
        }

        private void EX_C39_ProcessSearchBundleEntries_Load(object sender, EventArgs e)
        {

        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);

            Bundle bu3 = client.Search<Practitioner>(new string[] { "city=Ann Arbor" });
            string listPra = "";
            while (bu3 != null)
            {
                foreach (Bundle.EntryComponent ent in bu3.Entry)
                {
                    Practitioner pr = (Practitioner)ent.Resource;
                    listPra = listPra + pr.Identifier[0].Value + "-" + pr.Name[0].Family + "," + pr.Name[0].Given.First().ToString();


                }
                bu3 = client.Continue(bu3, PageDirection.Next);
            }
        }
    }
}

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
    public partial class MA_C09_ExecuteOperationEverything : Form
    {
        public MA_C09_ExecuteOperationEverything()
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
                UriBuilder UriBuilderx = new UriBuilder(FHIR_EndPoint);
                UriBuilderx.Path = "Patient/73412";
                Parameters par = new Parameters();
                par.Add("start", new FhirDateTime(2019, 11, 1));
                par.Add("end", new FhirDateTime(2020, 02, 2));
                Hl7.Fhir.Model.Resource ReturnedResource = client.InstanceOperation(UriBuilderx.Uri, "everything", par);
                if (ReturnedResource is Hl7.Fhir.Model.Bundle)
                {
                    Hl7.Fhir.Model.Bundle ReturnedBundle = ReturnedResource as Hl7.Fhir.Model.Bundle;
                }
            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }
    }
}

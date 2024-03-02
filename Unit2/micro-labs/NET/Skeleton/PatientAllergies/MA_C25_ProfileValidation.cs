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
    public partial class MA_C25_ProfileValidation : Form
    {
        public MA_C25_ProfileValidation()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            string fileName = AppContext.BaseDirectory + "/FHIR_RESOURCES/PATIENT_EXTENSIONS.XML";
            string xml = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
            try
            {
                Patient pa = parser.Parse<Patient>(xml);
                Parameters inParams = new Parameters();
                inParams.Add("resource", pa);
                OperationOutcome val = client.ValidateResource(pa);
                Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
                String results = s.SerializeToString(instance:val);
                
                MessageBox.Show(results);
    }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }


        }
    }
}

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
    public partial class MA_C24_ParsePatientXMLFile : Form
    {
        public MA_C24_ParsePatientXMLFile()
        {
            InitializeComponent();
        }

        private void MA_C24_ParsePatientXMLFile_Load(object sender, EventArgs e)
        {

        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string fileName = AppContext.BaseDirectory + "/FHIR_RESOURCES/PATIENT_OK.XML";
            string xml = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
            try
            { 
            Patient pa = parser.Parse<Patient>(xml);

            MessageBox.Show("Name:"+pa.Name[0].Family + ","+pa.Name[0].Given.First().ToString()+" Identifier:"+pa.Identifier[0].System+":"+pa.Identifier[0].Value);
            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }


        }
    }
}

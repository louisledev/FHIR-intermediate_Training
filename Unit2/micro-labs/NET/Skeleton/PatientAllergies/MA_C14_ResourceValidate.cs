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

namespace FhirIntermediateMaEx
{
    public partial class MA_C14_ResourceValidate : Form
    {
        public MA_C14_ResourceValidate()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string fileName = AppContext.BaseDirectory + "FHIR_RESOURCES/PATIENT.XML";
            string xml = System.IO.File.ReadAllText(fileName); 

            
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
                
            try
            {
                Resource parsedResource = parser.Parse<Resource>(xml);
                    
                    
            }
            catch (FormatException fe)
            {
                MessageBox.Show("Error Parsing Resource " + fe.Message.ToString());
                    
            }
                
                
            
        }
    

        private void MA_C14_ResourceValidate_Load(object sender, EventArgs e)
        {

        }
    }
}
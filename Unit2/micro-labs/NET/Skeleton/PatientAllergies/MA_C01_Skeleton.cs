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
    public partial class MA_C01_Skeleton : Form
    {
        public MA_C01_Skeleton()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";  
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            MessageBox.Show("I am just a skeleton so I do nothing");

        }

        private void MA_C01_Skeleton_Load(object sender, EventArgs e)
        {

        }
    }
}

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
using System.IO;

namespace FhirIntermediateMaEx
{
    public partial class EX_C06_UpdatePractitionerInstance : Form
    {
        public EX_C06_UpdatePractitionerInstance()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            String PractitionerLogicalId = "Practitioner/120376";
            Practitioner MyPractitioner = client.Read<Practitioner>(PractitionerLogicalId);
            byte[] imageBytes;
            string PhotoPath =AppContext.BaseDirectory + "physician.jpeg";
            using (Image image = Image.FromFile(PhotoPath))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    imageBytes = m.ToArray();

                    
                }
            }
            //Step 2: Modify the elements you need
            MyPractitioner.Photo = new List<Attachment>()
             {
                new Attachment()
                {
                    ContentType="application/jpeg",
                    Data=imageBytes
                }
             };
            //Step 3: Invoke in the Server
            try
            {
                Practitioner UpdatedPractitioner = client.Update<Practitioner>(MyPractitioner);
                MessageBox.Show(UpdatedPractitioner.VersionId);
                

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }
            //Step 4: Process Response
        
    }
    
    }
}

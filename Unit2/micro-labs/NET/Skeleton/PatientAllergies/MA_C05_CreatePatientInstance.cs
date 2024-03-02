using System;
using System.Windows.Forms;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirIntermediateMaEx
{
    public partial class MA_C05_CreatePatientInstance : Form
    {
        public MA_C05_CreatePatientInstance()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {
            /*
            Patient Data:
             Smith, Alan, born 06 May 1965, male, mrn: http://testpatient.id/mrn / 99999999    */
            //Step 1: Create The Instance
            var MyPatient = new Patient();
            //Step 2: Populate The Instance
            
            MyPatient.Name.Add(new HumanName().WithGiven("Alan").AndFamily("Smith"));
            MyPatient.Identifier.Add(new Identifier("http://testpatient.id/mrn", "99999999"));
            MyPatient.Gender = AdministrativeGender.Male;
            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                Patient CreatedPatient = client.Create<Patient>(MyPatient);
                MessageBox.Show(CreatedPatient.Id);

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }
          
        }

        private void MA_C05_CreatePatientInstance_Load(object sender, EventArgs e)
        {

        }
    }
}

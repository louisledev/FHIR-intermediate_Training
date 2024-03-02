using System;
using System.Windows.Forms;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirIntermediateMaEx
{
    public partial class MA_C04_ReadVariants : Form
    {
        public MA_C04_ReadVariants()
        {
            InitializeComponent();
        }

        private void btnDirectRead_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            string PatientLogicalId = "Patient/159";
            var MyPatient = client.Read<Patient>(PatientLogicalId);
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json=s.SerializeToString(MyPatient);
            this.txtResponse.Text = json;
        }

        private void MA_C04_ReadVariants_Load(object sender, EventArgs e)
        {

        }

        private void btnVersionRead_Click(object sender, EventArgs e)
        {
            // 
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            string PatientLogicalIdVersion = "http://fhir.hl7fundamentals.org/r4/Patient/159/_history/1";
            var MyPatient = client.Read<Patient>(PatientLogicalIdVersion);
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json = s.SerializeToString(MyPatient);
            this.txtResponse.Text = json;

        }

        private void btnURLRead_Click(object sender, EventArgs e)
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            var MyPatient=client.Read<Patient>(ResourceIdentity.Build("Patient", "159"));
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json = s.SerializeToString(MyPatient);
            this.txtResponse.Text = json;

        }
    }
}

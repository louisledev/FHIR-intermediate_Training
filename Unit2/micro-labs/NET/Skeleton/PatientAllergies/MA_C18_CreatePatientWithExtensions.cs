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
    public partial class MA_C18_CreatePatientWithExtensions : Form
    {
        public MA_C18_CreatePatientWithExtensions()
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
            //Simple Extension
            Extension exts = new Extension("http://hl7.org/fhir/us/core/StructureDefinition/us-core-birthsex", 
                new Code("M"));

            //Element Extension
            Extension exte = new Extension("http://hl7.org/fhir/StructureDefinition/patient-birthTime"
                ,new FhirDateTime("1965-06-05T20:30:40")
                );
            MyPatient.BirthDate = "1965-06-05";
            MyPatient.BirthDateElement.Extension.Add(exte);
            //Complex Extension
            Extension extc1 = new Extension("http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity",null);

            List<Coding> c1 = new List<Coding>();
            c1.Add(new Coding("urn:oid:2.16.840.1.113883.6.238", "2135-2", "Hispanic or Latino"));
            List<Coding> c2 = new List<Coding>();
            c2.Add(new Coding("urn:oid:2.16.840.1.113883.6.238", "2184-0", "Dominican"));

            extc1.Extension.Add(new Extension("ombCategory", new CodeableConcept()  { Coding = c1  }));
            extc1.Extension.Add(new Extension("detailed",    new CodeableConcept()  { Coding = c2  }));
            extc1.Extension.Add(new Extension("text",        new FhirString("Hispanic or Latino"   )));

            MyPatient.Extension.Add(exts);
            MyPatient.Extension.Add(extc1);

            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
         //       Patient CreatedPatient = client.Create<Patient>(MyPatient);
           //     MessageBox.Show(CreatedPatient.Id);

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }
    }
}

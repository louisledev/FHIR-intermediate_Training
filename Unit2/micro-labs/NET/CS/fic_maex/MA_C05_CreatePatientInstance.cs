using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fic_maex
{
  class MA_C05_CreatePatientInstance : fic_maexe
    {
 
        public void Execute()
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
                Console.WriteLine(CreatedPatient.Id);

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }
        
        }
    }
}

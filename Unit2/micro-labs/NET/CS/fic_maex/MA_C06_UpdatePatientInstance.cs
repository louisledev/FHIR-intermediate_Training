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
  class MA_C06_UpdatePatientInstance : fic_maexe
    {
 
        public void Execute()
        {
           //Step 1: Load the Patient from the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                String PatientLogicalId = "Patient/109";
                Patient MyPatient = client.Read<Patient>(PatientLogicalId);
                //Step 2: Modify the elements you need
                Address ad = new Address
                {
                    City="Montreal",
                    Country = "Canada",
                    PostalCode = "H2K 4J5",
                    State = "Montreal",
                    Line = new string[] { "3300 Washtenaw Avenue, Suite 227" }
                };
                MyPatient.Address.Add(ad);
                ContactPoint co = new ContactPoint
                {
                    System = ContactPoint.ContactPointSystem.Phone,
                    Value = "613-555-5555"
                };
                MyPatient.Telecom.Add(co);
                Patient UpdatedPatient = client.Update<Patient>(MyPatient);
                Console.WriteLine(UpdatedPatient.VersionId);

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        

        }
    }
}

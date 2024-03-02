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
  class MA_C07_DeletePatientInstance : fic_maexe
    {
 
        public void Execute()
        {
           
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            { 
            String PatientLogicalId = "Patient/109";
            client.Delete(PatientLogicalId);
            
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Message.ToString());
            }


        }
    }
}

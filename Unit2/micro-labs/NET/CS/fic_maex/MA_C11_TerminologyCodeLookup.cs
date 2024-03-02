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
  class MA_C11_TerminologyCodeLookup : fic_maexe
    {
 
        public void Execute()
        {
            string FHIR_EndPoint ="https://snowstorm-alpha.ihtsdotools.org/fhir/";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                Code c = new Code();
                c.Value = "73211009";
                FhirUri u = new FhirUri("http://snomed.info/sct");
                var cr=client.ConceptLookup(code:c,system:u);
                Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
                string json=s.SerializeToString(cr);
                Console.WriteLine(json);
                
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

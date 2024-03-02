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
  class MA_C12_TerminologyExpandValueSet : fic_maexe
    {
 
        public void Execute()
        {
            string FHIR_EndPoint = "https://snowstorm-alpha.ihtsdotools.org/fhir/";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                // 1.	Search for all the concepts related to diabetes – 73211009- (relationship: is-a)
                
                Uri u= new Uri("http://snomed.info/sct/73211009?fhir_vs=isa/");
                var response1=client.ExpandValueSet(valueset:u);
                Console.WriteLine(response1.ToString());
            
                // 2.  Search all the concepts in the general practice ref set / pain
                u = new Uri("http://snomed.info/sct/450970008?fhir_vs=ecl/");
                var response2 = client.ExpandValueSet(valueset:u);
                Console.WriteLine(response2.ToString());
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

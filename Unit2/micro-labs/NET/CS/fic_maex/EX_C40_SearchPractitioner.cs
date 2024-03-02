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
  class EX_C40_SearchPractitioner : fic_maexe
    {
 
        public void Execute()
        {
              string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                //A.2 Search By Specialty using simple criteria
                Bundle bu3 = client.Search<PractitionerRole>(new string[] { "specialty=http://snomed.info/sct|408443003" });
                Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
                s.Settings.Pretty = false;

                String results = s.SerializeToString(instance: bu3, summary: SummaryType.True);
                Console.WriteLine(results);
                //A.2 Search using Query
                var q = new SearchParams()
                    .Where("specialty=http://snomed.info/sct|408443003")
                    .Include("practitioner:practitioner")
                    .Include("organization:organization")
                    .LimitTo(50)
                    .OrderBy("family", Hl7.Fhir.Rest.SortOrder.Ascending);

                q.Add("City", "New York");
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

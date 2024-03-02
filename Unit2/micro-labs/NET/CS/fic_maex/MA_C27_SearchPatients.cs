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
  class MA_C27_SearchPatients : fic_maexe
    {
 
        public void Execute()
        {
              string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();

            try
            {
                /*
                    c.Search for all patients with any name matching “John”, gender = female
                    */
                Bundle bu_a = client.Search<Patient>(new string[] { "name=john" , "gender=female"});
                String results_a = s.SerializeToString(instance: bu_a, summary: SummaryType.True);
                Console.WriteLine("Results_a:"+results_a);

                /*
                    b.Search for the patient with identifier[system] http://hospital.gov/patients [value] 9999999 (Medical Record Number)
                    */

                Bundle bu_b = client.Search<Patient>(new string[] { "identifier=http://hospital.gov/patients|9999999"});
                String results_b = s.SerializeToString(instance: bu_b, summary: SummaryType.True);
                Console.WriteLine("Results_b:" + results_b);

                /*
                    a.Search for all patients with last(family) name exactly = “Smith”, gender = male, born after 05 - May - 1965 , using the SearchParams search variant
                    */

                var q = new SearchParams()
                    .Where("family:exactly=Smith");
                q.Add("gender", "male");
                q.Add("birthdate", "ge1968-05-05");
                Bundle bu_c = client.Search<Patient>(q);
                String results_c = s.SerializeToString(instance: bu_c, summary: SummaryType.True);
                Console.WriteLine("Results_c:" + results_c);

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

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
  class EX_C39_ProcessSearchBundleEntries : fic_maexe
    {
 
        public void Execute()
        {
             string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);

            Bundle bu3 = client.Search<Practitioner>(new string[] { "address-city=New York" });
            string listPra = "";
            while (bu3 != null)
            {
                foreach (Bundle.EntryComponent ent in bu3.Entry)
                {
                    Practitioner pr = (Practitioner)ent.Resource;
                    listPra = listPra + pr.Identifier[0].Value + "-" + pr.Name[0].Family + "," + pr.Name[0].Given.First().ToString();


                }
                bu3 = client.Continue(bu3, PageDirection.Next);
            }
            Console.WriteLine(listPra);
        }
        
    }
}

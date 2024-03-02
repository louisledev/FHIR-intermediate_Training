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
  class MA_C23_ProcessBundleEntries : fic_maexe
    {
 
          private string NAM(Patient res)
        {
            HumanName h = res.Name[0];
            string FullName = h.Family + "," + h.Given.First().ToString();
            return FullName;
        }

        public void Execute()
        {
            string fileName = AppContext.BaseDirectory + "FHIR_RESOURCES/IPS_DOCUMENT.JSON";
            string json = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirJsonParser();
          
            try
            {
                Bundle bt = parser.Parse<Bundle>(json);
                string PatientName = "";
                string Identifier = "";
                string ConditionCodes = "";
                foreach (Bundle.EntryComponent entry in bt.Entry)
                {
                    string fullUrl = entry.FullUrl;
                    string ResourceType = entry.Resource.TypeName;
                    if (ResourceType == "Patient")
                    {
                        Patient pat = (Patient)entry.Resource;
                        PatientName = NAM(pat);
                        Identifier = pat.Identifier[0].System + ":" + pat.Identifier[0].Value;

                    }
                    else
                    {
                        if (ResourceType == "Condition")
                        {
                            Condition co = (Condition)entry.Resource;
                            ConditionCodes = ConditionCodes + co.Code.Coding[0].System + ":" + co.Code.Coding[0].Code + ":" + co.Code.Coding[0].Display+ " / ";
                        }
                    }
                }
                Console.WriteLine("Name:" + PatientName + " Identifier:" + Identifier + " Conditions:" + ConditionCodes);

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

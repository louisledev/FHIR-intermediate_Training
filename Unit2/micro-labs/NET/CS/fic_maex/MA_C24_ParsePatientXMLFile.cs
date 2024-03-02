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
  class MA_C24_ParsePatientXMLFile: fic_maexe
    {
 
        public void Execute()
        {
              string fileName = AppContext.BaseDirectory + "/FHIR_RESOURCES/PATIENT_OK.XML";
            string xml = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
            try
            { 
            Patient pa = parser.Parse<Patient>(xml);

            Console.WriteLine("Name:"+pa.Name[0].Family + ","+pa.Name[0].Given.First().ToString()+" Identifier:"+pa.Identifier[0].System+":"+pa.Identifier[0].Value);
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }


        }
    }
}

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
  class MA_C14_ResourceValidate : fic_maexe
    {
 
        public void Execute()
        {
            //Remember to copy the FHIR_RESOURCES folder to the bin/debug/netcoreapp3.0 folder
            //or whichever your BaseDirectory is

            string fileName = AppContext.BaseDirectory + "FHIR_RESOURCES/PATIENT.XML";
            string xml = System.IO.File.ReadAllText(fileName); 

            
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
                
            try
            {
                Resource parsedResource = parser.Parse<Resource>(xml);
                    
                    
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Error Parsing Resource " + fe.Message.ToString());
                    
            }
         
        }
    }
}

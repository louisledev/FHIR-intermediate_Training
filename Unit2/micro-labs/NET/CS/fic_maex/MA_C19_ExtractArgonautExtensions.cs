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
  class MA_C19_ExtractArgonautExtensions : fic_maexe
    {
 
        public void Execute()
        {
              string fileName = AppContext.BaseDirectory + "FHIR_RESOURCES/PATIENT_EXTENSIONS.XML";
            string xml = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
            try
            {
                Patient pat = parser.Parse<Patient>(xml);
                string ExtList = "";
                Extension ComplexEx = pat.GetExtension("http://hl7.org/fhir/us/core/StructureDefinition/us-core-race");
                if (ComplexEx != null)
                {
                    foreach (Extension SimpleEx in ComplexEx.Extension)
                    {

                        string ExtURL = SimpleEx.Url;
                        string ExtType = "null";
                        string MyValue = "";

                        if (SimpleEx.Value != null)
                        {
                            ExtType = SimpleEx.Value.TypeName;
                            if (ExtType == "Coding")
                            {
                                Coding c = (Coding)SimpleEx.Value;
                                string MySystem = c.System;
                                string MyCode = c.Code;
                                MyValue = MySystem + ":" + MyCode;
                                ;
                                ExtList = ExtList + ExtURL + "=" + MyValue+"\r\n";
                            }
                        }

                    }
                }
                Console.WriteLine(ExtList);
            }

            catch (FormatException fe)
            {
                Console.WriteLine("Error Parsing Resource " + fe.Message.ToString());

            }

        }
    }
}

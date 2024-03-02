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
  class MA_C17_ExtractInfoIPSObservation : fic_maexe
    {
 
        public void Execute()
        {
            
            string fileName = AppContext.BaseDirectory + "FHIR_RESOURCES/OBSERVATION.XML";
            string xml = System.IO.File.ReadAllText(fileName);
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();
            try
            {

                Observation ob = parser.Parse<Observation>(xml);
                String catCode = "";
                String catSystem = "";
                String statusCode = "";
                String obsCode = "";
                String obsSystem = "";
                String obsDate = "";
                String obsValue = "";
                if (ob.Status != null)
                {
                    // Status Code
                    statusCode = ob.Status.ToString();
                    // Category allows repeats, so we get the first one
                    catSystem = ob.Category[0].Coding[0].System.ToString();
                    catCode = ob.Category[0].Coding[0].Code.ToString();
                    // Code does not allow repeats, but Coding does! -> we get the first one
                    obsSystem = ob.Code.Coding[0].System.ToString();
                    obsCode = ob.Code.Coding[0].Code.ToString();
                    // Date- Time
                    obsDate = ob.Effective.ToString();
                    // Value
                    if (ob.Value != null)
                    {

                        obsValue = ""; 
                         if (ob.Value.TypeName == "CodeableConcept")
                        {
                            CodeableConcept cc= (CodeableConcept)ob.Value;
                            obsValue = cc.Coding[0].System.ToString()+"-"+ cc.Coding[0].Code.ToString();

                        }
                       
                    };
                    //Narrative
                    String sText = ob.Text.Div.ToString();

                    string result = "OBSERVATION";
                    result = result + "**Category**";
                    result = result + catCode + "\r\n";
                    result = result + "System:";
                    result = result + catSystem + "\r\n";
                    result = result + "**Code**";
                    result = result + "Code:";
                    result = result + obsCode + "\r\n";
                    result = result + "System:";
                    result = result + obsSystem + "\r\n";
                    result = result + "**Value**";
                    result = result + obsValue + "\r\n";
                    result = result + "**Date/Time**";
                    result = result + obsDate + "\r\n";
                    result = result + "**Status Code**";
                    result = result + statusCode + "\r\n";
                    result = result + "**Text***";
                    result = result + sText + "\r\n";

                    Console.WriteLine(result);
                }
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Error Parsing Resource " + fe.Message.ToString());

            }

        }
    }
}

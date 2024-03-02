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
  class MA_C03_ChangeEncoding : fic_maexe
    {
 
        public void Execute()
        {
          string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            client.PreferredFormat = Hl7.Fhir.Rest.ResourceFormat.Json;
            Console.WriteLine("Format Changed to JSON");

        }
    }
}

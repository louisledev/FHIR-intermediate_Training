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
  class MA_C13_ValueSetValidate : fic_maexe
    {
 
        public void Execute()
        {
           string FHIR_EndPoint = "https://fhir.loinc.org";
            //This request uses basic authorization against the FHIR Loinc Server
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            client.OnBeforeRequest += (object msender, BeforeRequestEventArgs mer) =>
            {

                string authInfo = "kaminkerdiego:superloinc2019.";
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                mer.RawRequest.Headers.Add("Authorization", "Basic " + authInfo);

            };
            //Validate Code needs 3 arguments: ValueSet, System and Code
            //Does the code belong to the ValueSet?
            try
            {
                //ValueSet: LG33055-1
                //System: http://loinc.org
                //Code: 8867-4
                ValueSet v = new ValueSet();
                v.Id = "LG33055-1";
                Code cc = new Code();
                cc.Value = "8867-4";
                FhirUri cs = new FhirUri("http://loinc.org");
                var response1 = client.ValidateCode(valueSet: v, code: cc,system:cs);
                Console.WriteLine(response1.Message.ToString());
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }

        }
    }
}

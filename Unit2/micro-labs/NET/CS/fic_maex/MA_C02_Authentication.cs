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
  class MA_C02a_BasicAuth : fic_maexe
    {
 
        public void Execute()
        {
              string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            client.OnBeforeRequest += (object msender, BeforeRequestEventArgs mer) =>
             {

                 string authInfo = "username:password";
                 authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                 mer.RawRequest.Headers.Add("Authorization", "Basic " + authInfo);
             };
          

        }
    }
  class MA_C02B_BearerTokenAuth :fic_maexe
    {
 
        public void Execute()
        {
              string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            client.OnBeforeRequest += (object msender, BeforeRequestEventArgs mer) =>
            {
                mer.RawRequest.Headers.Add("Authorization", "Bearer ya29.QQIBibTwvKkE39hY8mdkT_mXZoRh7Ub9cK9hNsqrxem4QJ6sQa36VHfyuBe");
            };

        }
    }
  
}

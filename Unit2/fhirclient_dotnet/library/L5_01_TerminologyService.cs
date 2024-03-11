using Hl7.Fhir.Rest;
using System;
using System.Linq;

namespace fhirclient_dotnet
{
    public class TerminologyService
    {
        public String ExpandValueSetForCombo(
            string EndPoint,
            string Url,
            string Filter
        )
        {
            var valueSet = FhirClientHelper.ExpandValueSetAsync(EndPoint, new Uri(Url), Filter).Result;
            if (valueSet == null || valueSet.Expansion == null || valueSet.Expansion.Contains == null|| !valueSet.Expansion.Contains.Any())
            {
                return "Error:ValueSet_Filter_Not_Found";
            }
            
            string terms = "";
            foreach (var concept in valueSet.Expansion.Contains)
            {
                terms += $"{concept.Code}|{concept.Display}\n";
            }

            return terms;
        }
    }
}
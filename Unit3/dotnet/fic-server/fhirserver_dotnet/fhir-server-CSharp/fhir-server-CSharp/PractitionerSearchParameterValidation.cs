using Hl7.Fhir.Model;
using fhir_server_entity_model;
using System.Collections.Generic;
using System.Net;

namespace fhir_server_CSharp
{
    public static class PractitionerSearchParameterValidation
    {
        public static bool ValidateSearchParams(HttpListenerRequest request, ref bool hardIdSearch, out DomainResource operation, out List<LegacyFilter> criteria)
        {
            return PersonSearchParameterValidation.ValidateSearchParams(request, "Practitioner", ref hardIdSearch, out operation, out criteria);
        }
    }
}

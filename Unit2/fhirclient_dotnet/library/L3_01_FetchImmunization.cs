using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchImmunization
    {
        public string GetImmunizations
        (string ServerEndPoint,
            string IdentifierSystem,
            string IdentifierValue
        )
        {
            Patient? patient = FhirClientHelper.GetPatientById(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            var immunizations = FhirClientHelper.GetImmunizationsForPatient(ServerEndPoint, patient.Id);
            if (!immunizations.Any())
            {
                return "Error:No_Immunizations";
            }

            // status|code:display|immunization_date\n
            string result = "";
            foreach (var immunization in immunizations)
            {
                var occurence = immunization.Occurrence;
                string occurenceString = "";
                if (occurence is FhirDateTime dt)
                {
                    occurenceString  = dt.Value;
                }
                else if (occurence is FhirString str)
                {
                    occurenceString = str.Value;
                }

                Coding? coding = null;
                if (immunization.VaccineCode?.Coding is { Count: > 0 })
                {
                    coding = immunization.VaccineCode?.Coding[0];
                }
                result += $"{immunization.Status}|{coding?.Code}:{coding?.Display}|{occurenceString}\n";
            }
            return result;
        }
    }
}
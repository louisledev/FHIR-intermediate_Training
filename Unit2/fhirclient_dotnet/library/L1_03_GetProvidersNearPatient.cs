using System;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;
using Humanizer;

namespace fhirclient_dotnet
{
    public class GetProvidersNearPatient
    {
        public string GetProvidersNearCity
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

             var city = patient.Address.FirstOrDefault(a => a.CityElement != null)?.CityElement?.Value;
             if (string.IsNullOrEmpty(city))
             {
                 return "Error:Patient_w/o_City";
             }

             Func<Practitioner, string> getPractitionerDetails = (Practitioner practitioner) =>
             {
                 var phone = practitioner.Telecom.FirstOrDefault(t => t.System == ContactPoint.ContactPointSystem.Phone)?.Value ?? "-";
                 var address = practitioner.Address.FirstOrDefault()?.Line.FirstOrDefault()?.ToUpper() ?? "-";
                 var specialty = practitioner.Qualification.FirstOrDefault()?.Code?.Coding?.FirstOrDefault().Display?.ToUpper() ?? "-";
                 return $"Phone:{phone}|{address}|{specialty}";
             };
             
             var practitioners = FhirClientHelper.SearchPractitionersByCriteria(ServerEndPoint, new []{ $"address-city={city}"} ).ToList();
                if (practitioners.Count == 0)
                {
                    return "Error:No_Provider_In_Patient_City";
                }
                
                if (practitioners.Count == 1)
                {
                    var practitioner = practitioners[0];
                    var details = getPractitionerDetails(practitioner);
                    return $"OnlyPhysician,InTown|{details}\n";
                }

                var result = "";
                for (int i = 0; i < practitioners.Count; i++)
                {
                    var practitioner = practitioners[i];
                    var details = getPractitionerDetails(practitioner);
                    var word = (i + 1).ToWords().Transform(To.TitleCase);
                    var ordinal = (i + 1).ToOrdinalWords().Transform(To.TitleCase);
                    result += $"{word}Physician,{ordinal}|{details}\n";
                }
                return result;
         }
    }
}

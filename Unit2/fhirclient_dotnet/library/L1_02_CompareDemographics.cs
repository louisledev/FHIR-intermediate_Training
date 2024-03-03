using System;
using System.Linq;
using Hl7.Fhir.Model;

namespace fhirclient_dotnet
{
    public class CompareDemographics
    {
        public string GetDemographicComparison
        (string ServerEndPoint,
            string IdentifierSystem,
            string IdentifierValue,
            string myFamily,
            string myGiven,
            string myGender,
            string myBirthDate
        )
        {
            Patient? patient = FhirClientHelper.GetPatientById(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            // function that test if the provided string are equal, if not return red, else return green
            Func<string, string, string> testEquality = (string1, string2) => string1 == string2 ? "{green}" : "{red}";

            var theirFamily = patient.Name[0].Family;
            var theirGiven = string.Join(' ', patient.Name[0].GivenElement.Select(g => g.ToString()));
            var theirGender = patient.Gender.ToString() ?? "-";
            var theirBirthDate = patient.BirthDate;

            var result = $"{{family}}|{myFamily}|{theirFamily}|{testEquality(myFamily, theirFamily)}\n";
            result += $"{{given}}|{myGiven}|{theirGiven}|{testEquality(myGiven, theirGiven)}\n";
            result += $"{{gender}}|{myGender.ToUpper()}|{theirGender.ToUpper()}|{testEquality(myGender.ToUpper(), theirGender.ToUpper())}\n";
            result += $"{{birthDate}}|{myBirthDate}|{theirBirthDate}|{testEquality(myBirthDate, theirBirthDate)}\n";
            return result;
        }
    }
}
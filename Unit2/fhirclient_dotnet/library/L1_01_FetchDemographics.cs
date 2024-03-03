﻿using System;
using System.Linq;
using System.Net.Http;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchDemographics
    {
        public string GetPatientPhoneAndEmail
        (string ServerEndPoint,
         string IdentifierSystem,
         string IdentifierValue)
         {
            Patient? patient= FhirClientHelper.GetPatientById(ServerEndPoint, IdentifierSystem, IdentifierValue);
            if (patient == null)
                return "Error:Patient_Not_Found";
            var emailList = patient.Telecom
                    .Where(t => t.System == ContactPoint.ContactPointSystem.Email)
                    .Select(t => $"{t.Value}({t.Use})").ToList();
            var emails = string.Join("|", emailList);
            if (string.IsNullOrEmpty(emails))
                emails = "-";
            var phoneList = patient.Telecom
                    .Where(t => t.System == ContactPoint.ContactPointSystem.Phone)
                    .Select(t => $"{t.Value}({t.Use})").ToList();
            var phones = string.Join("|", phoneList);
            if (string.IsNullOrEmpty(phones))
                phones = "-";
            return $"Emails:{emails}\nPhones:{phones}\n";
         }
    }
}

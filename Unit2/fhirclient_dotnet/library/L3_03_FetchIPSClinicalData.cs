using System;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchIPS
    {
        public string GetIPSMedications
        (string ServerEndPoint,
            string IdentifierSystem,
            string IdentifierValue
        )
        {
            Patient? patient = FhirClientHelper.GetPatientByIdAsync(ServerEndPoint, IdentifierSystem, IdentifierValue).Result;
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            var bundle = FhirClientHelper.GetIPSDocumentForPatientAsync(ServerEndPoint, patient.Id).Result;
            if (bundle == null)
            {
                return "Error:No_IPS";
            }

            var medicationStatements = bundle.Entry
                .Where(e => e.Resource is MedicationStatement)
                .Select(e => e.Resource as MedicationStatement);

            var result = "";
            foreach (var ms in medicationStatements)
            {
                var start = ms.Effective as Period;
                Coding? coding = null;
                var medicationCodeableConcept = ms.Medication as CodeableConcept;
                if (medicationCodeableConcept != null)
                {
                    coding = medicationCodeableConcept.Coding.FirstOrDefault();
                }
                var medicationReference = ms.Medication as ResourceReference;
                if (medicationReference != null)
                {
                    var medication = bundle.Entry.FirstOrDefault(e => e.FullUrl == medicationReference?.Url.ToString())?.Resource as Medication;
                    coding = medication?.Code?.Coding.FirstOrDefault();
                }
                result += $"{ms.Status}|{start?.Start}|{coding?.Code}:{coding?.Display}\n";
            }
            return result;
        }

        public string GetIPSImmunizations
        (string ServerEndPoint,
            string IdentifierSystem,
            string IdentifierValue
        )
        {
            Patient? patient = FhirClientHelper.GetPatientByIdAsync(ServerEndPoint, IdentifierSystem, IdentifierValue).Result;
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            var bundle = FhirClientHelper.GetIPSDocumentForPatientAsync(ServerEndPoint, patient.Id).Result;
            if (bundle == null)
            {
                return "Error:No_IPS";
            }

            var immunizations = bundle.Entry
                .Where(e => e.Resource is Immunization)
                .Select(e => e.Resource as Immunization)
                // Comment from Rik: It is true that there is an Immunization but I believe the idea is to check for a status of "not-done". 
                .Where(i => i.Status != Immunization.ImmunizationStatusCodes.NotDone).ToList();

            if (immunizations.Count == 0)
            {
                return "Error:IPS_No_Immunizations";
            }
            
            var result = "";

            //   "Completed|1998-06-04T00:00:00+02:00|414005006:Diphtheria + Pertussis + Poliomyelitis + Tetanus vaccine\n";
            foreach (var immunization in immunizations)
            {
                result += $"{immunization!.Status}|{immunization?.Occurrence}|{immunization!.VaccineCode?.Coding.FirstOrDefault()?.Code}:{immunization.VaccineCode?.Coding.FirstOrDefault()?.Display}\n";
            }
            
            
            return result;

        }
    }
}
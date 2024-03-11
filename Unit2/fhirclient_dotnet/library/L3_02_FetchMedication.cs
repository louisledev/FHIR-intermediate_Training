using System;
using System.Linq;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchMedication
    {
        public string GetMedications
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

             var medicationRequests = FhirClientHelper.GetMedicationRequestsForPatientAsync(ServerEndPoint, patient.Id).Result;
             if (!medicationRequests.Any())
             {
                 return "Error:No_Medications";
             }
             
             // status|intent|authored_on|code:display|requester\n
             string result = "";
             foreach (var mr in medicationRequests)
             {
                 Coding? coding = null;
                 if (mr.Medication  is CodeableConcept cc)
                 {
                     coding = cc.Coding.FirstOrDefault();
                 }
                 result += $"{mr.Status}|{mr.Intent}|{mr.AuthoredOn}|{coding?.Code}:{coding?.Display}|{mr.Requester.Display}\n";
             }
             return result;
             
         }        
    }
}

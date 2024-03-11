using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using Hl7.Fhir.Utility;

namespace fhirclient_dotnet
{
    public class CreateUSCoreImm
    {
        public string CreateUSCoreR4Immunization
        (string ServerEndpoint,
         string PatientIdentifierSystem,
         string PatientIdentifierValue,
         string ImmunizationStatusCode,
         string ImmunizationDateTime,
         string ProductCVXCode,
         string ProductCVXDisplay,
         string ReasonCode)
        {
            Patient? patient = FhirClientHelper.GetPatientByIdAsync(ServerEndpoint, PatientIdentifierSystem, PatientIdentifierValue).Result;
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }

            Immunization immunization = new Immunization();
            immunization.Meta = new Meta();
            immunization.Meta.Profile = new string[] { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-immunization" };

            immunization.PrimarySource = true;

            var status = EnumUtility.ParseLiteral<Immunization.ImmunizationStatusCodes>(ImmunizationStatusCode, ignoreCase: true);
            immunization.Status = status;
            immunization.Occurrence = new FhirDateTime(ImmunizationDateTime);
            immunization.Patient = new ResourceReference()
            {
                Display = patient.Name[0].ToString(),
                Reference = "Patient/" + patient.Id
            };
            immunization.VaccineCode = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding
                    {
                        System = "http://hl7.org/fhir/sid/cvx",
                        Code = ProductCVXCode,
                        Display = ProductCVXDisplay
                    }
                }
            };
            immunization.ReasonCode = new List<CodeableConcept>
            {
                new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://snomed.info/sct",
                            Code = ReasonCode
                        }
                    }
                }
            };
            
            immunization.Text = new Narrative
            {
                Status = Narrative.NarrativeStatus.Generated,
                Div = "<div xmlns=\"http://www.w3.org/1999/xhtml\">Immunization</div>"
            };
        
            var serializer = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            var immunizationJson = serializer.SerializeToString(immunization);
            Console.WriteLine(immunizationJson);
            return immunizationJson;    
            
        }   
    }
}

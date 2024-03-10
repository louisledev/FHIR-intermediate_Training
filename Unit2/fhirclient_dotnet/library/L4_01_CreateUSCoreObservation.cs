using Hl7.Fhir.Model; 
using Hl7.Fhir.Rest; 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace fhirclient_dotnet
{
    public class CreateUSCoreObs
    {
        public string CreateUSCoreR4LabObservation
        (  
        string ServerEndpoint,
        string PatientIdentifierSystem,
        string PatientIdentifierValue,
        string ObservationStatusCode, 
        string ObservationDateTime,
        string ObservationLOINCCode,
        string ObservationLOINCDisplay,
        string ResultType,
        string NumericResultValue,
        string NumericResultUCUMUnit,
        string CodedResultSNOMEDCode,
        string CodedResultSNOMEDDisplay
        )
        {
            Patient? patient = FhirClientHelper.GetPatientById(ServerEndpoint, PatientIdentifierSystem, PatientIdentifierValue);
            if (patient == null)
            {
                return "Error:Patient_Not_Found";
            }
            
            Observation observation = new Observation();
            observation.Meta = new Meta();
            observation.Meta.Profile = new List<string> { "http://hl7.org/fhir/us/core/StructureDefinition/us-core-observation-lab" };
            
            observation.Category = new List<CodeableConcept>
            {
                new()
                {
                    Coding = new List<Coding>
                    {
                        new Coding("http://terminology.hl7.org/CodeSystem/observation-category", "laboratory", "Laboratory")
                    }
                }
            };
            //Patient Reference
            observation.Subject = new ResourceReference()
            {
                Display = patient.Name[0].ToString(),
                Reference = "Patient/" + patient.Id
            };
            
            // Creating a dummy performer to avoid validation errors
            observation.Performer = new List<ResourceReference>
            {
                new()
                {
                    Display = "Laboratory",
                    Reference = "Organization/1"
                }
            };
            
            //Observation Date
            observation.Issued = DateTimeOffset.Parse(ObservationDateTime);
            observation.Effective = new FhirDateTime(ObservationDateTime);

            if (!Enum.TryParse(ObservationStatusCode, true, out ObservationStatus status))
            {
                return "Error: Invalid Observation Status";
            }
            observation.Status = status;
            
            if (ResultType.ToUpper() == "NUMERIC")
            {
                //Observation Value
                if (!decimal.TryParse(NumericResultValue, CultureInfo.GetCultureInfo("en-US"),  out var decimalValue))
                {
                    return "Error: Invalid Numeric Result Value";
                }

                Quantity q = new Quantity()
                    { Value = decimalValue, Code = NumericResultUCUMUnit, System = "http://unitsofmeasure.org", Unit = NumericResultUCUMUnit };
                observation.Value = q;
            }
            else if (ResultType.ToUpper() == "CODED")
            {
                CodeableConcept cc = new CodeableConcept();
                Coding coding = new Coding("http://snomed.info/sct", CodedResultSNOMEDCode, CodedResultSNOMEDDisplay);
                cc.Coding = new List<Coding> { coding };
                observation.Value = cc;
            }
            else
            {
                return "Error: Invalid Result Type";
            }

            //Observation Code
            CodeableConcept observationCode = new CodeableConcept();
            Coding observationCoding = new Coding("http://loinc.org", ObservationLOINCCode, ObservationLOINCDisplay);
            observationCode.Coding = new List<Coding> { observationCoding };
            observation.Code = observationCode;
            
            // Create narrative
            observation.Text = new Narrative
            {
                Status = Narrative.NarrativeStatus.Generated,
                Div = "<div xmlns=\"http://www.w3.org/1999/xhtml\"><p><b>Observation</b></p><p><b>Status</b>: " + status + "</p><p><b>Issued</b>: " + observation.Issued + "</p><p><b>Code</b>: " + observationCode.Coding[0].Code + "</p><p><b>Display</b>: " + observationCode.Coding[0].Display + "</p><p><b>Value</b>: " + observation.Value + "</p></div>"
            };
            
            var serializer = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            var observationJson = serializer.SerializeToString(observation);
            Console.WriteLine(observationJson);
            return observationJson;
        }   
    }
}

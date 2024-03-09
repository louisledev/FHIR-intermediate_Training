using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet
{
    public class FetchEthnicity
    {
        public string GetEthnicity
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

            var usEthnicity = patient.Extension.FirstOrDefault(e => e.Url == "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity");
            if (usEthnicity == null)
            {
                return "Error:No_us-core-ethnicity_Extension";
            }
            
            if (!TryGetEthnicityProperties(usEthnicity, out Coding? omgCoding, out string? text, out List<Coding>? detailedCodingList, out string? errorMessage))
            {
                return errorMessage;
            }

            var ExpEthnicity = "text|Hispanic or Latino\n";
            ExpEthnicity += "code|2135-2:Hispanic or Latino\n";
            ExpEthnicity += "detail|2184-0:Dominican\n";
            ExpEthnicity += "detail|2148-5:Mexican\n";
            ExpEthnicity += "detail|2151-9:Chicano\n";
            
            var result = $"text|{text}\n";
            result += $"code|{omgCoding.Code}:{omgCoding.Display}\n";
            foreach (var coding in detailedCodingList)
            {
                result += $"detail|{coding.Code}:{coding.Display}\n";
            }
            return result;
        }
        
        
        private static bool TryGetEthnicityProperties(
            Extension usExtension, 
            [NotNullWhen(true)] out Coding? omgCoding,  
            [NotNullWhen(true)] out string? text,
            out List<Coding>? detailedCodingList,  
            [NotNullWhen(false)] out string? errorMessage)
        {
            text = null;
            detailedCodingList = null;
            errorMessage = null;
            omgCoding = null;
            
            const string NonConformanceErrorMessage = "Error:Non_Conformant_us-core-ethnicity_Extension";
            var ombCategoryExtension = usExtension.Extension.FirstOrDefault(e => e.Url == "ombCategory");
            
            omgCoding = ombCategoryExtension?.Value as Coding;
            if (omgCoding == null)
            {
                errorMessage = NonConformanceErrorMessage;
                return false;
            }
            
            if (string.IsNullOrEmpty(omgCoding.Code) || string.IsNullOrEmpty(omgCoding.System))
            {
                errorMessage = NonConformanceErrorMessage;
                return false;
            }

            var detailedExtensions = usExtension.Extension.Where(e => e.Url == "detailed");
            detailedCodingList = detailedExtensions.Select(e => e.Value as Coding).Where(v => v != null).ToList();

            var textExtension = usExtension.Extension.FirstOrDefault(e => e.Url == "text");
            var textValue = textExtension?.Value as FhirString;
            text = textValue?.Value;
            if (string.IsNullOrEmpty(text))
            {
                errorMessage = NonConformanceErrorMessage;
                return false;
            }
            
            return true;
        }
    }
}
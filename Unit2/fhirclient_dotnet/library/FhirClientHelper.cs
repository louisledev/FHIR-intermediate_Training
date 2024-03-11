using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace fhirclient_dotnet;

public class FhirClientHelper
{
    public static Patient? GetPatientById(string serverEndPoint, string identifierSystem, string identifierValue)
    {
        var client = new FhirClient(serverEndPoint, FhirClientSettings.CreateDefault(), new LoggingHandler(new HttpClientHandler()));
        Patient? patient= null;
        try
        {
            Bundle bundle = client.Search<Patient>(new[]
                { "identifier=" + identifierSystem + "|" + identifierValue });
            if (bundle.Entry.Count > 0)
            {
                return (Patient)bundle.Entry[0].Resource;
            }
            else
            {
                return null;
            }
        }
        catch (FhirOperationException e)
        { 
            Console.WriteLine($"Fail to get patient:  {e.Message}");
            return null;
        }
    }
    

    public static IEnumerable<Practitioner> SearchPractitionersByCriteria(string serverEndPoint, string[] criteria)
    {
        var client = new FhirClient(serverEndPoint, FhirClientSettings.CreateDefault(), new LoggingHandler(new HttpClientHandler()));
        var practitioners = new List<Practitioner>();
        Bundle bundle = client.Search<Practitioner>(criteria);
        while (bundle != null)
        {
            foreach (Bundle.EntryComponent ent in bundle.Entry)
            {
                Practitioner pr = (Practitioner)ent.Resource;
                if (pr != null) 
                {
                    practitioners.Add(pr);
                }
            }
            bundle = client.Continue(bundle, PageDirection.Next);
        }
        return practitioners;
    }
    
    public static IEnumerable<Immunization> GetImmunizationsForPatient(string serverEndPoint, string patientId)
    {
        var client = new FhirClient(serverEndPoint, FhirClientSettings.CreateDefault(), new LoggingHandler(new HttpClientHandler()));
        var immunizations = new List<Immunization>();
        var criteria = new [] { $"patient={patientId}" }; 
        Bundle bundle = client.Search<Immunization>(criteria);
        while (bundle != null)
        {
            foreach (Bundle.EntryComponent ent in bundle.Entry)
            {
                Immunization im = (Immunization)ent.Resource;
                if (im != null) 
                {
                    immunizations.Add(im);
                }
            }
            bundle = client.Continue(bundle, PageDirection.Next);
        }
        return immunizations;
    }
   
    public static IEnumerable<MedicationRequest> GetMedicationRequestsForPatient(string serverEndPoint, string patientId)
    {
        var client = new FhirClient(serverEndPoint, FhirClientSettings.CreateDefault(), new LoggingHandler(new HttpClientHandler()));
        var medications = new List<MedicationRequest>();
        var criteria = new [] { $"patient={patientId}" }; 
        Bundle bundle = client.Search<MedicationRequest>(criteria);
        while (bundle != null)
        {
            foreach (Bundle.EntryComponent ent in bundle.Entry)
            {
                MedicationRequest med = (MedicationRequest)ent.Resource;
                if (med != null) 
                {
                    medications.Add(med);
                }
            }
            bundle = client.Continue(bundle, PageDirection.Next);
        }
        return medications;
    }
    
    public static Bundle? GetIPSDocumentForPatient(string serverEndPoint, string patientId)
    {
        var client = new FhirClient(serverEndPoint, FhirClientSettings.CreateDefault(), new LoggingHandler(new HttpClientHandler()));
        var patientLocation = ResourceIdentity.Build("Patient", patientId);
        
        Parameters par = new Parameters();  
        try
        {
            Resource? returnedResource = null;
            returnedResource = client.InstanceOperation(
                    patientLocation,
                    "summary",
                    par,
                    useGet: true);
        
            if (returnedResource is Bundle returnedBundle)
            {
                if (returnedBundle.Type != Bundle.BundleType.Document)
                {
                    Console.WriteLine($"Returned bundle is not a document, but a {returnedBundle.Type}");
                    return null;
                }

                return returnedBundle;
            }

            Console.WriteLine($"Returned resource is not a bundle, but a {returnedResource?.TypeName}");
            return null;
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to perform instance operation: {ex}");
            return null;
        }
    }
    
    public static async Task<ValueSet?> ExpandValueSetAsync(string serverEndPoint, Uri valueSet, string filter)
    {
        var settings = FhirClientSettings.CreateDefault();
        settings.PreferredFormat = ResourceFormat.Json;
        var client = new FhirClient(serverEndPoint, settings, new LoggingHandler(new HttpClientHandler()));
        try
        {
            return await client.ExpandValueSetAsync(identifier: new FhirUri(valueSet), filter: new FhirString(filter));
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Failed to expand value set: {ex.Message}");
            return null;
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to expand value set: {ex.Outcome}");
            return null;
        }
    }
}
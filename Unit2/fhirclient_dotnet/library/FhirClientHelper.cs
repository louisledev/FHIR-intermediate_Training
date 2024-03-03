using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
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
}
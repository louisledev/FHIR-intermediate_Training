// See https://aka.ms/new-console-template for more information

using CodeLab_Unit2;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Task = System.Threading.Tasks.Task;

public class Program
{
    public static async Task Main()
    {

        var client = CreateClient();
        // await ReadResource(client);
        // await CreateResource(client);
        // await UpdateResource(client);
        // await ExtendedOperation(client);
        // await ConditionalOperation(client);
        // await TerminologyCodeSystem(client);
        // await TerminologyExpand(client);
        await TerminologyValueSet(client);
    }

    private static FhirClient CreateClient()
    {
        // string FHIR_EndPoint = "https://server.fire.ly/r4";
        // string FHIR_EndPoint = "https://server.fire.ly/administration/r4";
        //string FHIR_EndPoint = "https://r4.ontoserver.csiro.au/fhir";
        // string FHIR_EndPoint = "http://hapi.fhir.org/baseR4";
        // string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
        string FHIR_EndPoint = "http://wildfhir4.aegis.net/fhir4-0-1";
        var settings = FhirClientSettings.CreateDefault();
        settings.PreferredFormat = ResourceFormat.Json;
        return new FhirClient(FHIR_EndPoint, settings, new LoggingHandler(new HttpClientHandler()));
    }

    private static ResourceIdentity VerifyResourceIdentity(Uri location, bool needId, bool needVid)
    {
        var result = new ResourceIdentity(location);

        if (result.ResourceType == null) throw new ArgumentException(nameof(location), "Must be a FHIR REST url containing the resource type in its path");
        if (needId && result.Id == null) throw new ArgumentException(nameof(location), "Must be a FHIR REST url containing the logical id in its path");
        if (needVid && !result.HasVersion) throw new ArgumentException(nameof(location), "Must be a FHIR REST url containing the version id in its path");

        return result;
    }

    
    private static async Task TerminologyValueSet(FhirClient client)
    {
        try
        {
            //ValueSet: LG33055-1
            //System: http://loinc.org
            //Code: 8867-4
            ValueSet v = new ValueSet();
            v.Id = "LG33055-1";
            Code cc = new Code();
            cc.Value = "8867-4";
            FhirUri cs = new FhirUri("http://loinc.org");
            var response1 = await client.ValidateCodeAsync(valueSet: v, code: cc,system:cs);
            Console.WriteLine(response1.Message.ToString());
        }
        catch (FhirOperationException ex)
        {
                Console.WriteLine(ex.Message);
        }
    }
    
    private static async Task TerminologyExpand(FhirClient client)
    {
        try {  
            FhirString sFil = new FhirString();  
            sFil.Value= "|filter|";  
            // 1.	Search for all the concepts related to diabetes – 73211009- (relationship: is-a)
            FhirUri u = new FhirUri("http://snomed.info/sct?fhir_vs=isa/73211009");
            var response1= await client.ExpandValueSetAsync(identifier:u);
            Console.WriteLine(response1?.Expansion?.Contains);
            // 2.  Search all the concepts in the general practice ref set / pain
            u = new FhirUri("http://snomed.info/sct?fhir_vs=ecl/450970008");
            var response2 = await client.ExpandValueSetAsync(identifier: u);
            Console.WriteLine(response2?.Expansion?.Contains);
        }  
        catch (FhirOperationException ex)  
        { 
            Console.WriteLine("Error:" +ex.Message);  
        }  
    }
    
    private static async Task TerminologyCodeSystem(FhirClient client)
    {
        Code code = new Code();  
        code.Value = "73211009";  
        FhirUri system = new FhirUri("http://snomed.info/sct");
        try
        {
            bool useExplicitOperation = true;
            if (useExplicitOperation)
            {
                var parameters = new Parameters();
                parameters.Add("system", system);
                parameters.Add("code", code);
                // var location = ResourceIdentity.Build("CodeSystem", "snomedct");
                var opResult = await client.TypeOperationAsync( "lookup", "CodeSystem", parameters, useGet: true);
                Console.WriteLine($"operation validation result: {opResult?.TypeName}");
            }
            else
            {
                var result = await client.ValidateCodeAsync(system: system, code: code);
                Console.WriteLine($"validation result: {result?.Result}");
            }
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    private static async Task ConditionalOperation(FhirClient client)
    {
        const string identifier = "888889";
        var myPatient = new Patient();
        myPatient.Name.Add(new HumanName
        {
            Given = new[] { "Alan" },
            Family = "Smith"
        });
        myPatient.BirthDate = "1965-05-06";
        myPatient.Gender = AdministrativeGender.Male;
        myPatient.Identifier.Add(new Identifier("http://central.patient.id/mrn", identifier));
        
        SearchParams conditions = new SearchParams();  
        conditions.Add("identifier", $"http://central.patient.id/mrn|{identifier}" );  
        try  
        {  
            Patient? createdPatient = await client.CreateAsync(myPatient,conditions);  
            Console.WriteLine(createdPatient?.Id);
            await client.DeleteAsync(createdPatient.ResourceIdentity());
        }  
        catch (FhirOperationException ex)  
        {  
            Console.WriteLine(ex.Message);  
        }  
    }

    private static async Task ExtendedOperation(FhirClient client)
    {
        var createdPatient = await CreatedPatient(client);
        if (createdPatient == null)
        {
            Console.WriteLine("Failed to create patient");
            return;
        }
        var location = ResourceIdentity.Build(createdPatient.TypeName, createdPatient.Id);
        
        var resourceIdentity = VerifyResourceIdentity(location, true, false);
        
        // var location = (Uri)createdPatient?.ResourceIdentity().MakeRelative();
        if (location == null)
        {
            Console.WriteLine("Failed to get location");
            return;
        }

        Console.WriteLine($"Location: {location}");
        
        Parameters par = new Parameters();  
        // Note: For Firelyserver, start and end parameters are not supported.
        par.Add("start", new FhirDateTime(2019, 11, 1));  
        par.Add("end", new FhirDateTime(2020, 2, 2));
        try
        {
            bool useInstanceOperation = true;
            Resource? returnedResource = null;
            if (useInstanceOperation)
            { 
                returnedResource = await client.InstanceOperationAsync(
                    location,
                    "everything",
                    par,
                    useGet: true);
            }
            else
            {
                var tx = new TransactionBuilder(client.Endpoint).ResourceOperation(createdPatient.TypeName, createdPatient.Id, "", "everything", par, false).ToBundle();
                returnedResource = await client.TransactionAsync(tx);
            }

            if (returnedResource is Bundle returnedBundle)
            {
                Console.WriteLine($"Returned bundle with {returnedBundle.Entry.Count} entries");
            }
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to perform instance operation: {ex}");
        }
        
        await client.DeleteAsync(createdPatient.ResourceIdentity());
    }
    
    
    
    private static async Task UpdateResource(FhirClient client)
    {
        var createdPatient = await CreatedPatient(client);

        // Patient Address: 3600 Papineau Avenue, Montreal, Quebec (H2K 4J5), Canada
        // Patient Phone #: 613-555-5555
        createdPatient.Address.Add(
            new Address
            {
                Line = new[] { "3600 Papineau Avenue" },
                City = "Montreal",
                State = "Quebec",
                PostalCode = "H2K 4J5"
            });
        createdPatient.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Work, "613-555-5555"));
        try
        {
            var updatedPatient = await client.UpdateAsync(createdPatient);
            Console.WriteLine($"Successfully updated patient: {updatedPatient?.Id}");
            await client.DeleteAsync(updatedPatient.ResourceIdentity());
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to update patient: {ex?.Outcome}");
        }

    }

    private static async Task<Patient?> CreatedPatient(FhirClient client)
    {
        var myPatient = new Patient();
        myPatient.Name.Add(new HumanName
        {
            Given = new[] { "Alan" },
            Family = "Smith"
        });
        myPatient.BirthDate = "1965-05-06";
        myPatient.Gender = AdministrativeGender.Male;
        myPatient.Identifier.Add(new Identifier("http://testpatient.id/mrn", "99999999"));
        Patient? createdPatient = null;
        try
        {
            createdPatient = await client.CreateAsync(myPatient);
            Console.WriteLine($"Created patient: {createdPatient?.Id}");
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to create patient: {ex.Outcome}");
        }

        return createdPatient;
    }

    private static async Task CreateResource(FhirClient client)
    {
        var myPractitioner = new Practitioner();
        myPractitioner.Name.Add(new HumanName
            {
                Given = new[] { "Madeleine" },
                Family = "Dellacroix"
            });

        myPractitioner.Address.Add(new Address
            {
                Line = new[] { "3766 Papineau Avenue" },
                City = "Montreal",
                State = "Quebec",
                PostalCode = "H2K 4J5"
            }
        );

        myPractitioner.Identifier.Add(new Identifier("http://canada.gov/cpn", "51922"));
        myPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone, ContactPoint.ContactPointUse.Work, "613-555-0192"));
        myPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Email, ContactPoint.ContactPointUse.Work, "cpamxms9dq@groupbuff.com"));
        Practitioner.QualificationComponent qc = new Practitioner.QualificationComponent();
        qc.Code = new CodeableConcept("http://canada.gov/cpnq", "OB/GYN", "Gynecologist");
        myPractitioner.Qualification.Add(qc);
        
        try
        {
            Practitioner? createdPractitioner = await client.CreateAsync(myPractitioner);
            Console.WriteLine($"Created practitioner: {createdPractitioner?.Id}");
            await client.DeleteAsync(createdPractitioner.ResourceIdentity());
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to create practitioner: {ex?.Outcome}");
        }
        
        
        // Smith, Alan, born 06 May 1965, male, mrn: http://testpatient.id/mrn/99999999
        var myPatient = new Patient();
        myPatient.Name.Add(new HumanName
            {
                Given = new[] { "Alan" },
                Family = "Smith"
            });
        myPatient.BirthDate = "1965-05-06";
        myPatient.Gender = AdministrativeGender.Male;
        myPatient.Identifier.Add(new Identifier("http://testpatient.id/mrn", "99999999"));
        try
        {
            Patient? createdPatient = await client.CreateAsync(myPatient);
            Console.WriteLine($"Created patient: {createdPatient?.Id}");
            await client.DeleteAsync(createdPatient.ResourceIdentity());
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"Failed to create patient: {ex.Outcome}");
        }
    }

    private static async Task ReadResource(FhirClient client)
    {
        string PatientLogicalId = "Patient/159";
        var myPatient = await client.ReadAsync<Patient>(PatientLogicalId);
        if (myPatient != null)
        {
            Console.WriteLine($"patient : {myPatient?.Name[0].GivenElement[0].Value} : {myPatient.VersionId}");
        }

        string PatientLogicalIdWithVersion = "Patient/159/_history/6f453ab6-3286-4466-b57c-68f2b2fd124b";
        var myPatientWithVersion = await client.ReadAsync<Patient>(PatientLogicalIdWithVersion);
        if (myPatientWithVersion != null)
        {
            Console.WriteLine($"patient with version: {myPatientWithVersion?.Name[0].GivenElement[0].Value} : {myPatientWithVersion.VersionId}");
        }

        var myPatientWithResourceId = await client.ReadAsync<Patient>(ResourceIdentity.Build("Patient", "159"));
        if (myPatientWithResourceId != null)
        {
            Console.WriteLine($"patient with resource id: {myPatientWithResourceId?.Name[0].GivenElement[0].Value} : {myPatientWithResourceId.VersionId}");
        }
    }
}
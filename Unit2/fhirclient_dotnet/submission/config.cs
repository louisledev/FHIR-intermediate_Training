namespace fhirclient_dotnet
{
    public class MyConfiguration
    {
        public string ServerEndpoint { get; } =
            "http://hl7-ips-server.org:8080/fhir"; // "http://wildfhir4.aegis.net/fhir4-0-0";


        public string PatientIdentifierSystem { get; } =
            "http://fhirintermediate.org/patient_id";


        public string TerminologyServerEndpoint { get; } =
            "https://r4.ontoserver.csiro.au/fhir"; // "https://snowstorm.ihtsdotools.org/fhir";

        public string AssignmentSubmissionFHIRServer { get; } =
            "http://fhir.hl7fundamentals.org/r4";

        public string StudentId { get; } =
            "louis@latourdev.net";

        public string StudentName { get; } =
            "Louis Latour";
    }
}
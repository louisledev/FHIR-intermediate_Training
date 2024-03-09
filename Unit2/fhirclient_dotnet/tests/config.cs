namespace fhirclient_dotnet
{
    public  class MyConfiguration
    {
         // Postman collection: https://www.postman.com/red-shadow-929607/workspace/fhir-intermediate-week2-setup/request/224650-030cb061-71f5-4b12-b98e-8d927d12a7b8
         /*
            Setup local server:
            docker pull hapiproject/hapi:latest
            docker run -p 8080:8080 hapiproject/hapi:latest
            
            Then run the postman collection to setup the server (need to change the variables to point to http://localhost:8080/fhir)
        */
         // From Rick in Chat: As mentioned above, it works for me using http://hl7-ips-server.org:8080/fhir and for terminology server https://r4.ontoserver.csiro.au/fhir
         public string ServerEndpoint{get;}= "http://hl7-ips-server.org:8080/fhir";
         // public string ServerEndpoint{get;}= "http://wildfhir4.aegis.net/fhir4-0-1";
         // public string ServerEndpoint{get;}="http://hapi.fhir.org/baseR4";
         // public string ServerEndpoint{get;}="http://localhost:8080/fhir";
            
        public string PatientIdentifierSystem{get;}=
            "http://fhirintermediate.org/patient_id";
        
            
        // public string TerminologyServerEndpoint{get;}= "https://snowstorm.ihtsdotools.org/fhir";
        public string TerminologyServerEndpoint{get;}= "https://r4.ontoserver.csiro.au/fhir";

        
        public string AssignmentSubmissionFHIRServer{get;}=
              "http://fhir.hl7fundamentals.org/r4";

        public string StudentId{get;}=
             "louis@latourdev.net";
        
        public string StudentName{get;}=
            "Louis Latour";    
        
    }

}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using Hl7.Fhir.Model;  
using Hl7.Fhir.Rest;

namespace fic_maex
{
  class MA_C04a_Direct_Read  : fic_maexe
    {
 
        public void Execute()
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            string PatientLogicalId = "Patient/159";
            var MyPatient = client.Read<Patient>(PatientLogicalId);
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json=s.SerializeToString(MyPatient);
            Console.WriteLine( json);
    
        }

    }
    class MA_C04b_Version_Read :fic_maexe
    {
 
        public void Execute()
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            string PatientLogicalId = "Patient/159";
            var MyPatient = client.Read<Patient>(PatientLogicalId);
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json=s.SerializeToString(MyPatient);
            Console.WriteLine( json);
    
        }

    }
    class MA_C04c_URL_Read  :fic_maexe
    {
        public void Execute()
        {
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            var MyPatient=client.Read<Patient>(ResourceIdentity.Build("Patient", "159"));
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json = s.SerializeToString(MyPatient);
            Console.WriteLine( json);

        }

    }
}
   

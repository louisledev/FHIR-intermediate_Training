using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using System.IO;
namespace fic_maex
{
  class EX_C06_UpdatePractitionerInstance: fic_maexe
    {
 
        public void Execute()
        {
             string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            String PractitionerLogicalId = "Practitioner/120376";
            Practitioner MyPractitioner = client.Read<Practitioner>(PractitionerLogicalId);
            byte[] imageBytes;
            string PhotoPath =AppContext.BaseDirectory + "/FHIR_RESOURCES/physician.jpeg";
            imageBytes=System.IO.File.ReadAllBytes(PhotoPath);
            

            
            //Step 2: Modify the elements you need
            MyPractitioner.Photo = new List<Attachment>()
             {
                new Attachment()
                {
                    ContentType="application/jpeg",
                    Data=imageBytes
                }
             };
            //Step 3: Invoke in the Server
            try
            {
                Practitioner UpdatedPractitioner = client.Update<Practitioner>(MyPractitioner);
                Console.WriteLine(UpdatedPractitioner.VersionId);
                

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }
            //Step 4: Process Response
        
        }
    }
}

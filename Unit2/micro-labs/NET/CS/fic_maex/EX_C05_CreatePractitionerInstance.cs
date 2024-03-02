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
  class EX_C05_CreatePractitionerInstance : fic_maexe
    {
 
        public void Execute()
        {
              /*
            Practictioner Data:
            Dellacroix, Madeleine, Canada Practitioner # (http://canada.gov/cpn : 51922)
            Phone #: 613-555-0192 
            Address: 3766 Papineau Avenue, Montreal, Quebec, H2K 4J5 
            Email: qcpamxms9dq @groupbuff.com Specialty: Gynecologist(http://canada.gov/cpnq : OB/GYN)
              */
            //Step 1: Create The Instance
            var MyPractitioner = new Practitioner();
            //Step 2: Populate The Instance
            MyPractitioner.Active = true;
            MyPractitioner.Name.Add(new HumanName().WithGiven("Madeleine").AndFamily("Dellacroix"));
            MyPractitioner.Identifier.Add(new Identifier("http://canada.gov/cpn", "51922"));
            MyPractitioner.Address.Add
            (
                new Address()
                {
                    Line = new string[] { "3766 Papineau Ave" },
                    City = "Montreal",
                    State = "Quebec",
                    PostalCode = "H2K 4J5",
                    Country = "Canada",

                }
            );
           
            MyPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone,
                                                        ContactPoint.ContactPointUse.Work,
                                                        "613-555-0192"));
            MyPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Email,
                                                        ContactPoint.ContactPointUse.Work,
                                                        "qcpamxms9dq@groupbuff.com"));
            Practitioner.QualificationComponent qc = new Practitioner.QualificationComponent();
            qc.Code = new CodeableConcept("http://canada.gov/cpnq", "OB/GYN", "Gynecologist");
            MyPractitioner.Qualification.Add(qc);
            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            try
            {
                Practitioner CreatedPractitioner = client.Create<Practitioner>(MyPractitioner);
                Console.WriteLine(CreatedPractitioner.Id);

            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }
            //Step 4: Process Response
           
        }
        
    }
}
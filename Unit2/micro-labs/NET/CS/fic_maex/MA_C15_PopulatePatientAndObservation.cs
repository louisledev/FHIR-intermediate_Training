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
  class MA_C15_PopulatePatientAndObservation : fic_maexe
    {
 
        public void Execute()
        {
                /*
            [use] Official [Prefix] Mr. [Given] Adam [Family] Alvarado, [Suffix] II 
Photo: data (base64): iVBORw0KGgoBBBBNSUhEUgBBBBEBBBBBCAYBBBBfFcSJBBBBC0lEQVR4nGNgBBIBBBUBBXpeqz8=
Identifiers
[system] http://nygc.com/patients [value] 1234567 (Medical Record Number)
//We can change this identifier to ensure conditional creation works fine
[system] http://citizens-id.gov/citizens [value] 59999999-I2 (National Identifier)
Address: 1234 Elm Street, New York, NY (90210), USA
Phone # : (555) 777-9999
e-mail Address: alvarado@everymail.com
Gender: Male
Active: Yes
Marital Status: Married
Born on: May 20, 1978
Preferred Language: Spanish (Spain). Also speaks English
Organization in Charge: New York General Clinic (www.nygc.com) – 9999 General Clinic Avenue, New York, NY (90210), USA NPI-ID (http://npi.org/identifiers): 7777777
Observation: Lab –Serum Creatinine Value: 65 umol/L, March 3, 2020 07:00:00 EST / LOINC Code: 14682-9 (http://loinc.org)

             */
            Patient p = new Patient();
            //Patient Name, using methods
            HumanName n = new HumanName();
            n.Family = "Alvarado";
            n.GivenElement.Add(new FhirString("Adam"));
            n.Suffix = new string[] { "II" };
            n.Prefix = new string[] { "Mr." };
            n.Use = HumanName.NameUse.Official;
            p.Name.Add(n);
            //Addresses, everything in line
            p.Address.Add
            (
                new Address()
                {
                    Line = new string[] { "1234 Elm Street" },
                    City = "New York",
                    State = "NY",
                    PostalCode = "90210",
                    Country = "USA",

                }
                )
            ;
            p.BirthDate = "1978-05-20";
            //Identifiers
            p.Identifier.Add
                (new Identifier()
                {
                    System = "http://citizens-id.gov/citizens",
                    Value = "59999999-I2"
                ,
                    Use = Identifier.IdentifierUse.Official
                });
            p.Identifier.Add
                (new Identifier() { System = "http://nygc.com/patients", Value = "12345678A" });
            //Attachments
            byte[] imageBytes;
            string PhotoBytes = "iVBORw0KGgoBBBBNSUhEUgBBBBEBBBBBCAYBBBBfFcSJBBBBC0lEQVR4nGNgBBIBBBUBBXpeqz8=";
            imageBytes = Encoding.UTF8.GetBytes(PhotoBytes);
            p.Photo = new List<Attachment>()
             {
                new Attachment()
                {
                    ContentType="application/jpeg",
                    Data=imageBytes
                }
             };
            // Coded Values
            // Codeable Concept -> Coding
            // Coding -> List<Coding>
            Coding c1 = new Coding("urn:ietf:bcp:47", "es");
            CodeableConcept cc1 = new CodeableConcept();
            cc1.Coding = new List<Coding> { c1 };
            p.Communication.Add
                       (new Patient.CommunicationComponent()
                       {
                           Language = cc1,
                           Preferred = true
                       });

            CodeableConcept cc2 = new CodeableConcept();
            Coding c2 = new Coding("urn:ietf:bcp:47", "en-US");
            cc2.Coding = new List<Coding> { c2 };
            p.Communication.Add
                       (new Patient.CommunicationComponent()
                       {
                           Language = cc2,
                           Preferred = false
                       });
            //Gender
            p.Gender = AdministrativeGender.Male;
            //Marital Status
            CodeableConcept ccm = new CodeableConcept();
            Coding cm = new Coding("http://terminology.hl7.org/CodeSystem/v3-MaritalStatus", "M");
            ccm.Coding = new List<Coding> { cm };
            p.MaritalStatus = ccm;

            //Email and Telephone
            p.Telecom.Add(new ContactPoint()
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = "(555) 777-9999"
            });
            p.Telecom.Add(new ContactPoint()
            {
                System = ContactPoint.ContactPointSystem.Email,
                Value = "alvarado@everymail.com"
            });
            //Boolean
            p.Active = true;
            //Organization in Charge just identifier and display
            p.ManagingOrganization = new ResourceReference()
            {
                Display = "New York General Clinic",
                Identifier = new Identifier(system: "http://npi.org/identifiers", value: "7777777")
            };
            String DivNarrative =
                "<div xmlns='http://www.w3.org/1999/xhtml'>" +
                "Name:" + p.Name[0].ToString() + "<br/>" +
                "Identifier:" + p.Identifier[1].System.ToString() + "-" + p.Identifier[1].Value.ToString() + "<br/>" +
                "Gender:" + p.Gender.ToString() + "<br/>" +
                "BirthDate:" + p.BirthDate.ToString() + "<br/>" +
                "Active:" + p.Active.ToString() + "<br/>" +
                "Managing Org:" + p.ManagingOrganization.Display.ToString() + "<br/>" +
                "Address:" + p.Address[0].Line.First().ToString() + " " +
                  p.Address[0].State.ToString() + " " +
                  p.Address[0].City.ToString() + " (" +
                  p.Address[0].PostalCode.ToString() + ") " +
                  p.Address[0].Country.ToString() +
                "<br/>" +
                "Telecom:" + p.Telecom[0].System.ToString() + "-" + p.Telecom[0].Value.ToString() + "<br/>" +
                "</div>";


            p.Text = new Narrative()
            {
                Status = Narrative.NarrativeStatus.Generated,
                Div = DivNarrative

            };
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            Patient PatientCreated = new Patient();
            SearchParams conditions = new SearchParams();
            conditions.Add("identifier", p.Identifier[1].System + "|" + p.Identifier[1].Value);

            try
            {
                PatientCreated = client.Create<Patient>(p, conditions);
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }
            Observation o = new Observation();
            //Patient Reference
            o.Subject = new ResourceReference()
            {
                Display = PatientCreated.Name[0].ToString(),
                Reference = "Patient/" + PatientCreated.Id

            };
            //Observation Date
            o.Issued = DateTimeOffset.Parse("2020-03-03 07:00:00+0300");
            //Observation Value
            Quantity q = new Quantity()
            { Value = 65M, Code = "umol/L", System = "http://unitsofmeasure.org", Unit = "umol/L" };
            o.Value = q;
            //Observation Code
            CodeableConcept ccu = new CodeableConcept();
            Coding cu = new Coding("http://loinc.org", "14682-9");

            ccu.Coding = new List<Coding> { cu };
            o.Code = ccu;
            try
            {
                Observation ObservationCreated = client.Create<Observation>(o);
                Console.WriteLine("Patient:" + PatientCreated.Id + " " + "Observation:" + ObservationCreated.Id);
            }
            catch (FhirOperationException Exc)
            {
                Console.WriteLine(Exc.Outcome.ToString());
            }


        }
    }
}

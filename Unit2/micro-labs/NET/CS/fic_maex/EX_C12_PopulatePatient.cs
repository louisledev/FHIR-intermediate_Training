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
  class EX_C12_PopulatePatient : fic_maexe
    {
 
        public void Execute()
        {
          /*
            Name:
            [use] Official[Prefix] Ms. [Given] Eve[Family] Everywoman, [Suffix]  III
            Photo: data(base64): iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVR4nGNgAAIAAAUAAXpeqz8=
            Identifiers
            [system] http://hospital.gov/patients [value] 9999999AA (Medical Record Number)
            [system] http://citizens-id.gov/citizens [value] 69999999-ZZI (National Identifier)
            You may need to change this identifier to allow for conditional creation 
            Address: 9999 Patient Street, Ann Arbor, MI(90210), USA
            Phone # : (777) 555-9999
            e-mail Address: eve @everywoman.com
            Gender: Female
            Active: No
            Deceased on: Feb 13, 2019 10:30:00
            Marital Status: Widower
            Born on: July 23, 1968
            Preferred Language: English (USA). Also speaks Spanish
            Organization in Charge: Ann Arbor General Hospital (www.aagh.org) – 9999 General Hospital Street, Ann Arbor, MI (90210), USA
            Observation: Lab – Fasting Serum Glucose Value: 6,3 mmol/L, Jan 20 20 07:00:00 EST / LOINC Code: 14771-0 (http://loinc.org)
            */
            Patient p = new Patient();
            //Patient Name, using methods
            HumanName n = new HumanName();
            n.Family = "Everywoman";
            n.GivenElement.Add(new FhirString("Eve"));
            n.Suffix = new string[] { "III" };
            n.Prefix = new string[] { "Ms." };
            n.Use = HumanName.NameUse.Official;
            p.Name.Add(n);
            //Addresses, everything in line
            p.Address.Add
            (
                new Address()
                {
                    Line = new string[] { "9999 Patient Street" },
                    City = "Ann Arbor",
                    State = "MI",
                    PostalCode = "90210",
                    Country = "USA",

                }
                )
            ;
            //Dates (date type)
            p.BirthDate = "1968-07-23";
            //Dates (including hour)
            p.Deceased = new FhirDateTime(DateTimeOffset.Parse("2019-02-13 10:30:00"));
            //Identifiers
            p.Identifier.Add
                (new Identifier() { System = "http://citizens-id.gov/citizens", Value = "69999999-ZZI"
                ,Use=Identifier.IdentifierUse.Official});
            p.Identifier.Add
                (new Identifier() { System = "http://hospital.gov/patients", Value = "9999999AA" });
            //Attachments
            byte[] imageBytes;
            string PhotoBytes = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAC0lEQVR4nGNgAAIAAAUAAXpeqz8=";
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
            Coding c1 = new Coding("urn:ietf:bcp:47", "en-US");
            CodeableConcept cc1 = new CodeableConcept();
            cc1.Coding = new List<Coding> { c1 };
            p.Communication.Add
                       (new Patient.CommunicationComponent()
                       {
                           Language = cc1,
                           Preferred = true
                       });

            CodeableConcept cc2 = new CodeableConcept();
            Coding c2 = new Coding("urn:ietf:bcp:47", "es");
            cc2.Coding = new List<Coding> { c2 };
            p.Communication.Add
                       (new Patient.CommunicationComponent()
                       {
                           Language = cc2,
                           Preferred = false
                       });
            //Gender
            p.Gender = AdministrativeGender.Female;
            //Marital Status
            CodeableConcept ccm = new CodeableConcept();
            Coding cm = new Coding("http://terminology.hl7.org/CodeSystem/v3-MaritalStatus", "W");
            ccm.Coding = new List<Coding> { cm };
            p.MaritalStatus = ccm;

            //Email and Telephone
            p.Telecom.Add(new ContactPoint()
            { System = ContactPoint.ContactPointSystem.Phone,
                Value = "(777) 555-9999" });
            p.Telecom.Add(new ContactPoint()
            { System = ContactPoint.ContactPointSystem.Email,
                Value = "eve@everywoman.com" });
            //Boolean
            p.Active = false;
            //Organization in Charge just identifier and display
            p.ManagingOrganization = new ResourceReference()
            {
                Display = "Ann Arbor General Hospital",
                Identifier = new Identifier(system: "http://npi.org/identifiers", value: "999999")
            };
            String DivNarrative =
                "<div xmlns='http://www.w3.org/1999/xhtml'>" +
                "Name:" + p.Name[0].ToString()+"<br/>"+
                "Identifier:" + p.Identifier[1].System.ToString()+"-"+ p.Identifier[1].Value.ToString()+"<br/>" +
                "Gender:" + p.Gender.ToString()+ "<br/>" +
                "BirthDate:"+p.BirthDate.ToString()+ "<br/>" +
                "Active:"+p.Active.ToString() + "<br/>" +
                "Managing Org:"+p.ManagingOrganization.Display.ToString()+ "<br/>" +
                "Address:" + p.Address[0].Line.First().ToString() +" "+
                  p.Address[0].State.ToString() + " "+
                  p.Address[0].City.ToString() +" ("+
                  p.Address[0].PostalCode.ToString() +") "+
                  p.Address[0].Country.ToString() +
                "<br/>" +
                "Telecom:" + p.Telecom[0].System.ToString() +"-"+ p.Telecom[0].Value.ToString()+"<br/>" +
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
            conditions.Add("identifier",p.Identifier[1].System+ "|" + p.Identifier[1].Value);

            try
            {
                PatientCreated=client.Create<Patient>(p,conditions);
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
                Reference = "Patient/"+PatientCreated.Id

            };
            //Observation Date
            o.Issued= DateTimeOffset.Parse("2019-01-01 10:30:00+0300");
            //Observation Value
            Quantity q = new Quantity()
            {Value=6.3M,Code="mmol/L",System= "http://unitsofmeasure.org" ,Unit="mmol/L"};
            o.Value = q;
            //Observation Code
            CodeableConcept ccu = new CodeableConcept();
            Coding cu = new Coding("http://loinc.org", "14771-0");
            
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

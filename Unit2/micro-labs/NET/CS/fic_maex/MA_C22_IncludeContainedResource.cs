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
  class MA_C22_IncludeContainedResource : fic_maexe
    {
 
        public void Execute()
        {
                 //1-Create our Intollerance
            /*
            ClinicalStatus: active
            type: allergy
            category: food
            criticality: high
            patient: Patient/49293
            code: 91935009 (http://hl7.org/fhir/ValueSet/substance-code): Allergy to Peanuts
            onSet: 10 years old (Age)

            */

            AllergyIntolerance a = new AllergyIntolerance()
            {
                ClinicalStatus = new CodeableConcept()
                {
                    Coding = new List<Coding>
                    {
                     new Coding(system:"http://terminology.hl7.org/CodeSystem/condition-clinical",
                     code:"active")
                    }
                },
                Type = AllergyIntolerance.AllergyIntoleranceType.Allergy,
                Criticality = AllergyIntolerance.AllergyIntoleranceCriticality.High,
                Patient = new ResourceReference(reference: "Patient/49293"),
                Code = new CodeableConcept()
                {
                    Coding = new List<Coding>
                    {
                     new Coding(system:"http://hl7.org/fhir/ValueSet/substance-code",
                     code:"91935009",
                     display:"Allergy to Peanuts")
                    }
                },
                Onset = new Age()
                {
                    Code = "y",
                    Unit = "y",
                    Value = 10M
                }

            }

            ;
            //2-Create our contained resource for asserter. It's a Practitioner
            /*
              Practictioner Data:
              Dellacroix, Madeleine, Canada Practitioner # (http://canada.gov/cpn : 51922)
              Phone #: 613-555-0192 Address: 3766 Papineau Avenue, Montreal, Quebec, H2K 4J5 
              Email: qcpamxms9dq @groupbuff.com Specialty: Gynecologist(http://canada.gov/cpnq : OB/GYN)
            */
            //Step 1: Create The Instance
            var MyPractitioner = new Practitioner();
            //Step 2: Populate The Instance
            MyPractitioner.Active = true;
            MyPractitioner.Name.Add(new HumanName().WithGiven("Madeleine").AndFamily("Dellacroix"));
            MyPractitioner.Identifier.Add(new Identifier("http://canada.gov/cpn", "51922"));
            MyPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone,
                                                        ContactPoint.ContactPointUse.Work,
                                                        "613-555-0192"));
            MyPractitioner.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Email,
                                                        ContactPoint.ContactPointUse.Work,
                                                        "qcpamxms9dq@groupbuff.com"));
            Practitioner.QualificationComponent qc = new Practitioner.QualificationComponent();
            qc.Code = new CodeableConcept("http://canada.gov/cpnq", "OB/GYN", "Gynecologist");
            MyPractitioner.Qualification.Add(qc);
            //Set Internal Id
            MyPractitioner.Id = "#MyPractitioner";
            //Add Contained Resource
            a.Contained.Add(MyPractitioner);
            a.Asserter = new ResourceReference(MyPractitioner.Id);

            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();

            string json1 = s.SerializeToString(a);
            Console.WriteLine(json1);

        }
    }
}

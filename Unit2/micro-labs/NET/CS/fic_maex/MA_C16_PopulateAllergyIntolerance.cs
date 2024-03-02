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
  class MA_C16_PopulateAllergyIntolerance : fic_maexe
    {
 
        public void Execute()
        {
              //We will populate our first condition
            //Using less intermediate variable
            //mostly fluent
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
                Category = new List<AllergyIntolerance.AllergyIntoleranceCategory?>() { AllergyIntolerance.AllergyIntoleranceCategory.Food },
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
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json1 = s.SerializeToString(a);
            Console.WriteLine(json1);

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;

namespace FhirIntermediateMaEx
{
    public partial class EX_C11_PopulateCondition : Form
    {
        public EX_C11_PopulateCondition()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {

            //We will populate our first condition
            //Using less intermediate variable
            //mostly fluent

            Condition c = new Condition();

            c.ClinicalStatus = new CodeableConcept()
            {
                //Only ONE clinical status
                //Coding allows multiple coding for the same clinical status
                Coding = new List<Coding>
                {
                     new Coding(system:"http://terminology.hl7.org/CodeSystem/condition-clinical", code:"active")
                }
            };
            
            c.Stage.Add(

            //Special element for Stage with elements as subcomponents
            new Condition.StageComponent()
            {
                Type = new CodeableConcept()
                {
                    Coding = new List<Coding>
                        {
                            new Coding(system:"http://snomed.info/sct",code: "422375001",display:"Carcinoma of Colon, Clinical stage III")
                        }
                }
                ,
                Summary = new CodeableConcept()
                {
                    Coding = new List<Coding>
                        {
                            new Coding(system:"http://snomed.info/sct",code: "385356007",display:"Tumor Stage Finding")
                        }
                }
                ,
                Assessment = new List<ResourceReference> { new ResourceReference()
                {
                     Reference="DiagnosticReport/201993"
                }
            }
            }
            );
            
            c.Onset = new Age()
            {   Code = "y",
                Unit = "y",
                Value = 10M
            };

            //We will populate our second condition
            //Using  intermediate variables
            //less fluent

            Condition c2 = new Condition();
            Extension e2 = new Extension(
         "http://hl7.org/fhir/uv/ips/StructureDefinition/abatement-dateTime-uv-ips", new FhirDateTime(2018, 2, 2));

            c2.ClinicalStatus = new CodeableConcept()
            {
                //Only ONE clinical status
                //Coding allows multiple coding for the same clinical status
                Coding = new List<Coding>
                {
                     new Coding(system:"http://terminology.hl7.org/CodeSystem/condition-clinical", code:"active")
                }
            };

            Condition.StageComponent cstc = new Condition.StageComponent();

            cstc.Type = new CodeableConcept()
            {
                Coding = new List<Coding>
                {
                    new Coding(system:"http://snomed.info/sct",code: "422375001",display:"Carcinoma of Colon, Clinical stage III")
                }

            };
            cstc.Summary = new CodeableConcept()
            {
                Coding = new List<Coding>
                        {
                            new Coding(system:"http://snomed.info/sct",code: "385356007",display:"Tumor Stage Finding")
                        }
            };
            cstc.Assessment = new List<ResourceReference> { new ResourceReference()
                {
                     Reference="DiagnosticReport/201993"
                }
            };

         
            c2.Stage.Add(cstc);

            c2.Onset = new Date("2019-02-02");

            

            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            string json1 = s.SerializeToString(c);
            string json2 = s.SerializeToString(c2);

            c2.Extension.Add(e2);

            MessageBox.Show("Condition 1" + json1 + " Condition 2" + json2);

        }
        
    }
}

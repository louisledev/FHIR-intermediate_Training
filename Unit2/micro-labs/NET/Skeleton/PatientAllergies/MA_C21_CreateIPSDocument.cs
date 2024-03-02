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
    public partial class MA_C21_CreateIPSDocument : Form
    {
        public MA_C21_CreateIPSDocument()
        {
            InitializeComponent();
        }
        //Some helper functions 
        //So we do not write this stuff hundreds of times
        //And the code is better read
        // 
        //This helper function creates a CodeableConcept as a list Coding with 1 item
        //with system:code:display
        //
        private CodeableConcept CCB(string system, string code, string display)
        {
            CodeableConcept cc = new CodeableConcept()
            {
                Coding = new List<Coding> {
     new Coding(system: system, code: code, display: display)
    }


            };
            return cc;
        }
        //This helper function creates a Meta element
        //with a Profile

        private Meta PRO(string ProfileUrn)
        {
            Meta md = new Meta();
            md.Profile = new string[] {
    "urn:" + ProfileUrn
   };
            return md;
        }
        //This helper function creates a Narrative element
        //with a 'generated' status and whichever text we want
        private Narrative Narr(string MyNarrativeText)
        {
            Narrative myna = new Narrative();
            myna.Status = Narrative.NarrativeStatus.Generated;
            myna.Div = MyNarrativeText;
            return myna;
        }
        //This helper function creates an entry component to
        //add to our bundle

        private Bundle.EntryComponent BEC(string id, Resource res)
        {
            Bundle.EntryComponent e = new Bundle.EntryComponent()
            {
                FullUrl = id,
                Resource = res
            };
            return e;
        }
        //This helper function creates a string concatenating family and given


        private string NAM(Patient res)
        {
            HumanName h = res.Name[0];
            string FullName = h.Family + "," + h.Given.First().ToString();
            return FullName;
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {

            /*
            a) We start with the provided example
            Minimum Resources for an IPS are:
            Composition (1st Entry)
            +Patient
            +Organization
            +Author
            +MedicationStatement
            +Condition
            +AllergyIntolerance
            b) And add the immunization record

          */
            // We set up full Entry Ids for all the resources
            // They will serve as references between the resources inside the bundle

            String CompositionEntryId = Guid.NewGuid().ToString();
            String OrganizationEntryId = Guid.NewGuid().ToString();
            String PatientEntryId = Guid.NewGuid().ToString();
            String AuthorEntryId = Guid.NewGuid().ToString();
            String MedicationStatementEntryId = Guid.NewGuid().ToString();
            String ConditionEntryId = Guid.NewGuid().ToString();
            String AllergyIntoleranceMedicationEntryId = Guid.NewGuid().ToString();
            String AllergyIntoleranceFoodEntryId = Guid.NewGuid().ToString();
            //Adding a full entry id for our immunization entry
            String ImmunizationEntryId = Guid.NewGuid().ToString();
            //Adding a full entry id for the author of our immunization entry
            String ImmunizationAuthorEntryId = Guid.NewGuid().ToString();

            //We populate the organization
            //IPS Must-Support: identifier, name, telecom, address
            Organization org = new Organization();
            org.Identifier.Add(new Identifier()
            {
                System = "http://npi.org/identifiers",
                Value = "7777777"
            });
            Address aOrg = new Address();
            org.Name = "New York General Clinic";
            org.Address.Add(
             new Address()
             {
                 Line = new string[] {
       "1234 Org Street"
               },
                 City = "New York",
                 State = "NY",
                 PostalCode = "90210",
                 Country = "USA",

             }
            );
            org.Telecom.Add(new ContactPoint()
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = "(555) 888-7777"
            });

            org.Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Organization-uv-ips");


            // Setup a Reference to this Organization
            ResourceReference OrganizationRef;
            OrganizationRef = new ResourceReference()
            {
                Display = org.Name,
                Reference = "Organization/" + OrganizationEntryId
            };


            //We populate the author
            Practitioner pra = new Practitioner();
            //Step 1: Create The Instance
            //Step 2: Populate The Instance
            pra.Active = true;
            pra.Name.Add(new HumanName().WithGiven("Madeleine").AndFamily("Dellacroix"));
            pra.Identifier.Add(new Identifier("http://canada.gov/cpn", "51922"));
            pra.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone,
             ContactPoint.ContactPointUse.Work,
             "613-555-0192"));
            pra.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Email,
             ContactPoint.ContactPointUse.Work,
             "qcpamxms9dq@groupbuff.com"));
            Practitioner.QualificationComponent qc = new Practitioner.QualificationComponent();
            qc.Code = new CodeableConcept("http://canada.gov/cpnq", "OB/GYN", "Gynecologist");
            pra.Qualification.Add(qc);

            pra.Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Practitioner-uv-ips");

            //Setup a reference to this practitioner
            ResourceReference AuthorRef = new ResourceReference()
            {
                Display = pra.Name[0].Family + "," + pra.Name[0].Given.First().ToString(),
                Reference = "Practitioner/" + AuthorEntryId
            };


            //As requested,another author for the immunization
            Practitioner immpra = new Practitioner();
            //Step 1: Create The Instance
            //Step 2: Populate The Instance
            immpra.Active = true;
            immpra.Name.Add(new HumanName().WithGiven("Immune").AndFamily("Betty"));
            immpra.Identifier.Add(new Identifier("http://physicians-id.gov/physicians", "999999"));
            immpra.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Phone,
             ContactPoint.ContactPointUse.Work,
             "555-444-2222"));
            immpra.Telecom.Add(new ContactPoint(ContactPoint.ContactPointSystem.Email,
             ContactPoint.ContactPointUse.Work,
             "qcpamzms9dq@groupbuff.com"));

            immpra.Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Practitioner-uv-ips");

            //Setup a reference to this practitioner
            ResourceReference immAuthorRef = new ResourceReference()
            {
                Display = immpra.Name[0].Family + "," + immpra.Name[0].Given.First().ToString(),
                Reference = "Practitioner/" + ImmunizationAuthorEntryId
            };

            //We populate the patient
            //IPS Must-Support: name (family/given), telecom, gender, birthDate, address (line/city/state/postalCode
            // /country) , contact (relationship,name, telecom, address, organization - ref , communication ,
            // generalPractitioner
            Patient p = new Patient();
            HumanName n = new HumanName()
            {
                Given = new string[] {
      "Adama"
     },
                Family = "Alvarado",
                Suffix = new string[] {
      "II"
     },
                Prefix = new string[] {
      "Ms."
     },
                Use = HumanName.NameUse.Official
            };

            //Full Name
            p.Name.Add(n);


            //Identifier
            p.Identifier.Add(new Identifier(
             system: "http://citizens-id.gov/citizens", value: "123456"));
            //We will use this identifier to search for the patient We won't add it if it exists

            p.Address.Add(
             new Address()
             {
                 City = "NY",
                 Country = "US",
                 PostalCode = "90210",
                 State = "NY",
                 Line = new string[] {
       "1234 Elm Street"
               }
             });

            //Phone
            p.Telecom.Add(new ContactPoint()
            {
                System = ContactPoint.ContactPointSystem.Phone,
                Value = "(555) 777-9999"
            });

            //E-Mail Address
            p.Telecom.Add(new ContactPoint()
            {
                System = ContactPoint.ContactPointSystem.Email,
                Value = "alvarado@everymail.com"
            });

            //Gender
            p.Gender = AdministrativeGender.Male;

            //Active
            p.Active = true;


            //Birth Date
            p.BirthDate = "1978-06-20";


            //Organization in Charge
            p.ManagingOrganization = OrganizationRef;
            //Our author is one of the practitioners for this patient
            p.GeneralPractitioner.Add(AuthorRef);

            p.Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Patient-uv-ips");


            ResourceReference PatientRef = new ResourceReference()
            {
                Display = NAM(p),
                Reference = "Patient/" + PatientEntryId
            };
            /*
            Create the composition resource
            Composition resource mandatory elements for IPS are
            identifier, status, type
            author, title, confidentiality
            attester, custodian
            Mandatory sections for IPS: Problems, Medications, Allergies
            We create the header first, with all the context for the Document
            Then we add the sections
             */
            Composition cmp = new Composition()
            {
                //Document Unique Identifier issued by myhospital
                //Document Status: Final
                //Document Type: IPS
                // Subject for the Document: Reference to the Patient Entry
                // The date/time the Component was Created
                // Author
                // Confidentiality : Normal
                // Title
                // Attester
                Identifier = new Identifier("http://myhospital.org.uk", Guid.NewGuid().ToString()),
                Status = CompositionStatus.Final,
                Type = CCB("http://loinc.org", "60591-5", "Patient Summary Document"),
                Subject = PatientRef,
                DateElement = new FhirDateTime(DateTimeOffset.Now),
                Author = new List<ResourceReference>() {
      AuthorRef
     },
                Confidentiality = Composition.v3_ConfidentialityClassification.N,
                Title = "Patient Summary For " + NAM(p),
                Attester = new List<Composition.AttesterComponent>() {
      new Composition.AttesterComponent() {
       Mode = Composition.CompositionAttestationMode.Legal,
        Party = OrganizationRef
      }
     },
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Composition-uv-ips"),
                Custodian = OrganizationRef
            };


            // Sections

            // Allergies



            // Section Entries: Allergy to Penicillin
            AllergyIntolerance peni = new AllergyIntolerance()
            {

                Type = AllergyIntolerance.AllergyIntoleranceType.Allergy,
                Category = new List<AllergyIntolerance.AllergyIntoleranceCategory?>() {
      AllergyIntolerance.AllergyIntoleranceCategory.Medication
     },
                Code = CCB(system: "http://snomed.info/sct", code: "373270004", display: "Substance with penicillin structure and antibacterial mechanism of action (substance)"),
                Criticality = AllergyIntolerance.AllergyIntoleranceCriticality.High,
                Patient = PatientRef,
                Text = Narr("ALLERGY - MEDICATION - CRITICALITY - HIGH - PENICILLIN"),
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/AllergyIntolerance-uv-ips")
            };

            ResourceReference peniRef = new ResourceReference("AllergyIntolerance/" + AllergyIntoleranceMedicationEntryId);

            //                  No known food allergies

            Narrative alfonarrative = new Narrative();
            alfonarrative.Status = Narrative.NarrativeStatus.Generated;
            alfonarrative.Div = "No Known Food Allergies";

            AllergyIntolerance nofo = new AllergyIntolerance()
            {

                Type = AllergyIntolerance.AllergyIntoleranceType.Allergy,
                Category = new List<AllergyIntolerance.AllergyIntoleranceCategory?>() {
     AllergyIntolerance.AllergyIntoleranceCategory.Food
    },
                Patient = PatientRef,
                Code = CCB(system: "http://hl7.org/fhir/uv/ips/CodeSystem/absent-unknown-uv-ips", code: "no-known-food-allergies", display: "No Known Food Allergies"),
                Text = alfonarrative,
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/AllergyIntolerance-uv-ips")
            };
            ResourceReference nofoRef = new ResourceReference("AllergyIntolerance/" + AllergyIntoleranceFoodEntryId);

            Narrative alernarrative = new Narrative();
            alernarrative.Status = Narrative.NarrativeStatus.Generated;
            alernarrative.Div = "Food: Not Known Medication: Allergy to penicillin";
            //Section's mandatory elements: code, title, text, entries
            Composition.SectionComponent alg = new Composition.SectionComponent()
            {
                Code = CCB(system: "http://loinc.org", code: "48765-2", display: "Allergies and Intolerance Document"),
                Title = "Allergies and Intolerances",
                Text = alernarrative


            };
            alg.Entry.Add(peniRef);
            alg.Entry.Add(nofoRef);
            //Add the section to the composition
            cmp.Section.Add(alg);

            //Problems
            /*
                 Must-Support by IPS for Condition
                 clinicalStatus
                 verificationStatus
                 category
                 severity
                 code
                 subject
                 onsetDateTime
                 asserter
                 */
            Condition cond = new Condition()
            {
                ClinicalStatus = CCB("http://terminology.hl7.org/CodeSystem/condition-clinical", "active", "Active"),
                VerificationStatus = CCB("http://terminology.hl7.org/CodeSystem/condition-ver-status", "confirmed", "Confirmed"),
                Category = new List<CodeableConcept> {
     CCB("http://loinc.org", "75326-9", "Problem")
    },
                Severity = CCB("http://loinc.org", "LA6751-7", "Moderate"),
                Code = CCB("http://snomed.info/sct", "54329005", "Acute myocardial infarction of anterior wall"),
                Onset = new FhirDateTime("2019-01-01"),
                Subject = PatientRef,
                Asserter = AuthorRef,
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Condition-uv-ips"),
                Text = Narr("Acute myocardial infarction of anterior wall, 01-Jan-2019, Active, Confirmed")
            };

            ResourceReference condRef = new ResourceReference("Condition/" + ConditionEntryId);

            //Section's mandatory elements: code, title, text, entries
            Composition.SectionComponent prb = new Composition.SectionComponent()
            {
                Code = CCB(system: "http://loinc.org", code: "11450-4", display: "Problems"),
                Title = "Active Problems",
                Text = cond.Text


            };
            //Add the entry to the section
            prb.Entry.Add(condRef);
            //Add the section to the composition
            cmp.Section.Add(prb);

            // MedicationStatement

            ResourceReference mediRef = new ResourceReference("MedicationStatement/" + MedicationStatementEntryId);

            MedicationStatement medi = new MedicationStatement()
            {
                Status = MedicationStatement.MedicationStatusCodes.Active,
                Medication = CCB("http://snomed.info/sct", "108979001", "Clopidogrel"),
                Subject = PatientRef,
                InformationSource = AuthorRef,
                ReasonReference = new List<ResourceReference>() {
      condRef
     },
                Dosage = new List<Dosage>() {
      new Dosage() {
       Text = "75 mg orally once a day",
        Route = CCB(" http://standardterms.edqm.eu", "20053000", "Oral"),
        Timing = new Timing() {
         Code = CCB("http://terminology.hl7.org/CodeSystem/v3-GTSAbbreviation", "QD", "Daily")
        }
      }
     },
                Text = Narr("Clopidogrel, 75mg Orally, once a day"),
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/MedicationStatement-uv-ips")
            };
            //Section's mandatory elements: code, title, text, entries
            Composition.SectionComponent med = new Composition.SectionComponent()
            {
                Code = CCB(system: "http://loinc.org", code: "10160-9", display: "Medications"),
                Title = "Medication",
                Text = medi.Text


            };
            //Add the entry to the section
            med.Entry.Add(mediRef);
            //Add the section to the composition
            cmp.Section.Add(med);

            Immunization immu = new Immunization()
            {
                Status = Immunization.ImmunizationStatusCodes.Completed,
                VaccineCode = CCB("http://www.whocc.no/atc", "J07BB02", "influenza, inactivated, split virus or surface antigen "),
                Patient = PatientRef,
                PrimarySource = true,
                ReportOrigin = CCB("http://hl7.org/fhir/R4/codesystem-immunization-origin.html", "PROVIDER", "Other Provider"),
                Recorded = "2019-02-02",
                Route = CCB("http://standardterms.edqm.eu", "20035000", "Intramuscular use"),
                Performer = new List<Immunization.PerformerComponent>() {
                    new Immunization.PerformerComponent()
                        {
                            Actor=immAuthorRef,
                            Function= CCB("http://terminology.hl7.org/CodeSystem/v2-0443", "AP", "Administering Provider")
                         }
                },
                Text = Narr("Influenza 10-FEB-2019 Adm by Betty Immune, MD"),
                Meta = PRO("http://hl7.org/fhir/uv/ips/StructureDefinition/Immunization-uv-ips")
            };
            
            ResourceReference immuRef = new ResourceReference("Immunization/" + ImmunizationEntryId);
            //Section's mandatory elements: code, title, text, entries
            Composition.SectionComponent imm = new Composition.SectionComponent()
            {
                Code = CCB(system: "http://loinc.org", code: "11369-6", display: "Immunizations"),
                Title = "Immunizations",
                Text = immu.Text


            };
            //Add the entry to the section
            imm.Entry.Add(immuRef);
            //Add the section to the composition
            cmp.Section.Add(imm);


       

            Bundle bt = new Bundle();
            {
                //Bundles for Documents are of DOCUMENT type
                bt.Type = Bundle.BundleType.Document;
                bt.Id = Guid.NewGuid().ToString();
                //We add all the entries to the bundle
                bt.Entry.AddRange(
                 new List<Bundle.EntryComponent>() {
      BEC(CompositionEntryId, cmp),
       BEC(PatientEntryId, p),
       BEC(OrganizationEntryId, org),
       BEC(AuthorEntryId, pra),
       BEC(ImmunizationAuthorEntryId,immpra),
       BEC(ConditionEntryId, cond),
       BEC(MedicationStatementEntryId, medi),
       BEC(AllergyIntoleranceMedicationEntryId, peni),
       BEC(AllergyIntoleranceFoodEntryId, nofo),
       BEC(ImmunizationEntryId,immu)
                 }
                );

                try
                {
                    string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
                    var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);

                    Bundle t_result = client.Create<Bundle>(bt);
                    string t_response = t_result.Id;
                    MessageBox.Show(t_response);

                }
                catch (FhirOperationException Exc)
                {
                    MessageBox.Show(Exc.Outcome.ToString());
                }

            }
        }
    }
}
  

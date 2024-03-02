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
    public partial class MA_C20_CreateTransactionBundle : Form
    {
        public MA_C20_CreateTransactionBundle()
        {
            InitializeComponent();
        }

        private void btnTestClient_Click(object sender, EventArgs e)
        {

            //First we will create all the resource instances we need
            //Using temporary identifiers for the resource we want to relate
            //through references inside of the transaction
            //These references will be resolved by the FHIR server
            String PatientEntryId = Guid.NewGuid().ToString();
            String DeviceEntryId = Guid.NewGuid().ToString();
            String DiastolicEntryId = Guid.NewGuid().ToString();
            String SystolicEntryId = Guid.NewGuid().ToString();

            //We populate the patient
            //Very important to remember the patient identifier: we will use it to check if it doesn't exist
            //in the conditional request
            Patient p;
            p = new Patient();
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


            // Organization in Charge - Simple reference to an identifier/display, not url

            p.ManagingOrganization = new ResourceReference()
            {
                Identifier = new Identifier(system: "http://npi.org/identifiers", value: "7777777"),
                Display = "New York General Clinic"

            };


            Identifier did = new Identifier("http://wwww.americandevices.com", "123456");
            Device d = new Device();
            d.Identifier.Add(did);
            d.DeviceName.Add(new Device.DeviceNameComponent() { Name = "BLOOD PRESSURE MASTER" });
            d.Version.Add(new Device.VersionComponent() { Value = "1.23.78" });
            d.Manufacturer = "AMERICAN BLOOD PRESSURE DEVICES";
            d.ModelNumber = "2000 PLUS";
            d.SerialNumber = "123456";
            //The patient for the device is the one in the first entry of the transaction
            //See below the first addEntry/FullURL
            d.Patient = new ResourceReference()
            {
                Reference = "Patient/" + PatientEntryId,
                Display = p.Name[0].Given.First().ToString() + "," + p.Name[0].Family

            };
            String displayDev = // d.DeviceName[0].Name
             " " + d.ModelNumber +
             " " + d.SerialNumber;


            //First Observation: Diastolic Pressure, Related to the patient and device

            String display = p.Name[0].Given.First().ToString() + "," + p.Name[0].Family;

            Observation obsS = new Observation();
            obsS.Subject = new ResourceReference()
            {
                Display = display,
                Reference = "Patient/" + PatientEntryId

            };
            obsS.Device = new ResourceReference()
            {
                Reference = "Device/" + DeviceEntryId,
                Display = displayDev

            };

            obsS.Issued = new DateTimeOffset().LocalDateTime;
            // The value for the creatinine
            Quantity q = new Quantity()
            {
                Value = 120.0M,
                Unit = "mmHG",
                Code = "mmHG",
                System = "http://unitsofmeasure.org"
            };
            obsS.Value = q;
            //Systolic BP
            CodeableConcept lc;
            lc = new CodeableConcept();
            lc.Coding.Add(new Coding("http://loinc.org", "8480-6"));
            obsS.Code = lc;

            //Second Observation: Diastolic Pressure, Related to the patient and device
            //80 8462-4
            Observation obsD = new Observation();
            obsD.Subject = new ResourceReference()
            {
                Display = display,
                Reference = "Patient/" + PatientEntryId

            };
            obsD.Device = new ResourceReference()
            {
                Reference = "Device/" + DeviceEntryId,
                Display = displayDev

            };

            obsD.Issued = new DateTimeOffset().LocalDateTime;
            // The value for the creatinine
            Quantity q2 = new Quantity()
            {
                Value = 80.0M,
                Unit = "mmHG",
                Code = "mmHG",
                System = "http://unitsofmeasure.org"
            };
            obsD.Value = q2;
            //Systolic BP
            CodeableConcept lc2;
            lc2 = new CodeableConcept();
            lc2.Coding.Add(new Coding("http://loinc.org", "8462-4"));
            obsS.Code = lc2;

            //Patient
            String PatToken = p.Identifier[0].System + "|" + p.Identifier[0].Value;
            string PatUrl = "Patient?identifier=" + PatToken;
            string DevToken = d.Identifier[0].System + "|" + d.Identifier[0].Value;
            string DevUrl = "Device?identifier=" + DevToken;
            Bundle bt = new Bundle();
            {
                bt.Type = Bundle.BundleType.Transaction;
                bt.Id = Guid.NewGuid().ToString();
            }

            bt.Entry.Add(
             new Bundle.EntryComponent()
             {
                 FullUrl = PatientEntryId,
                 Resource = p,
                 Request = new Bundle.RequestComponent()
                 {
                     Method = Bundle.HTTPVerb.PUT,
                     Url = PatUrl
                 }

             });

            //Device

            bt.Entry.Add(
             new Bundle.EntryComponent()
             {
                 FullUrl = DeviceEntryId,
                 Resource = d,
                 Request = new Bundle.RequestComponent()
                 {
                     Method = Bundle.HTTPVerb.PUT,
                     Url = DevUrl
                 }

             });

            //Obs 1 : Systolic
            bt.Entry.Add(
             new Bundle.EntryComponent()
             {
                 FullUrl = SystolicEntryId,
                 Resource = obsS,
                 Request = new Bundle.RequestComponent()
                 {
                     Method = Bundle.HTTPVerb.POST,
                     Url = "Observation"
                 }

             });
            //Obs 2 : Diastolic
            bt.Entry.Add(
             new Bundle.EntryComponent()
             {
                 FullUrl = DiastolicEntryId,
                 Resource = obsD,
                 Request = new Bundle.RequestComponent()
                 {
                     Method = Bundle.HTTPVerb.POST,
                     Url = "Observation"
                 }

             });

            //Step 3: Invoke in the Server
            string FHIR_EndPoint = "http://fhir.hl7fundamentals.org/r4";
            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);




            try
            {
                Bundle t_result = client.Transaction(bt);
                string t_response = "";
                foreach (Bundle.EntryComponent r in t_result.Entry)
                {
                    t_response = t_response + r.Response.Location + "\r\n";
                }
                MessageBox.Show(t_response);

            }
            catch (FhirOperationException Exc)
            {
                MessageBox.Show(Exc.Outcome.ToString());
            }

        }
    }
}
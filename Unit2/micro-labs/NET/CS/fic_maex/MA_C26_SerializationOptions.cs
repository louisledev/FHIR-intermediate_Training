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
  class MA_C26_SerializationOptions  : fic_maexe
    {
 
        public void Execute()
        {
              Practitioner MyPractitioner = new Practitioner();
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
            Narrative myna = new Narrative();
            myna.Status = Narrative.NarrativeStatus.Generated;
            myna.Div = "This is something we don´t want to serialize";
            MyPractitioner.Text = myna;
            Hl7.Fhir.Serialization.FhirJsonSerializer s = new Hl7.Fhir.Serialization.FhirJsonSerializer();
            s.Settings.Pretty = false;
            
            String results = s.SerializeToString(instance:MyPractitioner,summary:SummaryType.True);
            Console.WriteLine(results);

        }
    }
}

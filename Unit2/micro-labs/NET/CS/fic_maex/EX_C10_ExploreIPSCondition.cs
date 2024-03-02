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
  class EX_C10_ExploreIPSCondition : fic_maexe
    {
 
        public void Execute()
        {
                string cnd = LoadCondition();
            var parser = new Hl7.Fhir.Serialization.FhirXmlParser();

            try
            {
                string Result = "No Clinical Status";
                Condition c = parser.Parse<Condition>(cnd);
                if (c.ClinicalStatus!=null)
                {
                    if (c.ClinicalStatus.Coding!=null)
                    {
                        if (c.ClinicalStatus.Coding.Count != 0)
                        {
                            Result=c.ClinicalStatus.Coding[0].Code
                                + "-" + c.ClinicalStatus.Coding[0].System
                                ;
                        }
                    }
                    

                }
                c.Text.Div.ToString();
                Console.WriteLine(Result);
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Error Parsing Resource " + fe.Message.ToString());

            }

        }
        
        private string LoadCondition()
        {

            string ret = "<Condition> " +
         "<id value=\"IPS-examples-Condition-01\"/>\n" +
         "    <meta>\n" +
         "        <profile\n" +
         "            value=\"http://hl7.org/fhir/uv/ips/StructureDefinition/condition-uv-ips\"/>\n" +
         "    </meta>\n" +
         "    <text>\n" +
         "        <status value=\"generated\"/>\n" +
         "        <div xmlns=\"http://www.w3.org/1999/xhtml\"><p><b>Generated Narrative with Details</b></p><p><b>id</b>: IPS-examples-Condition-01</p><p><b>meta</b>: </p><p><b>text</b>: </p><p><b>identifier</b>: c87bf51c-e53c-4bfe-b8b7-aa62bdd93002</p><p><b>clinicalStatus</b>: Active <span style=\"background: LightGoldenRodYellow\">(Details : {http://terminology.hl7.org/CodeSystem/condition-clinical code 'active' = 'Active)</span></p><p><b>verificationStatus</b>: Confirmed <span style=\"background: LightGoldenRodYellow\">(Details : {http://terminology.hl7.org/CodeSystem/condition-ver-status code 'confirmed' = 'Confirmed)</span></p><p><b>category</b>: Problem <span style=\"background: LightGoldenRodYellow\">(Details : {LOINC code '75326-9' = 'Problem', given as 'Problem'})</span></p><p><b>severity</b>: Moderate <span style=\"background: LightGoldenRodYellow\">(Details : {LOINC code 'LA6751-7' = 'LA6751-7', given as 'Moderate'})</span></p><p><b>code</b>: Menopausal flushing (finding) <span style=\"background: LightGoldenRodYellow\">(Details : {SNOMED CT code '198436008' = 'Hot flush', given as 'Menopausal flushing (finding)'})</span></p><p><b>subject</b>: <a href=\"#patient_IPS-examples-Patient-01\">Generated Summary: id: IPS-examples-Patient-01; 574687583; active; Martha DeLarosa ; ph: +31788700800(HOME); FEMALE; birthDate: 01/05/1972</a></p><p><b>recordedDate</b>: 01/10/2016</p></div>\n" +
         "    </text>\n" +
         "    <identifier>\n" +
         "        <system value=\"urn:oid:1.2.3.999\"/>\n" +
         "        <value value=\"c87bf51c-e53c-4bfe-b8b7-aa62bdd93002\"/>\n" +
         "    </identifier>\n" +
         /*
         "    <clinicalStatus>\n" +
         "        <coding>\n" +
         "            <system\n" +
         "                value=\"http://terminology.hl7.org/CodeSystem/condition-clinical\"/>\n" +
         "            <code value=\"active\"/>\n" +
         "        </coding>\n" +
         "    </clinicalStatus>\n" +
         */
         "    <verificationStatus>\n" +
         "        <coding>\n" +
         "            <system\n" +
         "                value=\"http://terminology.hl7.org/CodeSystem/condition-ver-status\"/>\n" +
         "            <code value=\"confirmed\"/>\n" +
         "        </coding>\n" +
         "    </verificationStatus>\n" +
         "    <category>\n" +
         "        <coding>\n" +
         "            <system value=\"http://loinc.org\"/>\n" +
         "            <code value=\"75326-9\"/>\n" +
         "            <display value=\"Problem\"/>\n" +
         "        </coding>\n" +
         "    </category>\n" +
         "    <severity>\n" +
         "        <coding>\n" +
         "            <system value=\"http://loinc.org\"/>\n" +
         "            <code value=\"LA6751-7\"/>\n" +
         "            <display value=\"Moderate\"/>\n" +
         "        </coding>\n" +
         "    </severity>\n" +
         "    <code>\n" +
         "        <coding>\n" +
         "            <extension url=\"http://hl7.org/fhir/StructureDefinition/translation\">\n" +
         "                <extension url=\"lang\">\n" +
         "                    <valueCode value=\"nl-NL\"/>\n" +
         "                </extension>\n" +
         "                <extension url=\"content\">\n" +
         "                    <valueString value=\"opvliegers\"/>\n" +
         "                </extension>\n" +
         "            </extension>\n" +
         "            <system value=\"http://snomed.info/sct\"/>\n" +
         "            <code value=\"198436008\"/>\n" +
         "            <display value=\"Menopausal flushing (finding)\"/>\n" +
         "        </coding>\n" +
         "    </code>\n" +
         "    <subject>\n" +
         "        <reference\n" +
         "            value=\"http://hapi.fhir.org/baseR4/Patient/IPS-examples-Patient-01\"/>\n" +
         "    </subject>\n" +
         "    <onsetDateTime value=\"2015-10-20T20:30:00\"/>\n" +
         "    <recordedDate value=\"2016-10\"/>\n" +
         "</Condition>\n";
            
            return ret;
        }
    }
    
}

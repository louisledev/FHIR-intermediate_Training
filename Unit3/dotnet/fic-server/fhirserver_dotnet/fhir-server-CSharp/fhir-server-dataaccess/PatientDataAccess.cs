using System;
using fhir_server_entity_model;
using System.Collections.Generic;
using System.Linq;

namespace fhir_server_dataaccess
{
    public static class PatientDataAccess
    {
        
        public static  List<LegacyPerson> GetAllPatients(string SpecificPatientId = null)
        {
            return PersonDataAccess.GetAllPersons(SpecificPatientId);
        }
    }
}


using System;
using fhir_server_entity_model;
using System.Collections.Generic;
using System.Linq;

namespace fhir_server_dataaccess
{
    public static class PractitionerDataAccess
    {
        public static  List<LegacyPerson> GetAllPractitioner(string SpecificPractitionerId = null)
        {
            return PersonDataAccess.GetAllPersons(SpecificPractitionerId, IsPractitioner);
        }
        
        public static bool IsPractitioner(long id)
        {
            List<LegacyPersonIdentifier> personIdentifiers = PersonDataAccess.GetPersonDocType(id);
            foreach (var docType in personIdentifiers)
            {
                var system = LegacyAPIAccess.getLegacyIdentifierCode(docType.identifier_type_id);
                if (system=="NPI")
                {
                    return true;
                }
            }
            return false;
        }
    }
}


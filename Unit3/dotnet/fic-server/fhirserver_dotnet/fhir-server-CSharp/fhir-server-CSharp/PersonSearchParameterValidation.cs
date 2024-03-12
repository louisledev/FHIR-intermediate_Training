using fhir_server_sharedservices;
using Hl7.Fhir.Model;
using fhir_server_entity_model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace fhir_server_CSharp
{
    public static class PersonSearchParameterValidation
    {
        public static bool ValidateSearchParams(HttpListenerRequest request, string personType, ref bool hardIdSearch, out DomainResource operation, out List<LegacyFilter> criteria)
        {
            operation = null;
            criteria = new List<LegacyFilter>(); 
        
            string searchParamId = string.Empty;
            bool rtnValue = true;

            string resourceBeingSearched = request.Url.AbsolutePath.Replace(FhirServerConfig.FHIRServerUrl, string.Empty);

            if (!string.IsNullOrEmpty(resourceBeingSearched) && resourceBeingSearched.Contains("/"))
            {
                searchParamId = resourceBeingSearched.Substring(resourceBeingSearched.IndexOf("/"));
                if (searchParamId.StartsWith("/"))
                {
                    resourceBeingSearched = resourceBeingSearched.Substring(0, resourceBeingSearched.IndexOf("/"));
                    searchParamId = searchParamId.Substring(1);
                }
            }
            else if (!string.IsNullOrEmpty(resourceBeingSearched) && resourceBeingSearched.Contains("?"))
            {
                searchParamId = resourceBeingSearched.Substring(resourceBeingSearched.IndexOf("?"));
                if (searchParamId.StartsWith("?"))
                {
                    resourceBeingSearched = resourceBeingSearched.Substring(0, resourceBeingSearched.IndexOf("?"));
                }
            }

            //As of now only GET is supported. Sorry about that as I did not have time to complete POST operation.
            //For the time being, please use the data inserted into the database manually. I inserted some data manually into DB.
            if (!request.HttpMethod.Trim().ToUpper().Equals("GET"))
            {
                rtnValue = false;
                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.MethodNotAllowed;
                operation = Utilz.getErrorOperationOutcome($"Unsupported http method '{request.HttpMethod}' for {personType} resource- Server knows how to handle: [GET] only for {personType} resource");
            }
            else if (string.IsNullOrEmpty(resourceBeingSearched))
            {
                rtnValue = false;
                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                operation = Utilz.getErrorOperationOutcome($"Unknown resource type '{resourceBeingSearched}' - Server knows how to handle: [Patient, Practitioner, MedicationRequest]");
            }
            else if (!resourceBeingSearched.Equals(personType, StringComparison.Ordinal))
            {
                rtnValue = false;
                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                operation = Utilz.getErrorOperationOutcome($"Unknown resource type '{resourceBeingSearched}' - Server knows how to handle: [Patient, Practitioner, MedicationRequest]");
            }
            else if (resourceBeingSearched.Equals(personType, StringComparison.Ordinal) && request.QueryString != null && request.QueryString.Count == 0 && !string.IsNullOrEmpty(searchParamId))
            {
                if (!long.TryParse(searchParamId, out _))
                {
                    rtnValue = false;
                    Program.HttpStatusCodeForResponse = (int)HttpStatusCode.NotFound;
                    operation = Utilz.getErrorOperationOutcome($"Resource {resourceBeingSearched}/{searchParamId} is not known");
                }
                else
                {   
                    LegacyFilter sc=new LegacyFilter();
                    sc.criteria=LegacyFilter.field.id;
                    sc.value=searchParamId.ToString();
                    criteria.Add(sc);
                    rtnValue=true;
                    hardIdSearch = true;
                    return rtnValue;
                }
            }
            else
            {
                if (request.QueryString != null && request.QueryString.Count > 0)
                {
                    foreach (var param in request.QueryString)
                    {
                        if (param.ToString().Equals("_id", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("_id", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, telecom, family, gender, name, identifier]");
                                break;
                            }
                            LegacyFilter sc=new LegacyFilter();
                            sc.criteria=LegacyFilter.field._id;
                            sc.value=request.QueryString[param.ToString()];
                            rtnValue=true;
                            criteria.Add(sc);
                        }
                        else if (param.ToString().Equals("identifier", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("identifier", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, telecom, family, gender, name, identifier]");
                                break;
                            }

                            string search_system = string.Empty;
                            string search_type = string.Empty;
                            string search_value = string.Empty;

                            string[] SystemAndValue = request.QueryString[param.ToString()].Split("|", StringSplitOptions.RemoveEmptyEntries);

                            if (SystemAndValue != null && SystemAndValue.Length > 1)
                            {
                                search_system = SystemAndValue[0];
                                search_type = SharedServices.GetSystemTypeMapping().SystemMap.Where(e => e.System.Equals(search_system, StringComparison.Ordinal)).Select(e => e.Type).FirstOrDefault();
                                search_value = SystemAndValue[1];

                                if (string.IsNullOrEmpty(search_type)) 
                                {
                                    rtnValue = false;
                                    Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                    operation = Utilz.getErrorOperationOutcome($"Identifier '{search_system}' is not a valid system for {personType} resource.");
                                    break;
                                }
                            }
                            else
                            {
                                search_value = request.QueryString[param.ToString()];
                            }
                            
                            if (personType == "Practitioner" && search_type.ToLower() != "npi")
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                string msg = $"HTTP 400 Bad Request: Practitioners can only be found knowing the NPI identifier - You are specifying : {search_type.ToUpper()}";
                                operation = Utilz.getErrorOperationOutcome(msg);
                                break;
                            }

                            if (!string.IsNullOrEmpty(search_type) && search_type.Equals("ID"))
                            {
                            }
                            else
                            {
                                LegacyFilter sc=new LegacyFilter();
                                sc.criteria=LegacyFilter.field.identifier;
                                sc.value=search_system+"|"+search_value;
                                criteria.Add(sc);
                                rtnValue=true;
                            }
                        }
                        else if (param.ToString().Equals("family", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("family", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }
                            else
                            {
                                LegacyFilter sc=new LegacyFilter();
                                sc.criteria=LegacyFilter.field.family;
                                sc.value=request.QueryString[param.ToString()];
                                criteria.Add(sc);
                                rtnValue=true;   
                            };
                        }
                        else if (param.ToString().Equals("name", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("name", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }

                                LegacyFilter sc=new LegacyFilter();
                                sc.criteria=LegacyFilter.field.name;
                                sc.value=request.QueryString[param.ToString()];
                                criteria.Add(sc);
                                rtnValue=true;   
                        
                        }
                        else if (param.ToString().Equals("given", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("given", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }

                            LegacyFilter sc=new LegacyFilter();
                            sc.criteria=LegacyFilter.field.given;
                            sc.value=request.QueryString[param.ToString()];
                            criteria.Add(sc);
                            rtnValue=true;   
                        
                        }
                        else if (param.ToString().Equals("birthdate", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("birthdate", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }
                                LegacyFilter sc=new LegacyFilter();
                                sc.criteria=LegacyFilter.field.birthdate;
                                sc.value=request.QueryString[param.ToString()];
                                criteria.Add(sc);
                                rtnValue=true;   
                        
                        }
                        else if (param.ToString().Equals("gender", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("gender", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }

                                LegacyFilter sc=new LegacyFilter();
                                sc.criteria=LegacyFilter.field.gender;
                                sc.value=request.QueryString[param.ToString()];
                                criteria.Add(sc);
                                 rtnValue=true;   
                            
                        }
                        else if (param.ToString().Equals("telecom", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("telecom", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }

                            var value = request.QueryString[param.ToString()];
                            if (!TryParseTelecom(value, out string telecomSystem, out string telecomValue))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Telecom '{value}' is not a valid for Patient resource.");
                                break;
                            }

                            if (telecomSystem.ToLower() != "email")
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.NotImplemented;
                                string msg = $"HTTP 501 Not Implemented: The underlying server only handles email addresses for the {personType.ToLower()}s, thus search by system=phone is not implemented";
                                operation = Utilz.getErrorOperationOutcome(msg);
                                break;
                            }
                            
                            LegacyFilter sc=new LegacyFilter();
                            sc.criteria=LegacyFilter.field.email;
                            sc.value=telecomValue;
                            criteria.Add(sc);
                            rtnValue=true;   
                            
                        }
                        else if (param.ToString().Equals("email", StringComparison.OrdinalIgnoreCase))
                        {
                            //check the case now;
                            if (!param.ToString().Equals("email", StringComparison.Ordinal))
                            {
                                rtnValue = false;
                                Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                                operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, email, telecom, family, gender, name, identifier]");
                                break;
                            }

                            var value = request.QueryString[param.ToString()];
                            
                            LegacyFilter sc=new LegacyFilter();
                            sc.criteria=LegacyFilter.field.email;
                            sc.value=value;
                            criteria.Add(sc);
                            rtnValue=true;   
                            
                        }
                        else
                        {
                            rtnValue = false;
                            Program.HttpStatusCodeForResponse = (int)HttpStatusCode.BadRequest;
                            operation = Utilz.getErrorOperationOutcome($"Unknown search parameter \"{param}\". Value search parameters for this search are: [_id, birthdate, telecom, family, gender, name, identifier]");
                            break;
                        }
                    }

                    
                }
            }

            return rtnValue;
        }

        private static bool TryParseTelecom(string telecom, out string system, out string value)
        {
            system = null;
            value = null;
            
            if (string.IsNullOrEmpty(telecom))
                return false;

            var parts = telecom.Split('|');
            if (parts.Length == 1)
            {
                system = "email";
                value = parts[0];
                return true;
            }
            
            if (parts.Length != 2)
                return false;

            system = parts[0];
            value = parts[1];
            return true;
        }
    }
}

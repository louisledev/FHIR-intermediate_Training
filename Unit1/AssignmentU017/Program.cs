using Hl7.Fhir.Model;

namespace fic_device
{
    class Program
    {
        static void Main(string[] args)
        {
            string PatientId = "X12984";
            string DeviceList = GetPatientDevices(PatientId);
            Console.WriteLine(DeviceList);
        }

        public static string GetPatientDevices(string PatientId)

        {
            string FHIR_EndPoint = "http://fhirserver.hl7fundamentals.org/fhir";

            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            var p = new Hl7.Fhir.Rest.SearchParams();
            p.Add("patient", PatientId);
            var results = client.Search<Device>(p);
            string output = "";
            while (results != null)
            {
                if (results.Total == 0) output = "No devices found";

                foreach (var entry in results.Entry)
                {
                    var device = (Device)entry.Resource;

                    var mandatoryOrMustSupportedEntries = new List<string>();
                    if (device.UdiCarrier.Count > 0)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.UdiCarrier[0].CarrierHRF);
                        mandatoryOrMustSupportedEntries.Add(device.UdiCarrier[0].DeviceIdentifier);
                    }
                    
                    if (device.Status.HasValue)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.Status.Value.ToString());
                    }

                    
                    if (device.DistinctIdentifier != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.DistinctIdentifier);
                    }

                    if (device.ManufactureDate != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.ManufactureDate);
                    }
                    
                    if (device.ExpirationDate != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.ExpirationDate);
                    }

                    if (device.LotNumber != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.LotNumber);
                    }
                    
                    if (device.SerialNumber != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.SerialNumber);
                    }
                    
                    if (device.Type != null)
                    {
                        mandatoryOrMustSupportedEntries.Add(device.Type.Coding[0].Display);
                    }
                    var content = String.Join('|', mandatoryOrMustSupportedEntries);
                    output = output + content + "\r\n";
                }
                // if there are more results, continue with the next page
                results = client.Continue(results);
            }
            return output;
        }
    }
}   
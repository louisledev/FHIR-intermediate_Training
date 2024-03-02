using System;
using Hl7.Fhir.Model;

namespace fic_goal
{
    class Program
    {
        static void Main(string[] args)
        {
            string PatientId = "X12984";
            string goalList = GetPatientGoals(PatientId);
            Console.WriteLine(goalList);
        }

        public static string GetPatientGoals(string PatientId)

        {
            string FHIR_EndPoint = "http://fhirserver.hl7fundamentals.org/fhir";

            var client = new Hl7.Fhir.Rest.FhirClient(FHIR_EndPoint);
            var p = new Hl7.Fhir.Rest.SearchParams();
            p.Add("subject", PatientId);
            var results = client.Search<Goal>(p);
            string output = "";
            while (results != null)
            {
                if (results.Total == 0) output = "No goal found";

                foreach (var entry in results.Entry)
                {
                    var goal = (Goal)entry.Resource;
                    string Content = goal.LifecycleStatus
                                     + "|" + goal.Description.Text;
                    output = output + Content + "\r\n";
                }

                // get the next page of results
                results = client.Continue(results);
            }

            return output;
        }
    }
}
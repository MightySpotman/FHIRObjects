using Dapper;
using FhirDomain.SQLRepoDefinitions;
using Hl7.Fhir.Model;
using Microsoft.Data.SqlClient;

namespace FhirDataAccess.SQLRepos
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Patient GetPatientByName(string name)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    string query = "SELECT * FROM Demographics WHERE PatientID = @id";

                    var patient = cn.QueryFirstOrDefault<Patient>(query, new { id = name }) ?? new Patient();
                    
                    Patient p = new Patient();
                    
                    return patient;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Patient GetPatientById(string id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection())
                {
                    string query = "SELECT * FROM Demographics WHERE PatientID = @id";

                    var patient = cn.QueryFirstOrDefault<Patient>(query, new { id = id }) ?? new Patient();

                    return patient;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

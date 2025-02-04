using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;

namespace FHIRObjects.SQLRepoDefinitions
{
    public interface IPatientRepository
    {
        Patient GetPatientByName(string name);

        Patient GetPatientById(string id);
    }
}

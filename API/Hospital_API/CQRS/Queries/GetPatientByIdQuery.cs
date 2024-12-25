using Hospital_API.CQRS.Interfaces;
using Hospital_API.Responses;

namespace Hospital_API.CQRS.Queries
{
    public record class GetPatientByIdQuery(Guid Id) : IQuery<PatientResponse>;
}

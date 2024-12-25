using Hospital_API.Constants;
using Hospital_API.CQRS.Interfaces;
using Hospital_API.Responses;

namespace Hospital_API.CQRS.Queries;

public record GetAllPatientsByBirthDateQuery(SearchingPrefix Prefix, DateTime StartDate, DateTime EndDate) : IQuery<IEnumerable<PatientResponse>>;
using Hospital_API.CQRS.Interfaces;
using Hospital_API.Requests;
using Hospital_API.Responses;

namespace Hospital_API.CQRS.Commands;

public record BatchPatientCommand(IEnumerable<PatientCreateRequest> Request) : ICommand<IEnumerable<PatientResponse>>;


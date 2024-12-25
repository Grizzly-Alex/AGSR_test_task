using Hospital_API.CQRS.Interfaces;
using Hospital_API.Requests;


namespace Hospital_API.CQRS.Commands;

public record class UpdatePatientCommand(PatientUpdateRequest Request) : ICommand;
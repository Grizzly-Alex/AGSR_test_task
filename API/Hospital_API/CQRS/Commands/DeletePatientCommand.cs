using Hospital_API.CQRS.Interfaces;

namespace Hospital_API.CQRS.Commands;

public record class DeletePatientCommand(Guid Id) : ICommand;

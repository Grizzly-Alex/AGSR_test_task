using Hospital_API.CQRS.Commands;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.ValueTypes;
using Hospital_API.CQRS.Interfaces;

namespace Hospital_API.CQRS.Handlers;

public class UpdatePatientHandler : ICommandHandler<UpdatePatientCommand>
{
    private readonly IRepository<Patient> _repository;

    public UpdatePatientHandler(IRepository<Patient> repository)
    {
        _repository = repository;      
    }


    public async Task<Result> Handle(UpdatePatientCommand command, CancellationToken cancellationToken)
    {
        Guid patientId = await _repository.Get(
            predicate: i => i.Name.Id == command.Request.Name.Id,
            selector: i => i.Id,
            token: cancellationToken);

        var request = command.Request;

        var updatedPatient = new Patient()            
        {
            Id = patientId,
            Name = new Name()
            {
                Id = request.Name.Id,
                Use = request.Name.Use,
                Family = request.Name.Family,
                Given = request.Name.Given,
            },
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            Active = request.Active,
        };

        var sucsess = await _repository.Update(updatedPatient, cancellationToken);

        return sucsess 
            ? Result.Success()
            : Result.Failure(new Error("update not successful"));
    }
}

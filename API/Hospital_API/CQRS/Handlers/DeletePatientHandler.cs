using Hospital_API.CQRS.Commands;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.ValueTypes;
using AutoMapper;
using Hospital_API.CQRS.Interfaces;

namespace Hospital_API.CQRS.Handlers;

public class DeletePatientHandler : ICommandHandler<DeletePatientCommand>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public DeletePatientHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<Result> Handle(DeletePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = await _repository.Get(
            predicate: i => i.Name.Id == command.Id,
            token: cancellationToken);

        if (patient == null)  
            return Result.Failure(new Error($"entity with specified id: {command.Id} not found."));

        var sucssess = await _repository.Delete(patient, cancellationToken);

        return sucssess 
            ? Result.Success() 
            : Result.Failure(new Error("deletion unsuccessful."));
    }
}

using Hospital_API.CQRS.Commands;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using AutoMapper;
using Hospital_API.CQRS.Interfaces;

namespace Hospital_API.CQRS.Handlers;

public class CreatePatientHandler : ICommandHandler<CreatePatientCommand, PatientResponse>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public CreatePatientHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
        
    }

    public async Task<Result<PatientResponse>> Handle(CreatePatientCommand command, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<Patient>(command.Request);
        var patientFromDb = await _repository.Create(patient, cancellationToken);
        var response = _mapper.Map<PatientResponse>(patientFromDb);

        return Result.Create(response);
    }
}

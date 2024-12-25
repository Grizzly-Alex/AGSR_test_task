using Hospital_API.CQRS.Commands;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using AutoMapper;
using Hospital_API.CQRS.Interfaces;


namespace Hospital_API.CQRS.Handlers;

public class BatchPatientHandler : ICommandHandler<BatchPatientCommand, IEnumerable<PatientResponse>>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public BatchPatientHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;     
    }

    public async Task<Result<IEnumerable<PatientResponse>>> Handle(BatchPatientCommand command, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<IEnumerable<Patient>>(command.Request);
        var patientCollectionFromDb = await _repository.Create(patient, cancellationToken);
        var response = _mapper.Map<IEnumerable<PatientResponse>>(patientCollectionFromDb);

        return Result.Create(response);
    }
}

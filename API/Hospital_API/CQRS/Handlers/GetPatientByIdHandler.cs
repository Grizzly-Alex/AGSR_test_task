using Hospital_API.CQRS.Queries;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using AutoMapper;
using Hospital_API.CQRS.Interfaces;


namespace Hospital_API.CQRS.Handlers;

public class GetPatientByIdHandler : IQueryHandler<GetPatientByIdQuery, PatientResponse>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public GetPatientByIdHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;     
    }

    public async Task<Result<PatientResponse>> Handle(GetPatientByIdQuery query, CancellationToken cancellationToken)
    {
        var patientFromDb = await _repository.Get(i => i.Name.Id == query.Id, cancellationToken);

        var response = _mapper.Map<PatientResponse>(patientFromDb);

        return Result.Create(response);
    }
}

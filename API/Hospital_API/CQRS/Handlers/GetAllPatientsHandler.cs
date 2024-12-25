using Hospital_API.CQRS.Queries;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using AutoMapper;
using Hospital_API.CQRS.Interfaces;


namespace Hospital_API.CQRS.Handlers;

public class GetAllPatientsHandler : IQueryHandler<GetAllPatientsQuery, IEnumerable<PatientResponse>>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public GetAllPatientsHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;       
    }

    public async Task<Result<IEnumerable<PatientResponse>>> Handle(GetAllPatientsQuery query, CancellationToken cancellationToken)
    {
        var collectionFromDb = await _repository.GetAll(orderBy: o => o.OrderBy(i => i.BirthDate),
            token: cancellationToken);

        var response = _mapper.Map<IEnumerable<PatientResponse>>(collectionFromDb);

        return response.Any()
            ? Result.Success(response)
            : Result.Failure<IEnumerable<PatientResponse>>(new Error("nothing found for your request"));
    }
}
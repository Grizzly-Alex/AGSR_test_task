using Hospital_API.Constants;
using Hospital_API.CQRS.Queries;
using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.Responses;
using Hospital_API.ValueTypes;
using AutoMapper;
using System.Linq.Expressions;
using Hospital_API.CQRS.Interfaces;


namespace Hospital_API.CQRS.Handlers;

public class GetAllPatientsByBirthDateHandler
    : IQueryHandler<GetAllPatientsByBirthDateQuery, IEnumerable<PatientResponse>>
{
    private readonly IRepository<Patient> _repository;
    private readonly IMapper _mapper;

    public GetAllPatientsByBirthDateHandler(IRepository<Patient> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;       
    }

    public async Task<Result<IEnumerable<PatientResponse>>> Handle(GetAllPatientsByBirthDateQuery query, CancellationToken cancellationToken)
    {
        var collectionFromDb = await _repository.GetAll(
            predicate: GetQueryExpression(query.Prefix, query.StartDate, query.EndDate),
            token: cancellationToken);

        var response = _mapper.Map<IEnumerable<PatientResponse>>(collectionFromDb);

        return response.Any()
            ? Result.Success(response)
            : Result.Failure<IEnumerable<PatientResponse>>(new Error("nothing found for your request"));
    }

    private Expression<Func<Patient, bool>>? GetQueryExpression(SearchingPrefix prefix, DateTime startDate, DateTime endDate) => prefix switch
    {
        SearchingPrefix.Eq => Equal(startDate, endDate),
        SearchingPrefix.Ne => NonEqual(startDate, endDate),

        SearchingPrefix.Gt => GreaterThan(startDate),
        SearchingPrefix.Lt => LessThan(endDate),

        SearchingPrefix.Ge => GreaterOrEqual(startDate),
        SearchingPrefix.Le => LessOrEqual(endDate),

        SearchingPrefix.Sa => StartsAfter(endDate),
        SearchingPrefix.Eb => EndsBefore(startDate),

        SearchingPrefix.Ap => Approximately(startDate, endDate),

        _ => Equal(startDate, endDate)
    };


    #region Queries
    private Expression<Func<Patient, bool>> Equal(DateTime startDate, DateTime endDate) 
        => i => i.BirthDate >= startDate && i.BirthDate <= endDate;

    private Expression<Func<Patient, bool>> NonEqual(DateTime startDate, DateTime endDate)
        => i => i.BirthDate < startDate || i.BirthDate > endDate;

    private Expression<Func<Patient, bool>> GreaterThan(DateTime startDate)
        => i => i.BirthDate > startDate;

    private Expression<Func<Patient, bool>> LessThan(DateTime endDate)
        => i => i.BirthDate < endDate;

    private Expression<Func<Patient, bool>> GreaterOrEqual(DateTime startDate)
        => i => i.BirthDate >= startDate;

    private Expression<Func<Patient, bool>> LessOrEqual(DateTime endDate)
        => i => i.BirthDate <= endDate;

    private Expression<Func<Patient, bool>> StartsAfter(DateTime endDate)
        => i => i.BirthDate > endDate;

    private Expression<Func<Patient, bool>> EndsBefore(DateTime startDate)
        => i => i.BirthDate < startDate;

    private Expression<Func<Patient, bool>> Approximately(DateTime startDate, DateTime endDate)
    {
        //the resource value is approximately the same to the parameter value.
        //Note that the recommended value for the approximation is 10 % of the
        //stated value(or for a date, 10 % of the gap between now and the date),
        //but systems may choose other values where appropriate

        //TODO: I don't understand this prefix. How can it be applied to a date?

        return i => i.BirthDate >= startDate && i.BirthDate <= endDate;
    }


    #endregion
}


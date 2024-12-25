using Hospital_API.Data.Models;
using Hospital_API.Interfaces;
using Hospital_API.ValueTypes;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Hospital_API.Data;

public class PatientRepository : IRepository<Patient>
{
    private readonly DbSet<Patient> _dbSet;
    private readonly Func<CancellationToken, Task<int>> _commit;

    public PatientRepository(AppDBContext context)
    {
        _commit = context.SaveChangesAsync;
        _dbSet = context.Set<Patient>();
    }

    public async Task<Patient?> Get(
        Expression<Func<Patient, bool>> predicate,
        CancellationToken token = default)
    {
        return await _dbSet
            .AsNoTracking()
            .Include(i => i.Name)
            .FirstOrDefaultAsync(predicate, token);
    }

    public async Task<IList<Patient>> GetAll(
        Expression<Func<Patient, bool>>? predicate = null,
        Func<IQueryable<Patient>, IOrderedQueryable<Patient>>? orderBy = null,
        CancellationToken token = default)
    {
        IQueryable<Patient> query = _dbSet;

        query = query.AsNoTracking();
        query = query.Include(i => i.Name);

        if (predicate is not null)
            query = query.Where(predicate);       

        return orderBy is not null
            ? await orderBy(query).ToListAsync(token)
            : await query.ToListAsync(token);
    }

    public async Task<TResult?> Get<TResult>(
        Expression<Func<Patient, TResult>> selector,
        Expression<Func<Patient, bool>>? predicate = null,
        Func<IQueryable<Patient>, IOrderedQueryable<Patient>>? orderBy = null,
        CancellationToken token = default)
    {
        IQueryable<Patient> query = _dbSet;

        query = query.AsNoTracking();
        query = query.Include(i => i.Name);

        if (predicate is not null)
            query = query.Where(predicate);


        return orderBy is not null
            ? await orderBy(query).Select(selector).FirstOrDefaultAsync(token)
            : await query.Select(selector).FirstOrDefaultAsync(token);
    }

    public async Task<IEnumerable<TResult>> GetAll<TResult>(
        Expression<Func<Patient, TResult>> selector,
        Expression<Func<Patient, bool>>? predicate = null,
        Func<IQueryable<Patient>, IOrderedQueryable<Patient>>? orderBy = null,
        CancellationToken token = default)
    {
        IQueryable<Patient> query = _dbSet;

        query = query.AsNoTracking();
        query = query.Include(i => i.Name);

        if (predicate is not null)
            query = query.Where(predicate);

        return orderBy is not null
            ? await orderBy(query).Select(selector).ToListAsync(token)
            : await query.Select(selector).ToListAsync(token);
    }

    public async Task<Patient?> Create(Patient entity, CancellationToken token = default)
    {
        var result = await _dbSet.AddAsync(entity, token);
        result.State = EntityState.Added;

        var changes = await _commit(token);

        return changes != default
            ? result.Entity
            : null;
    }

    public async Task<IEnumerable<Patient>?> Create(IEnumerable<Patient> entities, CancellationToken token = default)
    {
        await _dbSet.AddRangeAsync(entities, token);
        var changes = await _commit(token);

        return changes != default
            ? entities
            : null;
    }

    public async Task<bool> Update(Patient entity, CancellationToken token = default)
    {
        var result = _dbSet.Update(entity)
            .State = EntityState.Modified;

        var changes = await _commit(token);

        return changes != default;
    }

    public async Task<bool> Delete(Patient entity, CancellationToken token = default)
    {
        var result = _dbSet.Remove(entity)
            .State = EntityState.Deleted;

        var changes = await _commit(token);

        return changes != default;
    }
}
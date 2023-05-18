using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HeatingService.Application.Persistence.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class {
  protected HeatingDbContext Context { get; }

  protected RepositoryBase(HeatingDbContext context) {
    Context = context;
  }
  
  public void Add(T entity) {
    Context.Set<T>().Add(entity);
  }

  public Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges) {
    return Task.FromResult(trackChanges
      ? Context.Set<T>().Where(expression)
      : Context.Set<T>().Where(expression).AsNoTracking());
  }

  public async Task SaveAsync() {
    await Context.SaveChangesAsync();
  }
}

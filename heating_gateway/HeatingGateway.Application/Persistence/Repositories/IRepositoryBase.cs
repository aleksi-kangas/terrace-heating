using System.Linq.Expressions;

namespace HeatingGateway.Application.Persistence.Repositories; 

public interface IRepositoryBase<T> {
  void Add(T entity);
  Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = true);

  Task SaveAsync();
}

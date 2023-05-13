using System.Linq.Expressions;

namespace HeatingService.Application.Persistence; 

public interface IRepositoryBase<T> {
  void Add(T entity);
  Task<IQueryable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression, bool trackChanges = true);

  Task SaveAsync();
}
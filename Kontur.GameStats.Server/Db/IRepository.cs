using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    public interface IRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();

        TEntity GetOne(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetSorted(string indexProperty, int order, int count);

        bool Update(TEntity entity);

        int Insert(TEntity entity);
    }
}

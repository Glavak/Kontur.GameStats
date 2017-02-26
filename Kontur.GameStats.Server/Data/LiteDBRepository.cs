using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontur.GameStats.Server
{
    class LiteDBRepository<TEntity> : IRepository<TEntity>
    {
        LiteCollection<TEntity> table;

        public LiteDBRepository(LiteDatabase database)
        {
            var collectionName = typeof(TEntity).Name;
            table = database.GetCollection<TEntity>(collectionName);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return table.FindAll();
        }

        public TEntity GetOne(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return table.FindOne(predicate);
        }

        public IEnumerable<TEntity> GetSorted(string indexProperty, int order, int count)
        {
            return table.Find(Query.All(indexProperty, order), 0, count);
        }

        public bool Update(TEntity entity)
        {
            return table.Update(entity);
        }

        public int Insert(TEntity entity)
        {
            return table.Insert(entity);
        }
    }
}

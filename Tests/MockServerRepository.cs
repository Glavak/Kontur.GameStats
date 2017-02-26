using Kontur.GameStats.Server;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tests
{
    public class MockRepository<TElement> : IRepository<TElement>
    {
        public List<TElement> Elements = new List<TElement>();
        public int UpdateCalledTimes;
        public TElement LastUpdateElement;
        public int InsertCalledTimes;
        public TElement LastInsertElement;

        public IEnumerable<TElement> GetAll()
        {
            return Elements;
        }

        public TElement GetOne(System.Linq.Expressions.Expression<Func<TElement, bool>> predicate)
        {
            return Elements.First(predicate.Compile());
        }

        public IEnumerable<TElement> GetSorted(string indexProperty, int order, int count)
        {
            return Elements.OrderBy(x=>x, new ElementComparer<TElement>(indexProperty, order)).Take(count);
        }

        public bool Update(TElement entity)
        {
            UpdateCalledTimes++;
            LastUpdateElement = entity;

            return true;
        }

        public int Insert(TElement entity)
        {
            InsertCalledTimes++;
            LastInsertElement = entity;

            return 1;
        }
    }

    public class ElementComparer<TElement> : IComparer<TElement>
    {
        private readonly string propertyName;
        private readonly int order;

        public ElementComparer(string propertyName, int order)
        {
            this.propertyName = propertyName;
            this.order = order;
        }

        public int Compare(TElement x, TElement y)
        {
            var property = typeof(TElement).GetProperty(propertyName);

            var xValue = property.GetValue(x);
            var yValue = property.GetValue(y);

            var xComparable = xValue as IComparable;
            
            if(xComparable == null)
            {
                throw new Exception("Not comparable");
            }

            return xComparable.CompareTo(yValue) * order;
        }
    }
}

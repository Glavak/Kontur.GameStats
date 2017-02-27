using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Kontur.GameStats.Server;

namespace Tests
{
    public class MockRepository<TElement> : IRepository<TElement>
    {
        public List<TElement> Elements = new List<TElement>();
        public int InsertCalledTimes;
        public List<TElement> InsertElements = new List<TElement>();
        public List<TElement> UpdateElements = new List<TElement>();
        public int UpdateCalledTimes;

        public IEnumerable<TElement> GetAll()
        {
            return Elements;
        }

        public TElement GetOne(Expression<Func<TElement, bool>> predicate)
        {
            try
            {
                return Elements.First(predicate.Compile());
            }
            catch (InvalidOperationException)
            {
                // No such element
                return default(TElement);
            }
        }

        public IEnumerable<TElement> GetSorted(string indexProperty, int order, int count)
        {
            return Elements.OrderBy(x => x, new ElementComparer<TElement>(indexProperty, order)).Take(count);
        }

        public bool Update(TElement entity)
        {
            UpdateCalledTimes++;
            UpdateElements.Add(entity);

            return true;
        }

        public int Insert(TElement entity)
        {
            InsertCalledTimes++;
            InsertElements.Add(entity);

            return 1;
        }
    }

    /// <summary>
    /// Compare elements by IComparable property with given name, using reflection
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class ElementComparer<TElement> : IComparer<TElement>
    {
        private readonly int order;
        private readonly string propertyName;

        public ElementComparer(string propertyName, int order)
        {
            this.propertyName = propertyName;
            this.order = order;
        }

        public int Compare(TElement x, TElement y)
        {
            PropertyInfo property = typeof(TElement).GetProperty(propertyName);

            object xValue = property.GetValue(x);
            object yValue = property.GetValue(y);

            var xComparable = xValue as IComparable;

            if (xComparable == null)
                throw new Exception("Not comparable");

            return xComparable.CompareTo(yValue) * order;
        }
    }
}
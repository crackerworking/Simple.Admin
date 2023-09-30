using System.Collections.Concurrent;
using System.Linq.Expressions;

using Mi.Domain.Entities;
using Mi.Domain.Extension;

namespace Mi.Domain.DataAccess
{
    public class Updatable<T> where T : EntityBase, new()
    {
        public ConcurrentDictionary<string, object?> KeyValuePairs { get; private set; }

        private readonly object _lock = new object();

        public Updatable()
        {
            KeyValuePairs ??= new ConcurrentDictionary<string, object?>();
        }

        public Updatable<T> SetColumn<TProp>(Expression<Func<T, TProp>> expr, TProp value)
        {
            lock (_lock)
            {
                var key = expr.GetMemberName();
                KeyValuePairs.TryAdd(key, value);
                return this;
            }
        }
    }
}
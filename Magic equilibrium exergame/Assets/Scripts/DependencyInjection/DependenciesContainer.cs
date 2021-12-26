using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.DependencyInjection
{
    public class DependenciesContainer 
    {
        // private fields
        private readonly Dictionary<Type, Func<object>> _dependencies = new Dictionary<Type,Func<object>>();



        // Public
        public void Register(Type type, Func<object> builder)
        {
            _dependencies[type] = builder;
        }

        public void Register<T, V>(Func<V> builder) where V : class, T where T : class
        {
            var type = typeof(T);
            Register(type, builder);
        }

        public T Get<T>() where T : class
            => _dependencies.TryGetValue(typeof(T), out var builder)
                ? (T) builder.Invoke()
                : throw new InvalidOperationException($"Tpye {typeof(T)} not registered");
    }
}

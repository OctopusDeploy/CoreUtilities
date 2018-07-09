using System;
using System.Diagnostics;

namespace Octopus.CoreUtilities.Extensions
{
    using Unit = ValueTuple;
    using static Prelude;
    
    /// <summary>
    /// https://davefancher.com/2015/06/14/functional-c-fluent-interfaces-and-functional-method-chaining/
    /// https://app.pluralsight.com/library/courses/functional-programming-csharp/table-of-contents
    /// 
    /// Function extension libraries from the course:
    /// Functional Programming with C# by Dave Fancher
    /// </summary>
    [DebuggerNonUserCode]
    public static class FunctionalExtensions
    {
        public static T Tee<T>(this T @this, Action<T> action)
        {
            action(@this);
            return @this;
        }
        
        public static TResult Using<TDisposable, TResult>
        (
            Func<TDisposable> factory,
            Func<TDisposable, TResult> fn)
            where TDisposable : IDisposable
        {
            using (var disposable = factory())
            {
                return fn(disposable);
            }
        }
        
        public static TResult Map<TSource, TResult>(
            this TSource @this,
            Func<TSource, TResult> fn)
        {
            return fn(@this);
        }
        
        public static Func<Unit> ToFunc(this Action action)
            => () => { action(); return Unit(); };

        public static Func<T, Unit> ToFunc<T>(this Action<T> action)
            => t => { action(t); return Unit(); };

        public static Func<T1, T2, Unit> ToFunc<T1, T2>(this Action<T1, T2> action)
            => (T1 t1, T2 t2) => { action(t1, t2); return Unit(); };
    }
}
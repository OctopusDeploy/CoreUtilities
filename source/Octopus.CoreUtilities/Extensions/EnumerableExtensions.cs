using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Octopus.CoreUtilities.Extensions
{
    public static class EnumerableExtensions
    {
#if NET40 || NET452 // ToHashSet was added to System.Linq in 4.7.2, NetStandard 2.1
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(source, comparer);
        }
#endif

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this ICollection<T> collection)
            => collection.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this List<T> list)
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this T[] array)
            => array.Length > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TKey, TValue>(this IDictionary<TKey, TValue> list)
            where TKey : notnull
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TKey, TValue>(this ILookup<TKey, TValue> list)
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<TKey, TValue>(this Dictionary<TKey, TValue> list)
            where TKey : notnull
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this HashSet<T> list)
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this IReadOnlyList<T> list)
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this IReadOnlyCollection<T> list)
            => list.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this ICollection<T> collection)
            => collection.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this List<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this T[] array)
            => array.Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<TKey, TValue>(this IDictionary<TKey, TValue> list)
            where TKey : notnull
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<TKey, TValue>(this ILookup<TKey, TValue> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<TKey, TValue>(this Dictionary<TKey, TValue> list)
            where TKey : notnull
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this HashSet<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IReadOnlyList<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IReadOnlyCollection<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IEnumerable<T> items)
            => !items.Any();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IEnumerable<T> items, Func<T, bool> predicate)
            => !items.Any(predicate);

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
            => source == null || !source.Any();

        public static bool IsNullOrEmpty<T>(this string source)
            => string.IsNullOrEmpty(source);

        public static string StringJoin<T>(this IEnumerable<T> items, string separator, Func<T, string?>? toString = null)
        {
            toString ??= (i => i?.ToString());
            return string.Join(separator, items.Select(toString));
        }

        public static string ToSingleQuotedCommaSeperated<T>(this IEnumerable<T> items)
        {
            var joined = string.Join("', '", items);
            return joined == "" ? "" : $"'{joined}'";
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> ToEnumerable<T>(this T source)
        {
            return new T[] {source};
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> Yield<TSource>(this TSource item)
        {
            yield return item;
        }

#if !NET40
        public static IReadOnlyCollection<T> AsReadOnly<T>(this IEnumerable<T> items)
        {
            return new ReadOnlyCollection<T>(items.ToList());
        }
#endif

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
            where TKey : notnull
        {
            return keyValuePairs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source.TakeWhile(x => !predicate(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<TSource> SkipUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            => source.SkipWhile(x => !predicate(x));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<string> WhereNotNullOrWhiteSpace(this IEnumerable<string> source)
            => source.Where(item => !string.IsNullOrWhiteSpace(item));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> items)
            where T : class
            => items.Where(i => i != null)!;
    }
}
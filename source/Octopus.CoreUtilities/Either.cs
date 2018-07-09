using System;
using System.Collections.Generic;
using Octopus.CoreUtilities.Extensions;

namespace Octopus.CoreUtilities
{
    using static EitherExtensions;
    using static Prelude;
    using Unit = ValueTuple;

    public struct Either<L, R>
    {
        private L _left;

        public L Left
        {
            get
            {
                if (IsRight)
                    throw new InvalidOperationException("Can't access Left when Either is in Right state");
                return _left;
            }
            private set => _left = value;
        }

        private R _right;
        public R Right
        {
            get
            {
                if (IsLeft)
                    throw new InvalidOperationException("Can't access Right when Either is in Left state");

                return _right;
            }
            private set => _right = value;
        }

        public bool IsRight { get; }
        public bool IsLeft => !IsRight;

        internal Either(L left)
        {
            IsRight = false;
            _left = left;
            _right = default(R);
        }

        internal Either(R right)
        {
            IsRight = true;
            _right = right;
            _left = default(L);
        }

        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public static implicit operator Either<L, R>(Either.Left<L> left) => new Either<L, R>(left.Value);
        public static implicit operator Either<L, R>(Either.Right<R> right) => new Either<L, R>(right.Value);

        /// <summary>
        /// Catamorphism for Either, i.e. use left when in left and use right when in right.
        /// </summary>
        /// <typeparam name="TR"></typeparam>
        /// <param name="Left">Function to invoke when in left state</param>
        /// <param name="Right">Function to invoke when in right state</param>
        /// <returns></returns>
        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
           => IsLeft ? Left(this.Left) : Right(this.Right);

        /// <summary>
        /// Catamorphism for Either, i.e. use left when in left and use right when in right.
        /// </summary>
        /// <typeparam name="R"></typeparam>
        /// <param name="Left">Function to invoke when in left state</param>
        /// <param name="Right">Function to invoke when in right state</param>
        /// <returns></returns>
        public Unit Match(Action<L> Left, Action<R> Right)
           => Match(Left.ToFunc(), Right.ToFunc());

        public IEnumerator<R> AsEnumerable()
        {
            if (IsRight) yield return Right;
        }

        public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
    }

    public static class Either
    {
        public struct Left<TL>
        {
            internal TL Value { get; }
            internal Left(TL value) { Value = value; }

            public override string ToString() => $"Left({Value})";
        }

        public struct Right<TR>
        {
            internal TR Value { get; }
            internal Right(TR value) { Value = value; }

            public override string ToString() => $"Right({Value})";
        }
    }

    public static class EitherExtensions
    {
        public static Either.Left<TL> Left<TL>(TL l) => new Either.Left<TL>(l);
        public static Either.Right<TR> Right<TR>(TR r) => new Either.Right<TR>(r);
        
        public static Either<TL, TRr> Map<TL, TR, TRr>
           (this Either<TL, TR> @this, Func<TR, TRr> f)
           => @this.Match<Either<TL, TRr>>(
              l => Left(l),
              r => Right(f(r)));

        public static Either<TLNew, TRNew> Map<TL, TLNew, TR, TRNew>
           (this Either<TL, TR> @this, Func<TL, TLNew> left, Func<TR, TRNew> right)
           => @this.Match<Either<TLNew, TRNew>>(
              l => Left(left(l)),
              r => Right(right(r)));

        public static Either<TL, Unit> ForEach<TL, TR>
           (this Either<TL, TR> @this, Action<TR> act)
           => Map(@this, act.ToFunc());

        public static Either<TL, TRNew> Bind<TL, TR, TRNew>
           (this Either<TL, TR> @this, Func<TR, Either<TL, TRNew>> f)
           => @this.Match(
              l => Left(l),
              r => f(r));

        public static Either<TL, TRNew> Select<TL, TR, TRNew>(this Either<TL, TR> @this
           , Func<TR, TRNew> map) => @this.Map(map);

        
        public static Either<TL, TR> AsLeft<TL, TR>(this TL @source)
        {
            return @source;
        }

        public static Either<TL, TR> AsRight<TL, TR>(this TR @source)
        {
            return @source;
        }
    }
}
﻿#region Licence

/****************************************************************************
          Copyright 1999-2016 Vincent J. Jacquet.  All rights reserved.

    Permission is granted to anyone to use this software for any purpose on
    any computer system, and to alter it and redistribute it, subject
    to the following restrictions:

    1. The author is not responsible for the consequences of use of this
       software, no matter how awful, even if they arise from flaws in it.

    2. The origin of this software must not be misrepresented, either by
       explicit claim or by omission.  Since few users ever read sources,
       credits must appear in the documentation.

    3. Altered versions must be plainly marked as such, and must not be
       misrepresented as being the original software.  Since few users
       ever read sources, credits must appear in the documentation.

    4. This notice may not be removed or altered.

 ****************************************************************************
 * Adapted Eric Evans, Domain-Driven Design, Page 224
 ****************************************************************************/

#endregion

using System;
using System.Linq;

namespace WmcSoft
{
    public struct Specification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _spec;

        public Specification(ISpecification<T> spec) {
            _spec = spec ?? Specification.Null<T>();
        }

        public bool IsSatisfiedBy(T candidate) {
            return _spec.IsSatisfiedBy(candidate);
        }

        #region Operators

        public static implicit operator Specification<T>(Func<T, bool> x) {
            return new Specification<T>(Specification.Create(x));
        }

        #endregion
    }

    public static class Specification
    {
        public static NullSpecification<T> Null<T>() {
            return NullSpecification<T>.Default;
        }

        public static PredicateSpecification<T> Create<T>(Predicate<T> predicate) {
            return new PredicateSpecification<T>(predicate);
        }
        public static PredicateSpecification<T> Create<T>(Func<T, bool> predicate) {
            return new PredicateSpecification<T>(predicate);
        }

        public static AllSpecification<T> All<T>(params ISpecification<T>[] spec) {
            return new AllSpecification<T>(spec);
        }

        public static AnySpecification<T> Any<T>(params ISpecification<T>[] spec) {
            return new AnySpecification<T>(spec);
        }

        public static NoneSpecification<T> None<T>(params ISpecification<T>[] spec) {
            return new NoneSpecification<T>(spec);
        }
    }

    #region Specialized specifications

    public struct NullSpecification<T> : ISpecification<T>
    {
        public static readonly NullSpecification<T> Default = default(NullSpecification<T>);

        public bool IsSatisfiedBy(T candidate) {
            return true;
        }
    }

    public struct PredicateSpecification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> _predicate;

        public PredicateSpecification(Predicate<T> predicate) {
            if (predicate != null)
                _predicate = x => predicate(x);
            else
                _predicate = NullSpecification<T>.Default.IsSatisfiedBy;
        }
        public PredicateSpecification(Func<T, bool> predicate) {
            _predicate = (predicate != null)
                ? predicate
                : NullSpecification<T>.Default.IsSatisfiedBy;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _predicate(candidate);
        }
    }

    public struct AllSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] _spec;

        public AllSpecification(params ISpecification<T>[] spec) {
            if (spec == null)
                _spec = new ISpecification<T>[0];
            _spec = spec;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _spec.All(s => s.IsSatisfiedBy(candidate));
        }
    }

    public struct AnySpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] _spec;

        public AnySpecification(params ISpecification<T>[] spec) {
            if (spec == null)
                _spec = new ISpecification<T>[0];
            _spec = spec;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _spec.Any(s => s.IsSatisfiedBy(candidate));
        }
    }

    public struct NoneSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] _spec;

        public NoneSpecification(params ISpecification<T>[] spec) {
            if (spec == null)
                _spec = new ISpecification<T>[0];
            _spec = spec;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _spec.All(s => !s.IsSatisfiedBy(candidate));
        }
    }

    public struct AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _x;
        private readonly ISpecification<T> _y;

        public AndSpecification(ISpecification<T> x, ISpecification<T> y) {
            _x = x;
            _y = y;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _x.IsSatisfiedBy(candidate) && _y.IsSatisfiedBy(candidate);
        }
    }

    public struct OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _x;
        private readonly ISpecification<T> _y;

        public OrSpecification(ISpecification<T> x, ISpecification<T> y) {
            _x = x;
            _y = y;
        }

        public bool IsSatisfiedBy(T candidate) {
            return _x.IsSatisfiedBy(candidate) || _y.IsSatisfiedBy(candidate);
        }
    }

    public struct NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _x;

        public NotSpecification(ISpecification<T> x) {
            _x = x;
        }

        public bool IsSatisfiedBy(T candidate) {
            return !_x.IsSatisfiedBy(candidate);
        }
    }

    #endregion
}
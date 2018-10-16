#region Licence

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
    /// <summary>
    /// Light wrapper on a specification implementing operators.
    /// </summary>
    /// <remarks>This type erase the type of the underlying specification.</remarks>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct Specification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> _spec;

        public Specification(ISpecification<T> spec)
        {
            _spec = spec;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            if (_spec == null)
                return false;
            return _spec.IsSatisfiedBy(candidate);
        }

        #region Operators

        public static implicit operator Specification<T>(Func<T, bool> x)
        {
            return new Specification<T>(Specification.Create(x));
        }

        public static Specification<T> operator &(Specification<T> x, Specification<T> y)
        {
            if (x._spec == null) return x;
            if (y._spec == null) return y;

            return new Specification<T>(new AndSpecification<T>(x._spec, y._spec));
        }

        public static Specification<T> operator |(Specification<T> x, Specification<T> y)
        {
            if (x._spec == null) return y;
            if (y._spec == null) return x;

            return new Specification<T>(new OrSpecification<T>(x._spec, y._spec));
        }

        public static Specification<T> operator ~(Specification<T> x)
        {
            if (x._spec == null)
                return new Specification<T>(Specification.Never<T>());
            return new Specification<T>(new NotSpecification<T>(x._spec));
        }

        #endregion
    }

    public static class Specification
    {
        public static AlwaysSpecification<T> Always<T>()
        {
            return default;
        }

        public static NeverSpecification<T> Never<T>()
        {
            return default;
        }

        public static PredicateSpecification<T> Create<T>(Predicate<T> predicate)
        {
            return new PredicateSpecification<T>(predicate);
        }
        public static PredicateSpecification<T> Create<T>(Func<T, bool> predicate)
        {
            return new PredicateSpecification<T>(predicate);
        }

        public static AllSpecification<T> All<T>(params ISpecification<T>[] spec)
        {
            return new AllSpecification<T>(spec);
        }

        public static AnySpecification<T> Any<T>(params ISpecification<T>[] spec)
        {
            return new AnySpecification<T>(spec);
        }

        public static NoneSpecification<T> None<T>(params ISpecification<T>[] spec)
        {
            return new NoneSpecification<T>(spec);
        }

        public static NotSpecification<T> Not<T>(ISpecification<T> spec)
        {
            return new NotSpecification<T>(spec);
        }
    }

    #region Specialized specifications

    /// <summary>
    /// Implements a specification that is never satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct NeverSpecification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T candidate)
        {
            return false;
        }
    }

    /// <summary>
    /// Implements a specification that is always satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct AlwaysSpecification<T> : ISpecification<T>
    {
        public bool IsSatisfiedBy(T candidate)
        {
            return true;
        }
    }

    /// <summary>
    /// Implements a specification based on a <see cref="Predicate{T}"/> or a <see cref="Func{T, bool}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct PredicateSpecification<T> : ISpecification<T>
    {
        private readonly Func<T, bool> predicate;

        public PredicateSpecification(Predicate<T> predicate)
        {
            if (predicate != null)
                this.predicate = x => predicate(x);
            else
                this.predicate = default(AlwaysSpecification<T>).IsSatisfiedBy;
        }

        public PredicateSpecification(Func<T, bool> predicate)
        {
            this.predicate = (predicate != null)
                ? predicate
                : default(AlwaysSpecification<T>).IsSatisfiedBy;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return predicate(candidate);
        }
    }

    /// <summary>
    /// Implements a specification that is satisfied if all of the given specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct AllSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] specifications;

        public AllSpecification(params ISpecification<T>[] specifications)
        {
            if (specifications == null)
                this.specifications = new ISpecification<T>[0];
            this.specifications = specifications;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return specifications.All(s => s.IsSatisfiedBy(candidate));
        }
    }

    /// <summary>
    /// Implements a specification that is satisfied if any of the given specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct AnySpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] specifications;

        public AnySpecification(params ISpecification<T>[] specifications)
        {
            if (specifications == null)
                this.specifications = new ISpecification<T>[0];
            this.specifications = specifications;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return specifications.Any(s => s.IsSatisfiedBy(candidate));
        }
    }

    /// <summary>
    /// Implements a specification that is satisfied if none of the given specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct NoneSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T>[] specifications;

        public NoneSpecification(params ISpecification<T>[] spec)
        {
            if (spec == null)
                specifications = new ISpecification<T>[0];
            specifications = spec;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return specifications.All(s => !s.IsSatisfiedBy(candidate));
        }
    }

    /// <summary>
    /// Implements a specification that is satisfied if both of the given specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct AndSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> x;
        private readonly ISpecification<T> y;

        public AndSpecification(ISpecification<T> x, ISpecification<T> y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return x.IsSatisfiedBy(candidate) && y.IsSatisfiedBy(candidate);
        }
    }

    /// <summary>
    /// Implements a specification that is satisfied if any of the given specifications are satisfied.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct OrSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> x;
        private readonly ISpecification<T> y;

        public OrSpecification(ISpecification<T> x, ISpecification<T> y)
        {
            this.x = x;
            this.y = y;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return x.IsSatisfiedBy(candidate) || y.IsSatisfiedBy(candidate);
        }
    }

    /// <summary>
    /// Implements a specification inverted the given specificatio.
    /// </summary>
    /// <typeparam name="T">The type of the object to apply the predicate on.</typeparam>
    public struct NotSpecification<T> : ISpecification<T>
    {
        private readonly ISpecification<T> x;

        public NotSpecification(ISpecification<T> x)
        {
            this.x = x;
        }

        public bool IsSatisfiedBy(T candidate)
        {
            return !x.IsSatisfiedBy(candidate);
        }
    }

    #endregion
}

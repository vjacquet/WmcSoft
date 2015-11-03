#region Licence

/****************************************************************************
          Copyright 1999-2015 Vincent J. Jacquet.  All rights reserved.

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

 ****************************************************************************/

#endregion

using WmcSoft.Algebra;

namespace WmcSoft.Arithmetics
{
    public struct AdditiveMultiplicativeRingAdapter<T, A> : IRingLike<T>
        where A : IArithmetics<T>
    {
        class AdditiveTraits : IGroupLikeTraits
        {
            private readonly A _arithmetics;

            public AdditiveTraits(A arithmetics) {
                _arithmetics = arithmetics;
            }

            #region IGroupLikeTraits Membres

            public bool SupportIdentity {
                get { return true; }
            }

            public bool SupportInverse {
                get { return true; }
            }

            public bool IsAssociative {
                get { return true; }
            }

            public bool IsCommutative {
                get { return true; }
            }

            public bool IsIdempotent {
                get { return false; }
            }

            #endregion
        }

        class MultiplicativeTraits : IGroupLikeTraits
        {
            private readonly A _arithmetics;

            public MultiplicativeTraits(A arithmetics) {
                _arithmetics = arithmetics;
            }

            #region IGroupLikeTraits Membres

            public bool SupportIdentity {
                get { return true; }
            }

            public bool SupportInverse {
                get { return _arithmetics.SupportReciprocal; }
            }

            public bool IsAssociative {
                get { return true; }
            }

            public bool IsCommutative {
                get { return true; }
            }

            public bool IsIdempotent {
                get { return false; }
            }

            #endregion
        }

        private readonly A _arithmetics;

        public AdditiveMultiplicativeRingAdapter(A arithmetics) {
            _arithmetics = arithmetics;
        }

        #region IRingLike<T> Membres

        public T Add(T x, T y) {
            return _arithmetics.Add(x, y);
        }

        public T Negate(T x) {
            return _arithmetics.Negate(x);
        }

        public T Zero {
            get { return _arithmetics.Zero; }
        }

        public T Multiply(T x, T y) {
            return _arithmetics.Multiply(x, y);
        }

        public T Reciprocal(T x) {
            return _arithmetics.Reciprocal(x);
        }

        public T One {
            get { return _arithmetics.One; }
        }

        #endregion

        #region IRingLikeTraits Membres

        public IGroupLikeTraits Addition {
            get { return new AdditiveTraits(_arithmetics); }
        }

        public IGroupLikeTraits Multiplication {
            get { return new MultiplicativeTraits(_arithmetics); }
        }

        #endregion
    }
}

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

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WmcSoft.Collections.Generic.Internals
{
    internal class Ballot<T> : IEnumerable<KeyValuePair<T, int>>
    {
        [DebuggerDisplay("{Candidate,nq}")]
        class Paper
        {
            public T Candidate { get; set; }
        }

        class CandidateComparer : IEqualityComparer<Paper>
        {
            private readonly IEqualityComparer<T> _equalityComparer;

            public CandidateComparer(IEqualityComparer<T> equalityComparer = null) {
                _equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;
            }

            #region IEqualityComparer<Votes> Membres

            public bool Equals(Paper x, Paper y) {
                return _equalityComparer.Equals(x.Candidate, y.Candidate);
            }

            public int GetHashCode(Paper obj) {
                return _equalityComparer.GetHashCode(obj.Candidate);
            }

            #endregion
        }

        [DebuggerDisplay("#{Count,nq} (Rank {Rank,nq})")]
        class Score
        {
            public int Rank;
            public int Count;
        }

        class ScoresComparer : IComparer<Score>
        {
            public static ScoresComparer Default = new ScoresComparer();

            #region IComparer<Votes> Membres

            public int Compare(Score x, Score y) {
                var score = y.Count - x.Count;
                if (score == 0)
                    return x.Rank - y.Rank;
                return score;
            }

            #endregion
        }

        private readonly IDictionary<Paper, Score> _votes;
        private Paper _finder;

        public Ballot(IEqualityComparer<T> equalityComparer = null) {
            _votes = new Dictionary<Paper, Score>(new CandidateComparer(equalityComparer));
            _finder = new Paper();
        }

        public void Vote(T candidate) {
            _finder.Candidate = candidate;
            Score score;
            if (_votes.TryGetValue(_finder, out score)) {
                score.Count++;
            } else {
                _votes.Add(new Paper { Candidate = candidate }, new Score { Rank = _votes.Count, Count = 1 });
            }
        }

        public bool HasVotes {
            get {
                return _votes.Count > 0;
            }
        }

        #region IEnumerable<KeyValuePair<T,int>> Membres

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() {
            return _votes.OrderBy(v => v.Value, ScoresComparer.Default)
                .Select(v => new KeyValuePair<T, int>(v.Key.Candidate, v.Value.Count))
                .GetEnumerator();
        }

        #endregion

        #region IEnumerable Membres

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}

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
        // Paper enables voting to "null", because this value is not allowed by default on Dictionary.
        // CandidateComparer compares the Candidate on the Paper
        // Score has a Rank to allow stability: on ties, the first presented element wins.

        #region Utility classes

        [DebuggerDisplay("{Candidate,nq}")]
        struct Paper
        {
            public T Candidate;
        }

        struct CandidateComparer : IEqualityComparer<Paper>
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
            public int Count; // higher is better
            public int Rank;  // lower is better
        }

        struct ScoresComparer : IComparer<Score>, IComparer<KeyValuePair<Paper, Score>>
        {
            public static ScoresComparer Default = new ScoresComparer();

            public int Compare(KeyValuePair<Paper, Score> x, KeyValuePair<Paper, Score> y) {
                return Compare(x.Value, y.Value);
            }

            public int Compare(Score x, Score y) {
                var score = x.Count - y.Count;
                if (score != 0)
                    return score;
                return y.Rank - x.Rank;
            }
        }

        #endregion

        private readonly Dictionary<Paper, Score> _votes;

        public Ballot(IEqualityComparer<T> equalityComparer = null) {
            _votes = new Dictionary<Paper, Score>(new CandidateComparer(equalityComparer));
        }

        public void Vote(T candidate) {
            var finder = new Paper { Candidate = candidate };
            Score score;
            if (_votes.TryGetValue(finder, out score)) {
                score.Count++;
            } else {
                _votes.Add(new Paper { Candidate = candidate }, new Score { Rank = _votes.Count, Count = 1 });
            }
        }

        public bool HasVotes {
            get { return _votes.Count > 0; }
        }

        public T GetWinner() {
            return _votes.Max(ScoresComparer.Default).Key.Candidate;
        }

        #region IEnumerable<KeyValuePair<T,int>> Membres

        public IEnumerator<KeyValuePair<T, int>> GetEnumerator() {
            return _votes.OrderByDescending(v => v.Value, ScoresComparer.Default)
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
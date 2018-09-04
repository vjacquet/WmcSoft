using System.Collections.Generic;
using System.Linq;
using Xunit;
using WmcSoft.Collections.Generic;

namespace WmcSoft
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void CanConcatTwoArrays()
        {
            var a = new[] { 1, 2, 3 };
            var b = new[] { 4, 5 };
            var actual = a.Concat(b);
            var expected = new[] { 1, 2, 3, 4, 5 };
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanGetColumn()
        {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 2, 5 };
            var actual = array.GetColumn(1).ToArray();
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void CanEnumerateColumn()
        {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 2, 5 };
            var column = new ColumnBand<int>(array, 1);
            Assert.Equal(2, column.Count);
            Assert.Equal(5, column[1]);
            var actual = column.ToArray();
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void CanGetRow()
        {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 4, 5, 6 };
            var actual = array.GetRow(1).ToArray();
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void CanEnumerateRow()
        {
            var array = new[,] {
               { 1, 2, 3},
               { 4, 5, 6},
            };
            var expected = new[] { 4, 5, 6 };
            var row = new RowBand<int>(array, 1);
            Assert.Equal(3, row.Count);
            Assert.Equal(5, row[1]);
            var actual = row.ToArray();
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void CheckStructuralEquals()
        {
            var expected = new[] { 1, 2, 3 };
            var actual = new[] { 1, 2, 3 };
            Assert.True(expected.SequenceEqual(actual));
        }

        [Fact]
        public void CanTranspose()
        {
            var expected = new[,] { { 1, 3 }, { 2, 4 } };
            var array = new[,] { { 1, 2 }, { 3, 4 } };
            var actual = array.Transpose();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckToMultiDimensional()
        {
            var expected = new[,] { { 1, 2 }, { 3, 4 } };
            var array = new int[][] { new[] { 1, 2 }, new[] { 3, 4 } };
            var actual = array.ToMultiDimensional();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanFlattenJaggedArray()
        {
            var expected = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var jagged = new int[][] { new[] { 1, 2 }, null, new[] { 3, 4 }, new[] { 5 }, new[] { 6, 7, 8, 9 } };
            var actual = jagged.Flatten();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void JappedArrayOfNullRowsIsFlattenInAnEmptyArray()
        {
            var expected = new int[0];
            var jagged = new int[][] { null, null, null };
            var actual = jagged.Flatten();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRotateLeft()
        {
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 4, 5, 6, 7, 8, 9, 1, 2, 3 };
            var position = actual.Rotate(-3);
            Assert.Equal(6, position);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanRotateRight()
        {
            var actual = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 7, 8, 9, 1, 2, 3, 4, 5, 6 };
            var position = actual.Rotate(3);
            Assert.Equal(3, position);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumerateRange()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 3, 4, 5, 6 };
            var list = new List<int>();
            using (var enumerator = data.GetEnumerator(2, 4)) {
                while (enumerator.MoveNext())
                    list.Add(enumerator.Current);
            }
            var actual = list.ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanEnumeratePath()
        {
            var data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            var expected = new[] { 2, 5, 6, 8 };
            var actual = data.Path(1, 4, 5, 7).ToArray();
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanPlayTicTacToe()
        {
            var tictactoe = new[] { 0, 1, 0, 0, 1, 0, 0, 1, 0 };

            Assert.True(tictactoe.Path(1, 4, 7).All(p => p == 1));
        }
    }
}

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

using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft.Business.ProductModel
{
    /// <summary>
    /// Describes a set of Product instances of a specific product type that are all to be
    /// tracked together, usually for quality control purposes.
    /// </summary>
    public class Batch : ICollection<ProductInstance>
    {
        #region Fields

        private readonly ProductIdentifier _identifier;
        private readonly ProductIdentifier _batchOf;
        private readonly int _numberInBatch;
        private readonly List<ProductInstance> _instances;

        #endregion

        #region Lifecycle

        public Batch(ProductIdentifier batchOf, int numberInBatch)
            : this(new ProductIdentifier(), batchOf,numberInBatch) {
        }

        public Batch(ProductIdentifier identifier, ProductIdentifier batchOf, int numberInBatch) {
            _identifier = identifier;
            _batchOf = batchOf;
            _numberInBatch = numberInBatch;
            _instances = new List<ProductInstance>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// A unique identifier for the batch.
        /// </summary>
        public ProductIdentifier Identifier {
            get {
                return _identifier;
            }
        }

        /// <summary>
        /// The product identifier for the batch.
        /// </summary>
        public ProductIdentifier BatchOf {
            get {
                return _batchOf;
            }
        }

        /// <summary>
        /// The number of instances in the batch.
        /// </summary>
        public int NumberInBatch {
            get {
                return _numberInBatch;
            }
        }

        /// <summary>
        /// The date that the Batch of Product instances was produced - if the batch is produced
        /// over a period of time, this is the batch on which the Batch was completed.
        /// </summary>
        public DateTime? DateProduced { get; set; }

        /// <summary>
        /// The latest date on which the product instances in the Batch may be sold.
        /// </summary>
        public DateTime? SellBy { get; set; }

        /// <summary>
        /// The latest date on which the Product instances in the Batch may be used - this indicates
        /// the date by which the product instances will have spoiled (for perishable goods) or
        /// become obsolete or otherwise unsable (for nonperishable goods).
        /// </summary>
        public DateTime? UseBy { get; set; }

        /// <summary>
        /// The date onwhich the product instances pass their best quality.
        /// </summary>
        public DateTime? BestBefore { get; set; }

        /// <summary>
        /// The SerialNumber of the first and last Product instances in the Batch.
        /// </summary>
        /// <remarks>Note the assumption that serial numbers are consecutive within the Batch.</remarks>
        public Range<SerialNumber>? SerialNumbers { get; set; }

        /// <summary>
        /// Comments about the Batch.
        /// </summary>
        public string Comments { get; set; }

        #endregion

        #region ICollection<ProductInstance> Members

        public void Add(ProductInstance item) {
            _instances.Add(item);
        }

        public void Clear() {
            _instances.Clear();
        }

        public bool Contains(ProductInstance item) {
            return _instances.Contains(item);
        }

        public void CopyTo(ProductInstance[] array, int arrayIndex) {
            _instances.CopyTo(array, arrayIndex);
        }

        public int Count {
            get { return _instances.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public bool Remove(ProductInstance item) {
            return _instances.Remove(item);
        }

        #endregion

        #region IEnumerable<ProductInstance> Members

        public IEnumerator<ProductInstance> GetEnumerator() {
            return _instances.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return _instances.GetEnumerator();
        }

        #endregion
    }
}
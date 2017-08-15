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
using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.OrderModel
{
    /// <summary>
    /// Represents a record of a request by a buyer for a
    /// seller to supply some goods or services.
    /// </summary>
    public abstract class Order
    {
        #region Fields

        private readonly OrderIdentifier _identifier;
        private Dictionary<PartySummaryRoleInOrder, PartySummary> _parties;
        private OrderStatus _status;

        #endregion

        #region Lifecycle

        protected Order()
        {
            _identifier = new OrderIdentifier();

            OrderLines = new List<OrderLine>();
            ChargeLines = new List<ChargeLine>();
        }

        #endregion

        #region Properties

        public OrderIdentifier Identifier {
            get { return _identifier; }
        }

        public OrderStatus Status {
            get { return _status; }
        }

        public ICollection<OrderLine> OrderLines { get; private set; }
        public ICollection<ChargeLine> ChargeLines { get; private set; }

        public string TermsAndConditions { get; private set; }

        #endregion

        #region PartySummary support

        public PartySummary GetPartySummary(PartySummaryRoleInOrder role)
        {
            if (_parties != null && _parties.TryGetValue(role, out PartySummary partySummary))
                return partySummary;
            return null;
        }

        public void AddPartySummary(PartySummaryRoleInOrder role, PartySummary partySummary)
        {
            if (_parties == null)
                _parties = new Dictionary<PartySummaryRoleInOrder, PartySummary>();
            _parties[role] = partySummary;
        }

        public void RemovePartySummary(PartySummaryRoleInOrder role)
        {
            if (_parties != null)
                _parties.Remove(role);
        }

        public PartySummary Vendor => GetPartySummary(PartySummaryRoleInOrder.Vendor);
        public PartySummary SalesAgent => GetPartySummary(PartySummaryRoleInOrder.SalesAgent);
        public PartySummary PaymentReceiver => GetPartySummary(PartySummaryRoleInOrder.PaymentReceiver);
        public PartySummary OrderInitiator => GetPartySummary(PartySummaryRoleInOrder.OrderInitiator);
        public PartySummary Purchaser => GetPartySummary(PartySummaryRoleInOrder.Purchaser);
        public DeliveryReceiver OrderReceiver => (DeliveryReceiver)GetPartySummary(PartySummaryRoleInOrder.OrderReceiver);

        #endregion
    }
}

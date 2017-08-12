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

using WmcSoft.Business.PartyModel;

namespace WmcSoft.Business.OrderModel
{
    /// <summary>
    /// Represents an event sent to an <see cref="OrderStatus.Open "/> <see cref="Order"/> 
    /// that captures an amendment to the <see cref="Order"/>.
    /// </summary>
    public abstract class AmendEvent : LifecycleEvent
    {
        public AmendEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }

        public string Reason { get; set; }
    }

    /// <summary>
    /// Represents an event that, when applied to an <see cref="OrderStatus.Open "/> <see cref="Order"/>, 
    /// results in a change to a specific <see cref="OrderLine"/>, the addition of a new <see cref="OrderLine"/>,
    /// or the deletion of an existing <see cref="OrderLine"/>.
    /// </summary>
    public class AmendOrderLineEvent : AmendEvent
    {
        private readonly OrderLineIdentifier _orderLineIdentifier;
        private readonly OrderLine _newOrderLine;

        public AmendOrderLineEvent(OrderIdentifier identifier, OrderLineIdentifier orderLineIdentifier, OrderLine newOrderLine)
            : base(identifier)
        {
            _orderLineIdentifier = orderLineIdentifier;
            NewOrderLine = newOrderLine;
        }

        public OrderLine OldOrderLine { get; }
        public OrderLine NewOrderLine { get; }
    }

    /// <summary>
    /// Represents an event that, when applied to an <see cref="OrderStatus.Open "/> <see cref="Order"/>, 
    /// results in a change to a specific <see cref="PartySummary"/>, the addition of a new <see cref="PartySummary"/>,
    /// or the deletion of an existing <see cref="PartySummary"/>.
    /// </summary>
    public class AmendPartySummaryEvent : AmendEvent
    {
        private readonly OrderLineIdentifier _orderLineIdentifier;
        private readonly string _role;

        public AmendPartySummaryEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }

        public PartySummary OldPartySummary { get; }
        public PartySummary NewPartySummary { get; }
    }

    /// <summary>
    /// Represents an event that, when applied to an <see cref="OrderStatus.Open "/> <see cref="Order"/>, 
    /// results in a change to its <see cref="Order.TermsAndConditions"/>, the addition of <see cref="Order.TermsAndConditions"/> 
    /// if these were previously absent, or the deletion of an existing <see cref="Order.TermsAndConditions"/>.
    /// </summary>
    public class AmendTermsAndConditionsEvent : AmendEvent
    {
        public AmendTermsAndConditionsEvent(OrderIdentifier identifier)
            : base(identifier)
        {
        }

        public string OldTermsAndConditions { get; }
        public string NewTermsAndConditions { get; }
    }
}
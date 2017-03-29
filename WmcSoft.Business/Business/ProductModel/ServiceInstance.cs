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
using WmcSoft.Time;

namespace WmcSoft.Business.ProductModel
{
    public abstract class ServiceInstance : ProductInstance
    {
        #region Fields

        private ServiceDeliveryStatus _serviceDeliveryStatus;

        #endregion

        #region Lifecycle

        public ServiceInstance(ProductType productType)
            : base(productType) {
        }

        public ServiceInstance(ProductType productType, SerialNumber serialNumber, Batch batch)
            : base(productType, serialNumber, batch) {
        }

        public ServiceInstance(ProductType productType, DateTime scheduledStart, DateTime scheduledEnd)
            : base(productType) {
            ScheduledPeriod = new Interval<TimePoint>(scheduledStart, scheduledEnd);
        }

        #endregion

        #region Properties

        public TimePoint? Start { get; private set; }
        public TimePoint? End { get; private set; }
        public Interval<TimePoint> ScheduledPeriod { get; private set; }

        public ServiceDeliveryStatus ServiceDeliveryStatus {
            get { return _serviceDeliveryStatus; }
        }

        #endregion

        #region Methods

        public void Reschedule(DateTime scheduledStart, DateTime scheduledEnd) {
            if (_serviceDeliveryStatus != ServiceDeliveryStatus.Scheduled)
                throw new InvalidOperationException();

            DoReschedule(scheduledStart, scheduledEnd);

            ScheduledPeriod = new Interval<TimePoint>(scheduledStart, scheduledEnd);
        }

        public void Execute() {
            if (_serviceDeliveryStatus != ServiceDeliveryStatus.Scheduled)
                throw new InvalidOperationException();

            DoExecute();

            Start = GetNow();
            _serviceDeliveryStatus = ServiceDeliveryStatus.Executing;
        }

        public void Complete() {
            if (_serviceDeliveryStatus != ServiceDeliveryStatus.Executing)
                throw new InvalidOperationException();

            DoComplete();

            End = GetNow();
            _serviceDeliveryStatus = ServiceDeliveryStatus.Completed;
        }

        public void Cancel() {
            if (_serviceDeliveryStatus != ServiceDeliveryStatus.Executing && _serviceDeliveryStatus != ServiceDeliveryStatus.Scheduled)
                throw new InvalidOperationException();

            DoCancel();

            End = GetNow();
            _serviceDeliveryStatus = ServiceDeliveryStatus.Cancelled;
        }

        protected virtual DateTime GetNow() {
            return DateTime.Now;
        }

        protected virtual void DoReschedule(DateTime scheduledStart, DateTime scheduledEnd) { }
        protected virtual void DoExecute() { }
        protected virtual void DoComplete() { }
        protected virtual void DoCancel() { }

        #endregion
    }
}

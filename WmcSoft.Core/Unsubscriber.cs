using System;

namespace WmcSoft
{
    public class Unsubscriber : IDisposable
    {
        #region Private field

        Action unsubscribe;

        #endregion

        #region Lifecycle

        public Unsubscriber(Action unsubscribe) {
            this.unsubscribe = unsubscribe;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Explicitly unsubscribe the <see cref="IObserver<T>"/> from the <see cref="IObservable<T>"/>.
        /// </summary>
        public void Unsubscribe() {
            ((IDisposable)this).Dispose();
        }

        #endregion

        #region IDisposable Membres

        ~Unsubscriber() {
            Dispose(false);
        }

        void IDisposable.Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing) {
            if (unsubscribe != null) {
                unsubscribe();
                unsubscribe = null;
            }
        }

        #endregion
    }
}

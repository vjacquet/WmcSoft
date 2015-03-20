using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WmcSoft
{
    /// <summary>
    /// Stack disposables to Dispose them in reverse order.
    /// </summary>
    public class DisposableStack : IDisposable
    {
        readonly Stack<IDisposable> _stack;

        public DisposableStack() {
            _stack = new Stack<IDisposable>();
        }

        ~DisposableStack() {
            Dispose(false);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Push(IDisposable disposable) {
            _stack.Push(disposable);
        }

        protected virtual void Dispose(bool disposing) {
            while (_stack.Count != 0) {
                _stack.Pop().Dispose();
            }
        }
    }
}

/*
 * Les coulisses du CLR: 9 structures et algorithmes de données parallèles réutilisables
 * 
 * http://msdn.microsoft.com/msdnmag/issues/07/05/CLRInsideOut/default.aspx?loc=fr
 * 
 */

// 2015-03-24 VJA
//  - renamed fields using this library conventions.
//  - added readonly where applicable.
//  - added sealed.
//  - implemented IDisposable.

using System;
using System.Collections.Generic;
using System.Threading;

namespace WmcSoft.Threading
{
    public sealed class LockFreeStack<T>
    {

        class StackNode
        {
            internal readonly T value;
            internal StackNode next;
            internal StackNode(T val) { value = val; }
        }

        private volatile StackNode _head;

#pragma warning disable 0420
        public void Push(T item) {
            StackNode node = new StackNode(item);
            StackNode head;
            do {
                head = _head;
                node.next = head;
            } while (_head != head || Interlocked.CompareExchange(ref _head, node, head) != head);
        }

        public T Pop() {
            StackNode head;
            SpinWait s = new SpinWait();

            while (true) {
                StackNode next;
                do {
                    head = _head;
                    if (head == null)
                        goto emptySpin;
                    next = head.next;
                } while (_head != head || Interlocked.CompareExchange(ref _head, next, head) != head);
                break;

            emptySpin:
                s.Spin();
            }

            return head.value;
        }
#pragma warning restore 0420

    }

}

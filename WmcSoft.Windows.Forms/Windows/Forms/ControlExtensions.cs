﻿#region Licence

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
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class ControlExtensions
    {
        public static T AddNew<T>(this Control control)
            where T : Control, new() {
            T t = new T();
            control.Controls.Add(t);
            return t;
        }

        public static IEnumerable<T> AncestorsOrSelf<T>(this Control control)
              where T : Control {
            if (control == null)
                throw new NullReferenceException();

            var it = control;
            while (it != null) {
                T t = it as T;
                if (t != null)
                    yield return t;
                it = it.Parent;
            }
        }

        public static IEnumerable<T> Ancestors<T>(this Control control)
            where T : Control {
            return AncestorsOrSelf<T>(control.Parent);
        }

        public static IEnumerable<Control> Ancestors(this Control control) {
            return AncestorsOrSelf(control.Parent);
        }

        public static IEnumerable<Control> AncestorsOrSelf(this Control control) {
            if (control == null)
                throw new NullReferenceException();

            var it = control;
            while (it != null) {
                yield return it;
                it = it.Parent;
            }
        }

        public static IEnumerable<Control> DescendantsOrSelf(this Control control) {
            var stack = new Stack<Control>();
            stack.Push(control);
            while (stack.Count > 0) {
                Control top = stack.Pop();
                yield return top;
                foreach (Control child in top.Controls)
                    stack.Push(child);
            }
        }

        public static IEnumerable<T> DescendantsOrSelf<T>(this Control control)
            where T : Control {
            var stack = new Stack<Control>();
            stack.Push(control);
            while (stack.Count > 0) {
                Control top = stack.Pop();
                if (top is T)
                    yield return (T)top;
                foreach (Control child in top.Controls)
                    stack.Push(child);
            }
        }

        public static IEnumerable<Control> Descendants(this Control control) {
            var stack = new Stack<Control>(control.Controls.OfType<Control>());
            while (stack.Count > 0) {
                Control top = stack.Pop();
                yield return top;
                foreach (Control child in top.Controls)
                    stack.Push(child);
            }
        }

        public static IEnumerable<T> Descendants<T>(this Control control)
            where T : Control {
            var stack = new Stack<Control>(control.Controls.OfType<Control>());
            while (stack.Count > 0) {
                Control top = stack.Pop();
                if (top is T)
                    yield return (T)top;
                foreach (Control child in top.Controls)
                    stack.Push(child);
            }
        }
    }
}
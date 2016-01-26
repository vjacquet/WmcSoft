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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace WmcSoft.Windows.Forms
{
    public static class ControlExtensions
    {
        #region Linq-like extensions

        public static IEnumerable<Control> Where(this Control.ControlCollection collection, Func<Control, bool> predicate) {
            return collection.Cast<Control>().Where(predicate);
        }

        public static IEnumerable<U> Select<U>(this Control.ControlCollection collection, Func<Control, U> selector) {
            return collection.Cast<Control>().Select(selector);
        }
        public static IEnumerable<V> SelectMany<U, V>(this Control.ControlCollection collection, Func<Control, IEnumerable<U>> selector, Func<Control, U, V> resultSelector) {
            return collection.Cast<Control>().SelectMany(selector, resultSelector);
        }

        public static IEnumerable<V> Join<U, K, V>(this Control.ControlCollection collection, IEnumerable<U> inner, Func<Control, K> outerKeySelector, Func<U, K> innerKeySelector, Func<Control, U, V> resultSelector) {
            return collection.Cast<Control>().Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IEnumerable<V> GroupJoin<U, K, V>(this Control.ControlCollection collection, IEnumerable<U> inner, Func<Control, K> outerKeySelector, Func<U, K> innerKeySelector, Func<Control, IEnumerable<U>, V> resultSelector) {
            return collection.Cast<Control>().GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static IOrderedEnumerable<Control> OrderBy<K>(this Control.ControlCollection collection, Func<Control, K> keySelector) {
            return collection.Cast<Control>().OrderBy(keySelector);
        }

        public static IOrderedEnumerable<Control> OrderByDescending<K>(this Control.ControlCollection collection, Func<Control, K> keySelector) {
            return collection.Cast<Control>().OrderByDescending(keySelector);
        }

        public static IEnumerable<IGrouping<K, Control>> GroupBy<K>(this Control.ControlCollection collection, Func<Control, K> keySelector) {
            return collection.Cast<Control>().GroupBy(keySelector);
        }

        public static IEnumerable<IGrouping<K, E>> GroupBy<K, E>(this Control.ControlCollection collection, Func<Control, K> keySelector, Func<Control, E> elementSelector) {
            return collection.Cast<Control>().GroupBy(keySelector, elementSelector);
        }

        public static IEnumerable<Control> Where(this Control.ControlCollection collection, Func<Control, int, bool> predicate) {
            return collection.Cast<Control>().Where(predicate);
        }

        #endregion

        #region Navigation

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

        public static IEnumerable<T> FindControls<T>(this Control self, params string[] names) where T : Control {
            var set = new HashSet<string>(names);
            return self.Controls.OfType<T>().Where(c => set.Remove(c.Name));
        }

        public static bool IsAncestorOrSelf(this Control control, Control candidate) {
            if (control == null)
                throw new NullReferenceException();

            var it = candidate;
            while (it != null) {
                if (it == control)
                    return true;
                it = it.Parent;
            }

            return false;
        }

        #endregion

        #region Control extensions

        public const ContentAlignment AnyTopAlignment = ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft;
        public const ContentAlignment AnyMiddleAlignment = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
        public const ContentAlignment AnyBottomAlignment = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;

        public static int GetTextBaseline(this Control ctrl, ContentAlignment alignment) {
            var clientRectangle = ctrl.ClientRectangle;
            var tmAscent = ctrl.Font.FontFamily.GetCellAscent(ctrl.Font.Style);
            var tmHeight = ctrl.Font.FontFamily.GetEmHeight(ctrl.Font.Style);
            if ((alignment & AnyTopAlignment) != (ContentAlignment)0)
                return clientRectangle.Top + tmAscent;
            if ((alignment & AnyMiddleAlignment) != (ContentAlignment)0) 
                return clientRectangle.Top + (clientRectangle.Height / 2) - (tmHeight / 2) + tmAscent;
            return clientRectangle.Bottom - tmHeight + tmAscent;
        }

        /// <summary>
        ///   Given a control, figure out if it should be drawn Right To Left,
        ///   which might involve going up the parent chain.
        /// </summary>
        /// <param name="control">Control whose RTL value we want to explore.</param>
        /// <returns>True means we should be drawing RTL.</returns>
        public static bool GetRightToLeftValue(this Control control) {
            while (control != null) {
                switch (control.RightToLeft) {
                case RightToLeft.Yes:
                    return true;
                case RightToLeft.No:
                    return false;
                case RightToLeft.Inherit:
                    // Keep going up the parent chain...
                    break;
                }
                control = control.Parent;
            }
            return CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
        }

        #endregion

        #region Control specific extensions

        #region NotifyIcon

        public static void ShowBalloonTip(this NotifyIcon notifyIcon, TimeSpan timeout, string tipTitle, string tipText, ToolTipIcon tipIcon) {
            notifyIcon.ShowBalloonTip((int)timeout.TotalMilliseconds, tipTitle, tipText, tipIcon);
        }

        public static void ShowBalloonTip(this NotifyIcon notifyIcon, TimeSpan timeout) {
            notifyIcon.ShowBalloonTip((int)timeout.TotalMilliseconds);
        }

        #endregion

        #region NumericUpDown

        public static TextBox GetTextBox(this NumericUpDown numericUpDown) {
            return numericUpDown.Controls[1] as TextBox;
        }

        public static void ApplyOnText(this NumericUpDown numericUpDown, Action<TextBoxBase> action) {
            if (numericUpDown == null)
                return;
            var textBox = numericUpDown.Controls[1] as TextBoxBase;
            if (textBox == null)
                return;
            action(textBox);
        }

        #endregion

        #region TreeView

        public static bool RemoveIf(this TreeNodeCollection collection, Predicate<TreeNode> predicate) {
            var count = collection.Count;
            for (int i = 0; i < count; i++) {
                var node = collection[i];
                if (predicate(node)) {
                    collection.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public static int RemoveAll(this TreeNodeCollection collection, Predicate<TreeNode> predicate) {
            var count = collection.Count;
            var removed = 0;
            for (int i = 0; i < count;) {
                var node = collection[i];
                if (predicate(node)) {
                    collection.RemoveAt(i);
                    removed++;
                    count--;
                } else {
                    i++;
                }
            }
            return removed;
        }

        #endregion

        #endregion
    }
}

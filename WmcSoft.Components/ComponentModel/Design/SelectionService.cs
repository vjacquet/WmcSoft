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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace WmcSoft.ComponentModel.Design
{
    public class SelectionService : ISelectionService
    {
        private readonly object _rootComponent;
        private readonly ArrayList _selectedComponents;

        public SelectionService(IServiceProvider serviceProvider, object rootComponent) {
            this._rootComponent = rootComponent;

            _selectedComponents = new ArrayList();
            if (rootComponent != null)
                _selectedComponents.Add(rootComponent);

            // Subscribe to the ComponentRemoved event
            if (serviceProvider != null) {
                var c = serviceProvider.GetService<IComponentChangeService>();
                if (c != null) {
                    c.ComponentRemoved += new ComponentEventHandler(OnComponentRemoved);
                }
            }
        }

        public ICollection GetSelectedComponents() {
            return _selectedComponents.ToArray();
        }

        public event EventHandler SelectionChanging {
            add { this.Events.AddHandler(SelectionChangingEvent, value); }
            remove { this.Events.RemoveHandler(SelectionChangingEvent, value); }
        }
        private static readonly object SelectionChangingEvent = new Object();

        /// <summary>
        /// Fire the SelectionChanging event if anything is bound to it
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanging(EventArgs e) {
            EventHandler handler = (EventHandler)this.events[SelectionChangingEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        public event EventHandler SelectionChanged {
            add { this.Events.AddHandler(SelectionChangedEvent, value); }
            remove { this.Events.RemoveHandler(SelectionChangedEvent, value); }
        }
        private static readonly object SelectionChangedEvent = new Object();

        /// <summary>
        /// Fire the SelectionChanging event if anything is bound to it
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelectionChanged(EventArgs e) {
            EventHandler handler = (EventHandler)this.events[SelectionChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        public object PrimarySelection {
            get {
                if (_selectedComponents.Count > 0)
                    return _selectedComponents[0];
                return null;
            }
        }

        public int SelectionCount {
            get { return _selectedComponents.Count; }
        }

        public bool GetComponentSelected(object component) {
            return _selectedComponents.Contains(component);
        }

        public void SetSelectedComponents(ICollection components, SelectionTypes selectionType) {
            // Interpret the selection type
            bool primary = (selectionType & SelectionTypes.Primary) == SelectionTypes.Primary;
            bool toggle = (selectionType & SelectionTypes.Toggle) == SelectionTypes.Toggle;
            bool add = (selectionType & SelectionTypes.Add) == SelectionTypes.Add;
            bool remove = (selectionType & SelectionTypes.Remove) == SelectionTypes.Remove;
            bool replace = (selectionType & SelectionTypes.Replace) == SelectionTypes.Replace;

            // Components can be null, but not one of its items
            if (components == null) {
                components = new object[0];
            }
            foreach (object component in components) {
                if (component == null) {
                    throw new ArgumentNullException("components");
                }
            }
            if (primary && components.Count != 1) {
                throw new ArgumentException("components");
            }

            // If the selection type is Click, we want to know if shift or control is being held
            //bool control = false;
            //bool shift = false;
            //if ((selectionType & SelectionTypes.Click) == SelectionTypes.Click)
            //{
            //    control = ((Control.ModifierKeys & Keys.Control) == Keys.Control);
            //    shift = ((Control.ModifierKeys & Keys.Shift) == Keys.Shift);
            //}

            // Raise selectionchanging event
            //
            OnSelectionChanging(EventArgs.Empty);

            if (replace) {
                // Simply replace our existing collection with the new one
                _selectedComponents.Clear();
                foreach (object component in components) {
                    if (component != null && !_selectedComponents.Contains(component)) {
                        _selectedComponents.Add(component);
                    }
                }
            } else if (primary) {
                var enumerator = components.GetEnumerator();
                if (enumerator.MoveNext()) {
                    object component = enumerator.Current;
                    int index = _selectedComponents.IndexOf(component);
                    if (index >= 0) {
                        object temp = _selectedComponents[0];
                        _selectedComponents[0] = _selectedComponents[index];
                        _selectedComponents[index] = temp;
                    } else {
                        _selectedComponents.Clear();
                        _selectedComponents.Add(component);
                    }
                }
            } else {
                // Add or remove each component to or from the selection
                foreach (object component in components) {
                    if (_selectedComponents.Contains(component)) {
                        if (toggle || remove) {
                            _selectedComponents.Remove(component);
                        }
                    } else {
                        if (toggle || add) {
                            _selectedComponents.Add(component);
                        }
                    }
                }
            }

            if (_selectedComponents.Count == 0 && _rootComponent != null)
                _selectedComponents.Add(_rootComponent);

            OnSelectionChanged(EventArgs.Empty);
        }

        public void SetSelectedComponents(ICollection components) {
            // Use the Replace selection type because this needs to replace anything already selected
            SetSelectedComponents(components, SelectionTypes.Replace);
        }

        internal void OnComponentRemoved(object sender, ComponentEventArgs e) {
            if (_selectedComponents.Contains(e.Component)) {
                OnSelectionChanging(EventArgs.Empty);

                // Remove this component from the selected components
                _selectedComponents.Remove(e.Component);

                // Select root component if that leaves us with no selected components
                if (SelectionCount == 0 && _rootComponent != null) {
                    _selectedComponents.Add(_rootComponent);
                }

                OnSelectionChanged(EventArgs.Empty);
            }
        }

        #region Events

        protected EventHandlerList Events {
            get {
                if (this.events == null) {
                    this.events = new EventHandlerList();
                }
                return this.events;
            }
        }

        [NonSerialized]
        private EventHandlerList events;

        #endregion
    }
}

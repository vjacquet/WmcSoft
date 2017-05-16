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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Collections;

namespace WmcSoft.ComponentModel
{
    [ToolboxBitmap(typeof(SelectionServiceComponent))]
    public partial class SelectionServiceComponent : Component,
        ISelectionService
    {
        public IDisposable SelectionEventBarrier(object target)
        {
            return new EventBarrier(this.Events, target, SelectionChangingEvent, SelectionChangedEvent);
        }

        #region ObjectReferenceConverter class

        internal class ObjectReferenceConverter : ReferenceConverter
        {
            // Methods
            public ObjectReferenceConverter()
                : base(typeof(IComponent))
            {
            }
        }

        #endregion

        #region Private Fields

        ISelectionService selectionService;

        #endregion

        #region Lifecycle

        public SelectionServiceComponent()
        {
            InitializeComponent();
        }

        public SelectionServiceComponent(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region ISelectionService Membres

        bool ISelectionService.GetComponentSelected(object component)
        {
            return SelectionService.GetComponentSelected(component);
        }

        System.Collections.ICollection ISelectionService.GetSelectedComponents()
        {
            return SelectionService.GetSelectedComponents();
        }

        [Bindable(BindableSupport.Yes)]
        [DefaultValue(null)]
        public object PrimarySelection {
            get { return SelectionService.PrimarySelection; }
            set {
                if (SelectionService.PrimarySelection != value) {
                    if (value == null)
                        SelectionService.SetSelectedComponents(null, SelectionTypes.Primary);
                    else
                        SelectionService.SetSelectedComponents(new object[] { value }, SelectionTypes.Primary);
                }
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
        protected virtual void OnSelectionChanged(EventArgs e)
        {
            if (raisesSelectionChangedEvents) {
                EventHandler handler = (EventHandler)this.Events[SelectionChangedEvent];
                if (handler != null) {
                    handler(this, e);
                }
                OnPrimarySelectionChanged(e);
            } else {
                isSelectionDirty = true;
            }
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
        protected virtual void OnSelectionChanging(EventArgs e)
        {
            if (raisesSelectionChangedEvents) {
                EventHandler handler = (EventHandler)this.Events[SelectionChangingEvent];
                if (handler != null) {
                    handler(this, e);
                }
            }
        }

        public int SelectionCount {
            get { return this.SelectionService.SelectionCount; }
        }

        void ISelectionService.SetSelectedComponents(System.Collections.ICollection components, SelectionTypes selectionType)
        {
            this.SelectionService.SetSelectedComponents(components, selectionType);
        }

        void ISelectionService.SetSelectedComponents(System.Collections.ICollection components)
        {
            this.SelectionService.SetSelectedComponents(components);
        }

        #endregion

        #region Protected Properties

        protected ISelectionService SelectionService {
            get {
                if (selectionService == null) {
                    if (!Standalone) {
                        selectionService = GetService(typeof(ISelectionService)) as ISelectionService;
                    }

                    if (selectionService == null) {
                        var serviceProvider = (IServiceProvider)this.GetService(typeof(IServiceProvider));
                        selectionService = new Design.SelectionService(serviceProvider, RootComponent);
                    }

                    selectionService.SelectionChanging += new EventHandler(selectionService_SelectionChanging);
                    selectionService.SelectionChanged += new EventHandler(selectionService_SelectionChanged);
                }
                return selectionService;
            }
        }

        void selectionService_SelectionChanging(object sender, EventArgs e)
        {
            OnSelectionChanging(e);
        }

        void selectionService_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(e);
        }

        #endregion

        #region Public Properties

        [DefaultValue(false)]
        public bool RaisesSelectionChangedEvents {
            get {
                return raisesSelectionChangedEvents;
            }
            set {
                raisesSelectionChangedEvents = value;
                if (raisesSelectionChangedEvents && isSelectionDirty) {
                    OnSelectionChanged(EventArgs.Empty);
                }
            }
        }
        bool raisesSelectionChangedEvents;
        bool isSelectionDirty;

        [DefaultValue(false)]
        public bool Standalone { get; set; }

        [DefaultValue(null)]
        [TypeConverter(typeof(ObjectReferenceConverter))]
        public object RootComponent { get; set; }

        public event EventHandler PrimarySelectionChanged {
            add { Events.AddHandler(PrimarySelectionChangedEvent, value); }
            remove { Events.RemoveHandler(PrimarySelectionChangedEvent, value); }
        }
        private static readonly object PrimarySelectionChangedEvent = new Object();

        /// <summary>
        /// Fire the SelectionChanging event if anything is bound to it
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPrimarySelectionChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)this.Events[PrimarySelectionChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        [Bindable(BindableSupport.Yes)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ICollection Selection {
            get {
                return this.SelectionService.GetSelectedComponents();
            }
            set {
                this.SelectionService.SetSelectedComponents(value, SelectionTypes.Replace);
            }
        }

        #endregion
    }
}

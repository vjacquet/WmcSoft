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
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using WmcSoft.ComponentModel;

namespace WmcSoft.Windows.Forms
{
    [ComplexBindingProperties("DataSource", "DataMember")]
    public class BindableListView : ListView
    {
        #region Lifecycle

        public BindableListView() {
            InitializeComponent();
        }

        public BindableListView(IContainer container) {
            container.Add(this);
            InitializeComponent();
        }

        #endregion

        #region Properties

        [Editor("System.Windows.Forms.Design.DataMemberListEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DefaultValue((string)null)]
        [Category("Data")]
        public string DataMember {
            get { return bindingSource.DataMember; }
            set { SetDataBinding(DataSource, value); }
        }

        [Category("Data")]
        public event EventHandler DataMemberChanged {
            add { base.Events.AddHandler(DataMemberChangedEvent, value); }
            remove { base.Events.RemoveHandler(DataMemberChangedEvent, value); }
        }
        static object DataMemberChangedEvent = new object();

        protected virtual void OnDataMemberChanged(EventArgs e) {
            EventHandler handler = Events[DataMemberChangedEvent] as EventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        [DefaultValue((string)null)]
        [Category("Data")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.Repaint)]
        public object DataSource {
            get { return bindingSource.DataSource; }
            set { SetDataBinding(value, DataMember); }
        }

        [Category("Data")]
        public event EventHandler DataSourceChanged {
            add { base.Events.AddHandler(DataSourceChangedEvent, value); }
            remove { base.Events.RemoveHandler(DataSourceChangedEvent, value); }
        }
        static object DataSourceChangedEvent = new object();

        protected virtual void OnDataSourceChanged(EventArgs e) {
            EventHandler handler = this.Events[DataSourceChangedEvent] as EventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion

        BindingManagerBase bindingManagerBase;

        public void SetDataBinding(object dataSource, string dataMember) {
            bool dataSourceChanged = this.bindingSource.DataSource != dataSource;
            bool dataMemberChanged = this.bindingSource.DataMember != dataMember;

            if (dataSourceChanged || dataMemberChanged) {
                // unwire the binding manager
                if (bindingManagerBase != null) {
                    bindingManagerBase.CurrentItemChanged -= bindingManagerBase_CurrentItemChanged;
                    bindingManagerBase.PositionChanged -= bindingManagerBase_PositionChanged;
                }

                if (dataSourceChanged) {
                    bindingSource.DataSource = dataSource;
                    bindingSource.DataMember = dataMember;
                } else if (dataMemberChanged) {
                    bindingSource.DataMember = dataMember;
                }

                bindingManagerBase = this.BindingContext[dataSource, dataMember];

                // wire the binding manager
                if (bindingManagerBase != null) {
                    bindingManagerBase.CurrentItemChanged += bindingManagerBase_CurrentItemChanged;
                    bindingManagerBase.PositionChanged += bindingManagerBase_PositionChanged;
                }

                // notify changes
                if (dataSourceChanged)
                    OnDataSourceChanged(EventArgs.Empty);
                if (dataMemberChanged)
                    OnDataMemberChanged(EventArgs.Empty);
            }
        }

        void bindingManagerBase_CurrentItemChanged(object sender, EventArgs e) {
        }

        void bindingManagerBase_PositionChanged(object sender, EventArgs e) {
            int position = bindingManagerBase.Position;
            var selectedIndices = SelectedIndices;
            if (position >= 0 && position < Items.Count && !selectedIndices.Contains(position)) {
                var saved = bindingManagerBase;
                bindingManagerBase = null;
                SelectedIndices.Clear();
                SelectedIndices.Add(position);
                bindingManagerBase = saved;
            }
        }

        private BindingSource bindingSource;
        private IContainer components;

        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.bindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingSource
            // 
            this.bindingSource.DataError += new System.Windows.Forms.BindingManagerDataErrorEventHandler(this.bindingSource_DataError);
            this.bindingSource.CurrentItemChanged += new System.EventHandler(this.bindingSource_CurrentItemChanged);
            this.bindingSource.ListChanged += new System.ComponentModel.ListChangedEventHandler(this.bindingSource_ListChanged);
            // 
            // BindableListView
            // 
            this.View = System.Windows.Forms.View.Details;
            this.SelectedIndexChanged += new System.EventHandler(this.BindableListView_SelectedIndexChanged);
            this.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.BindableListView_RetrieveVirtualItem);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        private void bindingSource_ListChanged(object sender, ListChangedEventArgs e) {
            switch (e.ListChangedType) {
            case ListChangedType.ItemAdded:
                this.VirtualListSize = bindingSource.CurrencyManager.Count;
                break;
            case ListChangedType.ItemChanged:
                break;
            case ListChangedType.ItemDeleted:
                this.VirtualListSize = bindingSource.CurrencyManager.Count;
                break;
            case ListChangedType.ItemMoved:
                break;
            case ListChangedType.PropertyDescriptorAdded:
                break;
            case ListChangedType.PropertyDescriptorChanged:
                break;
            case ListChangedType.PropertyDescriptorDeleted:
                break;
            case ListChangedType.Reset:
                var itemType = ListBindingHelper.GetListItemType(bindingSource.DataSource, bindingSource.DataMember);
                var columns = (ColumnHeader[])TypeDescriptor.GetConverter(itemType).ConvertTo(null, typeof(ColumnHeader[]));
                Columns.Clear();
                Columns.AddRange(columns);
                VirtualListSize = bindingSource.CurrencyManager.Count;
                break;
            default:
                break;
            }
        }

        private void bindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e) {

        }

        private void BindableListView_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e) {
            TypeDescriptorContext context = new TypeDescriptorContext(ServiceProvider, null, null);
            Type itemType = ListBindingHelper.GetListItemType(this.bindingSource.DataSource, this.bindingSource.DataMember);
            ListViewItem lvitem = (ListViewItem)TypeDescriptor
                .GetConverter(itemType)
                .ConvertTo(context, CultureInfo.CurrentUICulture, this.bindingSource[e.ItemIndex], typeof(ListViewItem));
            e.Item = lvitem;
        }

        protected IServiceProvider ServiceProvider {
            get {
                if (serviceProvider == null) {
                    IServiceProvider parentProvider = this.GetService(typeof(IServiceProvider)) as IServiceProvider;
                    ServiceContainer serviceContainer = new ServiceContainer(parentProvider);
                    serviceContainer.AddService(typeof(IBindableComponent), this);
                    serviceProvider = serviceContainer;
                }
                return serviceProvider;
            }
        }
        IServiceProvider serviceProvider;

        private void bindingSource_CurrentItemChanged(object sender, EventArgs e) {
            this.Refresh();
        }

        private void BindableListView_SelectedIndexChanged(object sender, EventArgs e) {
            if (bindingManagerBase != null) {
                bindingManagerBase.Position = this.FocusedItem.Index;
            }
        }
    }
}

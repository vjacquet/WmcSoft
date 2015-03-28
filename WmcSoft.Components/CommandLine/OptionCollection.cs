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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Text;
using System.Collections;

namespace WmcSoft.CommandLine
{
    [ListBindable(false)]
    public class OptionCollection : KeyedCollection<string, Option>
    {
        CommandLine commandLine;

        public OptionCollection(CommandLine commandLine)
            : base() {
            this.commandLine = commandLine;
        }

        public void AddRange(Option[] options) {
            foreach (Option option in options) {
                this.Add(option);
            }
        }

        #region Overridables

        protected override string GetKeyForItem(Option item) {
            // In this example, the key is the part number.
            return item.SwitchName;
        }

        protected override void InsertItem(int index, Option newItem) {
            if (newItem.collection != null)
                throw new ArgumentException("The item already belongs to a collection.");

            base.InsertItem(index, newItem);
            newItem.collection = this;
        }

        protected override void SetItem(int index, Option newItem) {
            Option replaced = Items[index];

            if (newItem.collection != null)
                throw new ArgumentException("The item already belongs to a collection.");

            base.SetItem(index, newItem);
            newItem.collection = this;
            replaced.collection = null;
        }

        protected override void RemoveItem(int index) {
            Option removedItem = Items[index];

            base.RemoveItem(index);
            removedItem.collection = null;
        }

        protected override void ClearItems() {
            foreach (Option option in Items) {
                option.collection = null;
            }

            base.ClearItems();
        }

        internal void ChangeKey(Option item, string newKey) {
            base.ChangeItemKey(item, newKey);
        }

        #endregion

        public bool TryGetValue(string key, out Option value) {
            value = null;
            foreach (Option option in this.Items) {
                if (StringComparer.InvariantCultureIgnoreCase.Equals(key, GetKeyForItem(option))) {
                    value = option;
                    return true;
                }
            }
            foreach (Option option in this.Items) {
            }
            return false;
        }
    }

    #region Design

    class OptionCollectionEditor : CollectionEditor
    {
        public OptionCollectionEditor()
            : base(typeof(OptionCollection)) {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            this.editValue = value;
            object result = base.EditValue(context, provider, value);
            this.editValue = null;
            return result;
        }
        private object editValue;

        protected override Type[] CreateNewItemTypes() {
            if (base.Context != null) {
                var typeDiscoveryService = base.Context.GetService<ITypeDiscoveryService>();
                if (typeDiscoveryService != null) {
                    ICollection collection = typeDiscoveryService.GetTypes(typeof(Option), false);
                    if (collection.Count > 1) {
                        ArrayList types = new ArrayList(collection.Count);
                        foreach (Type type in collection) {
                            if (!type.IsAbstract) {
                                types.Add(type);
                            }
                        }
                        Type[] result = new Type[types.Count];
                        types.CopyTo(result, 0);
                        return result;
                    }
                }
            }
            return new Type[0];
        }

        protected override object CreateInstance(Type itemType) {
            string name = itemType.Name.Substring(0, 1).ToLower() + itemType.Name.Substring(1);

            var service = base.GetService(typeof(INameCreationService)) as INameCreationService;
            var container = base.GetService(typeof(IContainer)) as IContainer;
            if ((service != null) && (container != null)) {
                name = service.CreateName(container, itemType);
            }

            var host = (IDesignerHost)this.GetService(typeof(IDesignerHost));
            object instance = null;
            if (typeof(IComponent).IsAssignableFrom(itemType) && (host != null)) {
                if (host != null) {
                    instance = host.CreateComponent(itemType, name);
                    var initializer = host.GetDesigner((IComponent)instance) as IComponentInitializer;
                    if (initializer != null) {
                        initializer.InitializeNewComponent(null);
                    }
                }
            }
            if (instance == null) {
                instance = TypeDescriptor.CreateInstance(host, itemType, null, new object[] { name });
            } else {
                ((Option)instance).SwitchName = name;
            }
            return instance;
        }

        protected override Type CreateCollectionItemType() {
            return typeof(Option);
        }

        protected override string GetDisplayText(object value) {
            using (var writer = new StringWriter()) {
                ((Option)value).WriteTemplate(writer);
                return writer.ToString();
            }
        }
    }

    #endregion
}

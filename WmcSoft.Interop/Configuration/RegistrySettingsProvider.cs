//---------------------------------------------------------------------
//  This file is part of the Microsoft .NET Framework SDK Code Samples.
// 
//  Copyright (C) Microsoft Corporation.  All rights reserved.
// 
//This source code is intended only as a supplement to Microsoft
//Development Tools and/or on-line documentation.  See these other
//materials for detailed information regarding Microsoft code samples.
// 
//THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
//KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//PARTICULAR PURPOSE.
//---------------------------------------------------------------------
// This file has been updated by csells@sellsbrothers.com and is
// redistributed w/o permission from Microsoft, Corp (although
// I'm working on it)
//---------------------------------------------------------------------
// NOTE: This provider uses Assembly metadata such as ProductName, etc. 
// to determine a workable registry path in which to store settings. 
// Note that these are NOT secure metadata elements, however they are 
// reasonably safe from collision but not at all safe from malicious tampering.  
// A robust implementation of the provider would include a better pathing algorithm.

// NOTE: this provider is built to be a drop-in replacement for the
// LocalFileSettingsProvider (LFSP), so when it doubt, it attempts to
// do what the LFSP would do, except:
// -this provider doesn't support roaming settings
// -this provider ignores the values in the SettingsContext,
//  which means that settings groups won't work properly.
//---------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;


namespace WmcSoft.Configuration
{
    class RegistrySettingsProvider : SettingsProvider, IApplicationSettingsProvider
    {
        /// <summary>Gets or sets the name of the currently running application.</summary>
        /// <returns>A string that contains the application's display name.</returns>
        public override string ApplicationName {
            get { return applicationName; }
            set { applicationName = value; }
        }
        string applicationName;

        RegistrySettingsProvider()
        {
            applicationName = Application.ProductName;
        }

        public override void Initialize(string name, NameValueCollection values)
        {
            if (string.IsNullOrEmpty(name)) {
                name = "RegistrySettingsProvider";
            }
            base.Initialize(name, values);
        }

        // SetPropertyValue is invoked when ApplicationSettingsBase.Save is called
        // ASB makes sure to pass each provider only the values marked for that provider,
        // whether on a per setting or setting class-wide basis
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
        {
            // Iterate through the settings to be stored
            string version = GetCurrentVersionNumber();
            foreach (SettingsPropertyValue propval in values) {
                // If property hasn't been set, no need to save it
                if (!propval.IsDirty || (propval.SerializedValue == null)) { continue; }

                // Application-scoped settings can't change
                // NOTE: the settings machinery may cause or allow an app-scoped setting
                // to become dirty, in which case, like the LFSP, we ignore it instead
                // of throwning an exception
                if (IsApplicationScoped(propval.Property)) { continue; }

                using (RegistryKey key = CreateRegKey(propval.Property, version)) {
                    key.SetValue(propval.Name, propval.SerializedValue);
                }
            }
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            // Create new collection of values
            SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();
            string version = GetCurrentVersionNumber();

            // Iterate through the settings to be retrieved
            foreach (SettingsProperty prop in properties) {
                SettingsPropertyValue value = GetPropertyValue(prop, version);
                Debug.Assert(value != null);
                values.Add(value);
            }

            return values;
        }

        SettingsPropertyValue GetPropertyValue(SettingsProperty prop, string version)
        {
            SettingsPropertyValue value = new SettingsPropertyValue(prop);

            // Only User-scoped settings can be found in the Registry.
            // By leaving the Application-scoped setting's value at null,
            // we get the "default" value
            if (IsUserScoped(prop)) {
                using (RegistryKey key = CreateRegKey(prop, version)) {
                    value.SerializedValue = key.GetValue(prop.Name);
                }
            }

            value.IsDirty = false;
            return value;
        }

        // Looks in the "attribute bag" for a given property to determine if it is app-scoped
        bool IsApplicationScoped(SettingsProperty prop)
        {
            return HasSettingScope(prop, typeof(ApplicationScopedSettingAttribute));
        }

        // Looks in the "attribute bag" for a given property to determine if it is user-scoped
        bool IsUserScoped(SettingsProperty prop)
        {
            return HasSettingScope(prop, typeof(UserScopedSettingAttribute));
        }

        // Checks for app or user-scoped based on the attributeType argument
        // Also checks for sanity, i.e. a setting not marked as both or neither scope
        // (just like the LFSP)
        bool HasSettingScope(SettingsProperty prop, Type attributeType)
        {
            // TODO: add support for roaming
            Debug.Assert((attributeType == typeof(ApplicationScopedSettingAttribute)) || (attributeType == typeof(UserScopedSettingAttribute)));
            bool isAppScoped = prop.Attributes[typeof(ApplicationScopedSettingAttribute)] != null;
            bool isUserScoped = prop.Attributes[typeof(UserScopedSettingAttribute)] != null;

            // Check constraints
            if (isUserScoped && isAppScoped) {
                throw new ArgumentException("Can't mark a setting as User and Application-scoped: " + prop.Name + ".", "prop");
            } else if (!isUserScoped && !isAppScoped) {
                throw new ArgumentException("Must mark a setting as User or Application-scoped: " + prop.Name + ".", "prop");
            }

            // Return scope check result
            if (attributeType == typeof(ApplicationScopedSettingAttribute)) {
                return isAppScoped;
            } else if (attributeType == typeof(UserScopedSettingAttribute)) {
                return isUserScoped;
            } else {
                Debug.Assert(false);
                return false;
            }
        }

        // Creates a sub-key under HKCU\Software\CompanyName\ProductName\version
        RegistryKey CreateRegKey(SettingsProperty prop, string version)
        {
            Debug.Assert(!IsApplicationScoped(prop), "Can't get Registry key for a read-only Application scoped setting: " + prop.Name);
            return Registry.CurrentUser.CreateSubKey(GetSubKeyPath(version));
        }

        // Adds a specific version to the version-independent key path
        string GetSubKeyPath(string version)
        {
            Debug.Assert(!string.IsNullOrEmpty(version));
            return GetVersionIndependentSubKeyPath() + "\\" + version;
        }

        // Builds a key path based on the CompanyName and ProductName attributes in 
        // the AssemblyInfo file (editable directly or within the Project Properties UI)
        string GetVersionIndependentSubKeyPath()
        {
            return "Software\\" + Application.CompanyName + "\\" + Application.ProductName;
        }

        string GetPreviousVersionNumber()
        {
            Version current = new Version(GetCurrentVersionNumber());
            Version previous = null;

            // Enum setting versions
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(GetVersionIndependentSubKeyPath(), false)) {
                foreach (string keyName in key.GetSubKeyNames()) {
                    try {
                        Version version = new Version(keyName);
                        if (version >= current) { continue; }
                        if (previous == null || version > previous) { previous = version; }
                    } catch {
                        // If the version can't be created, don't cry about it...
                        continue;
                    }
                }
            }

            // Return the string of the previous version
            return (previous != null ? previous.ToString() : null);
        }

        string GetCurrentVersionNumber()
        {
            // The compiler will make sure this is a sane value
            return Application.ProductVersion;
        }

        #region IApplicationSettingsProvider Members

        // Will be called when MySettingsClass.GetPreviousVersion(propName) is called
        // This method's job is to retrieve a setting value from the previous version
        // of the settings w/o updating the setting at the storage location
        public SettingsPropertyValue GetPreviousVersion(SettingsContext context, SettingsProperty prop)
        {
            // If there's no previous setting version, return an empty property
            // NOTE: the LFSP returns an empty property for all app-scoped settings, so so do we
            string previousVersion = GetPreviousVersionNumber();
            if (IsApplicationScoped(prop) || string.IsNullOrEmpty(previousVersion)) {
                // NOTE: can't just return null, as the settings engine turns that into
                // a default property -- have to return a SettingsPropertyValue object
                // with the PropertyValue set to null to really build an empty property
                SettingsPropertyValue propval = new SettingsPropertyValue(prop);
                propval.PropertyValue = null;
                return propval;
            }

            // Get the property value from the previous version
            // NOTE: if it's null, the settings machinery will assume the current default value
            // ideally, we'd want to use the previous version's default value, but a) that's
            // likely to be the current default value and b) if it's not, that data is lost
            return GetPropertyValue(prop, previousVersion);
        }

        // Will be called when MySettingsClass.Reset() is called
        // This method's job is to update the location where the settings are stored
        // with the default settings values. GetPropertyValues, overriden from the
        // SettingsProvider base, will be called to retrieve the new values from the
        // storage location
        public void Reset(SettingsContext context)
        {
            // Delete the user's current settings so that default values are used
            try {
                Registry.CurrentUser.DeleteSubKeyTree(GetSubKeyPath(GetCurrentVersionNumber()));
            } catch (ArgumentException) {
                // If the key's not there, this is the exception we'll get
                // TODO: figure out a way to detect that w/o consuming
                // an overly generic exception...
            }
        }

        // Will be called when MySettingsClass.Upgrade() is called
        // This method's job is to update the location where the settings are stored
        // with the previous version's values. GetPropertyValues, overriden from the
        // SettingsProvider base, will be called to retrieve the new values from the
        // storage location
        public void Upgrade(SettingsContext context, SettingsPropertyCollection properties)
        {
            // If there's no previous version, do nothing (just like the LFSP)
            string previousVersion = GetPreviousVersionNumber();
            if (string.IsNullOrEmpty(previousVersion)) { return; }

            // Delete the current setting values
            Reset(context);

            // Copy the old settings to the new version
            string currentVersion = GetCurrentVersionNumber();
            using (RegistryKey keyPrevious = Registry.CurrentUser.OpenSubKey(GetSubKeyPath(previousVersion), false))
            using (RegistryKey keyCurrent = Registry.CurrentUser.CreateSubKey(GetSubKeyPath(currentVersion), RegistryKeyPermissionCheck.ReadWriteSubTree)) {
                foreach (string valueName in keyPrevious.GetValueNames()) {
                    object serializedValue = keyPrevious.GetValue(valueName);
                    if (serializedValue != null) { keyCurrent.SetValue(valueName, serializedValue); }
                }
            }
        }

        #endregion
    }
}

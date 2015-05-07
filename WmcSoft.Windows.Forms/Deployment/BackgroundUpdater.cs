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

 ---------------------------------------------------------------------
   This file is adapted from the Microsoft .NET Framework SDK Code Samples.
 
   Copyright (C) Microsoft Corporation.  All rights reserved.
 
   This source code is intended only as a supplement to Microsoft
   Development Tools and/or on-line documentation.  See these other
   materials for detailed information regarding Microsoft code samples.
 
   THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
   KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
   IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
   PARTICULAR PURPOSE.

 ****************************************************************************/

#endregion

using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WmcSoft.ComponentModel;

namespace WmcSoft.Deployment
{
    /// <summary>
    ///   Component to perform regular background updating.
    /// </summary>
    [DefaultProperty("ThrowIfNotNetworkDeployed")]
    [DefaultEvent("UpdateCompleted")]
    [ToolboxBitmap(typeof(BackgroundUpdater), "BackgroundUpdater.png")]
    public class BackgroundUpdater : Component
    {
        #region Private Fields

        static readonly int DefaultUpdateInterval = 15 * 60 * 1000;

        bool _throwIfNotNetworkDeployed;
        readonly Timer _updateTimer;

        #endregion

        #region Lifecycle

        /// <summary>
        ///   Initialises a new instance of our object.
        /// </summary>
        public BackgroundUpdater() {
            _updateTimer = new Timer();
            ResetUpdateInterval();
            _updateTimer.Tick += updateTimer_Tick;
        }

        /// <summary>
        ///   Initializes a new instance of this object, adding ourselves,
        ///   if necessary, to the specified container.
        /// </summary>
        /// <param name="container">The container to which, if specified, we should add ourselves.</param>
        public BackgroundUpdater(IContainer container)
            : this() {
            if (container != null) {
                container.Add(this);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///   Returns current version.
        /// </summary>
        [DescriptionResource(typeof(BackgroundUpdater), "CurrentVersion.Description")]
        [DefaultValue("1.0.0.0")]
        [Bindable(BindableSupport.Yes)]
        public Version CurrentVersion {
            get {
                if (DesignMode)
                    return new Version(1, 0);
                else if (ApplicationDeployment.IsNetworkDeployed)
                    return ApplicationDeployment.CurrentDeployment.CurrentVersion;
                return Assembly.GetEntryAssembly().GetName().Version;
            }
        }

        public event EventHandler CurrentVersionChanged {
            add { Events.AddHandler(CurrentVersionChangedEvent, value); }
            remove { Events.RemoveHandler(CurrentVersionChangedEvent, value); }
        }
        private readonly object CurrentVersionChangedEvent = new object();

        protected virtual void OnCurrentVersionChanged(EventArgs e) {
            EventHandler handler = (EventHandler)Events[CurrentVersionChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        /// <summary>
        ///   Controls whether we should fail silently (false) or throw an
        ///   Exception (true) if the programmer calls start and the 
        ///   application is not deployed via the ClickOnce framework.
        /// </summary>
        [DescriptionResource(typeof(BackgroundUpdater), "ThrowIfNotNetworkDeployed.Description")]
        [DefaultValue(false)]
        public bool ThrowIfNotNetworkDeployed {
            get { return _throwIfNotNetworkDeployed; }
            set { _throwIfNotNetworkDeployed = value; }
        }

        /// <summary>
        ///   Controls the interval between update checks.
        /// </summary>
        [DescriptionResource(typeof(BackgroundUpdater), "UpdateInterval.Description")]
        public TimeSpan UpdateInterval {
            get {
                return TimeSpan.FromMilliseconds(_updateTimer.Interval);
            }
            set {
                if (value < TimeSpan.Zero) {
                    throw new ArgumentOutOfRangeException("value");
                }
                _updateTimer.Interval = (int)value.TotalMilliseconds;
            }
        }

        bool ShouldSerializeUpdateInterval() {
            return _updateTimer.Interval != DefaultUpdateInterval;
        }
        void ResetUpdateInterval() {
            _updateTimer.Interval = DefaultUpdateInterval;
        }

        /// <summary>
        ///   Starts update checks in the background.
        /// </summary>
        public void Start() {
            _updateTimer.Start();
        }

        /// <summary>
        ///   Stops checking for updates.
        /// </summary>
        public void Stop() {
            _updateTimer.Stop();
        }

        /// <summary>
        ///   This event is raised whenever a background update is completed.  
        /// </summary>
        [DescriptionResource(typeof(BackgroundUpdater), "UpdateCompleted.Description")]
        public event UpdateCompletedEventHandler UpdateCompleted {
            add { Events.AddHandler(UpdateCompletedEvent, value); }
            remove { Events.RemoveHandler(UpdateCompletedEvent, value); }
        }
        private readonly object UpdateCompletedEvent = new object();

        protected virtual void OnUpdateCompleted(UpdateCompletedEventArgs e) {
            EventHandler handler = (EventHandler)Events[UpdateCompletedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        /// <summary>
        ///   This event is raised whenever a background update progresses.  
        /// </summary>
        [DescriptionResource(typeof(BackgroundUpdater), "UpdateProgressChanged.Description")]
        public event DeploymentProgressChangedEventHandler UpdateProgressChanged {
            add { Events.AddHandler(UpdateProgressChangedEvent, value); }
            remove { Events.RemoveHandler(UpdateProgressChangedEvent, value); }
        }
        private readonly object UpdateProgressChangedEvent = new object();

        protected virtual void OnUpdateProgressChanged(DeploymentProgressChangedEventArgs e) {
            EventHandler handler = (EventHandler)Events[UpdateProgressChangedEvent];
            if (handler != null) {
                handler(this, e);
            }
        }

        public void UpdateNow() {
            _updateTimer.Tick -= updateTimer_Tick;

            // If the app is network deployed, then start a background update.
            if (DesignMode) {
                // nothing to do in design mode
            } else if (ApplicationDeployment.IsNetworkDeployed) {
                // Get the application deployment 
                // Hook up the UpdateCompleted event and the UpdateProgressChanged event
                ApplicationDeployment deployment = ApplicationDeployment.CurrentDeployment;
                deployment.UpdateCompleted += deployment_UpdateCompleted;
                deployment.UpdateProgressChanged += deployment_UpdateProgressChanged;

                // Begin the asynchronous update
                deployment.UpdateAsync();
            } else if (_throwIfNotNetworkDeployed) {
                throw new NotNetworkDeployedException();
            }
        }

        #endregion

        #region Helpers

        private void updateTimer_Tick(object sender, EventArgs e) {
            UpdateNow();
        }

        void deployment_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e) {
            OnUpdateProgressChanged(e);
        }

        void deployment_UpdateCompleted(object sender, AsyncCompletedEventArgs e) {
            /// An update of the application was completed. 
            /// Even though we got an UpdateCompleted event, 
            /// we still need to check the following:
            /// 1. Was a newer version downloaded (UpdatedVersion > CurrentVersion) ?
            /// 2. Was the update cancelled (check e.Cancelled) ?
            /// 3. Was there was an error (check e.Error) ?
            /// Note: the form should handle checking e.Error and e.Cancelled.

            var deployment = (ApplicationDeployment)sender;

            try {
                OnUpdateCompleted(new UpdateCompletedEventArgs(deployment.UpdatedVersion, e.Error, e.Cancelled, e.UserState));
                OnCurrentVersionChanged(EventArgs.Empty);
            }
            finally {
                deployment.UpdateCompleted -= deployment_UpdateCompleted;
                deployment.UpdateProgressChanged -= deployment_UpdateProgressChanged;

                // prevent multiple registering when UpdateNow is being called several times
                _updateTimer.Tick -= updateTimer_Tick;
                _updateTimer.Tick += updateTimer_Tick;
            }
        }

        #endregion
    }

    public delegate void UpdateCompletedEventHandler(object sender, UpdateCompletedEventArgs e);

    /// <summary>
    ///   Feedback about the completion of the update.
    /// </summary>
    public class UpdateCompletedEventArgs : AsyncCompletedEventArgs
    {
        #region Private fields.

        private readonly Version _updatedVersion;

        #endregion

        #region Lifecycle

        /// <summary>
        ///   Initialises a new instance of this object, taking a few pieces
        ///   of information which we will expose to the user.
        /// </summary>
        /// <param name="version">The updated version downloaded.</param>
        /// <param name="error">The error that prevented the download from succeeding, or when all went well</param>
        /// <param name="cancelled">True if the operation was cancelled; otherwise false</param>
        /// <param name="userState">The user state.</param>
        public UpdateCompletedEventArgs(Version version, Exception error, bool cancelled, Object userState)
            : base(error, cancelled, userState) {
            _updatedVersion = version;
        }

        #endregion

        #region Public properties

        /// <summary>
        ///   The updated version of the application that was downloaded.
        /// </summary>
        /// <value>A System.Version object with the newly downloaded version.</value>
        public Version UpdatedVersion {
            get { return _updatedVersion; }
        }

        #endregion
    }

    [Serializable]
    public class NotNetworkDeployedException : InvalidDeploymentException
    {
        public NotNetworkDeployedException()
            : base(ResourceHelpers.GetString(typeof(BackgroundUpdater), "NotNetworkDeployedMessage")) {
        }
        public NotNetworkDeployedException(string message)
            : base(message) {
        }
        public NotNetworkDeployedException(string message, Exception inner)
            : base(message, inner) {
        }
        protected NotNetworkDeployedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context) {
        }
    }
}
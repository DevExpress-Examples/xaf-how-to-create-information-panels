using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.ExpressApp.Win.Controls;
using DevExpress.ExpressApp.Win.SystemModule;
using DevExpress.ExpressApp.Win.Templates;
using DevExpress.ExpressApp.Win.Templates.Utils;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using dxTestSolution.Module.BusinessObjects;
using dxTestSolution.Module.Controllers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace InfoPanel.Win {
    public partial class LightStyleMainRibbonForm1 : RibbonForm, IActionControlsSite, IContextMenuHolder, IWindowTemplate, IDockManagerHolder, IBarManagerHolder, ISupportViewChanged, IXafDocumentsHostWindow, ISupportUpdate, IViewSiteTemplate, ISupportStoreSettings, IViewHolder, ISidePanelHolder, ICustomTemplate {
        private static readonly object viewChanged = new object();
        private static readonly object settingsReloaded = new object();
        private UIType uiType;
        private StatusMessagesHelper statusMessagesHelper;

        protected virtual void InitializeImages() {
            ImageOptionsHelper.AssignImage(barMdiChildrenListItem.ImageOptions, "Action_WindowList");
            ImageOptionsHelper.AssignImage(barSubItemPanels.ImageOptions, "Action_Navigation");
            ImageOptionsHelper.AssignImage(barSubItemNavigationPane.ImageOptions, "NavigationOptions");
        }
        protected virtual void SynchronizeBarAndDockingControllerWithDefault() {
            mainBarAndDockingController.PropertiesBar.ScaleEditors = BarAndDockingController.Default.PropertiesBar.ScaleEditors;
            mainBarAndDockingController.PropertiesRibbon.ScaleEditors = BarAndDockingController.Default.PropertiesRibbon.ScaleEditors;
        }
        protected virtual void OnUITypeChanged() {
            UIType uiType = ((IXafDocumentsHostWindow)this).UIType;
            if (uiType == UIType.TabbedMDI) {
                SetupTabbedMdi();
            } else if (uiType == UIType.StandardMDI) {
                SetupStandardMdi();
            } else {
                SetupSdi();
            }
        }
        protected void SetupSdi() {
            ribbonControl.MdiMergeStyle = RibbonMdiMergeStyle.Never;
            documentManager.View = noDocumentsView;
            documentManager.ViewCollection.Remove(nativeMdiView);
            documentManager.ViewCollection.Remove(tabbedView);
            viewSitePanel.Visible = true;
            documentManager.ClientControl = viewSitePanel;
            barMdiChildrenListItem.Visibility = BarItemVisibility.Never;
        }
        protected void SetupStandardMdi() {
            ribbonControl.MdiMergeStyle = RibbonMdiMergeStyle.OnlyWhenMaximized;
            documentManager.View = nativeMdiView;
            documentManager.ViewCollection.Remove(noDocumentsView);
            documentManager.ViewCollection.Remove(tabbedView);
            SetupMdiCommon();
        }
        protected void SetupTabbedMdi() {
            ribbonControl.MdiMergeStyle = RibbonMdiMergeStyle.Always;
            documentManager.View = tabbedView;
            documentManager.ViewCollection.Remove(noDocumentsView);
            documentManager.ViewCollection.Remove(nativeMdiView);
            SetupMdiCommon();
        }
        private void SetupMdiCommon() {
            viewSitePanel.Visible = false;
            documentManager.MdiParent = this;
            barMdiChildrenListItem.Visibility = BarItemVisibility.Always;
        }

        protected virtual void RaiseViewChanged(DevExpress.ExpressApp.View view) {
            EventHandler<TemplateViewChangedEventArgs> handler = (EventHandler<TemplateViewChangedEventArgs>)Events[viewChanged];
            if (handler != null) {
                handler(this, new TemplateViewChangedEventArgs(view));
            }
        }
        protected virtual void RaiseSettingsReloaded() {
            EventHandler handler = (EventHandler)Events[settingsReloaded];
            if (handler != null) {
                handler(this, EventArgs.Empty);
            }
        }

        protected override FormShowMode ShowMode {
            get { return FormShowMode.AfterInitialization; }
        }

        public LightStyleMainRibbonForm1() {
            InitializeComponent();
            InitializeImages();
            SynchronizeBarAndDockingControllerWithDefault();
            ribbonControl.Manager.ForceLinkCreate();
            statusMessagesHelper = new StatusMessagesHelper(barContainerStatusMessages);
            OnUITypeChanged();
        }

        #region IActionControlsSite Members
        IEnumerable<IActionControlContainer> IActionControlsSite.ActionContainers {
            get {
                List<IActionControlContainer> actionContainers = new List<IActionControlContainer>(ribbonControl.ActionContainers);
                actionContainers.Add(sidePanelActionControlContainer);
                return actionContainers;
            }
        }
        IEnumerable<IActionControl> IActionControlsSite.ActionControls {
            get {
                List<IActionControl> actionControls = new List<IActionControl>(ribbonControl.ActionControls);
                return actionControls;
            }
        }
        IActionControlContainer IActionControlsSite.DefaultContainer {
            get { return barActionContainerDefault; }
        }
        #endregion

        #region IFrameTemplate Members
        void IFrameTemplate.SetView(DevExpress.ExpressApp.View view) {
            viewSiteManager.SetView(view);
            RaiseViewChanged(view);
        }
        ICollection<IActionContainer> IFrameTemplate.GetContainers() {
            return new IActionContainer[] { };
        }
        IActionContainer IFrameTemplate.DefaultContainer {
            get { return null; }
        }
        #endregion

        #region IWindowTemplate Members
        void IWindowTemplate.SetCaption(string caption) {
            ribbonControl.ApplicationCaption = " ";
            ribbonControl.ApplicationDocumentCaption = caption;
        }
        void IWindowTemplate.SetStatus(ICollection<string> statusMessages) {
            statusMessagesHelper.SetMessages(statusMessages);
        }
        bool IWindowTemplate.IsSizeable {
            get { return FormBorderStyle == FormBorderStyle.Sizable; }
            set { FormBorderStyle = (value ? FormBorderStyle.Sizable : FormBorderStyle.FixedDialog); }
        }
        #endregion

        #region IBarManagerHolder Members
        BarManager IBarManagerHolder.BarManager {
            get { return ribbonControl.Manager; }
        }
        event EventHandler IBarManagerHolder.BarManagerChanged {
            add { }
            remove { }
        }
        #endregion

        #region IDockManagerHolder Members
        DevExpress.XtraBars.Docking.DockManager IDockManagerHolder.DockManager {
            get { return dockManager; }
        }
        #endregion

        #region IContextMenuHolder
        PopupMenu IContextMenuHolder.ContextMenu {
            get { return contextMenu; }
        }
        #endregion

        #region ISupportViewChanged Members
        event EventHandler<TemplateViewChangedEventArgs> ISupportViewChanged.ViewChanged {
            add { Events.AddHandler(viewChanged, value); }
            remove { Events.RemoveHandler(viewChanged, value); }
        }
        #endregion

        #region IDocumentsHostWindow Members
        bool IDocumentsHostWindow.DestroyOnRemovingChildren {
            get { return true; }
        }
        DocumentManager IDocumentsHostWindow.DocumentManager {
            get { return documentManager; }
        }
        #endregion

        #region IXafDocumentsHostWindow Members
        UIType IXafDocumentsHostWindow.UIType {
            get { return uiType; }
            set {
                if (uiType != value) {
                    uiType = value;
                    OnUITypeChanged();
                }
            }
        }
        #endregion

        #region ISupportUpdate Members
        void ISupportUpdate.BeginUpdate() {
            if (ribbonControl.Manager != null) {
                ribbonControl.Manager.BeginUpdate();
            }
        }
        void ISupportUpdate.EndUpdate() {
            if (ribbonControl.Manager != null) {
                ribbonControl.Manager.EndUpdate();
            }
        }
        #endregion

        #region IViewSiteTemplate Members
        object IViewSiteTemplate.ViewSiteControl {
            get { return viewSiteManager.ViewSiteControl; }
        }
        #endregion

        #region ISupportStoreSettings Members
        void ISupportStoreSettings.SetSettings(IModelTemplate settings) {
            IModelTemplateWin templateModel = (IModelTemplateWin)settings;
            TemplatesHelper templatesHelper = new TemplatesHelper(templateModel);
            formStateModelSynchronizer.Model = templatesHelper.GetFormStateNode();
            sidePanelActionControlContainer.Model = templatesHelper.GetNavBarCustomizationNode();
            templatesHelper.SetRibbonSettings(ribbonControl);
        }
        void ISupportStoreSettings.ReloadSettings() {
            modelSynchronizationManager.ApplyModel();
            RaiseSettingsReloaded();
        }
        void ISupportStoreSettings.SaveSettings() {
            SuspendLayout();
            try {
                modelSynchronizationManager.SynchronizeModel();
            }
            finally {
                ResumeLayout();
            }
        }

        public void SetCustomString(string _customString) {
            Label label;
            if (this.sidePanel1.Controls.Count == 0) {
                label = new Label();
                label.Width = 1000;
                label.Height = 200;
                this.sidePanel1.Controls.Add(label);
            } else {
                label = (Label)this.sidePanel1.Controls[0];
            }
            label.Text = _customString;
        }

        event EventHandler ISupportStoreSettings.SettingsReloaded {
            add { Events.AddHandler(settingsReloaded, value); }
            remove { Events.RemoveHandler(settingsReloaded, value); }
        }
        #endregion

        #region IViewHolder Members
        DevExpress.ExpressApp.View IViewHolder.View {
            get { return viewSiteManager.View; }
        }
        #endregion

        #region ISidePanelHolder Members
        SidePanel ISidePanelHolder.SidePanel {
            get {
                return sidePanel;
            }
        }
     


        #endregion
    }
}

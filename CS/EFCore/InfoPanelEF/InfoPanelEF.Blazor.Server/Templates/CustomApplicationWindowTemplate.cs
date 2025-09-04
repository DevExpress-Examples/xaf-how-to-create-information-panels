using DevExpress.Blazor;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.ActionControls;
using DevExpress.ExpressApp.Blazor.SystemModule;
using DevExpress.ExpressApp.Blazor.Templates;
using DevExpress.ExpressApp.Blazor.Templates.ContextMenu.ActionControls;
using DevExpress.ExpressApp.Blazor.Templates.Navigation.ActionControls;
using DevExpress.ExpressApp.Blazor.Templates.Security.ActionControls;
using DevExpress.ExpressApp.Blazor.Templates.Toolbar.ActionControls;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.Persistent.Base;
using dxTestSolution.Module.Controllers;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InfoPanelEF.Blazor.Server.Templates {
    public class CustomApplicationWindowTemplate : WindowTemplateBase, IMainFormTemplate, IOptimizeRender, ISupportActionsToolbarVisibility, ISelectionDependencyToolbar, ISupportListEditorInlineActions, ISupportListEditorContextMenuActions, ITemplateToolbarProvider, ITabbedMdiMainFormTemplate, ITabbedMdiMainFormTemplateClosing, ICustomTemplate {
        public CustomApplicationWindowTemplate() : this(null) { }

        public CustomApplicationWindowTemplate(IModelOptionsBlazor modelOptions) {
            ModelOptionsBlazor = modelOptions;

            NavigateBackActionControl = new NavigateBackActionControl();
            AddActionControl(NavigateBackActionControl);
            AccountComponent = new AccountComponentAdapter();
            AddActionControls(AccountComponent.ActionControls);
            ShowNavigationItemActionControl = new ShowNavigationItemActionControl();
            AddActionControl(ShowNavigationItemActionControl);

            HeaderToolbar = new DxToolbarAdapter(new DxToolbarModel() {
                CssClass = "ps-2"
            });
            HeaderToolbar.AddActionContainer(nameof(PredefinedCategory.QuickAccess), ToolbarItemAlignment.Right);
            HeaderToolbar.AddActionContainer(nameof(PredefinedCategory.Notifications), ToolbarItemAlignment.Right);
            HeaderToolbar.AddActionContainer(nameof(PredefinedCategory.Diagnostic), ToolbarItemAlignment.Right);

            if (IsTabbedMdi) {
                HeaderToolbar.AddActionContainer(nameof(PredefinedCategory.Tools), ToolbarItemAlignment.Right);
                ChildTemplates = new ObservableCollection<ITabbedMdiDetailFormTemplate>();
                TabsModel = new DxTabsModel {
                    TabsPosition = ModelOptionsBlazor?.TabPosition ?? TabsPosition.Top,
                    RenderMode = TabsRenderMode.OnDemand,
                    ScrollMode = TabsScrollMode.NavButtons,
                    ActiveTabIndex = ActiveTemplateIndex,
                    ActiveTabIndexChanged = new EventCallback<int>(null, OnActiveTabIndexChanged),
                    CssClass = "xaf-tabbed-mdi h-100",
                    ChildContent = builder => {
                        foreach (var item in ChildTemplates) {
                            item.IsActive = item == ActiveTemplate;
                            builder.AddContent(0, item.TabPageModel.GetComponentContent());
                        }
                    }
                };
            }
            else {
                Toolbar = new DxToolbarAdapter(new DxToolbarModel());
                Toolbar.AddActionContainer(nameof(PredefinedCategory.ObjectsCreation));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.RecordsNavigation), ToolbarItemAlignment.Right);
                Toolbar.AddActionContainer(nameof(PredefinedCategory.SaveOptions), ToolbarItemAlignment.Right, isDropDown: true, defaultActionId: "SaveAndNew", autoChangeDefaultAction: true);
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Save), ToolbarItemAlignment.Right);
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Close));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.UndoRedo));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Edit));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.RecordEdit));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.View));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Reports));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Search));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Filters));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.FullTextSearch));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Tools));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Export));
                Toolbar.AddActionContainer(nameof(PredefinedCategory.Unspecified));

                ListEditorActionColumnAdapter = new ListEditorActionColumnAdapter();
                ListEditorActionColumnAdapter.AddActionContainer(nameof(PredefinedCategory.ListView));
                ListEditorActionColumnAdapter.AddActionContainer(nameof(PredefinedCategory.Edit));
                ListEditorActionColumnAdapter.AddActionContainer(nameof(PredefinedCategory.RecordEdit));

                DxContextMenuAdapter = new DxContextMenuAdapter(new DxContextMenuModel());
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.ObjectsCreation));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Save));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.SaveOptions), isDropDown: true, defaultActionId: "SaveAndNew", autoChangeDefaultAction: true);
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Edit));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.RecordEdit));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.UndoRedo));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Print));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.View));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Export));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Reports));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.OpenObject));
                DxContextMenuAdapter.AddActionContainer(nameof(PredefinedCategory.Menu));
            }
        }

        private int lockRaiseViewChangedEvents;
        private bool containsPendingEvent;
        private string customString;

        protected IModelOptionsBlazor ModelOptionsBlazor { get; set; }
        protected override IEnumerable<IActionControlContainer> GetActionControlContainers() {
            if (IsTabbedMdi) {
                return HeaderToolbar.ActionContainers;
            }
            return Toolbar.ActionContainers.Union(HeaderToolbar.ActionContainers.Union(ListEditorActionColumnAdapter.ActionContainers.Union(DxContextMenuAdapter.ActionContainers)));
        }
        protected override RenderFragment CreateComponent() => CustomApplicationWindowTemplateComponent.Create(this);
        protected override void BeginUpdate() {
            base.BeginUpdate();
            lockRaiseViewChangedEvents++;
            ((ISupportUpdate)Toolbar)?.BeginUpdate();
            ((ISupportUpdate)TabsModel)?.BeginUpdate();
        }
        protected override void EndUpdate() {
            ((ISupportUpdate)Toolbar)?.EndUpdate();
            ((ISupportUpdate)TabsModel)?.EndUpdate();
            base.EndUpdate();
            if (lockRaiseViewChangedEvents > 0) {
                lockRaiseViewChangedEvents--;
                if (lockRaiseViewChangedEvents == 0 && containsPendingEvent) {
                    containsPendingEvent = false;
                    RefreshTabs();
                }
            }
        }
        public void CloseViewTemplate(ITabbedMdiDetailFormTemplate childTemplate) {
            ChildTemplates.Remove(childTemplate);
            TemplateClosed?.Invoke(this, new DetailFormTemplateChangedEventArgs(childTemplate));
        }
        public void TryAddChildTemplate(ITabbedMdiDetailFormTemplate childTemplate) {
            if (childTemplate is null) return;

            if (!ChildTemplates.Contains(childTemplate)) {
                ChildTemplates.Add(childTemplate);
                childTemplate.Activated += ChildTemplate_Activated;
                childTemplate.Closed += ChildTemplate_Closed;
            }
            SetActiveTemplate(childTemplate);
        }

        private void ChildTemplate_Activated(object sender, EventArgs e) {
            var activeTemplate = (ITabbedMdiDetailFormTemplate)sender;
            SetActiveTemplate(activeTemplate);
            ActiveTemplateChanged?.Invoke(this, new DetailFormTemplateChangedEventArgs(activeTemplate));
        }

        private void OnActiveTabIndexChanged(int index) {
            if (index >= 0 && index < ChildTemplates.Count) {
                SetActiveTemplateIndex(index);
                RefreshTabs();
                ActiveTemplateChanged?.Invoke(this, new DetailFormTemplateChangedEventArgs(ChildTemplates[index]));
            }
        }

        private void ChildTemplate_Closed(object sender, EventArgs e) {
            var childTemplate = (ITabbedMdiDetailFormTemplate)sender;
            DetailFormTemplateClosingEventArgs args = new DetailFormTemplateClosingEventArgs(childTemplate);
            Closing?.Invoke(this, args);

            if (args.Cancel) return;

            childTemplate.Closed -= ChildTemplate_Closed;
            childTemplate.Activated -= ChildTemplate_Activated;
            CloseViewTemplate(childTemplate);
        }

        public void RemoveChildTemplate(ITabbedMdiDetailFormTemplate childTemplate) {
            ChildTemplates.Remove(childTemplate);
        }
        public void RefreshTabs() {
            if (lockRaiseViewChangedEvents == 0) {
                ChildTemplatesChanged?.Invoke(this, EventArgs.Empty);
            }
            else {
                containsPendingEvent = true;
            }
        }
        public void SetActiveTemplate(ITabbedMdiDetailFormTemplate childTemplate) {
            var activeIndex = ChildTemplates.IndexOf(childTemplate);
            SetActiveTemplateIndex(activeIndex);
            RefreshTabs();
        }
        public void SetActiveTemplateIndex(int index) {
            ActiveTemplateIndex = index;
            TabsModel.ActiveTabIndex = ActiveTemplateIndex;
        }

        public int ActiveTemplateIndex { get; private set; } = 0;
        public ITabbedMdiDetailFormTemplate ActiveTemplate {
            get {
                if (ActiveTemplateIndex >= 0 && ChildTemplates.Count > ActiveTemplateIndex) {
                    return ChildTemplates[ActiveTemplateIndex];
                }
                return null;
            }
        }

        public NavigateBackActionControl NavigateBackActionControl { get; }
        public AccountComponentAdapter AccountComponent { get; }
        public ShowNavigationItemActionControl ShowNavigationItemActionControl { get; }
        public DxToolbarAdapter Toolbar { get; }
        public DxToolbarAdapter HeaderToolbar { get; }
        public ListEditorActionColumnAdapter ListEditorActionColumnAdapter { get; }
        public DxContextMenuAdapter DxContextMenuAdapter { get; }
        public string AboutInfoString { get; set; }
        public DxTabsModel TabsModel { get; }
        public bool IsTabbedMdi => ModelOptionsBlazor?.UIType == UIType.TabbedMDI;
        protected ObservableCollection<ITabbedMdiDetailFormTemplate> ChildTemplates { get; private set; }
        public event EventHandler ChildTemplatesChanged;
        public event EventHandler<DetailFormTemplateChangedEventArgs> ActiveTemplateChanged;
        public event EventHandler<DetailFormTemplateChangedEventArgs> TemplateClosed;
        public event EventHandler<DetailFormTemplateClosingEventArgs> Closing;

        void ISupportActionsToolbarVisibility.SetVisible(bool isVisible) {
            if (Toolbar is not null) {
                Toolbar.Visible = isVisible;
            }
        }
        public event EventHandler CustomStringChanged;
        public string CustomString {
            get => customString;
            set {
                if (customString != value) {
                    customString = value;
                    CustomStringChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void SetCustomString(string _customString) {
            CustomString = _customString;
        }
    }
}
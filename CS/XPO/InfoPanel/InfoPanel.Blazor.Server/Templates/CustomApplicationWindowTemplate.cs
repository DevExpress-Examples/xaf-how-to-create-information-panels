using DevExpress.CodeParser.VB;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Templates;
using DevExpress.ExpressApp.Blazor.Templates.Navigation.ActionControls;
using DevExpress.ExpressApp.Blazor.Templates.Security.ActionControls;
using DevExpress.ExpressApp.Blazor.Templates.Toolbar.ActionControls;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Templates.ActionControls;
using DevExpress.Persistent.Base;
using dxTestSolution.Module.Controllers;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace InfoPanel.Blazor.Server.Templates {
    public class CustomApplicationWindowTemplate : WindowTemplateBase, ISupportActionsToolbarVisibility, ISelectionDependencyToolbar, ICustomTemplate, INotifyPropertyChanged {
        private string customString;

        public CustomApplicationWindowTemplate() {
            NavigateBackActionControl = new NavigateBackActionControl();
            AddActionControl(NavigateBackActionControl);
            AccountComponent = new AccountComponentAdapter();
            AddActionControls(AccountComponent.ActionControls);
            ShowNavigationItemActionControl = new ShowNavigationItemActionControl();
            AddActionControl(ShowNavigationItemActionControl);

            IsActionsToolbarVisible = true;
            Toolbar = new DxToolbarAdapter(new DxToolbarModel());
            Toolbar.AddActionContainer(nameof(PredefinedCategory.ObjectsCreation));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Save));
            Toolbar.AddActionContainer("Close");
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Export));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.UndoRedo));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Edit));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.RecordEdit));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.RecordsNavigation));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.View));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Reports));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Search));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Filters));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.FullTextSearch));
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Tools));
            Toolbar.AddActionContainer("Diagnostic");
            Toolbar.AddActionContainer(nameof(PredefinedCategory.Unspecified));
        }
        protected override IEnumerable<IActionControlContainer> GetActionControlContainers() => Toolbar.ActionContainers;
        protected override RenderFragment CreateComponent() => CustomApplicationWindowTemplateComponent.Create(this);
        protected override void BeginUpdate() {
            base.BeginUpdate();
            ((ISupportUpdate)Toolbar).BeginUpdate();
        }
        protected override void EndUpdate() {
            ((ISupportUpdate)Toolbar).EndUpdate();
            base.EndUpdate();

        }
        public bool IsActionsToolbarVisible { get; private set; }
        public NavigateBackActionControl NavigateBackActionControl { get; }
        public AccountComponentAdapter AccountComponent { get; }
        public ShowNavigationItemActionControl ShowNavigationItemActionControl { get; }
        public DxToolbarAdapter Toolbar { get; }
        public string AboutInfoString { get; set; }

        public string CustomString { get => customString; set { customString = value; OnPropertyChanged(); } }

        void ISupportActionsToolbarVisibility.SetVisible(bool isVisible) => IsActionsToolbarVisible = isVisible;


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetCustomString(string _customString) {
            CustomString = _customString;
        }
    }
}

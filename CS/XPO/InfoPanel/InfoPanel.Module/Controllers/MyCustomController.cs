using DevExpress.ExpressApp;
using dxTestSolution.Module.BusinessObjects;

namespace dxTestSolution.Module.Controllers {

    public interface ICustomTemplate {
        void SetCustomString(string _customString);
    }

    public abstract class MyCustomController : ObjectViewController<ListView, Contact> {
        protected override void OnActivated() {
            base.OnActivated();
            this.View.SelectionChanged += View_SelectionChanged;
        }
       public abstract ICustomTemplate GetTemplate();

        private void View_SelectionChanged(object sender, EventArgs e) {
            var infoPanelTemplate = GetTemplate();
            var selectedObjectNames = this.View.SelectedObjects.Cast<Contact>().Select(x => x.FirstName).ToList();
            var finalString = string.Join(";", selectedObjectNames);
            infoPanelTemplate.SetCustomString(finalString);
        }


    }
}

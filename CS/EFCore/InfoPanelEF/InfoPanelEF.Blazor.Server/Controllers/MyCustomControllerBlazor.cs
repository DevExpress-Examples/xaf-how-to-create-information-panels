using dxTestSolution.Module.Controllers;

namespace InfoPanel.Blazor.Server.Controllers {
    public class MyCustomControllerBlazor : MyCustomController {
        public override ICustomTemplate GetTemplate() {
            return this.Frame.Template as ICustomTemplate;
        }
    }
}

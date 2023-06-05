using DevExpress.ExpressApp.Win;
using dxTestSolution.Module.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoPanel.Win.Controllers {
    public class MyCustomControllerWin : MyCustomController {
        public override ICustomTemplate GetTemplate() {
            return ((WinWindow)this.Application.MainWindow).Form as ICustomTemplate;
        }
    }
}

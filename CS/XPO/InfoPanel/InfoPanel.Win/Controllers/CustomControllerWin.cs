using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;


using System.IO;
using System.ComponentModel;
using DevExpress.XtraEditors;
using dxTestSolution.Module.BusinessObjects;
using DevExpress.ExpressApp.Win;

namespace dxTestSolution.Module.Controllers {
    public interface IInfoPanelTemplateWin : IFrameTemplate {
        SidePanel SidePanel {
            get;
        }
    }

    public class CustomWinController : ObjectViewController<ListView, Contact> {
        protected override void OnActivated() {
            base.OnActivated();
            this.View.CurrentObjectChanged += View_CurrentObjectChanged;
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e) {
            var infoPanelTemplate = ((WinWindow)this.Application.MainWindow).Form as IInfoPanelTemplateWin;
            Label label;
            if (infoPanelTemplate.SidePanel.Controls.Count == 0) {
                label = new Label();
                label.Width = 1000;
                label.Height = 200;
                infoPanelTemplate.SidePanel.Controls.Add(label);
            } else {
                label = (Label)infoPanelTemplate.SidePanel.Controls[0];
            }
            label.Text = "The current Object is " + ((Contact)View.CurrentObject).FirstName;
        }


    }
}

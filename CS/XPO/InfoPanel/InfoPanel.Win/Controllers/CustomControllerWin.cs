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

    public class InfoPanelViewControllerWin : CustomizeTemplateViewControllerBase<IInfoPanelTemplateWin> {
        LabelControl label;
        protected override void AddControlsToTemplateCore(IInfoPanelTemplateWin template) {
            if (label == null) {
                label = new LabelControl();
            }
            template.SidePanel.Controls.Add(label);
        }
        protected override void RemoveControlsFromTemplateCore(IInfoPanelTemplateWin template) {
            template.SidePanel.Controls.Remove(label);
            label = null;
        }
        protected override void UpdateControls(DevExpress.ExpressApp.View view) {
            UpdateControls();
        }
        protected override void UpdateControls(object currentObject) {
            UpdateControls();
        }

              void UpdateControls() {
            label.Text = "The current View is " + View.Caption;
            if (View.CurrentObject != null) {
                label.Text += System.Environment.NewLine +
                    "The current object is " + View.CurrentObject.ToString();
            }
        }
    }

    public class CustomWinController : ObjectViewController<ListView,Contact> {
    //public class CustomWinController : ViewController {
        public CustomWinController() {
            var myAction1 = new SimpleAction(this, "MyWinAction1", null);
            myAction1.Execute += MyAction1_Execute;
            
        }

        private void MyAction1_Execute(object sender, SimpleActionExecuteEventArgs e) {
            //var os = Application.CreateObjectSpace(typeof(MyNonPersistentClass));
            //var obj = os.CreateObject<MyNonPersistentClass>();
            //var detailView = Application.CreateDetailView(os, obj);

            var tst = this.Frame as IInfoPanelTemplateWin;
            var t2=((WinWindow)this.Application.MainWindow).Form as IInfoPanelTemplateWin;
            var label = new Label();
            label.Width= 1000;
            label.Height = 200;
            label.Text = "The current View is " + View.Caption;
            t2.SidePanel.Controls.Add(label);
        }

        protected override void OnActivated() {
            base.OnActivated();
            var cnt = Frame.GetController<NewObjectViewController>();
            if(cnt != null) {

            }
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated() {
            base.OnDeactivated();
        }
    }
}

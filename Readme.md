<!-- default badges list -->
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E2690)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# XAF - How to: Create information panels

In certain scenarios, you may need to create additional functional areas in a UI to display specific data. You can solve this task using additional controls in XAF templates and by implementing the code that supplies data to the controls.

<kbd>![image](https://github.com/DevExpress-Examples/XAF_how-to-create-information-panels-e2690/assets/14300209/8d289a08-7952-419e-89e4-8aaf05fa6b2d)</kbd>

## Implementation Details

To accomplish this task, follow the steps below: 

1. Create a custom template as described in the following topic: [Templates](https://docs.devexpress.com/eXpressAppFramework/112609/ui-construction/templates).
2. Add to this custom template a UI element that should display the additional information (e.g. [div](https://www.w3schools.com/tags/tag_div.ASP)).
3. To pass data to this element, use a dedicated [ViewController](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController) that accesses the template and passes required information to this template.

> [!WARNING]
> We created this example for demonstration purposes and didn't intend to address all possible usage scenarios with it.
> If this example doesn't have some functionality or you wish to change its behavior, feel free to extend this example as you need. Please note that this is quite a complex task, that requires good knowledge of XAF: [UI Customization Categories by Skill Level](https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml#ui-customization-categories). You will likely need to research how our components work under the hood. Refer to the following help topic for more information: [Debug DevExpress .NET Source Code with PDB Symbols](https://docs.devexpress.com/GeneralInformation/403656/support-debug-troubleshooting/debug-controls-with-debug-symbols).
> Unfortunately, we can't help you much with such tasks as custom programming goes beyond our Support Service scope: [Technical Support Scope](https://www.devexpress.com/products/net/application_framework/xaf-considerations-for-newcomers.xml#support).

## Files to Review

- [MyCustomController.cs](CS/EFCore/InfoPanelEF/InfoPanelEF.Module/Controllers/MyCustomController.cs) 
- [MyCustomControllerBlazor.cs](CS/EFCore/InfoPanelEF/InfoPanelEF.Blazor.Server/Controllers/MyCustomControllerBlazor.cs) 
- [CustomApplicationWindowTemplate.cs](CS/EFCore/InfoPanelEF/InfoPanelEF.Blazor.Server/Templates/CustomApplicationWindowTemplate.cs) 
- [CustomApplicationWindowTemplateComponent.razor](CS/EFCore/InfoPanelEF/InfoPanelEF.Blazor.Server/Templates/CustomApplicationWindowTemplateComponent.razor)

## Documenation

- [Templates](https://docs.devexpress.com/eXpressAppFramework/112609/ui-construction/templates)
- [How to: Create a Custom WinForms Ribbon Template](https://docs.devexpress.com/eXpressAppFramework/112618/ui-construction/templates/in-winforms/how-to-create-a-custom-winforms-ribbon-template)

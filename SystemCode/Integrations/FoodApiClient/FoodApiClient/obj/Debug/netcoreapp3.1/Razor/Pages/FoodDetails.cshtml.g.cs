#pragma checksum "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c649189bc696d0d61cb82c8d4f06c27dd2de9664"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(FoodApiClient.Pages.Pages_FoodDetails), @"mvc.1.0.razor-page", @"/Pages/FoodDetails.cshtml")]
namespace FoodApiClient.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\_ViewImports.cshtml"
using FoodApiClient;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c649189bc696d0d61cb82c8d4f06c27dd2de9664", @"/Pages/FoodDetails.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4e936680d648c5044e6a30e89692716a226b5f56", @"/Pages/_ViewImports.cshtml")]
    public class Pages_FoodDetails : global::Microsoft.AspNetCore.Mvc.RazorPages.Page
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "./FoodList", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 3 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
  
    ViewData["Title"] = "FoodDetails";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Food Details</h1>\r\n<div>\r\n    <h4>Movie</h4>\r\n    <hr />\r\n    <dl class=\"row\">\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 13 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayNameFor(model => model.Food.ID));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 16 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.ID));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 19 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayNameFor(model => model.Food.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 22 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 25 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayNameFor(model => model.Food.Group));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 28 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.Group));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 31 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayNameFor(model => model.Food.Proteins));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 34 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.Proteins));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 35 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.ProteinUnit));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n        <dt class=\"col-sm-2\">\r\n            ");
#nullable restore
#line 38 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayNameFor(model => model.Food.Sugar));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dt>\r\n        <dd class=\"col-sm-10\">\r\n            ");
#nullable restore
#line 41 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.Sugar));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            ");
#nullable restore
#line 42 "d:\Documents\source\repos\FoodApiClient\FoodApiClient\Pages\FoodDetails.cshtml"
       Write(Html.DisplayFor(model => model.Food.SugarUnit));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </dd>\r\n    </dl>\r\n</div>\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c649189bc696d0d61cb82c8d4f06c27dd2de96647281", async() => {
                WriteLiteral("Back to List");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<FoodApiClient.Pages.FoodDetailsModel> Html { get; private set; }
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<FoodApiClient.Pages.FoodDetailsModel> ViewData => (global::Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<FoodApiClient.Pages.FoodDetailsModel>)PageContext?.ViewData;
        public FoodApiClient.Pages.FoodDetailsModel Model => ViewData.Model;
    }
}
#pragma warning restore 1591

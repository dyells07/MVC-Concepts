using Microsoft.AspNetCore.Razor.TagHelpers;

namespace ExamQ.TagHelpers
{
    public class PhoneTagHelper : TagHelper
    {
       
        public string Tel { get; set; }

        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("href", "tel:" + Tel);
            output.Content.SetContent(Tel);
        }
    }
}
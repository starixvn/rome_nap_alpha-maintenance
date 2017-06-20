namespace Alpha.Maintenance.Site
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;

    public static class RazorExtensions
    {
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.GetHashCode().ToString(),
                    Selected = (value.Equals(selectedValue))
                };

            return htmlHelper.DropDownList(
                name,
                items
                );
        }

        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, TEnum selectedValue, IDictionary<string, object> htmlAttributes)
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.GetHashCode().ToString(),
                    Selected = (value.Equals(selectedValue))
                };

            return htmlHelper.DropDownList(
                name,
                items,
                htmlAttributes);
        }
    }
}
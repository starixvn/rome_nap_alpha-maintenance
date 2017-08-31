namespace Alpha.Maintenance.Core.Helpers
{
    using System;
    using Announcements.Models;
    using Announcements.Specs;
    using Newtonsoft.Json.Linq;

    public class SpecificationConverter : JsonCreationConverter<ISpecification<IParam>>
    {
        protected override Type GetTargetType(Type type, JObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");
            }

            return Type.GetType(obj.GetValue("TypeName", StringComparison.OrdinalIgnoreCase).ToString());
        }
    }
}
using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using Etutor.Core;
using Etutor.DataModel.Context;
using Etutor.DataModel.Entities;
using System;
using System.Linq;
using System.Reflection;

namespace Etutor.BL.Setup
{
    public static class ODataSetup
    {
        public static IEdmModel GetEdmModel(IServiceProvider service)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder(service);

            builder.EnableLowerCamelCase();

            var dtoListTypes = Assembly.GetAssembly(typeof(ApplicationDbContext)).GetTypes()
                    .Where(type => typeof(IEntityBase).IsAssignableFrom(type) && type.IsClass);

            foreach (var type in dtoListTypes)
            {
                var entitySetConfiguration = builder.AddEntitySet(type.Name, builder.AddEntityType(type));

                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("CreatedAt"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("ModifiedAt"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("CreatedBy"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("ModifiedBy"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("IsDeleted"));
                if (type == typeof(User))
                {
                    entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("PasswordHash"));
                    entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("SecurityStamp"));
                    entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("ConcurrencyStamp"));
                }
            }

            return builder.GetEdmModel();
        }
    }
}

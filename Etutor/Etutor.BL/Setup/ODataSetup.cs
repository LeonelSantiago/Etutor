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

                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("FechaCreacion"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("FechaModificacion"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("CreadoPor"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("EditadoPor"));
                entitySetConfiguration.EntityType.RemoveProperty(type.GetProperty("Borrado"));
                if (type == typeof(Usuario))
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

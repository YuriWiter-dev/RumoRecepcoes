using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTI.Pedidos.Filters;

public class SwaggerJsonIgnoreFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        List<PropertyInfo> ignoredProperties = context.MethodInfo.GetParameters()
            .SelectMany(p => p.ParameterType.GetProperties()
            .Where(prop => prop.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null))
            .ToList();

        if (!ignoredProperties.Any())
            return;

        foreach (PropertyInfo property in ignoredProperties)
        {
            operation.Parameters = operation.Parameters
                .Where(p => (!p.Name.Equals(property.Name, StringComparison.InvariantCulture)))
                .ToList();
        }
    }
}

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog.Core;
using Serilog.Events;
using Taskio.Common.Options;

namespace Taskio.Common.Extensions;

public class SensitiveLoggerPolicy(IOptions<SensitiveLoggerOptions> options)
    : IDestructuringPolicy
{
    private readonly IOptions<SensitiveLoggerOptions> _options = options;

    public bool TryDestructure(object value,
                               ILogEventPropertyValueFactory propertyValueFactory,
                               [NotNullWhen(true)] out LogEventPropertyValue? result)
    {
        if (_options.Value.NameTemplates.Count == 0)
        {
            result = new StructureValue([]);
            return false;
        }

        IEnumerable<PropertyInfo> props = value
            .GetType()
            .GetTypeInfo()
            .DeclaredProperties;

        List<LogEventProperty> logEventProperties = [];

        foreach (var propertyInfo in props)
        {
            LogEventProperty checkedEventProperty;

            if (_options.Value.NameTemplates
                .Exists(template => propertyInfo.Name
                            .Contains(template,
                                      StringComparison.InvariantCultureIgnoreCase)))
            {
                checkedEventProperty = new LogEventProperty(propertyInfo.Name,
                                                            propertyValueFactory.CreatePropertyValue(
                                                                _options.Value.MaskPlaceholder));
            }
            else
            {
                checkedEventProperty = new LogEventProperty(propertyInfo.Name,
                                                            propertyValueFactory.CreatePropertyValue(
                                                                propertyInfo.GetValue(value)));
            }

            logEventProperties.Add(checkedEventProperty);
        }

        result = new StructureValue(logEventProperties);
        return true;
    }
}

using System.ComponentModel;
using System.Reflection;

namespace Game.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescriptionString(this CellType value)
    {
        FieldInfo? field = value
            .GetType()
            .GetField(value.ToString());

        if (field == null)
            return string.Empty;

        var attribute = field.GetCustomAttribute<DescriptionAttribute>(false);

        if (attribute == null)
            return string.Empty;

        return attribute.Description;
    }
}

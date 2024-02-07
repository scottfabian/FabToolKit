using System.Reflection;

namespace FabToolKit.Testing;

public static class TestHelper
{
    public static bool AreObjectsValuesEquals<T>(T obj1, T obj2)
    {
        // If both objects are null, or they are the same instance, return true
        if (Object.ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        // If either object is null, return false
        if (obj1 == null || obj2 == null)
        {
            return false;
        }

        // Get the type of the objects
        Type type = typeof(T);

        // Compare each property
        foreach (PropertyInfo property in type.GetProperties())
        {
            object obj1Value = property.GetValue(obj1);
            object obj2Value = property.GetValue(obj2);

            if (obj1Value == null && obj2Value == null)
            {
                continue;
            }

            if ((obj1Value == null) != (obj2Value == null))
            {
                return false;
            }

            // If the property type is a class and not a primitive type, call this method recursively
            if (property.PropertyType.IsClass && !property.PropertyType.Equals(typeof(string)))
            {
                if (!AreObjectsValuesEquals(obj1Value, obj2Value))
                {
                    return false;
                }
            }
            else if (!obj1Value.Equals(obj2Value))
            {
                return false;
            }
        }

        return true;
    }
}

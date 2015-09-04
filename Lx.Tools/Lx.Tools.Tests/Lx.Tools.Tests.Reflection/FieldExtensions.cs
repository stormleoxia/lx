using System;
using System.Linq;
using System.Reflection;

namespace Lx.Tools.Tests.Reflection
{
    public static class FieldExtensions
    {
        public static void SetField<T>(this object current, string fieldName, T value)
        {
            var fields = current.GetType().GetRuntimeFields();
            var fieldInfo = fields.FirstOrDefault(x => x.Name == fieldName);
            if (fieldInfo == null)
            {
                throw new ArgumentException("No field found.", "fieldName");
            }
            fieldInfo.SetValue(current, value);
        }
    }
}
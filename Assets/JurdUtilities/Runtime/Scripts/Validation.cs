using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace Jurd.Utilities
{
    [System.Flags]
    public enum ValidationFieldFlags
    {
        None = 0,
        Public = 1,
        SerializeFieldAttribute = 2,
        WarnOnNullAttribute = 4,
    }

    public class WarnOnNull : PropertyAttribute { }

    public static class Validation
    {
        /// <summary>
        /// Validates serialized and public fields within the object and logs an error for null values
        /// </summary>
        /// <remarks>
        /// USE <see cref="CheckFields(Object, ValidationFieldFlags, bool)"/> INSTEAD if possible.
        /// </remarks>
        /// <param name="validateFields">        
        /// can concatenate <see cref="ValidationFieldFlags"/> using bitwise OR "|"
        /// </param>
        /// <returns>
        /// True if all objects validated successfully
        /// </returns>
        public static bool CheckFields(object obj, ValidationFieldFlags validateFields = ValidationFieldFlags.WarnOnNullAttribute, bool outputOnError = true)
        {
            bool validatedSuccessfully = true;

            System.Type type = obj.GetType();
            FieldInfo[] fieldInfos = type
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(field => 
                    (
                        //TODO: maybe write a better way to do this like combining into a predicate or smth
                        CheckFieldFlag(field, validateFields, ValidationFieldFlags.Public)
                        || CheckFieldFlag(field, validateFields, ValidationFieldFlags.SerializeFieldAttribute)
                        || CheckFieldFlag(field, validateFields, ValidationFieldFlags.WarnOnNullAttribute)
                    )
                )
                .ToArray();

            foreach (var field in fieldInfos)
            {
                object value = field.GetValue(obj);

                if (value == null)
                {
                    if (outputOnError)
                    {
                        // TODO: throw error here?
                        string output = $"Field: {field.Name} is null in script: {type.Name} on object: {obj}!";
                        if (obj as UnityEngine.Object)
                        {
                            Debug.LogError(output, obj as UnityEngine.Object);
                        }
                        else
                        {
                            Debug.LogError(output);
                        }
                    }
                    validatedSuccessfully = false;
                }
            }

            return validatedSuccessfully;
        }

        private static bool CheckFieldFlag(FieldInfo field, ValidationFieldFlags setFlags, ValidationFieldFlags targetFlag)
        {
            if ((setFlags & targetFlag) != 0)
            {
                //Debug.Log($"field check for flag: {targetFlag}");
                switch(targetFlag)
                {
                    case ValidationFieldFlags.Public:
                        return field.IsPublic;

                    case ValidationFieldFlags.SerializeFieldAttribute:   
                        return field.IsDefined(typeof(SerializeField));

                    case ValidationFieldFlags.WarnOnNullAttribute:   
                        return field.IsDefined(typeof(WarnOnNull));

                    case ValidationFieldFlags.None:
                        break;

                    default:
                        Debug.LogError($"Error validating field {field}: Validation flag {targetFlag} not implemented!");
                        break;
                }
            }
            return false;
        }
    }
}
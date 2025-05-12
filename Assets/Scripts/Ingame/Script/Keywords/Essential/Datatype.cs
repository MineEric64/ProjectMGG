using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public class Datatype
    {
        public static bool IsTrue(object value)
        {
            return value is bool value2 && value2;
        }

        public static bool IsFalse(object value)
        {
            return value is bool value2 && !value2;
        }

        public static bool IsArray(object value)
        {
            return value is Array;
        }

        public static Array ToArray(object value)
        {
            return (Array)value;
        }

        public static object GetValueOfArray(object obj, object index)
        {
            int i = (int)index;
            Array arr = ToArray(obj);

            if (i >= 0 && i < arr.Length)
            {
                return arr.GetValue(i);
            }
            return null;
        }

        public static object SetValueOfArray(object obj, object index, object value)
        {
            int i = (int)index;
            Array arr = ToArray(obj);

            if (i >= 0 && i < arr.Length)
            {
                arr.SetValue(value, i);
            }
            return value;
        }

        public static bool IsMap(object value)
        {
            return value is Dictionary<string, object>;
        }

        public static Dictionary<string, object> ToMap(object value)
        {
            return (Dictionary<string, object>)value;
        }

        public static object GetValueOfMap(object obj, object key)
        {
            var map = (Dictionary<string, object>)obj;
            var key2 = key.ToString();

            if (map.ContainsKey(key2))
            {
                return map[key2];
            }
            return null;
        }

        public static object SetValueOfMap(object obj, object key, object value)
        {
            ((Dictionary<string, object>)obj)[key.ToString()] = value;
            return value;
        }

        public static bool IsFunction(object value)
        {
            return value is Function;
        }

        public static Function ToFunction(object value)
        {
            return (Function)value;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datatype
{
    public static bool IsTrue(object value)
    {
        return IsBoolean(value) && ToBoolean(value);
    }

    public static bool IsFalse(object value)
    {
        return IsBoolean(value) && (ToBoolean(value) == false);
    }

    public static bool IsBoolean(object value)
    {
        return value is bool;
    }

    public static bool ToBoolean(object value)
    {
        return (bool)value;
    }

    public static bool IsNumber(object value)
    {
        return value is double;
    }

    public static double ToNumber(object value)
    {
        return (double)value;
    }

    public static bool IsString(object value)
    {
        return value is string;
    }

    public static string ToString(object value)
    {
        return (string)value;
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
        int i = (int)ToNumber(index);
        Array arr = ToArray(obj);

        if (i >= 0 && i < arr.Length)
        {
            return arr.GetValue(i);
        }
        return null;
    }

    public static object SetValueOfArray(object obj, object index, object value)
    {
        int i = (int)ToNumber(index);
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
        var map = ToMap(obj);
        var key2 = ToString(key);

        if (map.ContainsKey(key2))
        {
            return map[key2];
        }
        return null;
    }

    public static object SetValueOfMap(object obj, object key, object value)
    {
        ToMap(obj)[ToString(key)] = value;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RpyTransform = transform;

public class VariableCollection
{
    public Dictionary<string, Character> Characters  = new Dictionary<string, Character>();
    public Dictionary<string, RpyAttributes> Images  = new Dictionary<string, RpyAttributes>();
    public Dictionary<string, RpyTransform> Transforms  = new Dictionary<string, RpyTransform>();
    public Dictionary<string, object> Others = new Dictionary<string, object>();

    public VariableCollection()
    {

    }
}
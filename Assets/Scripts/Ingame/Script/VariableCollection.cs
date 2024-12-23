using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RpyTransform = transform;

public class VariableCollection
{
    public Dictionary<string, Character> Characters { get; private set; } = new Dictionary<string, Character>();
    public Dictionary<string, string> Images { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, RpyTransform> Transforms { get; private set; } = new Dictionary<string, RpyTransform>();
    public Dictionary<string, string> Strings { get; private set; } = new Dictionary<string, string>();
    public Dictionary<string, int> Numbers { get; private set; } = new Dictionary<string, int>();
    //TODO: 렌파이에서 숫자를 암시적으로 문자열로 변환해주면 그냥 object로 하고 아니면 걍 이렇게 ㄱ (사실 제일 편한 거 하는 게 맞음) 나도 몰라 tlsqkf

    public VariableCollection()
    {

    }
}

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
    //TODO: �����̿��� ���ڸ� �Ͻ������� ���ڿ��� ��ȯ���ָ� �׳� object�� �ϰ� �ƴϸ� �� �̷��� �� (��� ���� ���� �� �ϴ� �� ����) ���� ���� tlsqkf

    public VariableCollection()
    {

    }
}

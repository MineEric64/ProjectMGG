using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectMGG.Ingame.Script.Keywords
{
    public enum CharType
    {
        Unknown,                    // ����� �� ���� ����
        WhiteSpace,                 // ����, ��, ����
        NumberLiteral,              // ���� ���ͷ�
        StringLiteral,              // ���ڿ� ���ͷ�
        IdentifierAndKeyword,       // �ĺ���, Ű����
        OperatorAndPunctuator,      // ������, ������
        Comment //�ּ�
    }
}
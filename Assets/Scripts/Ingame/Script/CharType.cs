using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
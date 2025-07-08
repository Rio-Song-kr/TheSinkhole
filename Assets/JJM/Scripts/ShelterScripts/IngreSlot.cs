using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngreSlot : MonoBehaviour
{
    public Image iconImage; // ��� ������ �̹��� �ֱ�
    public TextMeshProUGUI nameText; // ��� �̸� �ؽ�Ʈ
    public TextMeshProUGUI quantityText; // ��� ���� �ؽ�Ʈ

    public void Set(Sprite icon,string name, int have, int need)
    {
        iconImage.sprite = icon; // ��� ������ ����
        nameText.text = name; // ��� �̸� ����
        quantityText.text = $"{have} / {need}"; // �ʿ����/������� ���� ����
        quantityText.color = have >= need ? Color.white : Color.red; // ������ ���� ���� ����
    }
}

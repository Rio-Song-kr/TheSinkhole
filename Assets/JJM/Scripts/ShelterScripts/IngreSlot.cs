using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngreSlot : MonoBehaviour
{
    public Image iconImage; // 재료 아이콘 이미지 넣기
    public TextMeshProUGUI nameText; // 재료 이름 텍스트
    public TextMeshProUGUI quantityText; // 재료 수량 텍스트

    public void Set(Sprite icon,string name, int have, int need)
    {
        iconImage.sprite = icon; // 재료 아이콘 설정
        nameText.text = name; // 재료 이름 설정
        quantityText.text = $"{have} / {need}"; // 필요재료/소지재료 수량 설정
        quantityText.color = have >= need ? Color.white : Color.red; // 수량에 따라 색상 변경
    }
}

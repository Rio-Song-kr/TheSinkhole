using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUIManager : MonoBehaviour
{
    private PopupInfoUI m_popupInfoUI;
    public PopupInfoUI Popup => m_popupInfoUI;

    public void SetPopupInfoUI(PopupInfoUI popupInfoUI) => m_popupInfoUI = popupInfoUI;
}
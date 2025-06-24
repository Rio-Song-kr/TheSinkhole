using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class StatusUITest : MonoBehaviour
{
    [Header("Text UI")]
    [SerializeField] TextMeshProUGUI curHealthUIText;
    [SerializeField] TextMeshProUGUI curHungerUIText;
    [SerializeField] TextMeshProUGUI curThirstUIText;
    [SerializeField] TextMeshProUGUI curMentalityUIText;
    [Header("Slider UI")]
    [SerializeField] Slider sliderTest;
    [SerializeField] GameObject sliderFill;

    [Header("Image UI")]
    [SerializeField] Image imageTest;
    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        curHealthUIText.text = PlayerStatus.Instance.CurHealth.ToString();
        curHungerUIText.text = PlayerStatus.Instance.CurHunger.ToString();
        curThirstUIText.text = PlayerStatus.Instance.CurThirst.ToString();
        curMentalityUIText.text = PlayerStatus.Instance.CurMentality.ToString();
        StatusSlider();
    }

    void StatusSlider()
    {
        float value = PlayerStatus.Instance.CurHunger / PlayerStatus.Instance.MaxHunger;
        if (value <= 0) sliderFill.SetActive(false);
        else sliderFill.SetActive(true);
        sliderTest.value = value;
    }
}

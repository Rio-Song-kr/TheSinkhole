using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatusView : MonoBehaviour
{
    [Header("Player Health")]
    [SerializeField] private Image m_healthBarImage;
    [SerializeField] private TextMeshProUGUI m_healthBarText;

    [Header("Player Other Status")]
    [SerializeField] private Image m_hungerOverlayImage;
    [SerializeField] private Image m_thirstyOverlayImage;
    [SerializeField] private Image m_mentalityOverlayImage;

    public void SetHealthBarFillAmount(float amount) => m_healthBarImage.fillAmount = amount;
    public float GetHealthBarFillAmount() => m_healthBarImage.fillAmount;
    public void SetHealthText(string amount) => m_healthBarText.text = amount;

    public void SetHungerOverlayFillAmount(float amount) => m_hungerOverlayImage.fillAmount = amount;
    public float GetHungerOverlayFillAmount() => m_hungerOverlayImage.fillAmount;
    public void SetHungerOverlayColor(Color color) => m_hungerOverlayImage.color = color;
    public Color GetHungerOverlayColor() => m_hungerOverlayImage.color;

    public void SetThirstyOverlayFillAmount(float amount) => m_thirstyOverlayImage.fillAmount = amount;
    public float GetThirstyOverlayFillAmount() => m_thirstyOverlayImage.fillAmount;
    public void SetThirstyOverlayColor(Color color) => m_thirstyOverlayImage.color = color;
    public Color GetThirstyOverlayColor() => m_thirstyOverlayImage.color;

    public void SetMentalityOverlayFillAmount(float amount) => m_mentalityOverlayImage.fillAmount = amount;
    public float GetMentalityOverlayFillAmount() => m_mentalityOverlayImage.fillAmount;
    public void SetMentalityOverlayColor(Color color) => m_mentalityOverlayImage.color = color;
    public Color GetMentalityOverlayColor() => m_mentalityOverlayImage.color;
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusPresenter : MonoBehaviour
{
    private PlayerStatusView m_playerStatusView;
    private PlayerStatus m_playerStatus;

    private void Awake() => m_playerStatusView = GetComponent<PlayerStatusView>();

    private void Start() => m_playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

    private void Update()
    {
        UpdateHealthBar();
        UpdateHungerOverlay();
        UpdateThirstyOverlay();
        UpdateMentalityOverlay();
    }

    private void UpdateHealthBar()
    {
        float fillAmount = m_playerStatus.CurHealth / m_playerStatus.MaxHealth;
        if (fillAmount == m_playerStatusView.GetHealthBarFillAmount()) return;

        m_playerStatusView.SetHealthBarFillAmount(fillAmount);
        m_playerStatusView.SetHealthText($"{(int)m_playerStatus.CurHealth} / {m_playerStatus.MaxHealth}");
    }

    private void UpdateHungerOverlay()
    {
        float fillAmount = m_playerStatus.CurHunger / m_playerStatus.MaxHealth;
        if (fillAmount == m_playerStatusView.GetHungerOverlayFillAmount()) return;

        m_playerStatusView.SetHungerOverlayFillAmount(fillAmount);

        if (fillAmount > 0.35f && m_playerStatusView.GetHungerOverlayColor() != Color.white)
            m_playerStatusView.SetHungerOverlayColor(Color.white);
        else if (fillAmount <= 0.35f && m_playerStatusView.GetHungerOverlayColor() != Color.red)
            m_playerStatusView.SetHungerOverlayColor(Color.red);
    }

    private void UpdateThirstyOverlay()
    {
        float fillAmount = m_playerStatus.CurThirst / m_playerStatus.MaxThirst;
        if (fillAmount == m_playerStatusView.GetThirstyOverlayFillAmount()) return;

        m_playerStatusView.SetThirstyOverlayFillAmount(fillAmount);

        if (fillAmount > 0.35f && m_playerStatusView.GetThirstyOverlayColor() != Color.white)
            m_playerStatusView.SetThirstyOverlayColor(Color.white);
        else if (fillAmount <= 0.35f && m_playerStatusView.GetThirstyOverlayColor() != Color.red)
            m_playerStatusView.SetThirstyOverlayColor(Color.red);
    }

    private void UpdateMentalityOverlay()
    {
        float fillAmount = m_playerStatus.CurMentality / m_playerStatus.MaxMentality;
        if (fillAmount == m_playerStatusView.GetMentalityOverlayFillAmount()) return;

        m_playerStatusView.SetMentalityOverlayFillAmount(fillAmount);

        if (fillAmount > 0.35f && m_playerStatusView.GetMentalityOverlayColor() != Color.white)
            m_playerStatusView.SetMentalityOverlayColor(Color.white);
        if (fillAmount <= 0.35f && m_playerStatusView.GetMentalityOverlayColor() != Color.red)
            m_playerStatusView.SetMentalityOverlayColor(Color.red);
    }
}
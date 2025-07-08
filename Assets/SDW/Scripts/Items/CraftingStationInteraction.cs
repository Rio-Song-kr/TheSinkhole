using CraftingSystem;
using UnityEngine;

public class CraftingStationInteraction : MonoBehaviour
{
    [SerializeField] private Interaction m_interaction;
    private InteractionUIManager m_uiManager;
    private CraftingStation m_prevStation = null;
    private bool m_interactionKeyClicked;

    private void Awake()
    {
        m_interaction = GetComponent<Interaction>();
        m_uiManager = GetComponent<InteractionUIManager>();
    }

    private void Update()
    {
        if (!MouseInteraction()) return;

        //# 낮이 아니라면 return;
        if (!GameTimer.IsDay) return;

        var hitObject = m_interaction.Hit.collider;

        if (hitObject == null) return;

        if (!hitObject.CompareTag("Crafting"))
        {
            m_uiManager.ClearInteractionUI(InteractionType.Crafting);

            m_interactionKeyClicked = false;
            OutlineOff();
            return;
        }

        var station = hitObject.transform.GetComponent<CraftingStation>();

        if (station != m_prevStation)
            OutlineOff();

        m_prevStation = station;

        station.SetOutline(true);

        m_uiManager.SetInteractionUI(
            InteractionType.Crafting, true, "제작을 하려면 [E] 키를 눌러주세요.", false
        );

        if (!m_interactionKeyClicked) return;
        m_interactionKeyClicked = false;

        if (station == null) return;

        m_uiManager.ClearInteractionUI(InteractionType.Crafting);

        GameManager.Instance.UI.SetCursorUnlock();
        station.OpenStationUI();
    }

    private bool MouseInteraction()
    {
        if (!m_interaction.IsDetected || !GameManager.Instance.IsCursorLocked)
        {
            var currentType = GetCurrentInteractionType();
            m_uiManager.ClearInteractionUI(currentType);

            //# Outline Off
            OutlineOff();
            return false;
        }

        return true;
    }

    private void OutlineOff()
    {
        if (m_prevStation != null)
            m_prevStation.SetOutline(false);
        m_prevStation = null;
    }

    public void OnInteractionKeyPressed() => m_interactionKeyClicked = true;
    public void OnInteractionKeyReleased() => m_interactionKeyClicked = false;

    private InteractionType GetCurrentInteractionType() => InteractionType.Crafting;
}
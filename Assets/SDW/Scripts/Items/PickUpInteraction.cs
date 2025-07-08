using UnityEngine;

public class PickUpInteraction : MonoBehaviour
{
    protected static bool m_isInteractionKeyPressed;
    protected SceneItem m_prevSceneItem = null;
    protected Interaction m_interaction;
    protected Inventory m_inventory;
    protected InteractionUIManager m_uiManager;

    private void Awake()
    {
        m_interaction = GetComponent<Interaction>();
        m_inventory = GetComponent<Inventory>();
        m_uiManager = GetComponent<InteractionUIManager>();

        if (!m_interaction) Debug.Log("Player에 Interaction이 없습니다.");
        if (!m_inventory) Debug.Log("Player에 Inventory가 없습니다.");
        if (!m_uiManager) Debug.Log("Player에 InteractionUIManager가 없습니다.");
    }

    protected bool MouseInteraction()
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

    protected virtual InteractionType GetCurrentInteractionType() => InteractionType.Item; // 기본값

    protected void OutlineOff()
    {
        if (m_prevSceneItem != null)
            m_prevSceneItem.SetOutline(false);
        m_prevSceneItem = null;
    }

    public static void OnInteractionKeyPressed() => m_isInteractionKeyPressed = true;
    public static void OnInteractionKeyReleased() => m_isInteractionKeyPressed = false;
}
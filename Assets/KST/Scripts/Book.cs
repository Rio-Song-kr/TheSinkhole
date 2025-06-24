using UnityEngine;

public class Book : MonoBehaviour, Iinteractable
{
    [SerializeField] private int recoverAmount = 5;
    [SerializeField] private TestPlayerMental player;
    public void OnInteract(ToolType toolType)
    {
        player.RecoverMental(recoverAmount);
    }
}
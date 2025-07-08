using UnityEngine;

public class Book : MonoBehaviour, Iinteractable
{
    [SerializeField] private int recoverAmount = 5;
    [SerializeField] private TestPlayerMental player;
    public bool CanInteract()
    {
        return player != null;
    }

    public interactType GetInteractType()
    {
        return interactType.MouseClick;
    }

    public void OnInteract()
    {
        player.RecoverMental(recoverAmount);
    }
}
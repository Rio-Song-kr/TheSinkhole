public interface Iinteractable
{
    interactType GetInteractType();
    bool CanInteract();
    void OnInteract();
}

public interface IToolInteractable
{
    interactType GetInteractType();
    bool CanInteract(ToolType toolType);
    void OnInteract(ToolType toolType);
}

public interface ITileInteractable
{
    void OnTileInteractionStay(Interaction player);
    void OnTileInteractionExit(Interaction player);
}
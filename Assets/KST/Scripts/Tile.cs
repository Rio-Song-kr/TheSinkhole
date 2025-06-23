using UnityEngine;

public enum TileState {None = 0, Farmable}
public class Tile : MonoBehaviour, Iinteractable
{
    public TileState tileState = TileState.None;

    //타일마다 머터리얼 있어야 할 경우 변경할 수 있도록.
    //public Material farmMaterial, stoneMaterial;
    public void OnInteract(ToolType toolType)
    {
        //타일 상태
        switch (tileState)
        {
            case TileState.None: // 타일상태가 None이면서, 
                if (toolType == ToolType.None)
                {
                    Debug.Log("어떠한 타일과도 상호작용할 수 없습니다.");
                }
                else if (toolType == ToolType.Shovel)
                {
                    tileState = TileState.Farmable;
                    SetFarmable();
                    Debug.Log("타일이 농사가 가능한 상태로 변경됐습니다.");
                }
                break;

            case TileState.Farmable:
                if (toolType == ToolType.None)
                {
                    Debug.Log("맨손으로는 농사를 할 수 없습니다.");
                }
                else if (toolType == ToolType.Shovel)
                {
                    Debug.Log("삽으로 수확할 수 있도록");
                }
                else if (toolType == ToolType.Drill)
                {
                    Debug.Log("아무것도 아닌 땅으로 변경됐습니다.");
                    SetNoneGround();
                }
                break;

        }
    }

    void SetFarmable()
    {
        tileState = TileState.Farmable;

        if (!TryGetComponent<FarmTile>(out var _))
        {
            gameObject.AddComponent<FarmTile>();
        }
    }
    void SetNoneGround()
    {
        tileState = TileState.None;
        if (TryGetComponent<FarmTile>(out var farmTile))
        {
            Destroy(farmTile);
        }
    }
}
using TMPro;
using UnityEngine;


public class Tile : MonoBehaviour, IToolInteractable
{
    public TileState tileState = TileState.None;

    public GameObject InteractUiTextRef;
    //농사창 UI
    public GameObject FarmUIRef;


    public interactType GetInteractType()
    {
        return interactType.MouseClick;
    }
    public bool CanInteract(ToolType toolType)
    {
        switch (tileState)
        {
            case TileState.None:
                return toolType == ToolType.Pick;
            case TileState.Frontier:
                return toolType == ToolType.Shovel;
            default:
                return false;
        }
    }

    //타일마다 머터리얼 있어야 할 경우 변경할 수 있도록.
    //public Material farmMaterial, stoneMaterial;

    //곡괭이 -> 미개척지 -> 개척지
    //삽 -> 개척지 -> 경작지
    public void OnInteract(ToolType toolType)
    {
        //타일 상태
        switch (tileState)
        {
            case TileState.None: // 미 개척지. 다른 행동은 불능이며, 곡괭이를 통해서만 개척지로 변경 가능.
                if (toolType == ToolType.Pick)
                {
                    tileState = TileState.Frontier;
                    Debug.Log("타일이 개척지로 변경됐습니다.");
                }
                else
                {
                    Debug.LogWarning("현재 타일은 미 개척지입니다. 곡괭이를 사용해서 개척하세요");
                }
                break;

            case TileState.Frontier: // 개척지. 삽을 통해 경작지로 변경 가능하며, 추후 다른 건물을 짓는 것도 가능.
                if (toolType == ToolType.Shovel)
                {
                    tileState = TileState.Farmable;
                    SetFarmable();
                    Debug.Log("경작지로 변경됐습니다.");
                }
                // else if (toolType == ToolType.Hammer)
                // {
                //     tileState = TileState.DeffenceArea;
                //     SetDeffenceArea();
                //     Debug.Log("터렛건설 지역으로 변경됐습니다.");
                    
                // }
                break;

            // case TileState.Farmable: //경작지
            //     if (toolType == ToolType.Shovel)
            //     {
            //         //상호작용 아이콘 팝업
            //         interactUiText.SetActive(true);
            //         //농사창 팝업.
            //     }
            //     break;

        }
    }


    //테스트를 위해 잠시 public 메소드로 변경

    //FarmTile로 변경시 부착
    public void SetFarmable()
    {
        tileState = TileState.Farmable;

        if (!TryGetComponent<FarmTile>(out var _))
        {
            var go = gameObject.AddComponent<FarmTile>();
            // go.FarmUIObj = FarmUIRef;
            go.InteractUiText = InteractUiTextRef;
        }
    }

    //TurretTile로 변경시 부착
    public void SetDeffenceArea()
    {
        tileState = TileState.DeffenceArea;

        if (!TryGetComponent<TurretTile>(out var _))
        {
            var go = gameObject.AddComponent<TurretTile>();
            go.InteractUiText = InteractUiTextRef;
        }
    }


    public void SetNoneGround()
    {
        tileState = TileState.None;
        if (TryGetComponent<FarmTile>(out var farmTile))
        {
            Destroy(farmTile);
        }
        if (TryGetComponent<TurretTile>(out var turretTile))
        {
            Destroy(turretTile);
        }
    }
}
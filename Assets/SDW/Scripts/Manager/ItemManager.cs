using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private ItemDatabaseSO m_itemDatabaseSO;
    [SerializeField] private GameObject m_sceneItemPrefab;

    //todo 테스트를 위한 prefab 추후 변경 예정
    [SerializeField] private GameObject _redPrefab;
    [SerializeField] private GameObject _greenPrefab;

    private Dictionary<string, ItemPool<SceneItem>> m_itemPools;
    public Dictionary<string, ItemPool<SceneItem>> ItemPools => m_itemPools;

    //todo csv에서 불러와서 data를 덮어씌우는 기능이 있어야 함
    private void OnEnable()
    {
        if (m_itemDatabaseSO == null)
        {
            Debug.Log("Item Database SO가 연결되지 않았습니다.");
            return;
        }

        var itemLoader = new ItemLoadCSV();
        var itemList = itemLoader.ReadData("SDW/SaveData/Item.csv");

        //todo csv에서 읽어진 아이템의 길이만큼 생성
        m_itemPools = new Dictionary<string, ItemPool<SceneItem>>();
        m_itemDatabaseSO.ItemObjects = new ItemDataSO[itemList.Count];

        var parentObject = new GameObject();
        parentObject.name = "Items";

        for (int i = 0; i < itemList.Count; i++)
        {
            //# Scriptable Object 생성
            var newItemDataSO = ScriptableObject.CreateInstance<ItemDataSO>();
            newItemDataSO.name = itemList[i].ItemId.ToString();

            if (!Enum.TryParse<ItemType>(itemList[i].ItemType, true, out var itemType))
                itemType = ItemType.None; // 기본값 설정

            m_itemPools.Add(itemList[i].ItemName, new ItemPool<SceneItem>());

            //# CSV에서 읽은 Data 연결
            newItemDataSO.ItemData.ItemId = itemList[i].ItemId;
            newItemDataSO.ItemData.ItemName = itemList[i].ItemName;
            newItemDataSO.ItemType = itemType;
            newItemDataSO.ItemMaxOwn = itemList[i].ItemMaxOwn;
            newItemDataSO.ItemText = itemList[i].ItemText;

            //todo 테스트를 위한 prefab 추후 변경 예정
            newItemDataSO.ModelPrefab = i % 2 == 0 ? _redPrefab : _greenPrefab;

            //todo 데이터 초기화 및 모델, Icon 등 연결
            m_itemDatabaseSO.ItemObjects[i] = newItemDataSO;

            var itemObject = Instantiate(m_sceneItemPrefab);
            itemObject.transform.parent = parentObject.transform;
            itemObject.tag = "Item";

            var itemModel = Instantiate(newItemDataSO.ModelPrefab);
            itemModel.transform.parent = itemObject.transform;

            //# sceneItem으로 캐스팅
            var sceneItem = itemObject.GetComponent<SceneItem>();
            sceneItem.ItemDataSO = m_itemDatabaseSO.ItemObjects[i];
            sceneItem.SetupColliderSize();

            itemObject.SetActive(false);

            m_itemPools[itemList[i].ItemName].SetPool(itemObject);
        }
    }
}
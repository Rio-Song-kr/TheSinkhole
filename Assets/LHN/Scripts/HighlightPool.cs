using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPool : MonoBehaviour
{
    public LayerMask tileLayer; // "Tile" 레이어만 포함되도록 설정

    public GameObject highlightPrefab; // 하이라이트로 사용할 프리팹 (예: 초록색 반투명 큐브)
    public int poolSize = 100;         // 초기 생성할 하이라이트 오브젝트 수
    private Queue<GameObject> pool = new Queue<GameObject>(); // 오브젝트 풀 큐

    void Start()
    {
        // 미리 지정된 수만큼 하이라이트 오브젝트를 생성하고 비활성화 상태로 큐에 저장
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(highlightPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // 풀에서 하이라이트 오브젝트 하나 꺼내서 반환
    public GameObject GetHighlight()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true); // 꺼낼 때 활성화
            return obj;
        }
        return null; // 풀에 남은 오브젝트가 없으면 null 반환
    }

    // 사용이 끝난 하이라이트 오브젝트를 다시 풀에 반환
    public void ReturnHighlight(GameObject obj)
    {
        obj.SetActive(false); // 비활성화 후
        pool.Enqueue(obj);    // 큐에 다시 넣음
    }

    bool IsPlaceable(Vector3 worldPos)
    {
        // 위에서 아래로 Ray를 쏴서 Tile 레이어에 닿는지 확인
        Ray ray = new Ray(worldPos + Vector3.up * 5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, tileLayer))
        {
            return true;
        }
        return false;
    }

    public HighlightPool highlightPool;

    void ShowHighlights(List<Vector3> positions)
    {
        foreach (var pos in positions)
        {
            // 해당 위치가 설치 가능한지 확인
            if (IsPlaceable(pos))
            {
                // 풀에서 하이라이트 오브젝트 하나 가져오기
                GameObject highlight = highlightPool.GetHighlight();
                if (highlight != null)
                {
                    // 설치 가능한 위치에 하이라이트 배치
                    highlight.transform.position = pos;
                }
            }
        }
    }
}



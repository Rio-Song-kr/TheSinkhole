using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPool : MonoBehaviour
{
    public LayerMask tileLayer; // "Tile" ���̾ ���Եǵ��� ����

    public GameObject highlightPrefab; // ���̶���Ʈ�� ����� ������ (��: �ʷϻ� ������ ť��)
    public int poolSize = 100;         // �ʱ� ������ ���̶���Ʈ ������Ʈ ��
    private Queue<GameObject> pool = new Queue<GameObject>(); // ������Ʈ Ǯ ť

    void Start()
    {
        // �̸� ������ ����ŭ ���̶���Ʈ ������Ʈ�� �����ϰ� ��Ȱ��ȭ ���·� ť�� ����
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(highlightPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // Ǯ���� ���̶���Ʈ ������Ʈ �ϳ� ������ ��ȯ
    public GameObject GetHighlight()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true); // ���� �� Ȱ��ȭ
            return obj;
        }
        return null; // Ǯ�� ���� ������Ʈ�� ������ null ��ȯ
    }

    // ����� ���� ���̶���Ʈ ������Ʈ�� �ٽ� Ǯ�� ��ȯ
    public void ReturnHighlight(GameObject obj)
    {
        obj.SetActive(false); // ��Ȱ��ȭ ��
        pool.Enqueue(obj);    // ť�� �ٽ� ����
    }

    bool IsPlaceable(Vector3 worldPos)
    {
        // ������ �Ʒ��� Ray�� ���� Tile ���̾ ����� Ȯ��
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
            // �ش� ��ġ�� ��ġ �������� Ȯ��
            if (IsPlaceable(pos))
            {
                // Ǯ���� ���̶���Ʈ ������Ʈ �ϳ� ��������
                GameObject highlight = highlightPool.GetHighlight();
                if (highlight != null)
                {
                    // ��ġ ������ ��ġ�� ���̶���Ʈ ��ġ
                    highlight.transform.position = pos;
                }
            }
        }
    }
}



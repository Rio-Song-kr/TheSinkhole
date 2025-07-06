using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IDamageable
{
    [Header("쉘터 내구도")]
    public int maxDurability = 3000;//최대 내구도
    public int currentDurability = 3000;//현재 내구도

    public Shelter shelterObject; //쉘터 오브젝트
    

    void Update()
    {
        ColorChange();
    }
    //인터페이스 구현
    public void TakenDamage(int damage)
    {
        currentDurability -= damage;
        Debug.Log("쉘터가 " + damage + "의 피해를 받았습니다. 현재 내구도: " + currentDurability);

        if (currentDurability <= 0)
        {
            currentDurability = 0;
            OnShelterDestroyed();
        }
    }

    public void ColorChange()
    {
        //if(currentDurability >= maxDurability * 0.7)
        //{
        //    shelterObject.GetComponent<Renderer>().material.color = Color.green; //녹색
        //}
        //else if (currentDurability<= maxDurability * 0.7 && currentDurability >= maxDurability * 0.4)
        //{
        //    shelterObject.GetComponent<Renderer>().material.color = Color.yellow; //노란색
        //}
        //else if (currentDurability < maxDurability * 0.4)
        //{
        //    shelterObject.GetComponent<Renderer>().material.color = Color.red; //빨간색
        //}
    }

    private void OnShelterDestroyed()
    {
        Debug.Log("쉘터가 파괴되었습니다! 플레이어가 죽었습니다");
        //게임 오버 처리

        //파괴 연출, 오브젝트 비활성화 처리
        //gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelter : MonoBehaviour, IDamageable
{
    [Header("���� ������")]
    public int maxDurability = 3000;//�ִ� ������
    public int currentDurability = 3000;//���� ������

    public Shelter shelterObject; //���� ������Ʈ
    

    void Update()
    {
        ColorChange();
    }
    //�������̽� ����
    public void TakenDamage(int damage)
    {
        currentDurability -= damage;
        Debug.Log("���Ͱ� " + damage + "�� ���ظ� �޾ҽ��ϴ�. ���� ������: " + currentDurability);

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
        //    shelterObject.GetComponent<Renderer>().material.color = Color.green; //���
        //}
        //else if (currentDurability<= maxDurability * 0.7 && currentDurability >= maxDurability * 0.4)
        //{
        //    shelterObject.GetComponent<Renderer>().material.color = Color.yellow; //�����
        //}
        //else if (currentDurability < maxDurability * 0.4)
        //{
        //    shelterObject.GetComponent<Renderer>().material.color = Color.red; //������
        //}
    }

    private void OnShelterDestroyed()
    {
        Debug.Log("���Ͱ� �ı��Ǿ����ϴ�! �÷��̾ �׾����ϴ�");
        //���� ���� ó��

        //�ı� ����, ������Ʈ ��Ȱ��ȭ ó��
        //gameObject.SetActive(false);
    }
}

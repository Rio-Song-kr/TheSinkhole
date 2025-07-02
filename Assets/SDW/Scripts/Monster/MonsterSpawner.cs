using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 몬스터 생성을 위한 클래스
/// </summary>
public class MonsterSpawner : MonoBehaviour
{
    //! 의사코드
    //# 1. 밤이라면 MonsterManager에서 Monster를 선택해야 함
    //@ 1-1. Random으로 몬스터를 선택(날짜별 확률?)
    //# 2. 몬스터 생성
    //# 3. 날짜 * 3 만큼 생성 되었는가?
    //@ 3-1. 날짜만큼 생성되었으면 종료
    //@ 3-2. 날짜만큼 생성되지 않았으면 계속 생성
    //@ 3-2-1. 단, 직전 몬스터 생성된 후 일정 시간 뒤 생성

    //! 의문 사항 정리
    //# 첫 날은 1마리? 아니면 첫 날도 날짜 * 3?
    //# 각 등급별 몬스터의 생성 확률은 어떻게 할 것인가
    //# 밤 => 낮이 되더라고 재생성이 아닌, 다 잡을 때까지는 유지

    [SerializeField] private float m_creationInvervalTime = 5f;
    private float m_creationTime = 0f;
    private void Start()
    {
    }

    private void Update()
    {
        if (GameTimer.IsDay) return;

        //todo 추후 날짜별 몬스터 생성 수 등의 datatable이 추가되면 세부 구현

        // GameTimer.GetDay()
    }
}
# PlayerStatus 시스템 요약

`PlayerStatus`는 플레이어의 생존 스탯 변화와 시간단위별 변화를 관리하는 함수

---

## 주요 스탯 항목

| 스탯        | 설명                                                                 |
|-------------|----------------------------------------------------------------------|
| Health      | 체력. 0이 되면 플레이어 사망                                         |
| Hunger      | 배고픔. 0이 되면 허기 디버프 적용                                     |
| Thirst      | 갈증. 0이 되면 탈수 디버프 적용                                     |
| Mentality   | 정신력. 0이 되면 즉시 사망 (체력 0 처리)                            |
| MoveSpeed   | 이동속도. 디버프에 따라 변화함                                       |
| ActionSpeed | 행동속도 배율 (ex. 아이템 사용, 행동 시간 등 계산에 사용 예정), 디버프에 따라 변화함      |
| AtkSpeed    | 공격속도 배율 (추후 공격 시스템에 사용 예정)                         |

---

##  주요 기능 요약

| 함수                      | 설명                                                                 |
|---------------------------|----------------------------------------------------------------------|
| `SetHealth(float)`        | 체력을 퍼센트 기준으로 증감. 0 이하면 사망 처리                     |
| `SetHunger(float)`        | 배고픔 퍼센트 조절 및 디버프 트리거, 0 이하면 허기 디버프 발생                                |
| `SetThirst(float)`        | 갈증 퍼센트 조절 및 디버프 트리거, 0이하면 탈수 디버프 발생                                 |
| `SetMentality(float)`     | 정신력 퍼센트 조절. 0 이하면 즉시 사망                             |

## RealtimeStatusCycle(bool isDay)

- 플레이어의 생존 스탯들을 **현실 시간을 기반으로 단위시간(현재 1분)마다 감소**시키는 함수  
- 낮과 밤에 따라 감소 속도가 달라지며, **허기 디버프 여부에 따라 정신력 감소량**이 다름
- 단위 시간마다 변화하는 스탯 3개를 한번에 묶어 관리에 편리하게 함
### 호출 방식
- 낮과 밤을 다루는 부분 뒤에 배치해서 사용하는 것을 추천
  - 이유: 파라미터에 낮인지 밤인지 결정하는 bool 타입의 isDay를 사용해서 변화함
```csharp
PlayerStatus.Instance.RealtimeStatusCycle(true);  // 낮일 때
PlayerStatus.Instance.RealtimeStatusCycle(false); // 밤일 때
```
### 단위시간(현재 1분)당 변화량
| 스탯              | 낮 (isDay = true)  | 밤 (isDay = false) | 변동 조건                  |
| --------------- | ----------------- | ----------------- | ------------------- |
| Hunger (배고픔)    | -2.5% (`-0.025f`) | -1% (`-0.01f`)    |             |
| Thirst (갈증)     | -10% (`-0.1f`)    | -10% (`-0.1f`)    |              |
| Mentality (정신력) | -2.5% (`-0.025f`)    | -2.5% (`-0.025f`) | 허기 상태면 5%(`-0.05f`), 아니면 2.5%(`-0.025f`) |

---

## 사용 예시

```csharp
// 체력을 20% 회복
PlayerStatus.Instance.SetHealth(0.2f);

// 갈증을 10% 소모
PlayerStatus.Instance.SetThirst(-0.1f);

// 현재 이동속도 참조
float speed = PlayerStatus.Instance.CurPlayerMoveSpeed;

// 정신력 5% 감소
PlayerStatus.Instance.SetMentality(-0.05f);
```
---

### 디버프 시스템

- ### 허기 디버프 (`StarvationDebuff`)
  - 조건: `CurHunger <= 0`
  - 효과: 이동속도 50% 감소, 행동속도 2배 증가, 1분마다 체력 20% 감소, 정신력 자동소모 2배 증가
  - 해제: 배고픔이 다시 0 이상이 되면 자동 해제

- ### 탈수 디버프 (`DehydrationDebuff`)
  - 조건: `CurThirst <= 0`
  - 효과: 1분마다 체력 10% 감소
  - 해제: 갈증 수치가 다시 0 이상이 되면 자동 해제

-  각 디버프는 내부에서 `StartDebuff()`, `StopDebuff()`로 관리

---

##  테스트용 메서드 및 상수

- `PrintAllCurStatus()`: 현재 상태 전체 출력용 (디버깅용)
- `StatId`: 스탯 아이디
- `RealtimeOneMinute`: 리얼타임 60초 단위 정의

---

##  싱글톤 구조

- `PlayerStatus.Instance`를 통해 전역 접근 가능
- 중복 생성 방지 로직 포함 (`Awake()`에서 처리)

---

##  참고

- 각 `Set` 함수는 **퍼센트 단위의 변화**를 인자로 받음  
  예: `SetHunger(-0.1f)` → 최대 배고픔의 10% 감소

- `Cur스탯` 값들은 **Mathf.Clamp 처리**되어 `0 ~ Max스탯` 범위를 벗어나지 않음

---


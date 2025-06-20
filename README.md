# 게임 이름

## 게임 소개

## Git 관련

- 매일 오후 17:10 머지 진행

- 머지를 위한 최소 검토 인원 - 3명

- 문제 발생 시 해당 에러 창을 닫지 않고 보고(본인이 처음 보는 에러인 경우)

- branch 구성

  - main
  - develop - playable
  - develop
  - 개인 작업 branch(이름 약자\_기능)
    - e.g. SDW_Feat-UIInteraction
    - e.g. SDW_Bug-CharacterControl

- Merge 순서
  - 개인 작업 branch -> develop으로 merge
  - develop에서 플레이가 가능한 경우, develop - playable로 merge

* Project - 노션에도 동일하게 작성
  - Status, Start date, End date 필수
  - [Feat], [Bug] 등 Issue의 이름은 양식을 활용

## 컨벤션

### 코드 컨벤션 - [Unity C# 가이드 기준](https://unity.com/kr/resources/c-sharp-style-guide-unity-6)

```cs

// 예시
public int PublicField;
public static int MyStaticField;

private int m_packagePrivate;
private int m_myPrivate;
private static int m_myPrivate;

protected int m_myProtected;

const int k_MaxItems = 100;

// EXAMPLE: enums use singular nouns public
enum WeaponType
{
    Knife,
    Gun,
    RocketLauncher,
    BFG
}

// EXAMPLE: but a bitwise enum is plural (you can also use the 1 << bitnum style)
[Flags]
public enum AttackModes
{
  // Decimal                          // Binary
	None = 0,                          // 000000
	Melee = 1,                         // 000001
	Ranged = 2,                        // 000010
	Special = 4,                       // 000100
  MeleeAndSpecial = Melee | Special  // 000101
}

```

### Git 컨벤션

| Type     | 내용                                                    |
| -------- | ------------------------------------------------------- |
| Feat     | 새로운 기능 추가, 기존의 기능을 요구 사항에 맞추어 수정 |
| Fix      | 기능에 대한 버그 수정                                   |
| Set      | 프로젝트, 기타 환경 설정 등                             |
| Chore    | 그 외 기타 수정                                         |
| Docs     | 문서(주석 수정)                                         |
| Refactor | 기능의 변화가 아닌 코드 리팩터링                        |
| Test     | 테스트 코드 추가/수정                                   |

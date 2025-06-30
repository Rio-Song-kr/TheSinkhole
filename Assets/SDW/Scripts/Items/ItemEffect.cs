/// <summary>
/// ItemEffect를 설정하기 위한 클래스
/// 추후 캐릭터의 아이템 사용과 연결 작업을 위한 메서드 등은 주석 처리
/// </summary>
[System.Serializable]
// public class ItemEffect : IUsableItem
public class ItemEffect
{
    public StatusType Status;
    public int EffectAmount;

    /// <summary>
    /// 아이템의 사용 효과 생성자
    /// </summary>
    /// <param name="status">아이템이 적용될 스탯</param>
    /// <param name="effectAmount">스탯에 적용되는 수치</param>
    public ItemEffect(StatusType status, int effectAmount)
    {
        Status = status;
        EffectAmount = effectAmount;
    }

    // public void UseItem(ref PlayerStatus stat)
    // {
    //     switch (Effect)
    //     {
    //         case EffectType.Hp :
    //             stat.Hp += EffectAmount;
    //             break;
    //     }
    // }
}

// public interface IUsableItem
// {
//     public void Use(ref PlayerStatus stat);
// }
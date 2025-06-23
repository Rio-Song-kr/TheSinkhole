[System.Serializable]
// public class ItemEffect : IUsableItem
public class ItemEffect
{
    public StatusType Status;
    public int EffectAmount;

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
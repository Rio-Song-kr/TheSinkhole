using UnityEngine;
public class TestPlayerMental : MonoBehaviour
{
    public int mental=10;

    public void RecoverMental(int _amount)
    {
        mental += _amount;
    }
}
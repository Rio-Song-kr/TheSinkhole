using UnityEngine;
public class TestPlayerMental : MonoBehaviour
{
    public int Mental=10;

    public void RecoverMental(int _amount)
    {
        Mental += _amount;
    }
}
using UnityEngine;

public enum CropType
{
    None = 0,
    Potato,
    SweetPotato
}
public class Crop : MonoBehaviour
{
    public CropType type = CropType.None;
    public float growTime = 3f;
    private float timer = 0f;
    public GameObject grownCropPrefab;
    private bool isGrowing = false;
    void Update()
    {
        if (!isGrowing) return;

        timer += Time.deltaTime;
        if (timer >= growTime)
        {
            Grow();
        }
    }

    public void StartGrow()
    {
        isGrowing = true;
        timer = 0f;
    }
    void Grow()
    {
        //씨앗 -> 작물로 변경
        Instantiate(grownCropPrefab, transform.position, Quaternion.identity);
        //씨앗은 파괴.
        Destroy(gameObject);
    }

}
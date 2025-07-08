using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 제네릭 오브젝트 풀을 관리하는 클래스
/// Unity의 ObjectPool을 래핑하여 씬 변경 시 안전한 처리를 제공
/// </summary>
/// <typeparam name="T">MonoBehaviour를 상속받는 풀링할 오브젝트 타입</typeparam>
public class PoolManager<T> where T : MonoBehaviour
{
    private readonly IObjectPool<T> _pool;
    //# 추후 Scene Change 시 적용할 필드
    private bool _isSceneChanged = false;

    /// <summary>
    /// PoolManager의 생성자
    /// </summary>
    /// <param name="prefab">풀링할 프리팹</param>
    /// <param name="defaultCapacity">기본 용량</param>
    /// <param name="maxSize">최대 크기</param>
    /// <param name="transform">(선택) 부모 Transform</param>
    public PoolManager(T prefab, int defaultCapacity = 10, int maxSize = 20, Transform transform = null)
    {
        _pool = new ObjectPool<T>
        (
            () => transform == null ? Object.Instantiate(prefab) : Object.Instantiate(prefab, transform),
            obj => obj?.gameObject.SetActive(true),
            obj => obj?.gameObject.SetActive(false),
            obj => Object.Destroy(obj?.gameObject),
            true,
            defaultCapacity,
            maxSize
        );
    }

    /// <summary>
    /// 풀에서 오브젝트를 가져옴
    /// </summary>
    /// <returns>풀에서 가져온 오브젝트 (씬 변경 시 null 반환)</returns>
    public T Get() => _isSceneChanged ? null : _pool.Get();

    /// <summary>
    /// 오브젝트를 풀에 반환
    /// </summary>
    /// <param name="obj">반환할 오브젝트</param>
    public void Release(T obj)
    {
        if (_isSceneChanged) return;
        _pool?.Release(obj);
    }

    /// <summary>
    /// 씬 변경 시 호출되어 풀 사용을 비활성화
    /// </summary>
    private void OnSceneChanged() => _isSceneChanged = true;
}
using UnityEngine;

public abstract class PoolingObject : MonoBehaviour
{
    private MyObjectPool.ObjectPool pool;

    private void Start()
    {
        Debug.Assert(pool != null, "해당 인스턴스는 ObjectPool에 의해서 생성되어야 함.");
    }

    public void SetPool(MyObjectPool.ObjectPool pool)
    {
        this.pool = pool;
    }

    public void Return()
    {
        pool.Return(this);
    }

    /// <summary>
    /// 가져올 때 동작 추상화
    /// </summary>
    public virtual void OnBurrow()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 반환할 때 동작 추상화
    /// </summary>
    public virtual void OnReturn()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 생성되었을 때 동작 추상화
    /// </summary>
    public virtual void OnCreated()
    {
        gameObject.SetActive(false);
    }
}
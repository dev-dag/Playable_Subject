using System.Collections.Generic;
using UnityEngine;

namespace MyObjectPool
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private Transform parentTransform;
        [SerializeField] private PoolingObject poolingObjectPrefab;
        [SerializeField] private int warmUpCount = 0;

        private List<PoolingObject> usingList = new List<PoolingObject>();
        private List<PoolingObject> restingList = new List<PoolingObject>();
        private int objectCount = 0;

        private void Awake()
        {
            for (int i = 0; i < warmUpCount; i++) // 웜업
            {
                PoolingObject newObject = Create();
                restingList.Add(newObject);
            }
        }

        /// <summary>
        /// 오브젝트를 가져오는 함수
        /// </summary>
        public T Burrow<T>() where T : class
        {
            if (restingList.Count > 0)
            {
                PoolingObject poolingObject = restingList[0];
                restingList.RemoveAt(0);
                usingList.Add(poolingObject);
                poolingObject.OnBurrow();
                return poolingObject as T;
            }
            else
            {

                PoolingObject newObject = Create();
                usingList.Add(newObject);
                return newObject as T;
            }
        }

        /// <summary>
        /// 오브젝트를 반환하는 함수.
        /// </summary>
        public void Return(PoolingObject poolingObject)
        {
            if (usingList.Contains(poolingObject))
            {
                usingList.Remove(poolingObject);
                restingList.Add(poolingObject);
                poolingObject.OnReturn();
            }
        }

        /// <summary>
        /// 오브젝트를 생성하는 함수
        /// </summary>
        private PoolingObject Create()
        {
            PoolingObject newObject = Instantiate(poolingObjectPrefab, parentTransform).GetComponent<PoolingObject>();
            newObject.gameObject.name = poolingObjectPrefab.gameObject.name + "_" + objectCount++;
            newObject.SetPool(this);
            newObject.OnCreated();

            return newObject;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab = null;
    [SerializeField] private Queue<GameObject> pool = new Queue<GameObject>();
    [SerializeField] private Transform[] poolPos;

    [SerializeField] private int poolNum = 0;
    [SerializeField] private int stage = 0;
    private void Awake()
    {
        for (int i = 0; i < poolNum; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab, poolPos[i].position, Quaternion.identity);
            pool.Enqueue(obj);
            Obstacles obstacle;
            obj.TryGetComponent<Obstacles>(out obstacle);
            obstacle.SetPool(this);
            obstacle.SetInit();
            obj.SetActive(false);
        }
    }
    private void Start()
    {
        DropPool();
    }
    public void EnqueuePool(GameObject obj)
    {
        pool.Enqueue(obj);
        obj.SetActive(false);
        if (pool.Count == poolNum) DropPool();
    }
    private void DropPool()
    {
        for (int i = 0; i < poolNum; i++)
        {
            pool.Dequeue().SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab = null;
    [SerializeField] private Queue<GameObject> pool;
    [SerializeField] private Transform[] poolPos;
    [SerializeField] private Transform poolfolder;
    [SerializeField] private PlayerController myPlayer;


    [SerializeField] private int poolNum = 0;
    [SerializeField] private int stage = 0;
    private void Awake()
    {
        pool = new Queue<GameObject>();
        for (int i = 0; i < poolNum; i++)
        {
            GameObject obj = Instantiate(obstaclePrefab, poolPos[i].position, Quaternion.identity);
            pool.Enqueue(obj);
            Obstacles obstacle;
            obj.TryGetComponent<Obstacles>(out obstacle);
            obstacle.SetPool(this);
            obstacle.SetInit(i);
            obstacle.transform.SetParent(poolfolder);
            obj.SetActive(false);
        }
    }
    private void Start()
    {
        DropPool();
        myPlayer.SetInitEquipment();
    }
    public void EnqueuePool(GameObject obj)
    {
        pool.Enqueue(obj);
        obj.SetActive(false);
        Debug.Log(pool.Count);
        if (pool.Count == poolNum) DropPool();
    }
    private void DropPool()
    {
        for (int i = 0; i < poolNum; i++)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);    
            obj.transform.position = poolPos[i].position;
            obj.transform.rotation = Quaternion.identity;
            Debug.Log("now obj num : " + i);
        }
    }
}

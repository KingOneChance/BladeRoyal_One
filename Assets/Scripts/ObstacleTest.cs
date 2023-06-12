using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    Rigidbody2D rigidbody2D = null;
    BoxCollider2D boxCollider2D = null;
    private void Awake()
    {
        TryGetComponent<Rigidbody2D>(out rigidbody2D);
        TryGetComponent<BoxCollider2D>(out boxCollider2D);
        if (rigidbody2D == null) Debug.LogError("Obstacle has any rigidbody2D");
        if (boxCollider2D == null) Debug.LogError("Obstacle has any rigidbody2D");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && collision.gameObject.GetComponent<PlayerTest>().GetIsGround()==true)
        {
            boxCollider2D.isTrigger = true;
        }
        if(collision.collider.tag =="Ground")
        {
            Debug.Log("바닥에 닿음 , 콜리전");

            rigidbody2D.gravityScale = 0;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("플레이어와 닿음");
        if (collision.collider.tag == "GroundPlayer")
        {
            Debug.Log("바닥에 있는 플레이어와 닿음");
            boxCollider2D.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            Debug.Log("바닥에 닿음 , 트리거");
            rigidbody2D.mass = 0;
            rigidbody2D.gravityScale = 0;
        }
    }
  
}

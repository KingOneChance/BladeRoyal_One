using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTest : MonoBehaviour
{
    BoxCollider2D boxCollider2D = null;

    

    private void Awake()
    {
        TryGetComponent<BoxCollider2D>(out boxCollider2D);
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
            Debug.Log("�ٴڿ� ���� , �ݸ���");
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("�÷��̾�� ����");
        if (collision.collider.tag == "GroundPlayer")
        {
            Debug.Log("�ٴڿ� �ִ� �÷��̾�� ����");
            boxCollider2D.isTrigger = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}

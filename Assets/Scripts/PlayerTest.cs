using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody2D = null;
    [Range(0, 100)]
    [SerializeField] private float jumpPower;
    [Range(0, 10)]
    [SerializeField] private float massValue;
    [SerializeField] private bool isGround = true;

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGround = true;
        rigidbody2D.mass = 1;
        Debug.Log("Åë°ú : " + collision.gameObject.name);
    }
    private void Update()
    {
        if(!isGround)
        {
           Vector2  temp = rigidbody2D.GetPointVelocity(gameObject.transform.position);
            Debug.Log(temp.ToString());
        }
    }

    public void OnClick_Jump()
    {
        isGround = false;
        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        StartCoroutine(Co_CheckFallingSpeed());
    }
    
    IEnumerator Co_CheckFallingSpeed()
    {
        while(!isGround)
        {
            yield return null;
            if (rigidbody2D.GetPointVelocity(gameObject.transform.position).y < 0)
            {
                rigidbody2D.mass = massValue;
                break;
            }
        }
    }
}

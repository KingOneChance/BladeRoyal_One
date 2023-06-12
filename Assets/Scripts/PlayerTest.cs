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
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float DefenceCoolTime = 2;
    [SerializeField] private bool isGround = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canDeffence = true;
    [SerializeField] private Animator animator = null;

    public bool GetIsGround()
    {
        return isGround;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if( collision.gameObject.tag =="Ground")
        {
            isGround = true;
            rigidbody2D.mass = 1;
            gameObject.tag = "GroundPlayer";
            Debug.Log("통과 : " + collision.gameObject.name);
        }
    }
    #region About Jump Method
    public void OnClick_Jump()
    {
        if (!isGround) return;
        isGround = false;
        gameObject.tag = "Player";
        //animator.SetTrriger("Attack",true);
        rigidbody2D.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        StartCoroutine(Co_CheckFallingSpeed());
    }
    IEnumerator Co_CheckFallingSpeed()
    {
        while (!isGround)
        {
            yield return null;
            if (rigidbody2D.GetPointVelocity(gameObject.transform.position).y < 0)
            {
                rigidbody2D.mass = massValue;
                break;
            }
        }
    }
    #endregion

    #region About Attack Method

    public void OnClick_Attack()
    {
        if (!canAttack) return;
        Debug.Log("공격");
        canAttack = false;
        StartCoroutine(Co_ActionCoolTime(attackCoolTime, canAttack, "공격"));
        //animator.SetTrriger("Attack",true);
    }
    #endregion

    #region About Deffence Method
    public void OnClick_Deffence()
    {
        if (!canDeffence) return;
        Debug.Log("방어");
        canDeffence = false;
        StartCoroutine(Co_ActionCoolTime(DefenceCoolTime, canDeffence, "방어"));
        //animator.SetTrriger("Deffence",true);
    }
    #endregion
    IEnumerator Co_ActionCoolTime(float actionCool, bool canUse, string testName)
    {
        float timer = 0;
        while (timer < actionCool)
        {
            yield return null;
            timer += Time.deltaTime;
            Debug.Log(testName);
        }
        canUse = true;
    }
}

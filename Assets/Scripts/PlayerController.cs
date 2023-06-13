using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MyRigidBody
{
    public float initJumpVelocity;
    public float velocity;
    public float accelerationG;
    public float jumpForce;
    public float initFallVelocity;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private MyRigidBody myRigidBody;

    [Header("====Player StatCheck")]
    [SerializeField] private bool isGround = false;
    [SerializeField] private bool isFalling = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canDeffence = true;
    [SerializeField] private bool canJumpSkill = true;
    [SerializeField] private bool canAttackSkill = true;
   
    [Header("====Player Value")]
    [SerializeField] private float bottomHeight;
    [SerializeField] private float playerHeight;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float DefenceCoolTime = 2;

    [Header("====Player Value")]
    [SerializeField] private int hearts = 3;

    private void Start()
    {
        playerHeight = gameObject.transform.localScale.y;
    }
    private void OnEnable()
    {
        StartCoroutine(Co_ForcedGravity()); //플레이어 중력
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거 엔터");
        if (collision.tag == "Ground")
        {
            isGround = true;
        }
        switch (collision.tag)
        {
            case "Ground": isGround = true; break; //지면 접촉
            case "Obstacle": OnTouch_Obstacles(); break; //장애물 접촉
        }
    }
    #region Player Jump
    public void OnTouch_Obstacles()
    {
        if (isGround == true) return;
        myRigidBody.velocity = myRigidBody.initFallVelocity;
        isFalling = true;
    }
    public void OnClick_Jump()
    {
        if (!isGround) return;
        isGround = false;
        isFalling = false;
        myRigidBody.velocity = myRigidBody.initJumpVelocity;
        StartCoroutine(Co_ForcedGravity()); //플레이어 중력
    }
    //캐싱해두기
    WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();
    IEnumerator Co_ForcedGravity()
    {
        float jumpVel = myRigidBody.jumpForce;
        yield return null;
        while (isGround == false)
        {
            yield return waitFixedUpdate;
            if (isFalling == true) //낙하중
            {
                //V1 = V0 - a*t  => 등가속도 일 경우 속력방정식
                myRigidBody.velocity = myRigidBody.velocity + (myRigidBody.accelerationG * Time.deltaTime);
                //S = V*t
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime * myRigidBody.velocity);
            }
            else
            {
                myRigidBody.velocity = myRigidBody.velocity + (jumpVel * Time.deltaTime);
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime * myRigidBody.velocity);
                jumpVel += myRigidBody.accelerationG;
                if (jumpVel <= 0) //점프 가속도가 0이 이하가 되면 낙하로 전환
                {
                    isFalling = true;
                }
            }
        }
        transform.position = new Vector2(0, bottomHeight + playerHeight * 0.5f);
    }
    #endregion

    #region Player Actions About Attack, Defence ...add some
    public void OnClick_Attack()
    {
        if (!canAttack || !canDeffence) return;
        Debug.Log("공격");
        canAttack = false;
        StartCoroutine(Co_ActionCoolTime(attackCoolTime, "Attack"));
    }
    public void OnClick_Deffence()
    {
        if (!canDeffence) return;
        Debug.Log("방어");
        canDeffence = false;
        StartCoroutine(Co_ActionCoolTime(DefenceCoolTime, "Deffence"));
        //animator.SetTrriger("Deffence",true);
    }
    /// <summary>
    /// 어떤 행동인지에 따라 체크할 코루틴 : actionCool = 특정 액션의 설정된 쿨타임, canUse = 특정 액션 쿨타임 상태값,
    /// testName = 액션 이름,
    /// </summary>
    /// <param name="actionCool"></param>
    /// <param name="canUse"></param>
    /// <param name="testName"></param>
    /// <returns></returns>
    IEnumerator Co_ActionCoolTime(float actionCool, string testName = "TestName")
    {
        float timer = 0;
        while (timer < actionCool)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        if (testName.CompareTo("Attack") == 0)
        {
            canAttack = true;
        }
        else
        {
            canDeffence = true;
        }
    }
    #endregion
}

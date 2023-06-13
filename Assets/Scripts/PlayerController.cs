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

    [Header("====Player StateCheck")]
    [SerializeField] private bool isGround = false;
    [SerializeField] private bool isFalling = true;
    [SerializeField] private bool canAttack = true;
    [SerializeField] private bool canDeffence = true;
    [SerializeField] private bool keepDeffence = false;
    [SerializeField] private bool canJumpSkill = true;
    [SerializeField] private bool canAttackSkill = true;

    [Header("====Player Value")]
    [SerializeField] private float bottomHeight;
    [SerializeField] private float playerHeight;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float DefenceCoolTime = 2;

    [Header("====Player Tools")]
    [SerializeField] private MyRigidBody myRigidBody = null;
    [SerializeField] private Animator myAnimator = null;
    [SerializeField] private GameObject weaponHitBox = null;
    [SerializeField] private GameObject shieldHitBox = null;

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
        if (collision.tag == "Ground")
        {
            isGround = true;
            gameObject.tag = "GroundPlayer";
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
        gameObject.tag = "Player";
        myRigidBody.velocity = myRigidBody.initJumpVelocity;
        StartCoroutine(Co_ForcedGravity()); //플레이어 중력
        
        //점프 애니메이션
        myAnimator.SetTrigger("Jump");
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
        if (!canAttack || keepDeffence) return;
        canAttack = false;
        int ran = Random.Range(0, 2);
        if(ran==0) myAnimator.SetTrigger("Attack1");
        else myAnimator.SetTrigger("Attack2");
        weaponHitBox.SetActive(true);
        StartCoroutine(Co_ActionCoolTime(attackCoolTime, "Attack",weaponHitBox));
    }
    public void OnClick_Deffence()
    {
        if (!canDeffence) return;
        canDeffence = false;
        keepDeffence = true;
        myAnimator.SetTrigger("Deffence");
        shieldHitBox.SetActive(true);
        StartCoroutine(Co_ActionCoolTime(DefenceCoolTime, "Deffence",shieldHitBox));
        StartCoroutine(Co_KeepTimer(shieldHitBox));
    }
    WaitForSeconds keepTime = new WaitForSeconds(1f);
    IEnumerator Co_KeepTimer(GameObject hitBox)
    {
        yield return keepTime;
        keepDeffence = false;
        hitBox.SetActive(false);
    }
    /// <summary>
    /// 어떤 행동인지에 따라 체크할 코루틴 : actionCool = 특정 액션의 설정된 쿨타임, canUse = 특정 액션 쿨타임 상태값,
    /// testName = 액션 이름,
    /// </summary>
    /// <param name="actionCool"></param>
    /// <param name="canUse"></param>
    /// <param name="actionName"></param>
    /// <returns></returns>
    IEnumerator Co_ActionCoolTime(float actionCool, string actionName,GameObject hitBox)
    {
        float timer = 0;
        while (timer < actionCool)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        if (actionName.CompareTo("Attack") == 0)
        {
            canAttack = true;
            hitBox.SetActive(false);
        }
        else
        {
            canDeffence = true;
        }
    }
    #endregion

    public void SetInitEquipment()
    {
        weaponHitBox.SetActive(false);
        shieldHitBox.SetActive(false);
    }
}

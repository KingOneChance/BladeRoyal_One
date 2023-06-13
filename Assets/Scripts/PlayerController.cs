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
        StartCoroutine(Co_ForcedGravity()); //�÷��̾� �߷�
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
            case "Ground": isGround = true; break; //���� ����
            case "Obstacle": OnTouch_Obstacles(); break; //��ֹ� ����
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
        StartCoroutine(Co_ForcedGravity()); //�÷��̾� �߷�
        
        //���� �ִϸ��̼�
        myAnimator.SetTrigger("Jump");
    }
    //ĳ���صα�
    WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();
    IEnumerator Co_ForcedGravity()
    {
        float jumpVel = myRigidBody.jumpForce;
        yield return null;
        while (isGround == false)
        {
            yield return waitFixedUpdate;
            if (isFalling == true) //������
            {
                //V1 = V0 - a*t  => ��ӵ� �� ��� �ӷ¹�����
                myRigidBody.velocity = myRigidBody.velocity + (myRigidBody.accelerationG * Time.deltaTime);
                //S = V*t
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime * myRigidBody.velocity);
            }
            else
            {
                myRigidBody.velocity = myRigidBody.velocity + (jumpVel * Time.deltaTime);
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime * myRigidBody.velocity);
                jumpVel += myRigidBody.accelerationG;
                if (jumpVel <= 0) //���� ���ӵ��� 0�� ���ϰ� �Ǹ� ���Ϸ� ��ȯ
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
    /// � �ൿ������ ���� üũ�� �ڷ�ƾ : actionCool = Ư�� �׼��� ������ ��Ÿ��, canUse = Ư�� �׼� ��Ÿ�� ���°�,
    /// testName = �׼� �̸�,
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

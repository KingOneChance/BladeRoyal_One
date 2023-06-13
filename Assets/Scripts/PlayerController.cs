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
        StartCoroutine(Co_ForcedGravity()); //�÷��̾� �߷�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ʈ���� ����");
        if (collision.tag == "Ground")
        {
            isGround = true;
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
        myRigidBody.velocity = myRigidBody.initJumpVelocity;
        StartCoroutine(Co_ForcedGravity()); //�÷��̾� �߷�
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
        if (!canAttack || !canDeffence) return;
        Debug.Log("����");
        canAttack = false;
        StartCoroutine(Co_ActionCoolTime(attackCoolTime, "Attack"));
    }
    public void OnClick_Deffence()
    {
        if (!canDeffence) return;
        Debug.Log("���");
        canDeffence = false;
        StartCoroutine(Co_ActionCoolTime(DefenceCoolTime, "Deffence"));
        //animator.SetTrriger("Deffence",true);
    }
    /// <summary>
    /// � �ൿ������ ���� üũ�� �ڷ�ƾ : actionCool = Ư�� �׼��� ������ ��Ÿ��, canUse = Ư�� �׼� ��Ÿ�� ���°�,
    /// testName = �׼� �̸�,
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

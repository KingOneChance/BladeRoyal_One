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
    [SerializeField] private bool canDeffence = true;
    [SerializeField] private bool keepDeffence = false;
    [SerializeField] private bool keepJumpSkill = false;
    [SerializeField] private bool canJumpSkill = true;
    [SerializeField] private bool canAttackSkill = true;

    [Header("====Player Value")]
    [SerializeField] private float bottomHeight;
    [SerializeField] private float playerHeight;
    [SerializeField] private float attackCoolTime = 0.5f;
    [SerializeField] private float defenceCoolTime = 2;
    [SerializeField] private float attackSkillDamage = 5;

    [Header("====Player Tools")]
    [SerializeField] private MyRigidBody myRigidBody = null;
    [SerializeField] private Animator myAnimator = null;
    [SerializeField] private Animator shieldAnimator = null;
    [SerializeField] private Animator attackAnimator = null;
    [SerializeField] private GameObject[] attackSkillWeapons = null;
    [SerializeField] private WeaponHitBox weaponHitBox = null;
    [SerializeField] private GameObject shieldHitBox = null;
    [SerializeField] private GameObject jumpImpactBox = null;
    [SerializeField] private GameObject jumpSkillHitBox = null;

    private void Start()
    {
        playerHeight = gameObject.transform.localScale.y;
    }
    private void OnEnable()
    {
        StartCoroutine(Co_ForcedGravity(myRigidBody.jumpForce)); //플레이어 중력
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGround = true;
            gameObject.tag = "GroundPlayer";
            transform.position = new Vector2(0, bottomHeight + playerHeight * 0.5f);
            SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.Randing);
        }
        switch (collision.tag)
        {
            case "Ground": OnTouch_Ground(); break; //지면 접촉
            case "Obstacle": OnTouch_Obstacles(); break; //장애물 접촉
        }
    }
    #region Player Jump
    public void ReversAccSet(float vel)
    {
        if (tag == "GroundPlayer") return;
        myRigidBody.velocity = vel;
    }
    public void OnTouch_Ground()
    {
        isGround = true;
        jumpImpactBox.SetActive(false);
        myRigidBody.velocity = 0;
    }
    public void OnTouch_Obstacles()
    {
        if (isGround == true) return;
        myRigidBody.velocity = myRigidBody.initFallVelocity;
        isFalling = true;
    }


    public void OnClick_Jump()
    {
        if (!isGround) return;
        isFalling = false;
        jumpImpactBox.SetActive(true);
        gameObject.tag = "Player";
        myRigidBody.velocity = myRigidBody.initJumpVelocity;
        SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.Jump);
        GameManager.Instance.AddJumpCount(); //점프스킬 카운트
        StartCoroutine(Co_ForcedGravity(myRigidBody.jumpForce)); //플레이어 중력

        //점프 애니메이션
        myAnimator.SetTrigger("Jump");
    }
    //캐싱해두기
    WaitForFixedUpdate waitFixedUpdate = new WaitForFixedUpdate();
    IEnumerator Co_ForcedGravity(float jumpForce)
    {
        isGround = false;
        float jumpVel = jumpForce;
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
                //V1 = V0 - a*t  => 등가속도 일 경우 속력방정식
                myRigidBody.velocity = myRigidBody.velocity + (jumpVel * Time.deltaTime);
                //S = V*t
                transform.position = new Vector2(0, transform.position.y + Time.deltaTime * myRigidBody.velocity);
                jumpVel += myRigidBody.accelerationG;
                if (jumpVel <= 0) //점프 가속도가 0이 이하가 되면 낙하로 전환
                {
                    isFalling = true;
                }
            }
        }
    }
    #endregion

    #region Player Actions About Attack, Defence ...add some
    public void OnClick_Attack()
    {
        if (keepDeffence || keepJumpSkill) return;
        StartAttackAnimation();
        weaponHitBox.gameObject.SetActive(true);
        StartCoroutine(Co_ActionCoolTime(attackCoolTime, "Attack", weaponHitBox.gameObject));
    }
    /// <summary>
    /// 공격 애니메이션 2개중 랜덤 진행
    /// </summary>
    private void StartAttackAnimation()
    {
        int ran = Random.Range(0, 2);
        if (ran == 0) myAnimator.SetTrigger("Attack1");
        else myAnimator.SetTrigger("Attack2");
    }
    public void OnClick_Deffence()
    {
        if (!canDeffence) return;
        GameManager.Instance.DeffenceCoolTime(defenceCoolTime);
        canDeffence = false;
        keepDeffence = true;
        SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.Sheild);
        myAnimator.SetTrigger("Deffence");
        shieldHitBox.SetActive(true);
        StartCoroutine(Co_ActionCoolTime(defenceCoolTime, "Deffence", shieldHitBox));
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
    IEnumerator Co_ActionCoolTime(float actionCool, string actionName, GameObject hitBox)
    {
        float timer = 0;
        while (timer < actionCool)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        if (actionName.CompareTo("Attack") == 0)
        {
            hitBox.SetActive(false);
        }
        else
        {
            canDeffence = true;
        }
    }
    #endregion

    /// <summary>
    /// 장비 히트박스 비활성화 함수
    /// </summary>
    public void SetInitEquipment()
    {
        weaponHitBox.gameObject.SetActive(false);
        shieldHitBox.SetActive(false);
    }

    public void JumpSkill()
    {
        //애니메이션 켜기
        myAnimator.SetTrigger("Jump");
        StartCoroutine(Co_SetJumpAnimation());

        //점프 뛰면서 공격하기 무기 콜라이더 활성화
        isFalling = false;
        keepJumpSkill = true;
        gameObject.tag = "Player";
        StopCoroutine("Co_SetJumpAnimation");

        myRigidBody.velocity = myRigidBody.initJumpVelocity;
        GameManager.Instance.ResetJumpCount(); //점프스킬 카운트

        //사운드 켜기
        SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.JumpSkill);
       
        StartCoroutine(Co_ForcedGravity(AllConst.skillJumpVelocity)); //플레이어 중력
    }
    IEnumerator Co_SetJumpAnimation()
    {
        jumpSkillHitBox.SetActive(true);
        shieldAnimator.SetBool("SetJumpSkill", true);
        yield return new WaitForSeconds(AllConst.jumpSkillKeepTime);
        shieldAnimator.SetBool("SetJumpSkill", false);
        keepJumpSkill = false;
        weaponHitBox.gameObject.SetActive(false);
        jumpSkillHitBox.SetActive(false);
    }
    public void AttackSkill()
    {
        //애니메이션 켜기 , 데미지 증가
        //보조 무기 활성화
        StartAttackAnimation();
        
        //사운드 켜기
        SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.AttackSkill);
        
        StartCoroutine(Co_SetAttackAnimation());
    }
    IEnumerator Co_SetAttackAnimation()
    {
        attackAnimator.SetTrigger("AttackSkill");
        foreach (var obj in attackSkillWeapons) obj.SetActive(true);
        int originDam = weaponHitBox.GetDamageValue();
        weaponHitBox.SetDamage(originDam * 2);

        yield return new WaitForSeconds(AllConst.attackSkillKeepTime);

        foreach (var obj in attackSkillWeapons) obj.SetActive(false);
        weaponHitBox.SetDamage(originDam);
        GameManager.Instance.ResetAttackCount();
    }
}

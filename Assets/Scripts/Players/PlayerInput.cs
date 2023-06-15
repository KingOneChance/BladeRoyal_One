using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerController myPlayer = null;
    [SerializeField] private Vector2 beginPosition;
    [SerializeField] private Vector2 endPosition;
    [SerializeField] private Vector2 distanceBE;
    [SerializeField] private bool isSwipe = false;
    [SerializeField] private bool canJumpSkill = false;
    [SerializeField] private bool canAttackSkill = false;
    [SerializeField] private bool isJumpSkill = false;
    [SerializeField] private bool isAttackSkill = false;

    public void SetJumpSkillState(bool can)
    {
        canJumpSkill = can;
        isJumpSkill = can;
    }
    public void SetAttackSkillState(bool can)
    {
        canAttackSkill = can;
        isAttackSkill = can;
    }
    public bool CanUseJumpSkill()
    {
        if (beginPosition.y < 700 && beginPosition.y > 60 && endPosition.y > beginPosition.y) //스킬 y 공통범위
        {
            if (beginPosition.x < 360 && beginPosition.x > 60 && endPosition.x < 360 && endPosition.x > 60)// 점프 스킬 x범위
            {
                if (Vector2.Distance(beginPosition, endPosition) > 300) //드래그 범위 길이 이상
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CanUseAttackSkill()
    {
        if (beginPosition.y < 700 && beginPosition.y > 60 && endPosition.y > beginPosition.y) //스킬 y 공통범위
        {
            if (beginPosition.x < 1040 && beginPosition.x > 740 && endPosition.x < 1040 && endPosition.x > 740)
            {
                if (Vector2.Distance(beginPosition, endPosition) > 300) //드래그 범위 길이 이상
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void Update()
    {

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            beginPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;
        }
#else

        if (Input.touchCount > 0) //모바일일 경우 중복 터지 방지
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
                beginPosition = touch.position;
            if (touch.phase == TouchPhase.Ended)
                endPosition = touch.position;

        }
#endif

        if (canJumpSkill == true && CanUseJumpSkill() == true)
        {
            if (isJumpSkill == true)
            {
                isJumpSkill = false;
                Debug.Log("점프 스킬 사용");
                myPlayer.JumpSkill();
            }
        }
        if (canAttackSkill == true && CanUseAttackSkill() == true)
        {
            if (isAttackSkill == true)
            {
                isAttackSkill = false;
                Debug.Log("공격스킬 사용");
                myPlayer.AttackSkill();
            }
        }
    }
}

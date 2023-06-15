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
        if (beginPosition.y > 60 && endPosition.y < 670) //��ų y �������
        {
            if (beginPosition.x > 60 && endPosition.x < 360)// ���� ��ų x����
            {
                if (Vector2.Distance(beginPosition, endPosition) > 300) //�巡�� ���� ���� �̻�
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool CanUseAttackSkill()
    {
        if (beginPosition.y > 60 && endPosition.y < 670) //��ų y �������
        {
            if (beginPosition.x > 740 && endPosition.x < 1040)// ���� ��ų x����
            {
                if (Vector2.Distance(beginPosition, endPosition) > 300) //�巡�� ���� ���� �̻�
                {
                    return true;
                }
            }
        }
        return false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beginPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endPosition = Input.mousePosition;
        }


        /*     if(Input.touchCount>0) //������� ��� �ߺ� ���� ����
            {
                Touch touch = Input.GetTouch(0);
                beginPosition = touch.position;
            }*/


        if (canJumpSkill == true && CanUseJumpSkill() == true)
        {
            if (isJumpSkill == true)
            {
                isJumpSkill = false;
                Debug.Log("���� ��ų ���");
                myPlayer.JumpSkill();
            }
        }
        if (canAttackSkill == true && CanUseAttackSkill() == true)
        {
            if (isAttackSkill == true)
            {
                isAttackSkill = false;
                Debug.Log("���ݽ�ų ���");
                myPlayer.AttackSkill();
            }
        }
    }
}
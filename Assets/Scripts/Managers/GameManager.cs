using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour
{
    #region singltone
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                    _instance = new GameObject().AddComponent<GameManager>();
            }
            return _instance;
        }
    }
    #endregion


    [SerializeField] private ObstacleData obstacleData = null;
    [SerializeField] private WeaponHitBox weaponHitBox = null;
    [SerializeField] private uint combo = 0;

    [Header("====GameScene")]
    [SerializeField] private uint points = 0;
    [SerializeField] private int nowStage = 0;
    [SerializeField] private int hearts = 3;
    [SerializeField] private bool isOver = false;

    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private int jumpCount = 0; //스킬위한 카운트
    [SerializeField] private int attackCount = 0; //스킬위한 카운트


    [Header("====UI Component")]
    [SerializeField] private GameSceneUiManager Ui_Manager = null;
    [SerializeField] private TextMeshProUGUI comboText = null;

    /// <summary>
    /// 클래스화 시켜서 가독성 높이도록 수정 필요
    /// </summary>
    public void GameSceneSetting()
    {
        weaponHitBox = FindObjectOfType<WeaponHitBox>();
        Ui_Manager = FindObjectOfType<GameSceneUiManager>();
        playerInput = FindObjectOfType<PlayerInput>();
        Ui_Manager.SetPointUI(points);
        hearts = 3;
        nowStage = 1;
        points = 0;
        isOver = false;
        jumpCount = 0;
        attackCount = 0;
    }
    public GameSceneUiManager GetUiManager() => Ui_Manager;
    public ObstacleData GetObstacleData() => obstacleData;
    /// <summary>
    /// 스코어 관리
    /// </summary>
    /// <param name="point"></param>
    public void AddPoints(uint point)
    {
        points += point;
        Ui_Manager.SetPointUI(points);
    }
    /// <summary>
    /// 점프시 카운트 추가, 카운트 이상시 스킬 사용 가능
    /// </summary>
    public void AddJumpCount()
    {
        if (jumpCount >= AllConst.jumpSkillCount) return;
        jumpCount++;
        Ui_Manager.SetJumpBar(jumpCount);
        //Ui 슬라이더 표시, 
        if (jumpCount >= AllConst.jumpSkillCount)
        {
            playerInput.SetJumpSkillState(true);
            //Ui 슬라이더 표시
            Ui_Manager.SetJumpBar(jumpCount);
            Ui_Manager.ShowJumpArrow(true);
        }
    }
    /// <summary>
    /// 점프 스킬 사용시 초기화
    /// </summary>
    public void ResetJumpCount()
    {
        jumpCount = 0;
        playerInput.SetJumpSkillState(false);
        Ui_Manager.ShowJumpArrow(false);
        Ui_Manager.SetJumpBar(0);
    }
    /// <summary>
    /// 몬스터 킬마다 추가
    /// </summary>
    public void AddAttackCount()
    {
        if (attackCount >= AllConst.attackSkillCount) return;
        attackCount++;
        Ui_Manager.SetAttackBar(attackCount);

        if (attackCount >= AllConst.attackSkillCount)
        {
            playerInput.SetAttackSkillState(true);
            Ui_Manager.SetAttackBar(attackCount);
            Ui_Manager.ShowAttackArrow(true);
        }
    }
    /// <summary>
    /// 공격 스킬 사용시 초기화
    /// </summary>
    public void ResetAttackCount()
    {
        attackCount = 0;
        playerInput.SetAttackSkillState(false);
        Ui_Manager.ShowAttackArrow(false);
        Ui_Manager.SetAttackBar(0);
    }

    #region 콤보관리
    public void ReSetCombo() => combo = 0;
    public void AddCombo()
    {

    }
    #endregion

    /// <summary>
    /// 게임 종료 및 죽음 관련 함수
    /// </summary>
    public void DieHearts()
    {
        if (isOver) return;
        hearts--;
        if (hearts == 0)
        {
            isOver = true;
            Ui_Manager.ShowGameOverUI();
        }
        Ui_Manager.DieHeartUI(hearts);
    }
    /// <summary>
    /// 스테이지 업
    /// </summary>
    public void StageUp() => nowStage++;
    public int GetStageNum() => nowStage;
}

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
    [SerializeField] private int jumpCount = 0; //��ų���� ī��Ʈ
    [SerializeField] private int attackCount = 0; //��ų���� ī��Ʈ


    [Header("====UI Component")]
    [SerializeField] private GameSceneUiManager Ui_Manager = null;
    [SerializeField] private TextMeshProUGUI comboText = null;

    /// <summary>
    /// Ŭ����ȭ ���Ѽ� ������ ���̵��� ���� �ʿ�
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
    /// ���ھ� ����
    /// </summary>
    /// <param name="point"></param>
    public void AddPoints(uint point)
    {
        points += point;
        Ui_Manager.SetPointUI(points);
    }
    /// <summary>
    /// ������ ī��Ʈ �߰�, ī��Ʈ �̻�� ��ų ��� ����
    /// </summary>
    public void AddJumpCount()
    {
        if (jumpCount >= AllConst.jumpSkillCount) return;
        jumpCount++;
        Ui_Manager.SetJumpBar(jumpCount);
        //Ui �����̴� ǥ��, 
        if (jumpCount >= AllConst.jumpSkillCount)
        {
            playerInput.SetJumpSkillState(true);
            //Ui �����̴� ǥ��
            Ui_Manager.SetJumpBar(jumpCount);
            Ui_Manager.ShowJumpArrow(true);
        }
    }
    /// <summary>
    /// ���� ��ų ���� �ʱ�ȭ
    /// </summary>
    public void ResetJumpCount()
    {
        jumpCount = 0;
        playerInput.SetJumpSkillState(false);
        Ui_Manager.ShowJumpArrow(false);
        Ui_Manager.SetJumpBar(0);
    }
    /// <summary>
    /// ���� ų���� �߰�
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
    /// ���� ��ų ���� �ʱ�ȭ
    /// </summary>
    public void ResetAttackCount()
    {
        attackCount = 0;
        playerInput.SetAttackSkillState(false);
        Ui_Manager.ShowAttackArrow(false);
        Ui_Manager.SetAttackBar(0);
    }

    #region �޺�����
    public void ReSetCombo() => combo = 0;
    public void AddCombo()
    {

    }
    #endregion

    /// <summary>
    /// ���� ���� �� ���� ���� �Լ�
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
    /// �������� ��
    /// </summary>
    public void StageUp() => nowStage++;
    public int GetStageNum() => nowStage;
}

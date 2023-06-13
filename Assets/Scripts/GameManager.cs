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

    [SerializeField] private PlayerController myPlayer = null;
    [SerializeField] private ObstacleData obstacleData = null;
    [SerializeField] private uint points = 0;
    [SerializeField] private uint combo = 0;
    [SerializeField] private int nowStage = 0;
    [SerializeField] private int hearts = 3;
    [SerializeField] private bool isOver = false;

    [Header("====UI Component")]
    [SerializeField] private TextMeshProUGUI pointText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private GameObject[] heartsBox = null;
    [SerializeField] private GameObject gameOver = null;
    private void Start()
    {
    }
    public ObstacleData GetObstacleData() => obstacleData;
    /// <summary>
    /// ���ھ� ����
    /// </summary>
    /// <param name="point"></param>
    public void AddPoints(uint point)
    {
        points += point;
        pointText.text = points.ToString();
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
            gameOver.SetActive(true);
        }
        heartsBox[hearts].gameObject.SetActive(false);
    }
    /// <summary>
    /// �������� ��
    /// </summary>
    public void StageUp() => nowStage++;
    public int GetStageNum() => nowStage;
}

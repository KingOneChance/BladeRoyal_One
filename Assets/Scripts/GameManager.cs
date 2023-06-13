using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    [SerializeField] private int nowStage = 1;
    [SerializeField] private int hearts = 3;
    [SerializeField] private bool isOver = false;

    [Header("====UI Component")]
    [SerializeField] private TextMeshProUGUI pointText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private RawImage[] heartsBox = null;
    [SerializeField] private GameObject gameOver = null;

    public ObstacleData GetObstacleData()
    {
        return obstacleData;
    }
    public void AddPoints(uint point)
    {
        points += point;
        pointText.text = points.ToString();
    }

    public void DieHearts()
    {
        hearts--;
        if(hearts==0)
        {
            isOver = true;
            gameOver.SetActive(true);   
        }
        heartsBox[hearts].gameObject.SetActive(false);
    }

    #region ÄÞº¸°ü¸®
    public void ReSetCombo() => combo = 0;
    public void AddCombo()
    {
        
    }
    #endregion

}

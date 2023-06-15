using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameSceneUiManager : MonoBehaviour
{
    [Header("====UI Component")]
    [SerializeField] private TextMeshProUGUI pointText = null;
    [SerializeField] private TextMeshProUGUI comboText = null;
    [SerializeField] private TextMeshProUGUI hpText = null;
    [SerializeField] private GameObject[] heartsBox = null;
    [SerializeField] private GameObject gameOver = null;
    [SerializeField] private Slider hpBar = null;
    [SerializeField] private Slider jumpBar = null;
    [SerializeField] private Slider attackBar = null;
    [SerializeField] private GameObject jumpArrow = null;
    [SerializeField] private GameObject attackArrow = null;

    [SerializeField] private float maxHP;
    private void Awake()
    {
        GameManager.Instance.GameSceneSetting();
    }
    public void ShowJumpArrow(bool show) => jumpArrow.SetActive(show);
    public void ShowAttackArrow(bool show) => attackArrow.SetActive(show);
    public void SetPointUI(uint point) => pointText.text = point.ToString();
    public void DieHeartUI(int hearts) => heartsBox[hearts].gameObject.SetActive(false);
    public void ShowGameOverUI() => gameOver.SetActive(true);
    public void SetJumpBar(float count) => jumpBar.value = (count / AllConst.jumpSkillCount) * 100;
    public void SetAttackBar(float count) => attackBar.value = (count / AllConst.attackSkillCount) * 100;
    public void SetHPBar(float hp)
    {
        hpBar.value = (hp / maxHP) * 100;
        hpText.text = $"HP : {hp}  /  {maxHP}";
    }
    public void SetMaxHP(float maxHP) => this.maxHP = maxHP;
}

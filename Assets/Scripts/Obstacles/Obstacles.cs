using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleState
{
    public float maxHP;
    public float nowHP;
    public uint exp;
    public int idx;
    public Sprite sprite;
    public bool isBoss = false;
}
public class Obstacles : MonoBehaviour
{
    [SerializeField] private WeaponHitBox weaponsHitBox = null;
    [SerializeField] private DiceShield diceShield = null;
    [SerializeField] private ObstaclePool myPool = null;
    [SerializeField] private GameSceneUiManager UI_Manager = null;
    [SerializeField] private ObstacleState myState = null;
    [SerializeField] private SpriteRenderer mySpriteRenderer = null;
    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out mySpriteRenderer);
        myState.nowHP = myState.maxHP;
        weaponsHitBox = FindObjectOfType<WeaponHitBox>();
        diceShield = FindObjectOfType<DiceShield>();
        UI_Manager = FindObjectOfType<GameSceneUiManager>();
        weaponsHitBox.del_ColideAction += TakeAction;
    }

    private void OnEnable()
    {
        if (GameManager.Instance.GetStageNum() > 1)
        {
            SetMyState();
            if (myState.idx == 0) UI_Manager.SetMaxHP(myState.maxHP);
        }
    }
    private void Start()
    {
        gameObject.AddComponent<PolygonCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "GroundPlayer": GameManager.Instance.DieHearts(); break;
            case "JumpSkill": GetDamage(weaponsHitBox.GetDamageValue()*2); break;
        }
    }
    private void TakeAction(ColiderBox coliderBox, int idx)
    {
        if (myState.idx != idx) return;
        switch (coliderBox)
        {
            case ColiderBox.Shield: ShieldAction(); break;
            case ColiderBox.Weapon: GetDamage(weaponsHitBox.GetDamageValue()); break;
        }
    }
    /// <summary>
    /// 사운드 혹은 이펙트 작업
    /// </summary>
    private void ShieldAction()
    {

    }
    public void SetPool(ObstaclePool pool) => myPool = pool;

    public void SetInit(int idx)
    {
        myState.maxHP = 1;
        myState.nowHP = 1;
        myState.exp = GameManager.Instance.GetObstacleData().GetExp(0);
        myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(0);
        mySpriteRenderer.sprite = myState.sprite;
        myState.idx = idx;
        UI_Manager.SetMaxHP(myState.maxHP);
        UI_Manager.SetHPBar(myState.maxHP);
    }
    private void SetMyState()
    {
        int stageNum = GameManager.Instance.GetStageNum();
        if (stageNum == AllConst.stageNum)
        {
            //보스 호출 후 리턴   
            if (myState.idx == 0)
            {
                GameManager.Instance.SetBossStage();
                myState.isBoss = true;
                myState.maxHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum - 1);
                myState.nowHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum - 1);
                myState.exp = GameManager.Instance.GetObstacleData().GetExp(stageNum - 1);
                myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum - 1);
                mySpriteRenderer.sprite = myState.sprite;
                UI_Manager.SetMaxHP(myState.maxHP); //max hp바 세팅
                UI_Manager.SetHPBar(myState.nowHP); //현재 hp바 세팅
            }
            else
            {
                myPool.EnqueuePool(gameObject);
            }
            return; 
        }
        myState.maxHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum - 1);
        myState.nowHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum - 1);
        myState.exp = GameManager.Instance.GetObstacleData().GetExp(stageNum - 1);
       
        if (stageNum < AllConst.stageNum-1)
        {
            int ran = Random.Range(0, 5); //확률
            if (ran == 0)
                myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum);
            else
                myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum - 1);
        }
        else
        {
            myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum - 1);
        }
        mySpriteRenderer.sprite = myState.sprite;
    }
    private void GetDamage(int damage)
    {
        myState.nowHP -= damage;
        if (myState.nowHP <= 0)
        {
            if (myState.isBoss == true)
            {
                GameManager.Instance.StageClear();
                return;
            }
            UI_Manager.SetHPBar(myState.maxHP); //죽고나면 초기화 세팅
            weaponsHitBox.SetFirstObstacleIndex(myState.idx + 1);
            GameManager.Instance.AddPoints(myState.exp);
            GameManager.Instance.AddAttackCount(); //공격스킬 카운트
            myPool.EnqueuePool(gameObject);
        }
        else
        {
            UI_Manager.SetHPBar(myState.nowHP); //현재 hp바 세팅
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObstacleState
{
    public double maxHP;
    public double nowHP;
    public uint exp;
    public int idx;
    public Sprite sprite;
}
public class Obstacles : MonoBehaviour
{
    [SerializeField] private DiceWeapon diceWeapon = null;
    [SerializeField] private DiceShield diceShield = null;
    [SerializeField] private ObstaclePool myPool = null;

    [SerializeField] private ObstacleState myState;
    [SerializeField] private SpriteRenderer mySpriteRenderer;
    private void Awake()
    {
        TryGetComponent<SpriteRenderer>(out mySpriteRenderer);
        myState.nowHP = myState.maxHP;
        diceWeapon = FindObjectOfType<DiceWeapon>();
        diceShield = FindObjectOfType<DiceShield>();
        diceWeapon.del_ColideAction += TakeAction;
    }
    private void OnEnable()
    {
        if(GameManager.Instance.GetStageNum()>1)
            SetMyState();
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
            case "Weapon": GetDamage(diceWeapon.GetDamageValue()); break;
        }
    }
    private void TakeAction(ColiderBox coliderBox, int idx)
    {
        if (myState.idx != idx) return;
        switch (coliderBox)
        {
            case ColiderBox.Shield: ShieldAction(); break;
            case ColiderBox.Weapon: GetDamage(diceWeapon.GetDamageValue()); break;
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
    }
    private void SetMyState()
    {
        int stageNum = GameManager.Instance.GetStageNum();
        myState.maxHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum-1);
        myState.nowHP = GameManager.Instance.GetObstacleData().GetMaxHp(stageNum - 1);
        myState.exp = GameManager.Instance.GetObstacleData().GetExp(stageNum - 1);
        int ran = Random.Range(0, 5);
        if (ran == 0)
            myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum);
        else
            myState.sprite = GameManager.Instance.GetObstacleData().GetSprite(stageNum - 1);

        mySpriteRenderer.sprite = myState.sprite;
    }
    private void GetDamage(int damage)
    {
        myState.nowHP-= damage;
        if (myState.nowHP <= 0)
        {
            diceWeapon.SetFirstObstacleIndex(myState.idx + 1);
            GameManager.Instance.AddPoints(myState.exp);

            myPool.EnqueuePool(gameObject);
        }
    }
}

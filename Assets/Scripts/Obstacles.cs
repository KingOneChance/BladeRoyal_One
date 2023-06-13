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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "GroundPlayer": GameManager.Instance.DieHearts(); break;
            case "Weapon": GetDamage(); break;
        }
    }
    private void TakeAction(ColiderBox coliderBox, int idx)
    {
        if (myState.idx != idx) return;
        switch (coliderBox)
        {
            case ColiderBox.Shield: ShieldAction(); break;
            case ColiderBox.Weapon: GetDamage(); break;
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

    private void GetDamage()
    {
        myState.nowHP--;
        if (myState.nowHP <= 0)
        {
            diceWeapon.SetFirstObstacleIndex(myState.idx + 1);
            GameManager.Instance.AddPoints(myState.exp);


            myPool.EnqueuePool(gameObject);
        }
    }
}

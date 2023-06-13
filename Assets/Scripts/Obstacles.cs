using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColiderBox
{
    Weapon,
    Shield,
    PlayerBody
}
public delegate void Del_ColideAction(ColiderBox coliderBox);
[System.Serializable]
public class ObstacleState
{
    public double maxHP;
    public double nowHP;
    public Sprite sprite;
}
public class Obstacles : MonoBehaviour
{
    [SerializeField] private DiceWeapon diceWeapon = null;
    [SerializeField] private DiceShield diceShield = null;
    [SerializeField] private ObstaclePool myPool = null;

    [SerializeField] private ObstacleState myState;

    private void Awake()
    {
        myState.nowHP = myState.maxHP;
        diceWeapon = FindObjectOfType<DiceWeapon>();
        diceShield = FindObjectOfType<DiceShield>();
        diceWeapon.del_ColideAction += TakeAction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            GetDamage();
        }
    }
    private void TakeAction(ColiderBox coliderBox)
    {
        switch (coliderBox)
        {
            case ColiderBox.Shield: ShieldAction(); break;
            case ColiderBox.Weapon: GetDamage(); break;
        }
    }
    private void ShieldAction()
    {

    }

    public void SetPool(ObstaclePool pool) => myPool = pool;

    public void SetInit()
    {
        myState.maxHP = 1;
        myState.nowHP = 1;
    }

    private void GetDamage()
    {
        myState.nowHP--;
        if (myState.nowHP <= 0) myPool.EnqueuePool(gameObject);
    }
}

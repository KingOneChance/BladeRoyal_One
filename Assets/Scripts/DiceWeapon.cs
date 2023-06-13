using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceWeapon : MonoBehaviour
{
    public Del_ColideAction del_ColideAction;
    [SerializeField] private int firstObIdx=0;
    [SerializeField] private int damage=2;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            del_ColideAction(ColiderBox.Weapon, firstObIdx);
        }
    }
    public void SetFirstObstacleIndex(int idx)
    {
        firstObIdx = idx;
        if (idx >= AllConst.poolingNum) firstObIdx = 0;
    }
    public int GetDamageValue() => damage;
    public void SetDamage(int dam) => damage = dam;
    public void AddDamage(int dam) => damage += dam;
    
}

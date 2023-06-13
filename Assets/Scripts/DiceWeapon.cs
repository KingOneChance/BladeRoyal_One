using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceWeapon : MonoBehaviour
{
    public Del_ColideAction del_ColideAction;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        del_ColideAction(ColiderBox.Weapon);
    }
}

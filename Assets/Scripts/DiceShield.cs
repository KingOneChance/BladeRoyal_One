using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceShield : MonoBehaviour
{
    public Del_ColideAction del_ColideAction;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        del_ColideAction(ColiderBox.Shield);
    }
}

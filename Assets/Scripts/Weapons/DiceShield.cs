using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceShield : MonoBehaviour
{
    [SerializeField] private PlayerController myPlayer = null;
    [SerializeField] private bool isDeffence = false;
    [SerializeField] private uint point =50;
    private void OnEnable()
    {
        isDeffence = false; 
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDeffence) return;
        if(collision.gameObject.tag=="Obstacle")
        {
            isDeffence = true;
            GameManager.Instance.AddPoints(point);
            myPlayer.ReversAccSet(AllConst.rigidFallingSpeed);
            //사운드 재생
        }
    }
}

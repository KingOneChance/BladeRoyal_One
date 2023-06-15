using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Orb : AllWeapon
{
    [SerializeField] private GameObject[] orbs = null;
    [Range(0,5)]
    [SerializeField] private float moveSpeed;
    public override void WeaponAnimatiion()
    {
        //012 중 1번 중심으로 좌우로 퍼짐
        StartCoroutine(Co_OrbXMove());
    }
    WaitForFixedUpdate wait = new WaitForFixedUpdate();
    IEnumerator Co_OrbXMove()
    {
        int i = 0; //fixedupdate 주기 0.02s;
        while (i<5)
        {
            yield return wait;
            orbs[0].transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            orbs[2].transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            i++;
        }
        while (i < 10)
        {
            yield return wait;
            orbs[0].transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            orbs[2].transform.Translate(-Vector2.right * moveSpeed * Time.deltaTime);
            i++;
        }
    }
}

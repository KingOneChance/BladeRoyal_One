using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpColider : MonoBehaviour
{
    [SerializeField] private PlayerController myPlayer = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            myPlayer.ReversAccSet(AllConst.rigidImpactSpeed);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject camearPos = null;
    [SerializeField] private GameObject myPlayer = null;
    void Update()
    {
        if (myPlayer.transform.position.y > camearPos.transform.position.y)
            gameObject.transform.position = new Vector3(0, myPlayer.transform.position.y, -10);
    }
}

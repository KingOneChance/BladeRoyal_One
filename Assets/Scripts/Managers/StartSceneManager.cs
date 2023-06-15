using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject gameManager = null;
    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            DontDestroyOnLoad(Instantiate(gameManager));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSize : MonoBehaviour
{
    private void Start()
    {
        SetScreenSize();
    }
    public void SetScreenSize()
    {
        int setWidth = 1080;
        int setHeight = 1920;

        Screen.SetResolution(setWidth, setHeight, true);
    }
        
}

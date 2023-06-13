using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils { }
public static class AllConst
{
    public const int poolingNum = 6;
    public const int stageNum = 10;
    public const int waveNum = 2;
}
public enum ColiderBox
{
    Weapon,
    Shield,
    PlayerBody
}
public delegate void Del_ColideAction(ColiderBox coliderBox, int idx);

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils { }
public static class AllConst
{
    public const int poolingNum = 6;
    public const int stageNum = 6;
    public const int waveNum = 2;
    public const int rigidFallingSpeed = -10;
    public const int rigidImpactSpeed = -5;
    public const uint jumpSkillCount = 5;
    public const uint attackSkillCount = 10;
    public const float jumpSkillKeepTime = 0.5f;
    public const float attackSkillKeepTime = 3f;
    public const float skillJumpVelocity = 75;
}
public enum ColiderBox
{
    Weapon,
    Shield,
    PlayerBody
}
public delegate void Del_ColideAction(ColiderBox coliderBox, int idx);

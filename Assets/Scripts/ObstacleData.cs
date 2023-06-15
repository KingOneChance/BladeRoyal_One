using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesData", menuName = "ObstaclesData")]
public class ObstacleData : ScriptableObject
{
    [SerializeField] private int[] maxHp = new int[AllConst.stageNum];
    [SerializeField] private uint[] exp = new uint[AllConst.stageNum];
    [SerializeField] Texture2D[] textures;
    [SerializeField] Sprite[] sprites;
    public Texture2D GetTexture(int num) => textures[num];
    public Sprite GetSprite(int num) => sprites[num];
    public int GetMaxHp(int stageNum) => maxHp[stageNum];
    public uint GetExp(int stageNum) => exp[stageNum];
    private void Awake() //hp를 제곱형태로 취함
    {
        maxHp[0] = 1;
        exp[0] = 10;
        for (int i = 1; i < AllConst.stageNum; i++)
        {
            maxHp[i] = maxHp[i - 1] * 2;
            exp[i] = exp[i - 1] + 10;
        }
    }
}

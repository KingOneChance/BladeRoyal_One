using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstaclesTextureData", menuName = "ObstacleTexture")]
public class ObstacleTextureData : ScriptableObject
{
    [SerializeField] Texture2D[] textures;
    [SerializeField] Sprite[] sprites;
    public Texture2D GetTexture(int num) => textures[num];
    public Sprite GetSprite(int num) => sprites[num];

}

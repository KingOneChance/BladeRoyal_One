using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField] private Button[] stages = null;
    [SerializeField] private GameObject gameManager = null;
    [SerializeField] private GameObject moveDice1 = null;
    [SerializeField] private GameObject moveDice2 = null;
    [SerializeField] private int moveDice1Freq = 100;
    [SerializeField] private int moveDice2Freq = 200;
    [Range(-1, 1)]
    [SerializeField] private float dice1MoveX;
    [Range(-1, 1)]
    [SerializeField] private float dice1MoveY;
    [Range(-1, 1)]
    [SerializeField] private float dice2MoveX;
    [Range(-1, 1)]
    [SerializeField] private float dice2MoveY;
    private void Awake()
    {
        if (FindObjectOfType<GameManager>() == null)
        {
            DontDestroyOnLoad(Instantiate(gameManager));
        }
        SoundsMananager.Instance.SetSoundSourceToDic();

        foreach (var button in stages)
            button.interactable = false;
    }
    private void Start()
    {
        stages[0].interactable = true;
        SoundsMananager.Instance.StartSceneBGM();

        //끊임없이 회전
        StartCoroutine(Co_MoveDiceImage(dice1MoveX, dice1MoveY, moveDice1Freq, moveDice1));
        StartCoroutine(Co_MoveDiceImage(dice2MoveX, dice2MoveY, moveDice1Freq, moveDice2));
    }
    /// <summary>
    ///     /// x,y 범위 -1~1
    /// x = x축 움직임, y = y축 움직임 , dice= 다이스그림, count 회전 빈도
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="count"></param>
    /// <param name="dice"></param>
    /// <returns></returns>
    IEnumerator Co_MoveDiceImage(float x, float y, int count, GameObject dice)
    {
        Vector3 dir = new Vector3(1 * x, 1 * y, 0);
        int num = 0;
        while (num < count)
        {
            yield return null;
            num++;
            dice.transform.position = dice.transform.position + dir * Time.deltaTime;
        }
        StartCoroutine(Co_MoveDiceImage(-x, -y, count, dice));
    }
    public void PlayButtonSound() => SoundsMananager.Instance.TurnOnPlayerOnShot(EffectSoundType.Button);
    public void SetMapStageNum(int num) => GameManager.Instance.SetMapStageNum(num);
}

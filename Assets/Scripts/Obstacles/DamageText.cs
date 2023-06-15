using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text = null;
    [SerializeField] private WeaponHitBox weaponHitBox = null;
    [SerializeField] private GameObject ui_Canvas=null;
    [SerializeField] private int damage;
    [SerializeField] private float alphFade;
    [SerializeField] private Color mycolor;
    private void Awake()
    {
        mycolor = text.color;
        weaponHitBox = FindObjectOfType<WeaponHitBox>();
        ui_Canvas = GameObject.FindGameObjectWithTag("UiCanvas");
    }
    private void OnEnable()
    {
        damage = weaponHitBox.GetDamageValue();
        text.text = damage.ToString();
        text.color = mycolor;
        StartCoroutine(Co_FadeOut());
    }
    IEnumerator Co_FadeOut()
    {
        while (text.color.a > 0)
        {
            yield return null;
            Color color = text.color;
            color.a -= Time.deltaTime * alphFade;
            text.color = color;
            text.transform.Translate(Vector3.up * 0.02f + Vector3.right * 0.01f);
        }
        weaponHitBox.EnpoolText(gameObject);
    }

    void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }
}

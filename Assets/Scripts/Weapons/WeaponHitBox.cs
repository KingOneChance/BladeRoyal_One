using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBox : MonoBehaviour
{
    public Del_ColideAction del_ColideAction;
    [SerializeField] private int firstObIdx = 0;
    [SerializeField] private int damage = 2;
    [SerializeField] private AllWeapon nowWeapon = null;

    private void Awake()
    {
        nowWeapon = FindObjectOfType<AllWeapon>();
    }
    private void OnEnable()
    {
        nowWeapon.transform.Translate(Vector2.up * 0.8f);
        nowWeapon.WeaponAnimatiion();
    }
    private void OnDisable()
    {
        nowWeapon.transform.Translate(Vector2.up * -0.8f);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            del_ColideAction(ColiderBox.Weapon, firstObIdx);
            Vector3 tempPos = gameObject.transform.position + Vector3.up * 0.8f;
            ObjectPooler.SpawnFromPool<DamageText>("DamageText", tempPos);
        }
    }
    public void EnpoolText(GameObject obj) => obj.SetActive(false);
    public void SetFirstObstacleIndex(int idx)
    {
        firstObIdx = idx;
        if (idx >= AllConst.poolingNum) firstObIdx = 0;
    }
    public int GetDamageValue() => damage;
    public void SetDamage(int dam) => damage = dam;
    public void AddDamage(int dam) => damage += dam;
}

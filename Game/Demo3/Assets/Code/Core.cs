using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var damage = collision.gameObject.GetComponent<Bullet>().Damage;
        SufferDamage(damage);
    }

    public void Initialize()
    {
        Hp = MAX_HP;
        SetMaterial();
    }

    public void SufferDamage(float value)
    {
        Hp -= value;
        SetMaterial();
        if (Hp <= 0)
        {
            GameCenter.Instance.GameOver();
        }
    }

    private void SetMaterial()
    {
        var r = Hp / MAX_HP * 0.7f + 0.3f;
        var color = _renderer.material.GetColor("_Color");
        color.a = r;
        _renderer.material.SetColor("_Color", color);
    }

    private float Hp;
    private float CurrentHp;
    private const float MAX_HP = 100f;

    private MeshRenderer _renderer;
    private Color _targetColor;
}

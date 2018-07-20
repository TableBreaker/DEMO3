using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        UpdateLerpColor();
    }

    public void Initialize()
    {
        Hp = MAX_HP;
    }

    public void SufferDamage(float value)
    {
        Hp -= value;
        
    }

    private void UpdateLerpColor()
    {
        if (!_startLerp) return;

        
    }

    private void SetMaterial()
    {
        var r = Hp / MAX_HP * 0.7f + 0.3f;
        var color = _renderer.material.GetColor("_Color");
        color.a = r;
        _renderer.material.SetColor("_Color", color);
        _startLerp = true;
    }

    private float Hp;
    private const float MAX_HP = 100f;

    private MeshRenderer _renderer;
    private Color _targetColor;
    private bool _startLerp;
    private float _lerpTime;
    private float _totalLerpTime;
}

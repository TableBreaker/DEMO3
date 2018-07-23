using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void Initialize(MoveData data, Transmitter trans)
    {
        _selfTransmitter = trans;
        _moveData = data;
        transform.position = _moveData.StartPos;
        _moveTime = 0f;
        Damage = 10f;
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void OnMouseDown()
    {
        if (!_selfTransmitter)
            return;

        GameCenter.Instance.AddScore(_selfScore);
        _selfTransmitter.EliminateBullet(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_selfTransmitter)
            return;

        _selfTransmitter.EliminateBullet(this);
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(_moveData.StartPos, _moveData.EndPos, _moveTime / _moveData.MoveDuration);
        _moveTime += Time.deltaTime;
    }

    public float Damage { get; private set; }

    private Transmitter _selfTransmitter;
    private MoveData _moveData;

    private float _moveTime;
    private int _selfScore = 10;
}

public class MoveData
{
    public Vector3 StartPos;
    public Vector3 EndPos;
    public float MoveDuration;
}
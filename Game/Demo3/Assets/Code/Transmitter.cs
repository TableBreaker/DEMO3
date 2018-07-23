using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transmitter : MonoBehaviour
{
    private void Awake()
    {
        _bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        _topRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0f));
    }

    public void Initialize()
    {
        _currentTime = 0f;
        _interval = INTERVAL;
        _bulletDuration = BULLET_DURATION;
        _init = true;
    }

    public void Shutdown()
    {
        foreach (var bullet in _bulletList)
        {
            Destroy(bullet.gameObject);
        }
        _bulletList.Clear();
        _init = false;
    }

    private void Update()
    {
        if (!_init)
            return;

        _currentTime += Time.deltaTime;
        UpdateTransmit();
    }

    private void UpdateTransmit()
    {
        if (_currentTime < _interval)
            return;

        SpawnBulletRandom();
        _currentTime = 0f;
    }

    private void SpawnBulletRandom()
    {
        var pos = (int)(UnityEngine.Random.value * 4);
        Vector3 spawnPos = Vector3.zero;
        var value = UnityEngine.Random.value;
        switch(pos)
        {
            case 0:
                var x = Mathf.Lerp(_bottomLeft.x, _topRight.x, value);
                spawnPos = new Vector3(x, _topRight.y, 0f);
                break;
            case 1:
                var y = Mathf.Lerp(_bottomLeft.y, _topRight.y, value);
                spawnPos = new Vector3(_topRight.x, y, 0f);
                break;
            case 2:
                var x2 = Mathf.Lerp(_bottomLeft.x, _topRight.x, value);
                spawnPos = new Vector3(x2, _bottomLeft.y, 0f); 
                break;
            case 3:
                var y2 = Mathf.Lerp(_bottomLeft.y, _topRight.y, value);
                spawnPos = new Vector3(_bottomLeft.x, y2, 0f);
                break;

            default:
                return;
        }

        var bullet = Instantiate(_bulletPrefab, spawnPos, Quaternion.identity).GetComponent<Bullet>();
        var moveData = new MoveData
        {
            StartPos = spawnPos,
            EndPos = Vector3.zero,
            MoveDuration = _bulletDuration,
        };
        bullet.Initialize(moveData, this);
        _bulletList.Add(bullet);
    }

    public void EliminateBullet(Bullet bullet)
    {
        _bulletList.Remove(bullet);
        Destroy(bullet.gameObject);
    }

    public void UpgradeDifficulty()
    {
        if (_interval > 0.1f)
            _interval -= 0.1f;

        if (_bulletDuration > 0.2f)
            _bulletDuration -= 0.2f;
    }

    private List<Bullet> _bulletList = new List<Bullet>();
    private bool _init;
    private float _currentTime;
    private float _interval;
    private float _bulletDuration;

    private Vector3 _bottomLeft;
    private Vector3 _topRight;

    [SerializeField]
    private GameObject _bulletPrefab;

    private const float INTERVAL = 1f;
    private const float BULLET_DURATION = 3f;
}

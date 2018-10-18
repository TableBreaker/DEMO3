using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHitType
{
    LeftRight,
    BottomLeftTopRight,
    BottomTop,
    BottomRightTopLeft,
    RightLeft,
    TopRightBottomLeft,
    TopBottom,
    TopLeftBottomRight,

    SingleTouch,
    DoubleTouch,
}

public class HitTest : MonoBehaviour
{
    private void Update()
    {
        UpdateInput();
        UpdateClickList();
    }
    
    private void UpdateInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            // per frame test
            var ray = Camera.main.ScreenPointToRay(_currentFramePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var handler = hit.collider.GetComponent<HitHandler>();

                if (hit.collider.gameObject && Time.time < GetTimeFromList(hit.collider.gameObject) + DOUBLE_CLICK_INTERVAL)
                {
                    RemoveFromList(hit.collider.gameObject);
                    handler?.OnHit?.Invoke(EHitType.DoubleTouch);
                }
                else
                {
                    handler?.OnHit?.Invoke(EHitType.SingleTouch);
                    AddToClickList(hit.collider.gameObject);
                }
            }

            _frameCount = 0;
        }

        if (Input.GetMouseButton(0))
        {
            _currentFramePos = Input.mousePosition;

            // between two frame test
            if (_frameCount > 0)
            {
                var _lastWorldPos = Camera.main.ScreenToWorldPoint(_lastFramePos);
                var _currentWorldPos = Camera.main.ScreenToWorldPoint(_currentFramePos);

                _lastWorldPos.z = 0f;
                _currentWorldPos.z = 0f;
                var dir = _currentWorldPos - _lastWorldPos;
                var distance = dir.magnitude;
                var hitDir = DirToHitDir(dir);
                var ray = new Ray(_lastWorldPos, dir);
                var hits = Physics.RaycastAll(ray, distance);
                foreach (var ht in hits)
                {
                    var handler = ht.collider.GetComponent<HitHandler>();
                    // 可能上面已经destroy了
                    handler?.OnHit?.Invoke(hitDir);
                }
            }

            _lastFramePos = _currentFramePos;
            _frameCount++;
        }
    }

    private void UpdateClickList()
    {
        for (var i = _singleClickList.Count - 1; i >= 0; i--)
        {
            var pair = _singleClickList[i];
            var time = pair.Key;
            var go = pair.Value;

            if (Time.time > time + DOUBLE_CLICK_INTERVAL)
            {
                _singleClickList.Remove(pair);
            }
        }
    }

    private void AddToClickList(GameObject obj)
    {
        if (ContainsObj(obj))
            return;

        _singleClickList.Add(new KeyValuePair<float, GameObject>(Time.time, obj));
    }

    private void RemoveFromList(GameObject obj)
    {
        KeyValuePair<float, GameObject>? p = null;
        foreach (var pair in _singleClickList)
        {
            if (pair.Value != obj) continue;

            p = pair;
            break;
        }

        if (p != null)
        {
            _singleClickList.Remove(p.GetValueOrDefault());
        }
    }

    private bool ContainsObj(GameObject obj)
    {
        foreach (var pair in _singleClickList)
        {
            if (pair.Value == obj)
                return true;
        }

        return false;
    }

    private float GetTimeFromList(GameObject obj)
    {
        foreach (var pair in _singleClickList)
        {
            if (pair.Value == obj)
                return pair.Key;
        }

        return -1f;
    }

    private EHitType DirToHitDir(Vector3 dir)
    {
        var angle = Vector3.SignedAngle(dir, Vector3.right, -Vector3.forward);
        angle = angle > 0f ? angle : angle + 360f;
        return (EHitType)(((int)(angle + 22.5f) / 45) % 8);
    }

    private Vector3 _lastFramePos;
    private Vector3 _currentFramePos;
    private int _frameCount = 0;

    private List<KeyValuePair<float, GameObject>> _singleClickList = new List<KeyValuePair<float, GameObject>>();

    private const float DOUBLE_CLICK_INTERVAL = 0.5f;
}

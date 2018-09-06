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
        if (Input.GetMouseButtonUp(0))
        {
            // per frame test
            var ray = Camera.main.ScreenPointToRay(_currentFramePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var handler = hit.collider.GetComponent<HitHandler>();
                handler?.OnHit?.Invoke(EHitType.SingleTouch);
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
    
    private EHitType DirToHitDir(Vector3 dir)
    {
        var angle = Vector3.SignedAngle(dir, Vector3.right, -Vector3.forward);
        angle = angle > 0f ? angle : angle + 360f;
        return (EHitType)(((int)(angle + 22.5f) / 45) % 8);
    }

    private Vector3 _lastFramePos;
    private Vector3 _currentFramePos;
    private int _frameCount = 0;
}

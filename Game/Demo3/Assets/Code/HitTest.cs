using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // per frame test
            _currentFramePos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(_currentFramePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var handler = hit.collider.GetComponent<HitHandler>();
                handler?.OnHit?.Invoke();
            }

            // between two frame test
            if (_frameCount > 0)
            {
                var _lastWorldPos = Camera.main.ScreenToWorldPoint(_lastFramePos);
                var _currentWorldPos = Camera.main.ScreenToWorldPoint(_currentFramePos);

                _lastWorldPos.z = 0f;
                _currentWorldPos.z = 0f;
                var dir = _currentWorldPos - _lastWorldPos;
                var distance = dir.magnitude;
                ray = new Ray(_lastWorldPos, dir);
                var hits = Physics.RaycastAll(ray, distance);
                foreach (var ht in hits)
                {
                    var handler = ht.collider.GetComponent<HitHandler>();

                    // 可能上面已经destroy了
                    handler?.OnHit?.Invoke();
                }
            }

            _lastFramePos = _currentFramePos;
            _frameCount++;
        }

        if (Input.GetMouseButtonUp(0))
        {
            _frameCount = 0;
        }
    }

    private Vector3 _lastFramePos;
    private Vector3 _currentFramePos;
    private int _frameCount = 0;
}

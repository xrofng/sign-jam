using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomationUIFollowWorld : Automation
{
    [SerializeField] Transform FollowingTarget;
    [SerializeField] Vector3 Offset;
    [SerializeField] bool IsLerping = false;
    [SerializeField] float Speed = 7;
    public float CurrentSpeed => Speed;
    [SerializeField] Vector3 DirectionalMultiplier = Vector3.one;

    private Camera _cam;
    private Vector3 _nextPos;
    private Vector3 _targetPos;

    protected override void Initialization()
    {
        base.Initialization();
        _cam = Camera.main;
    }

    protected override void ProcessAutomation()
    {
        base.ProcessAutomation();

        if (!FollowingTarget)
        {
            return;
        }

        _targetPos = _cam.WorldToScreenPoint(FollowingTarget.position + Offset);
        _nextPos = _targetPos;
        if (IsLerping)
        {
            _nextPos = transform.position;
            _nextPos.x = Mathf.Lerp(_nextPos.x, _targetPos.x, Time.deltaTime * Speed * DirectionalMultiplier.x);
            _nextPos.y = Mathf.Lerp(_nextPos.y, _targetPos.y, Time.deltaTime * Speed * DirectionalMultiplier.y);
            _nextPos.z = Mathf.Lerp(_nextPos.z, _targetPos.z, Time.deltaTime * Speed * DirectionalMultiplier.z);
        }
        transform.position = _nextPos;
    }

    public void SetFollowingTarget(Transform target)
    {
        FollowingTarget = target;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }

    public bool IsInReachingLenght(float length = 0.75f)
    {
        return Vector3.Distance(transform.position, _targetPos) < length;
    }

    public void SetOffset(Vector3 offset)
    {
        Offset = offset;
    }
}

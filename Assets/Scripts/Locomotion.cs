using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion 
{
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; set; }
    private Vector3 _direction;
    public Vector3 Direction { get { return _direction; } }
    public float NormalizedJump { get; set; }

    private AnimationCurve _jumpPowerCurve;
    private float _rotationSpeed;
    private float _speed;
    private Vector3 _velRef;
    private Transform _transform;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    public void Setup(Transform transform, CapsuleCollider collider, AnimationCurve curve, float rotationSpeed, float speed, Rigidbody rigigbody)
    {
        _transform = transform;
        _collider = collider;
        _jumpPowerCurve = curve;
        _rotationSpeed = rotationSpeed; 
        _speed = speed;
        _rigidbody = rigigbody;
    }

    public void Tick(float delta)
    {
        GroundCheck();
        
    }

    private void GroundCheck()
    {
        int mask = LayerMask.GetMask("Ground");
        Vector3 start = _transform.position + _collider.center;
        Vector3 extents = Vector3.one * (_collider.radius * 0.5f);
        extents.y = 0f;
        if(Physics.Raycast (_transform.position + _collider.center, Vector3.down, out RaycastHit hit, 100f, mask, QueryTriggerInteraction.Ignore))
        //if (Physics.BoxCast(start, extents, Vector3.down, out RaycastHit hit, _transform.rotation, 100f, mask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(_transform.position + _collider.center, hit.point, Color.red);
            IsGrounded = hit.distance < (_collider.center.y + 0.1f);
        }
        else
            IsGrounded = false;
    }

    public void Move(Vector3 normalizedDirection)
    {
        //get a normalzied angle between the current forward direction and the input direction
        float normalziedAngle = Vector3.Angle(_transform.forward, normalizedDirection) / 180f;

        //check that we have any input from the player before we rotate
        if (normalizedDirection.magnitude > 0f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(normalizedDirection, Vector3.up);    //<-- create rotation from a vector direction
            float rotSpeed = Mathf.Lerp(_rotationSpeed * 0.25f, _rotationSpeed, normalziedAngle);
            _transform.rotation = Quaternion.RotateTowards(_transform.rotation, targetRotation, rotSpeed * Time.fixedDeltaTime);  //<-- smooth rotate a quaternion towards another quaternion
        }

        float finalSpeed = Mathf.Lerp(_speed, _speed * 0.25f, normalziedAngle);
        normalizedDirection = normalizedDirection * (finalSpeed * Time.fixedDeltaTime);

        //smooth the direction to remove sudden stopps, "jitter" and a sensation of no "weight"
        _direction = Vector3.SmoothDamp(Direction, normalizedDirection, ref _velRef, Time.fixedDeltaTime, 60f, Time.fixedDeltaTime);

        //check if we are jumping
        if (IsJumping && NormalizedJump <= 1f)
        {
            float jumpValue = _jumpPowerCurve.Evaluate(NormalizedJump) * 5.5f;
            _direction.y = jumpValue;    //<-- direction.y = up
        }
        else //<-- not jumping (aka falling)
        {
            IsJumping = false;
            _direction.y = _rigidbody.velocity.y;    //<-- itc keep the players current rigidbody velocity in y, (or up/down direction)
        }

        _rigidbody.velocity = Direction;    //<-- velocity does not care what rotation the object has.
    }
}

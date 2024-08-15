using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class TestMover : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _deltaSpeed;
    [SerializeField] private float _rotationSpeed;

    private const string Horizontal = nameof(Horizontal);

    private Rigidbody _rigidbody;
    private Vector3 _currentSpeed;

    private float _rotationDirection;
    private bool _isMove;
    

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();   
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
            _isMove=true;
        else
            _isMove=false;

        _rotationDirection =Input.GetAxisRaw(Horizontal);
    }

    private void FixedUpdate()
    {
        if (_isMove)
            Move();

        Rotate(_rotationDirection);
        
    }

    private void Move()
    {
        Vector3 moveDirection = Vector3.up;
        Vector3 deltaSpeed = new Vector3(0f, _deltaSpeed, 0f);
        moveDirection = _rigidbody.rotation * moveDirection;
        moveDirection *= _deltaSpeed;
        _currentSpeed = _rigidbody.velocity;
        _currentSpeed += moveDirection;
        _currentSpeed = Vector3.ClampMagnitude(_currentSpeed, _maxSpeed);
        _rigidbody.velocity= _currentSpeed;
    }

    private void Rotate(float direction)
    {
        Vector3 lastRotation = _rigidbody.rotation.eulerAngles;
        Vector3 deltaRotation;

        if (direction == 0f)
            return;

        deltaRotation = Vector3.forward * _rotationSpeed * -direction * Time.fixedDeltaTime;

        lastRotation += deltaRotation;

        _rigidbody.rotation = Quaternion.Euler(lastRotation);
    }
}

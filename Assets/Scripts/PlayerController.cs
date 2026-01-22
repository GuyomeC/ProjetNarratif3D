using UnityEngine;
using UnityEngine.InputSystem; // Nécessaire pour le nouveau système d'input

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Physique")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 720f;

    [Header("Input")]
    [SerializeField] private InputAction _moveAction;

    private Vector3 _input;
    private Vector3 _isoInput;

    private void OnEnable()
    {
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void Update()
    {
        GatherInput();
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void GatherInput()
    {
        Vector2 rawInput = _moveAction.ReadValue<Vector2>();
        _input = new Vector3(rawInput.x, 0, rawInput.y);

        _isoInput = _input.ToIso();
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_isoInput, Vector3.up);

        Quaternion nextRotation = Quaternion.RotateTowards(_rb.rotation, rot, _turnSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(nextRotation);
    }

    private void Move()
    {
        if (_input == Vector3.zero) return;

        Vector3 movement = _isoInput.normalized * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
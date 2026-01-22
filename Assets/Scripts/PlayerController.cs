using UnityEngine;
using UnityEngine.InputSystem; // Nécessaire pour le nouveau système d'input

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Physique")]
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 720f; // Augmenté pour plus de réactivité

    [Header("Input")]
    // Permet de définir les touches directement dans l'inspecteur
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
        // On gère le mouvement et la rotation ensemble dans la boucle physique
        Move();
        Look();
    }

    private void GatherInput()
    {
        // Lecture du Vector2 du nouveau système d'input
        Vector2 rawInput = _moveAction.ReadValue<Vector2>();
        _input = new Vector3(rawInput.x, 0, rawInput.y);

        // On convertit l'input en isométrique une seule fois ici pour éviter le calcul en boucle physique
        _isoInput = _input.ToIso();
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        // Calcul de la rotation cible
        var rot = Quaternion.LookRotation(_isoInput, Vector3.up);

        // Utilisation de MoveRotation pour respecter la physique du Rigidbody
        Quaternion nextRotation = Quaternion.RotateTowards(_rb.rotation, rot, _turnSpeed * Time.fixedDeltaTime);
        _rb.MoveRotation(nextRotation);
    }

    private void Move()
    {
        // Si pas d'input, on ne force pas la position (laisse la physique agir)
        if (_input == Vector3.zero) return;

        // Calcul de la nouvelle position basée sur la direction isométrique
        // Note: On normalise _isoInput pour éviter d'aller plus vite en diagonale
        Vector3 movement = _isoInput.normalized * _speed * Time.fixedDeltaTime;

        _rb.MovePosition(_rb.position + movement);
    }
}

public static class Helpers
{
    // Mise en cache de la matrice (inchangé, c'était très bien)
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
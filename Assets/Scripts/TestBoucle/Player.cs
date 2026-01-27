using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private MapManager _mapManager;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.AddForce(Vector3.up * 3f, ForceMode.Impulse);
            int nombrecube = _mapManager.listeTile.Count;
            _mapManager.PlayerJump();
        }
    }
}

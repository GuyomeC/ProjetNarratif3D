using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private MapManager _mapManager;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private ForceMode ForceMode;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode);
            int nombrecube = _mapManager.listeTile.Count;
            _mapManager.PlayerJump();
        }
    }
}

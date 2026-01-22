using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointAClickSystem : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] NavMeshAgent player;


    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame) { 
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = MainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }
    }
}

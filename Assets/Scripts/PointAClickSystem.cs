using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointAClickSystem : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] NavMeshAgent player;
    [SerializeField] float fixedYRotation = 0f;

    public GameManager GM;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !GM.IsInDialogue && !GM.CanShowDialogue)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = MainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }

        //Vector3 currentRotation = player.transform.eulerAngles;
        //currentRotation.y = fixedYRotation;
        //player.transform.eulerAngles = currentRotation;
    }
}
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PointAClickSystem : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] NavMeshAgent player;
    [SerializeField] float fixedYRotation = 0f;

    public GameManager GM;
    public DialogManager DM;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !GM.IsInDialogue && !GM.CanShowDialogue && !DM.MenuPause.IsInPause)
        {
            Vector3 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = MainCamera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }

        if (DM.MenuPause.IsInPause)
        {
            player.isStopped = true;
        }
        else
        {
            player.isStopped = false;
        }
    }
}
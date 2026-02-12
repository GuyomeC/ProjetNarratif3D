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
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame
            && !GM.IsInDialogue && !GM.CanShowDialogue && !DM.MenuPause.IsInPause)
        {
            player.transform.rotation = Quaternion.Euler(0, fixedYRotation, 0);
            Vector2 pointerPosition = Pointer.current.position.ReadValue();
            Ray ray = MainCamera.ScreenPointToRay(pointerPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }

        player.isStopped = DM.MenuPause.IsInPause;
    }
}
using UnityEngine;
using UnityEngine.AI;

public class PointAClickSystem : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    [SerializeField] NavMeshAgent player;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) { 
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit))
            {
                player.SetDestination(hit.point);
            }
        }
    }
}

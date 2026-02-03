using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{

    [SerializeField] public GameObject tilePrefab;
    [SerializeField] public GameObject MapLobby;
    [SerializeField] public List<GameObject> listeTile = new List<GameObject>();
    [SerializeField] private GameObject _lastCube;

    public float moveDuration = 0.25f;
    public int TileId = 1;
    public TextMeshProUGUI tileIdText;

    void Start()
    {
        listeTile.Clear();
        listeTile.Add(MapLobby);
        for (int i = 0; i < 11; i++)
        {
            GameObject tile = Instantiate(tilePrefab, new Vector3(0, 0, i * 6), Quaternion.identity);
            listeTile.Add(tile);
        }

        tileIdText.text = TileId.ToString();
    }

    void Update()
    {

    }

    public void PlayerJump()
    { 
        StartCoroutine(PlayerJumpRoutine());
        TileId++;
        tileIdText.text = TileId.ToString();
    }

    IEnumerator PlayerJumpRoutine()
    {
        int nombrecube = listeTile.Count;
        _lastCube = listeTile[nombrecube - 1];

        yield return StartCoroutine(MoveTiles());

        GameObject tileRemove = listeTile[0];
        listeTile.RemoveAt(0);
        Destroy(tileRemove);

        GameObject tile = Instantiate(
            tilePrefab,
            new Vector3(0, 0, _lastCube.transform.position.z + 6),
            Quaternion.identity
        );

        listeTile.Add(tile);
        _lastCube = tile;
    }

    IEnumerator MoveTiles()
    {
        float elapsed = 0f;

        Vector3[] startPositions = new Vector3[listeTile.Count];
        Vector3[] targetPositions = new Vector3[listeTile.Count];

        for (int i = 0; i < listeTile.Count; i++)
        {
            startPositions[i] = listeTile[i].transform.position;
            targetPositions[i] = startPositions[i] - new Vector3(0, 0, 6);
        }

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;

            for (int i = 0; i < listeTile.Count; i++)
            {
                listeTile[i].transform.position =
                    Vector3.Lerp(startPositions[i], targetPositions[i], t);
            }

            yield return null;
        }

        for (int i = 0; i < listeTile.Count; i++)
        {
            listeTile[i].transform.position = targetPositions[i];
        }
    }
}

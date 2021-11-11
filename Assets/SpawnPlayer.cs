using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    private GameManager_MP gameManager;
    public GameObject pacmanPrefab, ghostPrefab;

    public Transform pacmanPosition, ghostPosition;
    private void Awake()
    {
        gameManager = GetComponent<GameManager_MP>();
        Vector3 pacPos = new Vector3(pacmanPosition.position.x, pacmanPosition.position.y, pacmanPosition.position.z);
        Vector3 ghostPos = new Vector3(ghostPosition.position.x, ghostPosition.position.y, pacmanPosition.position.z);
        if (PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == null || (int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"] == 0)
        {
            GameObject playerToSpawn = pacmanPrefab;
            PhotonNetwork.Instantiate(playerToSpawn.name, pacPos, Quaternion.identity);
        }
        else
        {
            GameObject playerToSpawn = ghostPrefab;
            PhotonNetwork.Instantiate(ghostPrefab.name, ghostPos, Quaternion.identity);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    Player player;
    [SerializeField] TMP_Text text;
    public Image backgroundImage;
    public Color backgroundHighlightColor;
    public GameObject leftArrowButton, rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public GameObject[] playerAvatar;



    public void Setup(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
        if (_player.IsLocal)
        {
            backgroundImage.color = backgroundHighlightColor;
            leftArrowButton.SetActive(true);
            rightArrowButton.SetActive(true);
        }
        else
        {
            leftArrowButton.SetActive(false);
            rightArrowButton.SetActive(false);
        }
        UpdatePlayerItem(player);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }


    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

    public void OnClickLeftArrow()
    {
        if((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = playerAvatar.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"]-1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRighttArrow()
    {
        if ((int)playerProperties["playerAvatar"] == playerAvatar.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if(player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    private void UpdatePlayerItem(Player player)
    {
        if(player.CustomProperties.ContainsKey("playerAvatar"))
        {
            if((int)player.CustomProperties["playerAvatar"] == 0)
            {
                playerAvatar[0].SetActive(true);
                playerAvatar[1].SetActive(false);
            }
            else
            {
                playerAvatar[0].SetActive(false);
                playerAvatar[1].SetActive(true);
            }
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }
}

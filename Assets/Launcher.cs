using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text roomNameText;

    [SerializeField] TMP_InputField joinRoomNameInputField;
    [SerializeField] TMP_Text joinErrorText;
    [SerializeField] TMP_Text startGameErrorText;
    [SerializeField] GameObject playerListPrefab;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject playButton;
    [SerializeField] GameObject mainPanel, inRoomPanel, joinRoomPanel;
    public GameObject loadingText;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        SetLoadingtext(true);
        Play();
    }

    private void SetLoadingtext(bool active)
    {
        loadingText.SetActive(active);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        //Michsky.UI.Dark.MainPanelManager.Instance.PanelAnim(1);
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Play()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to master");
            PhotonNetwork.ConnectUsingSettings();

        }
        else if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
        {
            SetLoadingtext(false);
            mainPanel.SetActive(false);
            joinRoomPanel.SetActive(false);
            inRoomPanel.SetActive(true);
            roomNameText.text = "Room:" + PhotonNetwork.CurrentRoom.Name;
            Player[] players = PhotonNetwork.PlayerList;
            int playersAmount = players.Count();
            Debug.Log("Player amount = " + playersAmount);
            string defaultNickname = PhotonNetwork.NickName;
            for (int i = 0; i < playersAmount - 1; i++)
            {
                Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
                Debug.Log(i + " player instantiate");
            }
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[playersAmount - 1]);
            Debug.Log(playersAmount - 1 + " player instantiate");
            SetPlayButton();
        }
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        SetLoadingtext(false);
        mainPanel.SetActive(true);
        inRoomPanel.SetActive(false);
        joinRoomPanel.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        int num = CheckCharacter();
        if (num == 1)
        {
            SetLoadingtext(true);
            PhotonNetwork.LoadLevel("MP");
        }
        else if (num == 0 )
        {
            startGameErrorText.text = "Someone has to be Ghost..";
        }
        else
        {
            startGameErrorText.text = "You can't play Pacman without Pacman!";
        }
    }
    private int CheckCharacter()
    {
        int num = 0;
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties["playerAvatar"] != null)
                num += (int)player.CustomProperties["playerAvatar"];
        }
        return num;
    }

    public void CreateRoom()
    {
        if(CheckNameInput())
        {
            SetLoadingtext(true);
            PhotonNetwork.NickName = playerNameInputField.text;
            RoomOptions roomOptions = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = 2, BroadcastPropsChangeToAll = true };
            PhotonNetwork.CreateRoom(CreateRandomName(), roomOptions);
        }
    }
    private bool CheckNameInput()
    {
        if (string.IsNullOrEmpty(playerNameInputField.text))
        {
            playerNameInputField.placeholder.color = new Color(1, 0, 0, 1);
            return false;
        }
        else
        {
            playerNameInputField.placeholder.color = new Color(1, 1, 1, 1);
            return true;
        }
    }
    public void OpenJoinRoomPanel()
    {
        if(CheckNameInput())
        {
            mainPanel.SetActive(false);
            joinRoomPanel.SetActive(true);
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("There is another room with this name, fuckkkkk, trying again");
        CreateRoom();

    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Join room old");
        SetLoadingtext(false);
        mainPanel.SetActive(false);
        joinRoomPanel.SetActive(false);
        inRoomPanel.SetActive(true);
        roomNameText.text = "Room:" + PhotonNetwork.CurrentRoom.Name;
        Player[] players = PhotonNetwork.PlayerList;
        int playersAmount = players.Count();
        int count = 0;
        string defaultNickname = PhotonNetwork.NickName;
        for (int i = 0; i < playersAmount - 1; i++)
        {
            if (players[i].NickName == PhotonNetwork.NickName)
            {
                count++;
                PhotonNetwork.NickName = defaultNickname;
                PhotonNetwork.NickName += "(" + count + ")";
            }
            Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[playersAmount - 1]);
        SetPlayButton();
    }
    public void ChangeToJoinRoom()
    {
        if(CheckNameInput())
        {
            mainPanel.SetActive(false);
            joinRoomPanel.SetActive(true);
        }
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        //base.OnMasterClientSwitched(newMasterClient);
        SetPlayButton();
    }
    private void SetPlayButton()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && PhotonNetwork.IsMasterClient)
            playButton.SetActive(true);
        else
            playButton.SetActive(false);
    }

    
    private string CreateRandomName()
    {
        string name = "";

        for (int counter = 1; counter <= 5; ++counter)
        {
            bool upperCase = (Random.Range(0, 2) == 1);

            int rand = 0;
            if (upperCase)
            {
                rand = Random.Range(65, 91);
            }
            else
            {
                rand = Random.Range(97, 123);
            }

            name += (char)rand;
        }
        Debug.Log(name);
        return name;
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomNameInputField.text))
        {
            joinRoomNameInputField.placeholder.color = new Color(1, 0, 0, 1);
            return;
        }
        SetLoadingtext(true);
        joinRoomNameInputField.placeholder.color = new Color(1, 1, 1, 1);
        PhotonNetwork.NickName = playerNameInputField.text;
        PhotonNetwork.JoinRoom(joinRoomNameInputField.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetLoadingtext(false);
        if (returnCode == 32758)
            joinErrorText.text = "Can't find room named '" + joinRoomNameInputField.text + "'";
        else if (returnCode == 32765)
            joinErrorText.text = "Room full";
        else
            joinErrorText.text = message;
        joinRoomNameInputField.text = "";
        Debug.Log("Is connected = " + PhotonNetwork.IsConnected + "\nIs in lobby = " + PhotonNetwork.InLobby);
    }


    public void LeaveRoom()
    {
        startGameErrorText.text = "";
        SetLoadingtext(true);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Room left");
        SetLoadingtext(false);
        inRoomPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void CheckThenChange(int index)
    {
        if (string.IsNullOrEmpty(playerNameInputField.text))
        {
            playerNameInputField.placeholder.color = new Color(1, 0, 0, 1);
            return;
        }
        playerNameInputField.placeholder.color = new Color(1, 1, 1, 1);
        //Michsky.UI.Dark.MainPanelManager.Instance.PanelAnim(index);

    }

    public void ResetValue()
    {
        joinRoomNameInputField.text = "";
        joinErrorText.text = "";
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Player[] players = PhotonNetwork.PlayerList;
        int count = 0;
        string defaultNickname = newPlayer.NickName;
        for (int i = 0; i < players.Count() - 1; i++)
        {
            if (players[i].NickName == newPlayer.NickName)
            {
                count++;
                newPlayer.NickName = defaultNickname;
                newPlayer.NickName += "(" + count + ")";
            }
        }
        Instantiate(playerListPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
        SetPlayButton();
    }


    public void ReturnToMainMenu()
    {
        animator.SetTrigger("In");
        Invoke(nameof(LoadMainMenu), 1f);
    }

    private void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }


}

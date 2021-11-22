using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager_MP : MonoBehaviourPunCallbacks
{
    public Ghost_MP ghost;
    public Pacman_MP pacman;
    public Transform pellets;
    public Animator fadeAnimator;
    public int curPellets = 244;
    public Text scoreText;
    public GameObject[] pacmanLivesUI;
    public GameObject playerLeftBox;
    public Text leftText;
    public float slowMotionScale = 0.25f;
    private float defaultFixedDeltaTime = 0.02f;
    public float ghostFrightenedTime = 10f;
    public int score { get; private set; }
    public int lives { get; private set; }

    public bool isPacman = false;
    public GameObject PauseMenu;

    private bool isPaused = false;
    private bool confirmQuit = false;
    [SerializeField] GameObject confirmBox;
    public Result result;

    private void Start()
    {
        result = Result.instance;
        //CameraShaker.Instance.ShakeOnce(2f, 2f, 2f, 3f);
        //Invoke(nameof(NewGame), 3f);
        NewGame();
    }
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    private void NewGame()
    {
        curPellets = 244;
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        foreach(Transform pellet in pellets)
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }
    
    private void ResetState()
    {
        pacman.ResetState();
        ghost.ResetState();
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score+"";
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        if(lives == 3)
        {
            foreach (GameObject pm in pacmanLivesUI)
                pm.SetActive(true);
        }
        else if(lives == 2)
        {
            pacmanLivesUI[2].GetComponent<AnimateImage>().enabled = true;
            Invoke(nameof(DeactiveLivesUI), 1.5f);
        }
        else if(lives == 1)
        {
            pacmanLivesUI[1].GetComponent<AnimateImage>().enabled = true;
            Invoke(nameof(DeactiveLivesUI), 1.5f);
        }
        else
        {
            pacmanLivesUI[0].GetComponent<AnimateImage>().enabled = true;
            Invoke(nameof(DeactiveLivesUI), 1.5f);
        }
    }
    private void DeactiveLivesUI()
    {
        pacmanLivesUI[lives].SetActive(false);
    }
    private void GameOver()
    {
        StartCoroutine(SlowMotionSequence());
        Invoke(nameof(FadeIn), 2f);
        Invoke(nameof(ChangeOverScene), 3f);
    }
    private void FadeIn()
    {
        fadeAnimator.SetTrigger("In");
    }

    public void GhostEaten()
    {
        StartCoroutine(SlowMotionSequence());
        CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 1f);
    }

    public void PacmanEaten()
    {
        pacman.PacmanDied();
        SetLives(lives - 1);
        if(lives >0)
        {
            Invoke(nameof(ResetState),3f);
        }
        else
        {
            result.ghostWins = true;
            GameOver();
        }
        CameraShaker.Instance.ShakeOnce(6f, 6f, 0.1f, 1f);
    }

    public void PalletEaten(Pallet_MP pallet)
    {
        pallet.gameObject.SetActive(false);
        curPellets--;
        SetScore(score + pallet.points);
        if(!HasRemainingPallets())
        {
            pacman.PacmanWin();
            pacman.gameObject.SetActive(false);
            result.ghostWins = false;
            CameraShaker.Instance.ShakeOnce(3f, 3f, 0.1f, 1f);
            GameOver();
        }
    }

    public void PowerPalletEaten(PowerPallet_MP pallet)
    {
        if(!ghost.isEaten)
            ghost.GhostFrightened(ghostFrightenedTime);
        PalletEaten(pallet);
        pacman.PowerPalletEatenFX();
        CameraShaker.Instance.ShakeOnce(1f, 1f, 0.1f, 0.5f);
        CancelInvoke();
    }

    private bool HasRemainingPallets()
    {
        foreach(Transform pallet in pellets)
        {
            if (pallet.gameObject.activeSelf)
                return true;
        }

        return false;
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            pacman.isAcceptingInput = false;
            ghost.isAcceptingInput = false;
            PauseMenu.SetActive(true);
            //Time.timeScale = 0;
        }
        else
        {
            confirmBox.SetActive(false);
            confirmQuit = false;
            isPaused = false;
            pacman.isAcceptingInput = true;
            ghost.isAcceptingInput = true;
            PauseMenu.SetActive(false);
            //Time.timeScale = 1;
        }
    }

    public IEnumerator SlowMotionSequence()
    {
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        yield return new WaitForSeconds(0.25f);
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    //Change GameOver when over
    public void ChangeOverScene()
    {
        PhotonNetwork.LoadLevel("GAMEOVER_MP");
    }

    public void QuitGame()
    {
        if(confirmQuit)
        {
            FadeIn();
            PhotonNetwork.Disconnect();
        }
        else
        {
            confirmQuit = true;
            confirmBox.SetActive(true);
        }

    }
    public void QuitGame2()
    {
        FadeIn();
        PhotonNetwork.Disconnect();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PauseMenu.SetActive(false);
        playerLeftBox.SetActive(true);
        leftText.text = otherPlayer.NickName + " LEFT THE GAME";
        if (isPacman)
            pacman.isAcceptingInput = false;
        else
            ghost.isAcceptingInput = false;
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Room");
    }

}

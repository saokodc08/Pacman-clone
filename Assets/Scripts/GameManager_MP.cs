using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager_MP : MonoBehaviour
{
    public Ghost_MP ghost;
    public Pacman_MP pacman;
    public Transform pellets;
    public Animator fadeAnimator;
    public int curPellets = 244;
    public Text scoreText;
    public GameObject[] pacmanLivesUI;
    public Text ghostEatenScore;
    [SerializeField] private GameObject ghostEatenScoreGameobject;
    public float slowMotionScale = 0.25f;
    private int ghostMultiplier = 1;
    private float defaultFixedDeltaTime = 0.02f;
    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        //CameraShaker.Instance.ShakeOnce(2f, 2f, 2f, 3f);
        //Invoke(nameof(NewGame), 3f);
        NewGame();
    }
    

    private void Update()
    {

    }

    private void SetPlayer()
    {
        pacman = GameObject.FindGameObjectWithTag("Player").GetComponent<Pacman_MP>();
        ghost = GameObject.FindGameObjectWithTag("Player_1").GetComponent<Ghost_MP>();
    }
    private void NewGame()
    {
        //SetPlayer();
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
        /*for (int i = 0; i < ghosts.Length; i++)
        {
            ghosts[i].gameObject.SetActive(false);
        }
        pacman.gameObject.SetActive(false);*/
        StartCoroutine(SlowMotionSequence());
        Invoke(nameof(FadeIn), 2f);
        Invoke(nameof(ChangeOverScene), 3f);
    }
    private void FadeIn()
    {
        fadeAnimator.SetTrigger("In");
    }

    public void GhostEaten(Ghost ghost)
    {
        StartCoroutine(SlowMotionSequence());
        CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 1f);
        int newPoints = ghost.points * ghostMultiplier;
        ghostEatenScore.text = newPoints+"";
        ghostEatenScoreGameobject.SetActive(true);
        SetScore(score + newPoints);
        ghostMultiplier++;    
    }



    public void PacmanEaten()
    {
        pacman.PacmanDied();
        SetLives(lives - 1);
        if(lives >0)
        {
            Invoke("ResetState",3f);
        }
        else
        {
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
            StartCoroutine(SlowMotionSequence());
            CameraShaker.Instance.ShakeOnce(3f, 3f, 0.1f, 1f);
            pacman.PacmanWin();
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NewGame), 3.0f);
        }
    }

    public void PowerPalletEaten(PowerPallet_MP pallet)
    {

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

    public IEnumerator SlowMotionSequence()
    {
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = defaultFixedDeltaTime * Time.timeScale;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1;
        Time.fixedDeltaTime = defaultFixedDeltaTime;
    }

    //Change GameOver when over
    public void ChangeOverScene()
    {
        SceneManager.LoadScene("GAMEOVER_LV1");
    }    
}
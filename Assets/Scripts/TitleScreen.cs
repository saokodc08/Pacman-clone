using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Animator animator;
    
    public void StartGame()
    {
        //animator.SetActive(true);
        animator.SetTrigger("In");
        Invoke(nameof(LoadScene), 1f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMultiplayerScene()
    {
        animator.SetTrigger("In");
        Invoke(nameof(LoadSceneMul), 1f);
    }

    private void LoadSceneMul()
    {
        SceneManager.LoadScene(2);

    }
}

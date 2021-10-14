using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public GameObject animator;
    
    public void StartGame()
    {
        animator.SetActive(true);
        animator.GetComponent<Animator>().SetTrigger("In");
        Invoke(nameof(LoadScene), 0.5f);
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(1);

    }
}

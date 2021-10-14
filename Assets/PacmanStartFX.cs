using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanStartFX : MonoBehaviour
{
    [SerializeField] private GameObject pacman;
    [Range(0,3)]
    [SerializeField] private float timeToEnablePacman;
    private void Awake()
    {
        pacman.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        Invoke("EnablePacman", timeToEnablePacman);
        Destroy(gameObject, 3f);
    }
    private void OnDestroy()
    {
        pacman.GetComponent<Pacman>().isAcceptingInput = true;
    }
    private void EnablePacman()
    {
        pacman.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

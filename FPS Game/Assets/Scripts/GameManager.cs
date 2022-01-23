using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public float waitAfterDying = 2f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(transform.gameObject);
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    void Start()
    {
        //Confinde = Visible and locked int he window
        //Locked = Unvisible mouse and locked
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
       
    } 

    public void PlayerDied()
    {
        StartCoroutine(PlayerDiedCo());
        
    }

    public IEnumerator PlayerDiedCo()
    {
        yield return new WaitForSeconds(waitAfterDying);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}

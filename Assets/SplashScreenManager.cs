using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ai Demo");
    }


    void LoadNextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ai Demo");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

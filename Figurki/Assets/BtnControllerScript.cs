using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnControllerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Start_btn()
    {
        SceneManager.LoadScene(3);
    }

    public void Exit_btn()
    {
        Application.Quit();
    }
}

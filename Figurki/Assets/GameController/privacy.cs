using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class privacy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Privacy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Privacy()
    {
        print("Privacy Scene");
        SceneManager.LoadScene(2);
    }
}

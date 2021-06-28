using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JsonController : MonoBehaviour {

	private string jsonURL = config.webLink;
    private string parse_url;

    [System.Obsolete]
    void Start () {

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            SceneManager.LoadScene(1);
            StaticClass.CrossSceneInformation = config.privLink;
        }
        else
        {
            //  StartCoroutine(GetData());
            this.StartCoroutine(this.RequestRoutine(jsonURL, this.ResponseCallback));

        }
    }

    string recentData = "";

    private IEnumerator RequestRoutine(string url, Action<string> callback = null)
    {
        // Using the static constructor
        var request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        var data = request.downloadHandler.text;

        if (callback != null)
            callback(data);

        if (request.isNetworkError)
        {
            Debug.Log("Something went wrong, and returned error: " + request.error);
        }
        else
        {
            Debug.Log(data);
            if (request.responseCode == 200)
            {
                Debug.Log("Request finished successfully!");
                try
                {
                    this.ResponseDataController(data);
                }
                catch
                {
                    GoToScene(1);
                }
               
            }
            else { 
                Debug.Log("Request failed (status:" + request.responseCode + ")");
                GoToScene(1);
            }
        }
    }

    // Callback to act on our response data
    private void ResponseCallback(string data)
    {
        Debug.Log(data);
        recentData = data;
    }

    private void ResponseDataController(string data)
    {
        FildResp jsonData = JsonUtility.FromJson<FildResp>(data);

        string parse_uid = jsonData.uid;
        parse_url = jsonData.viewUrl;
        string parseSHowView = jsonData.showView;

        Debug.Log("String parse_uid = " + parse_uid);
        Debug.Log("String parseSHowView = " + parseSHowView);
        Debug.Log("String parse_url = " + parse_url);

        if (jsonData.showView == "1")
        {
            GoToScene(2);
        }
        else
        {
            GoToScene(1);
        }
    }

	public void ReplaceScene(string Scene){
		SceneManager.LoadScene (Scene);
	}

    [System.Serializable]
    public class FildResp
	{
		public string uid;
		public string showView;
		public string viewUrl;
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
       // this.StartCoroutine(this.RequestRoutine(this.jsonURL, this.ResponseCallback));
    }

    public void GoToScene(int scene)
    {
        switch (scene)
        {
            case 1:
                Console.WriteLine("GameScene");
                StaticClass.CrossSceneInformation = config.privLink;
                SceneManager.LoadScene(1);
                break;
            case 2:
                Console.WriteLine("WebScene");
                StaticClass.CrossSceneInformation = parse_url;
                SceneManager.LoadScene(2);
                break;
        }
    }

}


public static class StaticClass {
	public static string CrossSceneInformation { get; set; }
}


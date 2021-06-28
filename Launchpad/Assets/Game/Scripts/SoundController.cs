using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    GameObject musicPanel;
    GameObject soundPanel;
    GameObject controllPanel;
    float targetTime;
    int minute;
    int sec;
    string curentTimer_str;
    bool isTimerOn;

    GameObject[] music_toggle;
    GameObject[] sound_btn;

    int playTime;

    float RecTimer;


    List<string> timeLineMusic = new List<string>();
    bool isRecordNow;
    bool isPlayNow;
    //CONST
    string MUSIC_BUTTON_TAG = "MUSIC_TAG";
    string FX_BUTTON_TAG    = "FX_TAG";
    string STOP_NAME_BTN    = "Stop";
    string PLAY_NAME_BTN    = "Play_toggle";
    string REC_NAME_BTN     = "Rec_toggle";
    string TIMER_NAME       = "Timer";

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        InitMusicButton();
        SetAudioClipMusicPanel(music_toggle);
        SetAudioClipMusicPanel(sound_btn);
        SetListener();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerOn)
        {
            StartTimer();
        }
        else
        {
            //refresh timer
            SetDefaultTimer();
        }

        StartRecordSample();

           

            PlayRecSample();
            print("Play_"+isPlayNow);
        
      //  print("IS_PLAY = "+GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn);
        foreach(string obj in timeLineMusic)
        {
            print(obj);
        }
    }


    void SetListener()
    {
        //MUSIC
        for(int i = 0; i < music_toggle.Length; i++)
        {
            Toggle toggle = music_toggle[i].GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(delegate {
                ToggleChanged(toggle);
            });
        }
        //FX
       foreach(GameObject btn in sound_btn)
        {
            Button btn_fx = btn.GetComponent<Button>();
            btn_fx.onClick.AddListener(delegate
            {
                ButtonFXPlay(btn_fx);
            });
        }

        //Stop
        GameObject.Find(STOP_NAME_BTN).GetComponent<Button>().onClick.AddListener(StopBtn);

        //Play
        GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().onValueChanged.AddListener((value)=>PlayBtn(value));

        //Rec
        GameObject.Find(REC_NAME_BTN).GetComponent<Toggle>().onValueChanged.AddListener((value) => RecBtn(value));
    }

    void StartTimer()
    {
        targetTime += Time.deltaTime;
        //float milisec = targetTime.ToString("F1");
        //  print(milisec);
        sec = (int)targetTime;

        string sec_str = "00";
        string min_str = "00";

       if(targetTime > 59.9f)
        {
            targetTime = 0;
            minute += 1;
        }
        if (sec<10)
        {
            sec_str = "0"+sec.ToString();
           // print("{ 0" + minute + ":0" + sec + " }");
        }
        else
        {
            sec_str = sec.ToString();
        }

        if (minute < 10)
        {
            min_str = "0" + minute.ToString();
        }
        else
        {
            min_str = minute.ToString();
        }

        curentTimer_str = min_str + ":" + sec_str;
        GameObject.Find(TIMER_NAME).GetComponent<Text>().text = curentTimer_str;
    }

    void SetDefaultTimer()
    {
        targetTime = 0;
        minute = 0;
        sec = 0;
        GameObject.Find(TIMER_NAME).GetComponent<Text>().text = "00:00";
    }

    //REC
    void RecBtn(bool isRec)
    {
        isTimerOn = isRec;
        
        if (GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn)
        {
            GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn = false;
        }

        RefreshMusicToggle();

        if (isRec)
        {
            ClearMusicTimeLine();
            isRecordNow = isRec;
        }
        else
        {
            isRecordNow = false;
  
   
            foreach(string str in timeLineMusic)
            {
                print(str);
            }
            
        }
    }

    void ClearMusicTimeLine()
    {
        timeLineMusic.Clear();
    }

    //PLAY
    void PlayBtn(bool isPlay)
    {
        isTimerOn = isPlay;
       
        if (GameObject.Find(REC_NAME_BTN).GetComponent<Toggle>().isOn)
        {
            GameObject.Find(REC_NAME_BTN).GetComponent<Toggle>().isOn = false;
        }
        playTime = 0;
        isPlayNow = isPlay;
        print("ISPLAY"+isPlay);
    }

    //STOP
    void StopBtn()
    {
        RefreshMusicToggle();
        RefreshControllButton();
    }

    void RefreshMusicToggle()
    {
        foreach (GameObject toggle in music_toggle)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }
    }

    void RefreshControllButton()
    {
        if (GameObject.Find(REC_NAME_BTN).GetComponent<Toggle>().isOn)
        {
            GameObject.Find(REC_NAME_BTN).GetComponent<Toggle>().isOn = false;
        }

        if (GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn)
        {
            GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn = false;
        }
    }

    void ToggleChanged(Toggle toggle)
    {
        if (timeLineMusic.Count > 0)
        {
            timeLineMusic[timeLineMusic.Count - 1] = toggle.name;
        }

        if (toggle.isOn)
        {
            toggle.GetComponent<AudioSource>().Play();
        //    playTime += 1;
        }
        else
        {
            toggle.GetComponent<AudioSource>().Stop();
          //  playTime += 1;
        }   
    }

    void ButtonFXPlay(Button btn)
    {
        btn.GetComponent<AudioSource>().Play();
        if (isRecordNow)
        {
            if (timeLineMusic.Count > 0)
            {
                timeLineMusic[timeLineMusic.Count - 1] = btn.name;
            }
            
           // timeLineMusic.Add();
           // playTime += 1;
        }
        
    }

    void SetAudioClipMusicPanel(GameObject[] musicBtn)
    {
        foreach (GameObject btn in musicBtn)
        {
            string nameBtn = btn.name;
            string nameBtnPath = nameBtn.Remove(nameBtn.Length - 1, 1);
            string path = "Audio/" + nameBtnPath + "/" + nameBtn;
            btn.GetComponent<AudioSource>().clip = Resources.Load(path) as AudioClip;
        }
    }

    void InitMusicButton()
    {
        musicPanel = GameObject.Find("Panel1");
        soundPanel = GameObject.Find("Panel2");
        controllPanel = GameObject.Find("Panel3");

        music_toggle = GameObject.FindGameObjectsWithTag(MUSIC_BUTTON_TAG);
        sound_btn = GameObject.FindGameObjectsWithTag(FX_BUTTON_TAG);
    }

    //BUTTON_CONNTROLLER

    void StartRecordSample()
    {
        if (isRecordNow)
        {
            print("RECORD!!!!"+ timeLineMusic.Count);
            timeLineMusic.Add("");
            
        }
    }
  
    void PlayRecSample()
    {
        if (isPlayNow)
        {
            print("PlayRecSample");
            playTime += 1;
            if (timeLineMusic.Count > playTime)
            {
                print(timeLineMusic.Count + "} playTime =" + playTime);   
                GameObject btn = GameObject.Find(timeLineMusic[playTime]);

                if (btn.GetComponent<Toggle>() != null)
                {
                    btn.GetComponent<Toggle>().isOn = !btn.GetComponent<Toggle>().isOn;
                }
                if (btn.GetComponent<Button>() != null)
                {
                    btn.GetComponent<Button>().onClick.Invoke();
                }
                
            }
            else
            {
                isPlayNow = false;
                RefreshMusicToggle();
                GameObject.Find(PLAY_NAME_BTN).GetComponent<Toggle>().isOn = false;
                playTime = 0;
            }
        }
        else
        {
            print("StopRecSample");
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    List<GameObject> figureList;
    public Sprite[] figure_spr;
    public Sprite[] contourSpr;
    GameObject touchObject;
    Vector3 prePosition;
    int scoreInt;
    float currCountdownValue;

    public RuntimeAnimatorController anim;

    GameObject[] contourObj;
    
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        InitVariable();
        InitContourObj();
        InitFigure();

        StartCoroutine(TimerCorutine());
        //   figureList[0].transform.position = GetWorldSapceRect(contourObj[0].GetComponent<RectTransform>()).position+ contourObj[0].GetComponent<RectTransform>().rect.size/2;

    }

    void InitVariable()
    {
        contourObj = GameObject.FindGameObjectsWithTag("CONTOUR_TAG");
        figureList = new List<GameObject>();
        scoreInt = 0;
        
    }

    void InitContourObj()
    {
        for(int i = 0; i < contourObj.Length; i++)
        {
            contourObj[i].GetComponent<Image>().sprite = contourSpr[i];
            contourObj[i].name = contourSpr[i].name;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Touch();
        TimerUpdate();

        
    }

    void Touch()
    {
        if (Input.touchCount > 0)
        {
            if(currCountdownValue == 0)
            {
                SceneManager.LoadScene(1);
            }


            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);


            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    foreach (GameObject fig in figureList)
                    {
                        if (RectTransformUtility.RectangleContainsScreenPoint(fig.GetComponent<RectTransform>(), touch.position))
                        {
                           // print("name = " + fig.GetComponent<Image>().sprite.name);
                            touchObject = fig;
                            prePosition = fig.transform.position;
                           // fig.transform.position = touch.position+touch.deltaPosition;
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    if(RectTransformUtility.RectangleContainsScreenPoint(touchObject.GetComponent<RectTransform>(), touch.position))
                    {
                        touchObject.transform.position = touch.position + touch.deltaPosition;
                        
                    }
                            break;
                case TouchPhase.Ended:
                   // print("Ended");
                    foreach (GameObject conture in contourObj)
                    { 
                        if (GetWorldSapceRect(touchObject.GetComponent<RectTransform>()).Overlaps(GetWorldSapceRect(conture.GetComponent<RectTransform>())))
                        { 
                            if (conture.name == touchObject.name + "_contour")
                            {
                                print("Contains Overlaps " + conture.name);
                                touchObject.transform.position = GetWorldSapceRect(conture.GetComponent<RectTransform>()).position + conture.GetComponent<RectTransform>().rect.size / 2;
                                StartCoroutine(RemoveFigureCorutine(touchObject));
                                scoreInt += 1;
                            }
                        }
                    }
                       
                    break;
                default:

                    break;
            }
        }
    }

    IEnumerator RemoveFigureCorutine(GameObject obj)
    {
        obj.GetComponent<Animator>().Play("RotateFigure");
        yield return new WaitForSeconds(1);
        figureList.Remove(obj);
        Destroy(obj);
        UpdateScore();
        yield return new WaitForSeconds(1);
        if (figureList.Count < 6)
        {
            InitFigure();
        }
    }

    Rect GetWorldSapceRect(RectTransform rt)
    {
        var r = rt.rect;
        r.center = rt.TransformPoint(r.center);
        r.size = rt.TransformVector(r.size);
        return r;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        print(collision.gameObject.name);
    }
    

    void InitFigure()
    {
        int figCounnt = figureList.Count;
        for (int i = 0; i < 15- figCounnt; i++)
        {
           GameObject obj = CreateFigure();
           figureList.Add(obj);
        }
        SetPositionFigure();

    }

    GameObject CreateFigure()
    {
        GameObject obj;

        obj = new GameObject();
        obj.AddComponent<Image>();
        obj.transform.SetParent(GameObject.Find("FigurePanel").transform);
        obj.GetComponent<Image>().sprite = figure_spr[Random.RandomRange(0, figure_spr.Length)];
        obj.GetComponent<Image>().SetNativeSize();
        obj.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        obj.name = obj.GetComponent<Image>().sprite.name;
        obj.AddComponent<Animator>();
        obj.GetComponent<Animator>().runtimeAnimatorController = anim;
        return obj;
    }

    void SetPositionFigure()
    {
        GameObject figurePanel = GameObject.Find("FigurePanel");
        Vector2 sizeFigurePanel = figurePanel.GetComponent<RectTransform>().rect.size;
        // print(figurePanel.GetComponent<RectTransform>().localPosition);
        Vector2 deltaSize = figureList[0].GetComponent<RectTransform>().rect.size;
        for (int i = 0; i < figureList.Count; i++)
        {
            figureList[i].transform.localPosition = new Vector3((i%5)*(sizeFigurePanel.x/ 5), (i)%3 * sizeFigurePanel.y/3, 0)+new Vector3(deltaSize.x,deltaSize.y,0);
        }
    }


    void UpdateScore()
    {
        GameObject score = GameObject.Find("Score");
        score.GetComponent<Text>().text = scoreInt.ToString();
    }

    IEnumerator TimerCorutine(float countdownValue = 59)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
    }

    void TimerUpdate()
    {
        GameObject timer = GameObject.Find("Timer");
        timer.GetComponent<Text>().text = currCountdownValue.ToString();
    }
}

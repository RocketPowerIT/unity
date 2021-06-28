using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum STR_LEN { two = 2, three, four, five, six, seven }

public class GameScript : MonoBehaviour
{
    public Transform canvas;
    public Sprite normalToogleSprite;
    public Sprite selectToogleSprite;

    List<GameObject> toogleList = new List<GameObject>();

    private string defaultWord;
    List<char> charList = new List<char>();

    List<char> inputWordList = new List<char>();
    public GameObject[] inputLatter;
    List<GameObject> textList;

    List<string> dicWord;
    public GameObject infoTableSucsess;

    List<string> defaultWordList = new List<string>();
    List<List<string>> dicWordList = new List<List<string>>();

    //STAR
    public GameObject star_panel;
    public Font myFont;

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        InitDicWord();
        InitStartWord();
        InitStar();

        AddToggleListener();
        ButtonController();
    }

    void InitDicWord()
    {

        //first
        InputList("САМОЛЮБИЕ",
            "ИСЛАМ", "МАСЛО", "МЮСЛИ", "САМБО", "СМОЛА",
            "ЛИСА", "ОСЕЛ", "САЛО", "СЕЛО", "СИЛА",
            "БАЛ", "БАС", "БЕС", "БИС", "ЛЕС", "ЛИС", "ЛОБ", "ЛОМ", "МЕЛ", "ОСА", "СОМ", "ЮЛА",
            "АС", "ИЛ");

        InputList("РАБОТА",
            "ssdf", "sadf", "sadfsadfsad", "sdaf", "СМОЛА",
            "ЛИСА", "ОСЕЛ", "САЛО", "СЕЛО", "СИЛА",
            "БАЛ", "БАС", "БЕС", "БИС", "asdfsadf", "asdf", "ЛОБ", "ЛОasdfsafsafsafdsdaМ", "asasdf", "ddddd", "СОМ", "asdf",
            "АС", "ИЛ");

        // ADD RANDOM METHOD (Index)
        // ....
        //

        defaultWord = defaultWordList[0];
        dicWord = dicWordList[0];
        SortMyWordList();
    }

    //word

    //REUSE

    void InputList(string keyWord, params string[] answerWord)
    {
        defaultWordList.Add(keyWord);
        dicWordList.Add(new List<string>());
        dicWordList[defaultWordList.Count - 1].AddRange(answerWord);
    }

    private void Update()
    {

    }

    #region init Start Word

    void InitStartWord()
    {
        initDefaultCharList();

        for (int i = 0; i < charList.Count; i++)
        {
            makeToggle(charList[i]);
        }

        print(Screen.width);
        setDefaultWordPosition();
    }

    void setDefaultWordPosition()
    {
        int countToggle = toogleList.Count;
        int maxWith = Screen.width;
        float with = maxWith / countToggle;

        for (int i = 0; i < toogleList.Count; i++)
        {
            toogleList[i].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(with / 2 + (i * with), 0, 0);
        }
    }

    void initDefaultCharList()
    {
        foreach (char c in defaultWord)
        {
            charList.Add(c);
        }
    }

    void makeToggle(char c)
    {
        GameObject toggleObj = createToggleObj(canvas, c);
        GameObject bgObj = createBackgroundObj(toggleObj, c);
        GameObject checkMarkObj = createCheckmarkObj(bgObj, c);
        GameObject labelObj = createLabelObj(toggleObj, c);
        toogleList.Add(attachAllComponents(toggleObj, bgObj, checkMarkObj, labelObj, c));
    }

    //1.Create a *Toggle* GameObject then make it child of the *Canvas*.
    GameObject createToggleObj(Transform cnvs, char c)
    {
        GameObject toggle = new GameObject("Toggle_" + c);
        toggle.transform.SetParent(cnvs.transform);
        toggle.layer = LayerMask.NameToLayer("UI");
        return toggle;
    }

    //2.Create a Background GameObject then make it child of the Toggle GameObject.
    GameObject createBackgroundObj(GameObject toggle, char c)
    {
        GameObject bg = new GameObject("Background_" + c);
        bg.transform.SetParent(toggle.transform);
        bg.layer = LayerMask.NameToLayer("UI");
        return bg;
    }

    //3.Create a Checkmark GameObject then make it child of the Background GameObject.
    GameObject createCheckmarkObj(GameObject bg, char c)
    {
        GameObject chmk = new GameObject("Checkmark_" + c);
        chmk.transform.SetParent(bg.transform);
        chmk.layer = LayerMask.NameToLayer("UI");
        return chmk;
    }

    //4.Create a Label GameObject then make it child of the Toggle GameObject.
    GameObject createLabelObj(GameObject toggle, char c)
    {
        GameObject lbl = new GameObject("Label_" + c);
        lbl.transform.SetParent(toggle.transform);
        lbl.layer = LayerMask.NameToLayer("UI");
        return lbl;
    }

    //5.Now attach components like Image, Text and Toggle to each GameObject like it appears in the Editor.
    GameObject attachAllComponents(GameObject toggle, GameObject bg, GameObject chmk, GameObject lbl, char c)
    {
        //Attach Text to label
        Text txt = lbl.AddComponent<Text>();
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = c.ToString();
        Font arialFont =
            (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        txt.font = myFont;
        txt.fontSize = 48;
        txt.fontStyle = FontStyle.Bold;
        txt.lineSpacing = 1;
        txt.color = new Color(50 / 255, 50 / 255, 50 / 255, 255 / 255);
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        txtRect.anchorMin = new Vector2(0, 0);
        txtRect.anchorMax = new Vector2(1, 1);
        //txtRect.y

        //Attach Image Component to the Checkmark
        Image chmkImage = chmk.AddComponent<Image>();
        chmkImage.sprite = selectToogleSprite;
        chmkImage.SetNativeSize();
        chmkImage.type = Image.Type.Simple;

        //Attach Image Component to the Background
        Image bgImage = bg.AddComponent<Image>();
        bgImage.sprite = normalToogleSprite;
        bgImage.SetNativeSize();
        bgImage.type = Image.Type.Sliced;
        RectTransform bgRect = txt.GetComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 1);
        bgRect.anchorMax = new Vector2(0, 1);

        //Attach Toggle Component to the Toggle
        Toggle toggleComponent = toggle.AddComponent<Toggle>();
        toggleComponent.transition = Selectable.Transition.ColorTint;
        toggleComponent.targetGraphic = bgImage;
        toggleComponent.isOn = false;
        toggleComponent.toggleTransition = Toggle.ToggleTransition.Fade;
        toggleComponent.graphic = chmkImage;
        toggle.GetComponent<RectTransform>().sizeDelta = new Vector2();
        toggle.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
        return toggle;
    }

    #endregion

    void AddToggleListener()
    {

        foreach (GameObject t in toogleList)
        {
            t.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
            {
                if (inputWordList.Count < 5)
                {
                    int ind = toogleList.IndexOf(t);
                    print("index = {" + charList[ind] + "}_isActive " + value);

                    if (value)
                    {
                        inputWordList.Add(charList[ind]);
                        UpdateLatterChar();

                    }
                    t.GetComponent<Toggle>().interactable = false;
                }
                else
                {
                    print("InputWordList isFULL");
                    t.GetComponent<Toggle>().isOn = false;
                }
            });
        }

    }

    IEnumerator RemoveInfoTable()
    {
        infoTableSucsess.SetActive(true);
        yield return new WaitForSeconds(2);
        infoTableSucsess.SetActive(false);
    }

    void UpdateLatterChar()
    {
        print("upd");
        char c = inputWordList[inputWordList.Count - 1];
        inputLatter[inputWordList.Count - 1].SetActive(true);

        GameObject lbl = new GameObject("Label__" + c);
        lbl.transform.SetParent(inputLatter[inputWordList.Count - 1].transform);
        lbl.layer = LayerMask.NameToLayer("UI");
        Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        lbl.AddComponent<Text>().text = c.ToString();
        lbl.GetComponent<Text>().font = myFont;
        lbl.GetComponent<Text>().fontStyle = FontStyle.Bold;
        lbl.GetComponent<Text>().fontSize = 48;
        lbl.GetComponent<Text>().color = new Color(50 / 255, 50 / 255, 50 / 255, 255 / 255);
        lbl.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        lbl.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        lbl.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        lbl.transform.position = inputLatter[inputWordList.Count - 1].transform.position;
    }

    #region STAR_BLOCK_WORD

    void InitStar()
    {   //CHECK COUNT STAR
       

        int two = 0;
        int three = 0;
        int four = 0;
        int five = 0;
        int six = 0;
        int seven = 0;


        foreach (string word in dicWord)
        {
            int strLen = (int)word.Length;
            switch (strLen)
            {
                case (int)STR_LEN.two:
                    two += 1;
                    break;
                case (int)STR_LEN.three:
                    three += 1;
                    break;
                case (int)STR_LEN.four:
                    four += 1;
                    break;
                case (int)STR_LEN.five:
                    five += 1;
                    break;
                case (int)STR_LEN.six:
                    six += 1;
                    break;
                case (int)STR_LEN.seven:
                    seven += 1;
                    break;
            }
        }

        print("=========COUNT WORD=========  " +
            "\n TWO     = " + two +
            "\n THREE   = " + three +
            "\n FOUR    = " + four +
            "\n FIVE    = " + five +
            "\n SIX     = " + six +
            "\n SEVEN   = " + seven
            );

        

        List<GameObject> all2starList = new List<GameObject>();
        List<GameObject> all3starList = new List<GameObject>();
        List<GameObject> all4starList = new List<GameObject>();
        List<GameObject> all5starList = new List<GameObject>();
        List<GameObject> all6starList = new List<GameObject>();
        List<GameObject> all7starList = new List<GameObject>();

        all2starList = initStarWordList(two, (int)STR_LEN.two);
        all3starList = initStarWordList(three, (int)STR_LEN.three);
        all4starList = initStarWordList(four, (int)STR_LEN.four);
        all5starList = initStarWordList(five, (int)STR_LEN.five);
        all6starList = initStarWordList(six, (int)STR_LEN.six);
        all7starList = initStarWordList(seven, (int)STR_LEN.seven);

        List<List<GameObject>> allStar = new List<List<GameObject>>();
        allStar.Add(all2starList);
        allStar.Add(all3starList);
        allStar.Add(all4starList);
        allStar.Add(all5starList);
        //  allStar.Add(all6starList);
        //  allStar.Add(all7starList);

        var panelTransform = star_panel.GetComponent<RectTransform>();
        float w_size = panelTransform.rect.width/2;
        float h_size = panelTransform.rect.height/2;
        float x_position = panelTransform.position.x;
        float y_position = panelTransform.position.y;
        print("w_size = "+w_size+ "\n h_size = " + h_size + "\n x_position = " + x_position + "\n y_position = " + y_position);

        //getCountWord
        int maxWord = dicWord.Count;

        //row and column
        int maxColum = 5;
        int maxRow = maxWord / maxColum;

        int lastRow = maxWord % maxColum;

        print("Counnt Word" + maxWord);
        print("macCOLUM = " + maxColum + " macRow = " + maxRow + " last row = "+ lastRow);

        float stepCol_size = (float)h_size / maxColum;
        int stepRow_size = (int)w_size / maxRow;
        print(stepCol_size + "}{" + stepRow_size);

        int globalIndexAllStar = 0;
        int _row = 1;
        
        float with_star = allStar.First().First().GetComponentInChildren<RectTransform>().rect.width;

        for (int i =0;i<allStar.Count; i++)
        {
            for (int k = 0; k < allStar[i].Count; k++)
            {
                globalIndexAllStar += 1;

                if(globalIndexAllStar%5 == 0)
                {

                    _row += 1;

                    allStar[i][k].transform.localPosition = new Vector3(
                                                                    stepCol_size*3f * _row,
                                                                    stepRow_size * (globalIndexAllStar % 5),
                                                                    0
                                                                    );
                }
                else
                {
                    allStar[i][k].transform.localPosition = new Vector3(
                                                                    stepCol_size* 3 * _row ,
                                                                    stepRow_size * (globalIndexAllStar % 5),
                                                                    0
                                                                    );
                }
              //  allStar[k]
            }
        }
        print(globalIndexAllStar);
        print(all2starList.First().GetComponentInChildren<RectTransform>().rect.width);
       

        foreach(var obj in all2starList)
        {
          //  obj.GetComponentInChildren<RectTransform>().localPosition = new Vector3(100,100,0);
        }
    }

    List<GameObject> initStarWordList(int wordCount,int countStarChar)
    {
        List<GameObject> obj_list = new List<GameObject>();

        List<String> word_list = new List<string>();

        foreach (string word in dicWord)
        {
            int strLen = (int)word.Length;
            if(strLen == countStarChar)
            {
                word_list.Add(word);
            }
        }

        for (int i = 0; i < wordCount; i++)
        {
            obj_list.Add(CreateStarCount(countStarChar, word_list[i]));
        }
        word_list.Clear();

        return obj_list;
    }

    GameObject CreateStarCount(int count,string word_str)
    {
        GameObject star = new GameObject("STAR_OBJ_" + word_str);
        for(int i =0; i<count; i++)
        {
            var obj = CreateStarObj();
            obj.GetComponent<RectTransform>().localPosition = new Vector3(24*i,0,0);
            obj.GetComponent<Image>().transform.SetParent(star.transform);

        }
        star.transform.SetParent(star_panel.transform);
        //Add Label
        createLabelAnswerWord(word_str).transform.SetParent(star.transform);

        return star;
    }

    GameObject createLabelAnswerWord(string word_str)
    {
        /*
        GameObject lbl = new GameObject("Label__" + c);
        lbl.transform.SetParent(inputLatter[inputWordList.Count - 1].transform);
        lbl.layer = LayerMask.NameToLayer("UI");
        lbl.AddComponent<Text>().text = c.ToString();
        lbl.GetComponent<Text>().font = arialFont;
        lbl.GetComponent<Text>().fontStyle = FontStyle.Bold;
        lbl.GetComponent<Text>().fontSize = 48;
        lbl.GetComponent<Text>().color = new Color(50 / 255, 50 / 255, 50 / 255, 255 / 255);
        lbl.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        lbl.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        lbl.GetComponent<RectTransform>().anchorMin = new Vector2(1, 1);
        lbl.transform.position = inputLatter[inputWordList.Count - 1].transform.position;
        */

        GameObject obj = new GameObject("Label"+word_str);
        Font arialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        var lbl_add = obj.AddComponent<Text>();
        lbl_add.font = arialFont;
        lbl_add.text = word_str;
        lbl_add.fontSize = 26;
        lbl_add.horizontalOverflow = HorizontalWrapMode.Overflow;
        lbl_add.SetNativeSize();
        obj.GetComponent<RectTransform>().pivot = new Vector2(0.2f,0.5f);
        obj.SetActive(false);
        lbl_add.alignment = TextAnchor.MiddleCenter;
        string TAG = "WORD";
        obj.tag = TAG;


        return obj;
    }

    GameObject CreateStarObj()
    {
        GameObject star_obj = new GameObject("Simple_Star");
        star_obj.tag = "STAR";
        Image img = star_obj.AddComponent<Image>();

        //Assets/Resources/star_BASE_1.png
        Sprite _sprite = Resources.Load<Sprite>("star_BASE_1");
        img.sprite = _sprite;
        img.SetNativeSize();
        img.type = Image.Type.Simple;

        return star_obj;
    }

    #endregion

    #region Button Panel

    public void ButtonController()
    {
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("buttonGroup"))
        {
            if (fooObj.name == "OK_btn")
            {


                fooObj.GetComponent<Button>().onClick.AddListener(() =>
                {

                var parent_obj = star_panel.GetComponentInChildren<Transform>();
                  print(parent_obj.transform.childCount);
                    parent_obj.FindChild("STAR_2");

                    /*
                    //test
                    GameObject go = GameObject.Find("STAR_COUNT_2");
                    print(go.transform.childCount);

                    //    print();
                    var obj = go.transform.GetChild(0);

                    var obj1 = go.transform.GetChild(1);
                    obj.parent = null;
                    obj1.parent = null;
                    //   Destroy(obj.GetComponentInChildren<Transform>());


                    //test
                    //test
              
                  //  List<GameObject> goList = new List<GameObject>();
                  //  goList = GameObject.Find("STAR_COUNT_2");
                  //  print(go.transform.childCount);

                    //    print();
                    var obj11 = go.transform.GetChild(0);

                    var obj111 = go.transform.GetChild(1);
                    obj11.parent = null;
                    obj111.parent = null;
                    //   Destroy(obj.GetComponentInChildren<Transform>());


                    //test
                          */
                    string str = "";
                    foreach (char c in inputWordList)
                    {
                        str = str + c;
                    }
                    print(str);
                    // SequenceEqual
                    if (dicWord.Contains(str))
                    {
                        print("CONTAIN { " + str + " }");
                        StartCoroutine(RemoveInfoTable());
                       
                        //open Word

                        var obj = GameObject.Find("STAR_OBJ_" + str);
                        var child = obj.transform.GetChild(obj.transform.childCount-1);
                        child.parent = star_panel.transform;
                        child.gameObject.SetActive(true);
                        obj.SetActive(false);


                        RefreshWordInput();
                    }
                });

            }
            else if (fooObj.name == "Cancel_btn")
            {

                fooObj.GetComponent<Button>().onClick.AddListener(() =>
                {
                    RefreshWordInput();

                });
            }

        }
    }

    void RefreshWordInput()
    {
        foreach (GameObject t in toogleList)
        {
            t.GetComponent<Toggle>().isOn = false;
            t.GetComponent<Toggle>().interactable = true;
        }
        inputWordList.Clear();
        foreach (GameObject t in inputLatter)
        {
            t.SetActive(false);
            Destroy(t.GetComponentInChildren<Text>());
        }
    }

    #endregion

    void SortMyWordList()
    {
        for (int i = 0; i < dicWord.Count; i++)
        {
            string tmp;
            for (int j = 0; j < dicWord.Count; j++)
            {
                if (dicWord[i].Length < dicWord[j].Length)
                {
                    tmp = dicWord[i];
                    dicWord[i] = dicWord[j];
                    dicWord[j] = tmp;
                }
            }
        }
        for(int n = 0; n<dicWord.Count;n++)
        print("dic sort =" + dicWord[n]);
    }
}

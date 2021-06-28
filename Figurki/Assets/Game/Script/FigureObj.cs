using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FigureObj : MonoBehaviour
{
    private Sprite figure_spr;
    public Sprite Figure_spr { get; set; }

    GameObject CreateFigure()
    {
        GameObject obj;
        obj = new GameObject("obj");
        obj.AddComponent<Image>();
        
        return obj;
    }

    void SetImage(Sprite spr, GameObject obj)
    {
        obj.GetComponent<Image>().sprite = this.figure_spr;
    }




}

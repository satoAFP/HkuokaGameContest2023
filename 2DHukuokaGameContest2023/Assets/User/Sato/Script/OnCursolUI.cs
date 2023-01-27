using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCursolUI : MonoBehaviour
{
    [SerializeField, Header("ïœâªÇ≥ÇπÇÈêF")] private Color color;


    // Update is called once per frame
    void Update()
    {
        Vector2 mouse = Input.mousePosition;
        mouse.x -= 1920 / 2;
        mouse.y -= 1080 / 2;

        GameObject parent = gameObject.transform.parent.gameObject;
        Vector2 pos = parent.GetComponent<RectTransform>().localPosition;
        Vector2 size = parent.GetComponent<RectTransform>().sizeDelta;


        if (mouse.x > pos.x - (size.x / 2) && mouse.x < pos.x + (size.x / 2) &&
            mouse.y > pos.y - (size.y / 2) && mouse.y < pos.y + (size.y / 2)) 
        {
            parent.GetComponent<Outline>().effectColor = color;
            
        }
        else
        {
            parent.GetComponent<Outline>().effectColor = new Color(1, 1, 1, 0);
        }
    }
}

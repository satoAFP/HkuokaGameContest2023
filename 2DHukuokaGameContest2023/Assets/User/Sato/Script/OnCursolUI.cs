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

        Vector2 pos = gameObject.transform.parent.GetComponent<RectTransform>().localPosition;
        Vector2 size = gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta;


        if (mouse.x > pos.x - (size.x / 2) && mouse.x < pos.x + (size.x / 2) &&
            mouse.y > pos.y - (size.y / 2) && mouse.y < pos.y + (size.y / 2)) 
        {
            gameObject.transform.parent.gameObject.GetComponent<Outline>().effectColor = color;
            
        }
        else
        {
            gameObject.transform.parent.gameObject.GetComponent<Outline>().effectColor = new Color(1, 1, 1, 0);
        }
    }
}

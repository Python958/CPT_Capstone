using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMouseFollow : MonoBehaviour
{
    private RectTransform pos;
    private Canvas myCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        Canvas[] Canvases = GetComponentsInParent<Canvas>();
        myCanvas = Canvases[Canvases.Length - 1];

        if (Cursor.visible) { Cursor.visible = false; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible) { Cursor.visible = false; }
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);
        transform.SetAsLastSibling();
    }
}

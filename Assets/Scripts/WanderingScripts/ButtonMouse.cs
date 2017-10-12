using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMouse : MonoBehaviour {

    public bool Reverse;
    public Texture2D activeCursor;
    public Texture2D noActiveCursor;
	// Use this for initialization
	void Start () {
        //Cursor.SetCursor(activeCursor, new Vector2(0, 0), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnMouseEnter()
    {
        try
        {
            if (!Reverse)
            {
                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Замена курсора на активный");

                Cursor.SetCursor(activeCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
            }
            else
            {
                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Замена курсора на НЕ активный");

                Cursor.SetCursor(noActiveCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Ошибка при замене текстуры курсора: " + e.ToString());
        }
    }

    private void OnMouseExit()
    {
        try
        {
            if (!Reverse)
            {
                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Замена курсора на НЕ активный");

                Cursor.SetCursor(noActiveCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
            }
            else
            {
                //Debug.Log(DateTime.Now.ToString("hh:mm:ss:ffff") + ": Замена курсора на активный");

                Cursor.SetCursor(activeCursor, new Vector2(0, 0), CursorMode.ForceSoftware);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Ошибка при замене текстуры курсора: " + e.ToString());
        }
    }
}

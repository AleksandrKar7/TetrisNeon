using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFrame : MonoBehaviour {

    public GameObject gObjFrame;
    public Image imgAvatar;
    public Text txtNickName;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitializeFramePlayerPanel(string pathToAvatar, string nickName)
    {
        txtNickName.text = nickName;

        if (File.Exists(pathToAvatar))
        {
            WWW www = new WWW("file://" + pathToAvatar);

            imgAvatar.sprite = Sprite.Create(((www.texture)),
                         new Rect(1, 1, ((www.texture)).width - 1, ((www.texture)).height - 1),
                         new Vector2());
        }
    }

    public void SetActive(bool itActive)
    {
        gObjFrame.SetActive(itActive);
    }
}

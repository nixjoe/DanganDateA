using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public static Manager instance;
    private MenuHandler menu;

    void Start () {
		
        if (FindObjectsOfType<Manager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            Init();
        }

	}
	
    void Init()
    {
        menu = FindObjectOfType<MenuHandler>();
    }

    public static MenuHandler Menu
    {
        get
        {
            return instance.menu;
        }   
    }
}

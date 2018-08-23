using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public TMPro.TextMeshProUGUI tooltip;
    private float tooltipTime;

    void Start () {
		
	}
	
	void Update () {

        Color c = tooltip.color;

        if (tooltipTime > 0)
        {
            c.a = Mathf.Lerp(c.a, 1, Time.deltaTime * 12);
            tooltipTime -= Time.deltaTime;
        }
        else
        {
            c.a = Mathf.Lerp(c.a, 0, Time.deltaTime * 12);
        }
        tooltip.color = c;


    }

    public void ShowTooltip(float time, string content)
    {
        tooltipTime = time;

        tooltip.text = content;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour {

    public TMPro.TextMeshProUGUI tooltip, health, location, money;
    private float tooltipTime;

    public Animator menuAnimator, difficultyAnimator, popupAnimator;
    private bool difOpen = false;
    private bool menuOpen = true;

    void Start () {
        Time.timeScale = 0;
    }
    
    IEnumerator DisplayPopup(float time, string content)
    {
        popupAnimator.GetComponent<TMPro.TextMeshProUGUI>().text = content;

        popupAnimator.gameObject.SetActive(true);

        popupAnimator.Play("Open");

        yield return new WaitForSeconds(time);

        popupAnimator.Play("Close");
        yield return new WaitForSeconds(0.2f);

        popupAnimator.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        popupAnimator.gameObject.SetActive(false);
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

    public void ToggleDifficulty()
    {
        if (difOpen)
        {
            difficultyAnimator.Play("Close");
        }
        else
        {
            difficultyAnimator.Play("Open");
        }

        difOpen = !difOpen;
    }

    public void ToggleMenu()
    {
        if (menuOpen)
        {
            if (difOpen)
            {
                ToggleDifficulty();
            }

            Time.timeScale = 1;

            menuAnimator.Play("Close");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Locked;

            StartCoroutine(DisplayPopup(3, "CLEAN UP SPY IN GARDEROBE"));
        }
        else
        {
            Time.timeScale = 0;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            menuAnimator.Play("Open");
        }

        menuOpen = !menuOpen;
    }

    public void ShowPopup(float time, string content)
    {
        StartCoroutine(DisplayPopup(time, content));
    }

    public void SetHealth(int amount)
    {
        health.text = amount.ToString();
    }
}

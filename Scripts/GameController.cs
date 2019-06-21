using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject pauseUI;
    public GameObject bagUI;
    public GameObject bagPanel;

    private bool clickBag;

	void Start () {
        pauseUI.SetActive(false);
    }

    public void PauseGame()
    {
        MapsManager.isPause = true;
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void ContinueGame()
    {
        MapsManager.isPause = false;
        pauseUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ClickBagButton()
    {
        if (bagUI.transform.position.x == 999)
        {
            bagUI.GetComponent<RectTransform>().Translate(transform.right * -245);
            bagPanel.GetComponent<RectTransform>().Translate(transform.right * -245);
            bagUI.GetComponentInChildren<Text>().text = ">";
        }
        else if (bagUI.transform.position.x != 999)
        {
            bagUI.GetComponent<RectTransform>().Translate(transform.right * 245);
            bagPanel.GetComponent<RectTransform>().Translate(transform.right * 245);
            bagUI.GetComponentInChildren<Text>().text = "<";
        }
    }
}

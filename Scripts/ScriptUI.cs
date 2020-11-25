using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptUI : MonoBehaviour
{
    GameObject player;
    [SerializeField]
    GameObject startPanel;
    [SerializeField] Button buttonBoostForce;
    [SerializeField] Image rays;
    [SerializeField] Image filled;
    [SerializeField] Text quantityBoostBollText;
    static public Button ButtonBoostForce { get; set; }
    static public Text QuantityBoostBollText { get; set; }
    static public Image Rays { get; set; }
    static public Image Filled { get; set; }

    static public bool isStarted { get; set; } = false;
    private void Awake()
    {
        Rays = rays;
        ScriptUI.Filled = filled;
        ButtonBoostForce = buttonBoostForce;
        QuantityBoostBollText = quantityBoostBollText;
        StartCoroutine(Initialization());
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        shell.QuantityBoostBoll = 0;
    }

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        startPanel.SetActive(false);
        ScriptUI.isStarted = true;
    }


   


}

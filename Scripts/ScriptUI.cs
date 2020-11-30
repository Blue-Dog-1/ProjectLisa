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
    [SerializeField] Text lavelText;
    [Space]
    [SerializeField] Text lavelLable;
    [Header("End Level Panel 'ELP' ")]
    [SerializeField] GameObject endLevelPanel;
    [SerializeField] List<StarAnim> starsELP;

    [Space]
    [Header("Bust blok ")]
    [SerializeField] Button buttonBoostForce;
    [SerializeField] Image rays;
    [SerializeField] Image filled;
    [SerializeField] Text quantityBoostBollText;

    [Space]
    [Header("Stars blok")]
    [SerializeField] List<Image> stars;
    [SerializeField] Sprite activStars;
    [SerializeField] Sprite disActivStars;

    [Space]
    [Header("Fireworks")]
    [SerializeField] GameObject Fireworks;
    [SerializeField] Text QuantityAbsorbedObjects;
    [SerializeField] Text OutLableRatioAbsorbedObjects;
    [SerializeField] Text quantityAbsorbedObjectsText;


    // boost blok
    static public Button ButtonBoostForce { get; private set; }
    static public Text QuantityBoostBollText { get; private set; }
    static public Image Filled { get; private set; }
    static public Text QuantityAbsorbedObjectsText { get; set; }

    static public bool isStarted { get; set; } = false;

    [SerializeField] [Range(0f, 1)]
    float[] thresholdsStars = new float[3] { 0.4f, 0.65f, 0.85f };


    private void Awake()
    {
        isStarted = false;
        Events.Rays = rays;
        Filled = filled;
        ButtonBoostForce = buttonBoostForce;
        QuantityBoostBollText = quantityBoostBollText;
        QuantityAbsorbedObjectsText = quantityAbsorbedObjectsText;

        StartCoroutine(Initialization());
        Events.Finish += Finish;
        Events.Finish += StopAllCoroutines;
        Events.Move += () => {
            if (isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                    StartCoroutine(loop());
        };


    }
    private void Start()
    {
        lavelLable.text += PlayerPrefs.GetInt("Level", 0).ToString();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public void Restart()
    {
        Events.Cliner();
        shell.QuantityBoostBoll = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        startPanel.SetActive(false);
        ScriptUI.isStarted = true;
    }

    
    void Finish()
    {
        Fireworks.SetActive(true);
        /*
         * hiding blok UI an object
         */
        buttonBoostForce.gameObject.transform.parent.gameObject.SetActive(false);
        stars[0].gameObject.transform.parent.parent.gameObject.SetActive(false);

        /*
         * Show blok end level
         */
        endLevelPanel.SetActive(true);

        var allObjects = Shredder.QuantityObjects + Attraction.QuantityObjects;
        var ratio = 1f / allObjects;
        var ratioAbsorbedObjects = ratio * Attraction.QuantityObjects;
        
        starsELP[0].StartAnimation((ratioAbsorbedObjects > thresholdsStars[0]));
        starsELP[0].isActov = (ratioAbsorbedObjects > thresholdsStars[1]);
        starsELP[1].isActov = (ratioAbsorbedObjects > thresholdsStars[2]);

        
        OutLableRatioAbsorbedObjects.text = "" + ratioAbsorbedObjects;
        QuantityAbsorbedObjects.text = Attraction.QuantityObjects.ToString();
        SaveLevel();
    }
    
    void SaveLevel()
    {
        var carenLevel = PlayerPrefs.GetInt("Level", 0);
        lavelText.text += carenLevel;
        PlayerPrefs.SetInt("Level", carenLevel + 1);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// real-time star counting
    /// </summary>
    public IEnumerator loop()
    {
        Transform finish = GameObject.FindGameObjectWithTag("Finish").transform;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        Debug.Log(player.name);
        float obsolutDistance = (finish.position - player.position).magnitude;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float carentDistance = (finish.position - player.position).magnitude;
            float ratioTime = (1f / obsolutDistance) * (obsolutDistance - carentDistance);

            int allObjects = Shredder.QuantityObjects + Attraction.QuantityObjects;
            float ratio = 1f / allObjects;
            float ratioAbsorbedObjects = ratio * Attraction.QuantityObjects;

            int i = 0;
            
            foreach (Image star in stars)
            {
                if ((ratioAbsorbedObjects * ratioTime) > thresholdsStars[i])
                {
                    star.sprite = activStars;
                    var calor = star.color;
                    calor.a = 1f;
                    star.color = calor;
                }
                else star.sprite = disActivStars;
                i++;
            }
        }
    }
}
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

    [Space]
    [Header("Distence to Finish")]
    [SerializeField] Image DistenceFinish;


    [Space]
    [Header("No internet connection")]
    [SerializeField] GameObject NoInternetConnection;


    [Space]
    [SerializeField] int minThresholdsAds;
    [SerializeField] int maxThresholdsAds;

    [Space]
    [Header("Pause button change sprite")]
    [SerializeField] Image pauseImage;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite continueSprite;

    bool showAds = false;


    // boost blok
    static public Button ButtonBoostForce { get; private set; }
    static public Text QuantityBoostBollText { get; private set; }
    static public Image Filled { get; private set; }
    static public Text QuantityAbsorbedObjectsText { get; set; }
    static public bool isStarted { get; set; } = false;

    [SerializeField] [Range(0f, 1)]
    float[] thresholdsStars = new float[3] { 0.4f, 0.65f, 0.85f };

    bool isPause = false;
    private void Awake()
    {
        isStarted = false;
        Events.Rays = rays;
        Filled = filled;
        ButtonBoostForce = buttonBoostForce;
        QuantityBoostBollText = quantityBoostBollText;
        QuantityAbsorbedObjectsText = quantityAbsorbedObjectsText;

        startPanel.SetActive(true);
        Events.Finish += Finish;
        Events.Finish += StopAllCoroutines;
        Events.Move += () => {
            if (isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                    StartCoroutine(loop());
        };

        Events.Restart += delegate()  {
            Events.Cliner();
            StartCoroutine(LoadLevel());
        };


    }
    private void Start()
    {
        showAds = false;
        lavelLable.text += PlayerPrefs.GetInt("Level", 1).ToString();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        #region check internet connection
        string HtmlText = Events.GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            //No connection
            Debug.Log("No connection");
            NoInternetConnection.SetActive(true);
            buttonBoostForce.gameObject.transform.parent.gameObject.SetActive(false);
            stars[0].gameObject.transform.parent.parent.gameObject.SetActive(false);
            StopAllCoroutines();
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found
        }
        else
        {
            //success
            Debug.Log("Invernet Conected");
            NoInternetConnection.SetActive(false);
            StartCoroutine(Initialization());
        }
        #endregion
    }
    public void Restart()
    {
        startPanel.SetActive(true);
        shell.QuantityBoostBoll = 0;

        if (showAds)
            Events.ShowAds?.Invoke();
        else
        {
            Events.Restart?.Invoke();
        }    
    }

    IEnumerator LoadLevel()
    {
        var scene = SceneManager.GetActiveScene().name;
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        while(operation.isDone)
        {
            yield return null;
        }
    }
    public void Pause()
    {
        if (!isPause)
        {
            Time.timeScale = 0f;
            isPause = true;
            pauseImage.sprite = continueSprite;
        }
        else
        {
            Time.timeScale = 1f;
            isPause = false;
            pauseImage.sprite = pauseSprite;
        }
    }
    IEnumerator Initialization()
    {
        yield return new WaitForSeconds(2f);
        startPanel.SetActive(false);
        isStarted = true;
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

        var allObjects = Events.QuantityObjects + Attraction.QuantityObjects;
        var ratio = 1f / allObjects;
        var ratioAbsorbedObjects = ratio * Attraction.QuantityObjects;
        
        starsELP[0].StartAnimation((ratioAbsorbedObjects > thresholdsStars[0]));
        starsELP[0].isActov = (ratioAbsorbedObjects > thresholdsStars[1]);
        starsELP[1].isActov = (ratioAbsorbedObjects > thresholdsStars[2]);
        
        OutLableRatioAbsorbedObjects.text = "" + ratioAbsorbedObjects;
        QuantityAbsorbedObjects.text = Attraction.QuantityObjects.ToString();

        #region if absorbed objects count < thresholds first stars show ads ad dont save
        if ((ratioAbsorbedObjects > thresholdsStars[0]))
            SaveLevel();
        else
        {
            showAds = true;
            QuantityAbsorbedObjects.text = "few absorbed objects to go to the next level";
            // red color text 
            QuantityAbsorbedObjects.color = new Color(1f, 0f, 0f);
        }
        #endregion


        #region last impression counter
        var LIC = PlayerPrefs.GetInt("Last Impression Counter", 1);
        if (LIC >= minThresholdsAds && RandomClamp(LIC, maxThresholdsAds))
        {
            showAds = true;
            PlayerPrefs.SetInt("Last Impression Counter", 1);
        }
        else 
            PlayerPrefs.SetInt("Last Impression Counter", LIC + 1);

        #endregion

    }
    
    bool RandomClamp(int value, int max)
    {
        if (Random.Range(value, max) == value)
            return true;
        else return false;
    }

    void SaveLevel()
    {
        var carenLevel = PlayerPrefs.GetInt("Level", 1);
        lavelText.text += carenLevel;
        PlayerPrefs.SetInt("Level", carenLevel + 1);
        PlayerPrefs.Save();
    }
    
    public void ResetPlayerPrefs() => Events.ResetSave();

    public void ShowSettings(bool show)
    {
        GameObject.Find("SettingsPanel").SetActive(show);
        Pause();
    }
    
    /// <summary>
    /// real-time star counting
    /// </summary>
    public IEnumerator loop()
    {
        Transform finish = GameObject.FindGameObjectWithTag("Finish").transform;
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        float obsolutDistance = (finish.position - player.position).magnitude;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            float carentDistance = (finish.position - player.position).magnitude;
            float ratioTime = (1f / obsolutDistance) * (obsolutDistance - carentDistance);

            DistenceFinish.fillAmount = ratioTime;

            int allObjects = Events.QuantityObjects + Attraction.QuantityObjects;
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
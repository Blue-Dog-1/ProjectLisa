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
    [SerializeField] Text lavelLable;
    [Header("End Level Panel 'ELP' ")]
    [SerializeField] GameObject endLevelPanel;
    [SerializeField] List<Image> starsELP;

    [Header("Bust blok ")]
    [SerializeField] Button buttonBoostForce;
    [SerializeField] Image rays;
    [SerializeField] Image filled;
    [SerializeField] Text quantityBoostBollText;


    [Header("Stars blok")]
    [SerializeField] List<Image> stars;
    [SerializeField] Sprite activStars;
    [SerializeField] Sprite disActivStars;

    // boost blok
    static public Button ButtonBoostForce { get; private set; }
    static public Text QuantityBoostBollText { get; private set; }
    static public Image Rays { get; private set; }
    static public Image Filled { get; private set; }

    static public bool isStarted { get; set; } = false;

    [SerializeField] [Range(0f, 1)]
    float[] thresholdsStars = new float[3] { 0.4f, 0.65f, 0.85f };
    private void Awake()
    {

        Rays = rays;
        Filled = filled;
        ButtonBoostForce = buttonBoostForce;
        QuantityBoostBollText = quantityBoostBollText;

        StartCoroutine(Initialization());
        Events.Finish += Finish;
        Events.Finish += StopAllCoroutines;
        Events.Move += () =>
        {
            if (isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    StartCoroutine(loop(Events.seconds));
                }
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
        Debug.Log("END LAVEL");
        
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

        int i = 0;
        foreach (Image star in starsELP)
        {
            if (ratioAbsorbedObjects > thresholdsStars[i])  
            {
                star.sprite = activStars;
                var calor = star.color;
                calor.a = 1f;
                star.color = calor;
            }
            else star.sprite = disActivStars;
            i++;
        }

        SaveLevel();

    }
    void SaveLevel()
    {
        var carenLevel = PlayerPrefs.GetInt("Level", 0);
        Debug.Log(PlayerPrefs.GetInt("Level", 0));

        PlayerPrefs.SetInt("Level", carenLevel + 1);

        PlayerPrefs.Save();
    }

    public IEnumerator loop(float seconds)
    {
        float counterTime = 0f;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            counterTime += 0.5f;
            float ratioTime = (1f / seconds) * counterTime;
            
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
    

    /*
     * 
     * переменая T это процентное соотншение пройденого время оно равно = (1f / Все время) * проеденое время;
     * 
     * переменая К количестов всех объектов = количество седеных объектов + количестов потеряных объектов;
     * 
     * пременая М это соотношение сьеденых обектов к потеряным = К * количество сьеденых объектов;
     * 
     * результат переменя А = М * Т;  в зовисемости от времени Т бутет расти М (ели М = 1 а Т = 0,1 то и м будет равно 0,1 )
     *  
     * 
     */





}

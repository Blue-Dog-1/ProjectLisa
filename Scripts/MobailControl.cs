using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobailControl : MonoBehaviour
{
    private Vector2 vector;
    private Vector3 direction;
    private Vector3 newPos;
    [SerializeField] float velocity;
    [SerializeField] float minDistans;
    [SerializeField] float maxDistans; 
    [SerializeField] float speed;
    [SerializeField] float seconds;
    [SerializeField] GameObject ControllPanel;

    
    static public float Speed { get; set; }
    Transform parent;
    

    private void Awake()
    {
        Speed = speed;
        Events.seconds = seconds;
        Events.Player = gameObject;
    }
    void Start()
    {
        Events.Move += () => {
            if (ScriptUI.isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    ControllPanel.SetActive(true);
                    Events.Move = () =>
                    {
                        parent.Translate(Vector3.forward * speed);
                    };
                }
        };

        direction = Vector3.zero;
        newPos = transform.position;
        StartCoroutine(loop());
        parent = transform.parent.transform;

        Events.Finish += () => { 
            Events.Move = () => { };
            gameObject.SetActive(false);
        };
    }
    
    IEnumerator loop()
    {
        float time = 2f;
        while (time > 0)
        {
            yield return new WaitForSeconds(0.05f);
            transform.Translate(Vector3.down * Mathf.Lerp(0, 1, Time.deltaTime));
            time -= 0.05f;
        }
    }
    void FixedUpdate()
    {
        Events.Move?.Invoke();
    }

    public void OnFnish() => Events.onFinish();
    
}


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

    
    static public float Speed { get; set; }
    Vector2 pos;
    Transform parent;

    

    private void Awake()
    {
        MobailControl.Speed = speed;
        Events.seconds = seconds;
    }
    void Start()
    {
        Events.Move += () => {
            if (ScriptUI.isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    StartCoroutine(Timer(seconds));
                    Events.Move = () =>
                    {
                        if (Input.touchCount > 0)
                        {
                            Touch touch = Input.GetTouch(0);

                            if (touch.phase == TouchPhase.Began)
                                pos = touch.position;
                            else if (touch.phase == TouchPhase.Ended)
                                pos = Vector2.zero;

                            if (touch.phase == TouchPhase.Stationary)
                                pos = touch.position;

                            vector = (pos - touch.position).normalized;

                            direction.x = -vector.x;
                            //direction.z = -vector.y;
                            transform.position += direction * velocity;

                        }
                        parent.Translate(Vector3.forward * speed);
                    };
                }
        };

        direction = Vector3.zero;
        pos = Vector2.zero;
        newPos = transform.position;
        StartCoroutine(loop());
        parent = transform.parent.transform;

        Events.Finish += () => { Events.Move = () => { }; };
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

    IEnumerator Timer(float Seconds)
    {
        yield return new WaitForSeconds(Seconds);
        Events.onFinish();

    }

    

    
}


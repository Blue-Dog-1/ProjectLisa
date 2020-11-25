using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobailControl : MonoBehaviour
{
    private Vector2 vector;
    private Vector3 direction;
    private Vector3 newPos;

    [SerializeField]
    private float velocity;
    [SerializeField]
    private float minDistans;
    [SerializeField]
    private float maxDistans; 

    public float Speed;

    Vector2 pos;
    Transform parent;

    delegate void Move();
    Move move;
    void Start()
    {
        move = () => {
            if (ScriptUI.isStarted)
                if (Input.touchCount > 0 || Input.GetMouseButton(0))
                {
                    move = () =>
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
                        parent.Translate(Vector3.forward * Speed);
                    };
                }
        };

        direction = Vector3.zero;
        pos = Vector2.zero;
        newPos = transform.position;
        StartCoroutine(loop());
        parent = transform.parent.transform;
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
        move.Invoke();
    }

    

    
}

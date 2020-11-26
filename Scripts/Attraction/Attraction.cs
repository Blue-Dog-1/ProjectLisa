using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attraction : MonoBehaviour
{
    public float radius;
    public float force;
    Vector3 direction;
    float distnsforse;
    private Rigidbody itemRB;
    private Vector3 Scale;
    [Range(0f, 0.3f)]
    public float GrowthRate;
    private Collider[] col;
    [Space] [Range(1, 90)]
    public int angle_deviation = 10;
    float inverseSquares;

    [HideInInspector]
    public MeshDeform meshDeform;
    public GameObject Sphere;
    public static bool isBoostForce { get; private set; } = false;

    static public Transform Playar { get; private set; }
    public Text QuantityAbsorbedObjectsText;
    static public int QuantityObjects { get; private set; } = 0;
    void Start()
    {
        Scale = transform.localScale;
        Playar = gameObject.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        col = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider c in col)
        {

            itemRB = c.GetComponent<Rigidbody>();
            if (itemRB != null) {
                direction = gameObject.transform.position - itemRB.position;

                inverseSquares = force / (1 + (direction.magnitude * direction.magnitude));

                itemRB.AddForce(direction.normalized * inverseSquares, ForceMode.Force);

            }
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
#endif
    public void OnTriggerEnter(Collider collider)
    {
        collider.enabled = false;
        collider.gameObject.GetComponent<MeshDeform>()._start(gameObject.transform);
        QuantityObjects++;
        QuantityAbsorbedObjectsText.text = QuantityObjects.ToString();
    }

    
    public void BoostForce(Button b)
    {
        ScriptUI.Rays.enabled = false;
        StartCoroutine(BosstRadius(4f, 2.2f, b));
    }
    public IEnumerator BosstRadius(float time, float boost, Button button)
    {
        isBoostForce = true;
        float speed = 5f;
        Sphere.SetActive(true);
        Vector3 Scale = transform.localScale * boost;
        Vector3 ScaleDefolt = transform.localScale;
        float _radius = radius * boost;
        float _radiusDefolt = radius;
        float halfTime = time / 2;
        force *= boost;

        while (time > 0f)
        {
            if (time > halfTime)
            {
                yield return new WaitForSeconds(0.05f);
                transform.localScale = Vector3.Lerp(transform.localScale, Scale, Time.deltaTime * speed);
                radius = Mathf.Lerp(radius, _radius, Time.deltaTime * speed);
                time -= 0.05f;
            }
            else if (time < halfTime)
            {
                yield return new WaitForSeconds(0.05f);
                transform.localScale = Vector3.Lerp(transform.localScale, ScaleDefolt, Time.deltaTime * speed);
                radius = Mathf.Lerp(radius, _radiusDefolt, Time.deltaTime * speed);
                time -= 0.05f;
            }
        }
        force /= boost;


        transform.localScale = ScaleDefolt;
        radius = _radiusDefolt;
        Sphere.SetActive(false);

        
        button.interactable = (shell.QuantityBoostBoll >= shell.ActivationThreshold);
        ScriptUI.Rays.enabled = (shell.QuantityBoostBoll >= shell.ActivationThreshold);
        isBoostForce = false;

        
    }

        

}

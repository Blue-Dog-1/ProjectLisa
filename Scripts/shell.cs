using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class shell : MonoBehaviour
{
    [SerializeField]
    public static int QuantityBoostBoll { get; set; } = 0;
    [SerializeField] int activationThreshold;
    static public int ActivationThreshold { get; set; }

    private void Awake()
    {
        shell.ActivationThreshold = activationThreshold;

    }
    private void Start()
    {

        ScriptUI.ButtonBoostForce.onClick.AddListener(ButtonClik);
        Debug.Log(shell.ActivationThreshold);
    }
    void ButtonClik()
    {
        QuantityBoostBoll -= ActivationThreshold;
        if (QuantityBoostBoll < ActivationThreshold)
        {
            ScriptUI.Rays.enabled = false;
            ScriptUI.ButtonBoostForce.interactable = false;
        }
        ScriptUI.Filled.fillAmount = Mathf.Clamp((1f / ActivationThreshold) * QuantityBoostBoll, 0f, 1f);
        ScriptUI.QuantityBoostBollText.text = QuantityBoostBoll.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        QuantityBoostBoll++;

        ScriptUI.QuantityBoostBollText.text = QuantityBoostBoll.ToString();

        ScriptUI.Filled.fillAmount += (1f / shell.ActivationThreshold);
        if (QuantityBoostBoll >= ActivationThreshold)
        {
            ScriptUI.Rays.enabled = true;
            ScriptUI.ButtonBoostForce.interactable = !Attraction.isBoostForce;
        }
        other.enabled = false;
        other.gameObject.GetComponent<MeshDeform>()._start(gameObject.transform);
    }
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTracker : MonoBehaviour
{
    private AttractorController m_Attractor;
    [SerializeField] private bool m_IsLeftHand;
    private bool m_SavingIndicatorPlaying;
    [SerializeField] private GameObject m_SavingText;
    [SerializeField] private GameObject m_TriggerVisualizer;
    
    private void Start()
    {
        m_Attractor = GetComponent<AttractorController>();
        m_SavingText.SetActive(false);
    }

    private void Update()
    {
        float trigger;
        if (m_IsLeftHand) trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch);
        else trigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.RTouch);
        
        //Debug.Log(trigger);
        if (trigger > 0.05f)
        {
            float radius = trigger / 8f;
            m_Attractor.SetRadius(radius);
            m_TriggerVisualizer.transform.localScale = Vector3.one * radius;
            m_Attractor.StartDrawing();
        }
        else
        {
            m_Attractor.StopDrawing();
        }
        
        // Save
        if (m_IsLeftHand)
        {
            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.LTouch))
            {
                m_Attractor.SaveToFile();
                if (!m_SavingIndicatorPlaying)
                {
                    StartCoroutine(SavingIndicator());
                }
            }
            
        }
    }
    
    IEnumerator SavingIndicator()
    {
        m_SavingIndicatorPlaying = true;
        m_SavingText.SetActive(true);
        m_SavingText.GetComponent<TextMesh>().text = "Export\nComplete";
        yield return new WaitForSeconds(2f);
        m_SavingText.SetActive(false);
        m_SavingIndicatorPlaying = false;
    }
}

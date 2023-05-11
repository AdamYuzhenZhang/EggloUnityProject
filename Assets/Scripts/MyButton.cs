using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MyButton : MonoBehaviour
{
    [SerializeField] private Material m_Default;
    [SerializeField] private Material m_Clicked;
    public UnityEvent methods;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FingerTip"))
        {
            Debug.Log("Collided");
            GetComponent<MeshRenderer>().material = m_Clicked;
            methods.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FingerTip"))
        {
            Debug.Log("Exit");
            GetComponent<MeshRenderer>().material = m_Default;
        }
    }
}

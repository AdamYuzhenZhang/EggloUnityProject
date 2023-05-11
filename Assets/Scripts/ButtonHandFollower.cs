using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandFollower : MonoBehaviour
{
    [SerializeField] private Transform m_ParentTransform;
    private Camera m_Cam;
    [SerializeField] private float m_Threshold = 0.95f;
    private bool m_IsFacingCamera = true;
    [SerializeField] private GameObject m_Buttons;
    private Quaternion m_InitialLocalRotation;
    private void Start()
    {
        m_Cam = Camera.main;
        m_InitialLocalRotation = m_Buttons.transform.localRotation;
    }

    private void Update()
    {
        transform.position = m_ParentTransform.position;
        transform.rotation = m_ParentTransform.rotation;
        bool facing = FacingCamera();
        if (m_IsFacingCamera != facing)
        {
            RespondToFacing(facing);
            m_IsFacingCamera = facing;
        }
    }
    
    // hide buttons if is facing camera
    private bool FacingCamera()
    {
        Vector3 up = -transform.up;
        Vector3 toCam = (m_Cam.transform.position - transform.position).normalized;
        return (Vector3.Dot(up, toCam) > m_Threshold);
    }

    private void RespondToFacing(bool isFacing)
    {
        m_Buttons.SetActive(isFacing);
        if (isFacing)
        {
            // detach from parent
            m_Buttons.transform.parent = null;
            m_Threshold = 0.6f;
            // disable after 3 seconds
            //StartCoroutine(HideMenuAfterDelay());
        }
        else
        {
            // attach
            m_Buttons.transform.parent = transform;
            m_Buttons.transform.localPosition = Vector3.zero;
            m_Buttons.transform.localRotation = m_InitialLocalRotation;
            m_Threshold = 0.95f;
        }
    }

    IEnumerator HideMenuAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        m_Buttons.transform.parent = transform;
        m_Buttons.transform.localPosition = Vector3.zero;
        m_Buttons.transform.localRotation = m_InitialLocalRotation;
        m_Threshold = 0.95f;
        
    }
}

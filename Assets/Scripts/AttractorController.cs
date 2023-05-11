using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Controls the start and stop of drawing
/// </summary>
public class AttractorController : MonoBehaviour
{
    private Vector3 m_AttractorPosition;
    private Vector3 m_MovingDirection;
    private Vector3 m_LastPosition;

    [SerializeField] private DataStructure m_DataStructure;

    [SerializeField] private float m_FixedUpdateRate;
    [SerializeField] private float m_Radius;
    [SerializeField] private float m_CountDownTime;

    private bool m_StartGenerating;

    [SerializeField] private GameObject m_TextRoot;
    [SerializeField] private TextMesh m_TextMesh;

    public bool m_StopDraw;
    
    private void Start()
    {
        Time.fixedDeltaTime = m_FixedUpdateRate;
        m_LastPosition = transform.position;
        //StartCoroutine(WaitOnStart());
        m_TextRoot.SetActive(false);
    }

    public void SetRadius(float radius)
    {
        m_Radius = radius;
    }

    private void FixedUpdate()
    {
        if (m_StartGenerating && !m_StopDraw)
        {
            m_AttractorPosition = transform.position;
            m_MovingDirection = m_AttractorPosition - m_LastPosition;
            m_LastPosition = m_AttractorPosition;

            // create a new random point
            m_DataStructure.CreateNewPointOnPlate(m_AttractorPosition, m_MovingDirection, m_Radius, m_CountDownTime);
        }
    }

    public void Draw5Seconds()
    {
        StartCoroutine(DrawFor5Seconds());
    }
    
    IEnumerator DrawFor5Seconds()
    {
        m_TextRoot.SetActive(true);
        float seconds = 2f;
        m_TextMesh.text = seconds.ToString("F1");
        while (seconds >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            seconds -= 0.1f;
            m_TextMesh.text = seconds.ToString("F1");
        }
        m_StartGenerating = true;
        seconds = 2f;
        m_TextMesh.text = seconds.ToString("F1");
        while (seconds >= 0f)
        {
            yield return new WaitForSeconds(0.1f);
            seconds -= 0.1f;
            m_TextMesh.text = seconds.ToString("F1");
        }
        m_StartGenerating = false;
        m_TextRoot.SetActive(false);

    }

    public void StartDrawing()
    {
        m_StartGenerating = true;
    }
    public void StopDrawing()
    {
        m_StartGenerating = false;
    }

    public void SaveToFile()
    {
        m_DataStructure.SavePointsToCSV();
    }

    IEnumerator WaitOnStart()
    {
        yield return new WaitForSeconds(5f);
        m_StartGenerating = true;
        yield return new WaitForSeconds(5f);
        m_StartGenerating = false;
    }
}

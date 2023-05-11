using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointController : MonoBehaviour
{
    public DataStructure m_DataStructure;

    public float m_CountDownTime;
    public Vector3 m_TargetPosition;

    private Vector3 m_Velocity = Vector3.zero;
    private bool m_Active = true;
    private float m_StartTime;


    [SerializeField] private GameObject m_PointMesh;
    [SerializeField] private Material m_DisabledMaterial;

    private void Start()
    {
        // calculate initial speed
        //StartCoroutine(SelfCountDown());
    }

    private void Update()
    {
        if (m_Active)
        {
            // move to target location
            transform.position = Vector3.SmoothDamp(transform.position, m_TargetPosition, ref m_Velocity, m_CountDownTime / 4f);
        }
    }

    // disable itself after delay
    IEnumerator SelfCountDown()
    {
        yield return new WaitForSeconds(m_CountDownTime);
        DisableThisPoint();
    }

    private void DisableThisPoint()
    {
        m_Active = false;
        m_DataStructure.m_PointCloud.Remove(this.gameObject);
        m_PointMesh.GetComponent<Renderer>().material = m_DisabledMaterial;
    }

    public void ChangeMaterialBrightness(float intensity)
    {
        //Debug.Log(intensity);
        Material mat = m_PointMesh.GetComponent<Renderer>().material;
        mat.SetColor("_EmissionColor", new Color(1-intensity, 1-intensity, 1-intensity));
        mat.color = new Color(1-intensity, 1-intensity, 1-intensity);
        //m_PointMesh.GetComponent<MeshRenderer>().material. = new Color(intensity, 0, 0);
    }
}

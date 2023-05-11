using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointFlowStir : MonoBehaviour
{
    [SerializeField] private GameObject m_TargetHand;
    [SerializeField] private DataStructure m_Data;
    private List<GameObject> m_PointCloud = new List<GameObject>();
    
    private Vector3 m_AttractorPosition;
    private Vector3 m_MovingDirection;
    private Vector3 m_LastPosition;
    private void Start()
    {
        m_LastPosition = transform.position;
        StartCoroutine(GetPointsFromData());
    }

    IEnumerator GetPointsFromData()
    {
        yield return new WaitForSeconds(3f);
        m_PointCloud = m_Data.m_PointCloud;
        foreach (GameObject pt in m_PointCloud)
        {
            StaticPointController ptController = pt.GetComponent<StaticPointController>();
            ptController.m_Attractor = this.gameObject;
        }
    }
    
    private void Update()
    {
        transform.position = m_TargetHand.transform.position;
        
        m_AttractorPosition = transform.position;
        m_MovingDirection = m_AttractorPosition - m_LastPosition;
        m_LastPosition = m_AttractorPosition;
        
        foreach (GameObject pt in m_PointCloud)
        {
            StaticPointController ptController = pt.GetComponent<StaticPointController>();
            ptController.m_Direction = m_MovingDirection;
        }
    }
}

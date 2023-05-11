using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInteractor : MonoBehaviour
{
    [SerializeField] private DataStructure m_DataStructure;
    [SerializeField] private float m_DistanceThreshold = 0.3f;

    private void Update()
    {
        // check distance and change color
        List<GameObject> m_PointCloud = m_DataStructure.m_PointCloud;
        foreach (GameObject point in m_PointCloud)
        {
            float distance = Vector3.Distance(point.transform.position, transform.position);
            if (distance <= m_DistanceThreshold)
            {
                // light up
                float intensity = Mathf.Clamp(m_DistanceThreshold / (distance * 10), 0.2f, 1);
                point.GetComponent<PointController>().ChangeMaterialBrightness(intensity);
            }
        }

}
}

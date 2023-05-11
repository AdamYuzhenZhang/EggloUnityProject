using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointCloud : MonoBehaviour
{
    [SerializeField] private Vector3 m_Bounds;
    [SerializeField] private Vector3 m_Center;

    [SerializeField] private GameObject m_PointVisual;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(UpdatePoint());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator UpdatePoint()
    {
        while (true)
        {
            RandomPointInBound();
            yield return new WaitForSeconds(2f);
        }
    }

    public Vector3 RandomPointInBound()
    {
        // random values
        float x = Random.Range(m_Center.x - m_Bounds.x, m_Center.x + m_Bounds.x);
        float y = Random.Range(m_Center.y - m_Bounds.y, m_Center.y + m_Bounds.y);
        float z = Random.Range(m_Center.z - m_Bounds.z, m_Center.z + m_Bounds.z);
        Vector3 randomPoint = new Vector3(x, y, z);
        // Display the point
        m_PointVisual.transform.position = randomPoint;
        return randomPoint;
    }
}

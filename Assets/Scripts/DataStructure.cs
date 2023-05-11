using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using System.IO;
using System.Security.Cryptography;

/// <summary>
/// Handles point spawning
/// Stores all points
/// Connect all points
/// </summary>
public class DataStructure : MonoBehaviour
{
    // saving data
    [SerializeField] private string m_Filename = "D:/PointCloud.csv";
    [SerializeField] private bool m_OculusLocal = false;
    
    [SerializeField] private GameObject m_PointPrefab;
    [SerializeField] private GameObject m_LinePrefab;

    [SerializeField] private int m_NumConnection = 3;
    [SerializeField] private float m_MaxDistance = 0.08f;

    public List<GameObject> m_PointCloud = new List<GameObject>();
    public List<GameObject> m_LineConnections = new List<GameObject>();

    [SerializeField] private bool m_GeneratePointOnStart;
    [SerializeField] private int m_NumOfPoints = 50;
    [SerializeField] private float range = 0.2f;
    private void Start()
    {
        if (m_GeneratePointOnStart)
        {
            InitializePoints(m_NumOfPoints, Camera.main.transform.position + Camera.main.transform.forward * 0.5f);
        }
        //StartCoroutine(SaveDataAfterDelay());
    }

    // create point cloud at random positions around start positionn
    public void InitializePoints(int numberOfPoints, Vector3 startPosition)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            Debug.Log(Random.insideUnitSphere);
            GameObject point = Instantiate(m_PointPrefab, Random.insideUnitSphere * range + startPosition, Quaternion.identity);
            m_PointCloud.Add(point);
        }
    }
    
    // generate a point on a circular plane
    public void CreateNewPointOnPlate(Vector3 centerPosition, Vector3 upDirection, float radius, float countDownTime)
    {
        Vector3 randomPosition = radius * Random.insideUnitSphere;
        randomPosition = Vector3.ProjectOnPlane(randomPosition, upDirection);
        randomPosition += centerPosition;
        GameObject point = Instantiate(m_PointPrefab, centerPosition, Quaternion.identity);
        PointController ptController = point.GetComponent<PointController>();
        ptController.m_DataStructure = this;
        ptController.m_CountDownTime = countDownTime;
        ptController.m_TargetPosition = randomPosition;
        m_PointCloud.Add(point);
    }

    private void Update()
    {
        ConnectActiveDots();
    }

    // connects all active dots and store them in the lineConnections
    private void ConnectActiveDots()
    {
        // Disconnect all active lines
        foreach (GameObject line in m_LineConnections)
        {
            Destroy(line);
        }
        m_LineConnections.Clear();
        // connect again
        int min = Mathf.Max(0, m_PointCloud.Count - m_NumOfPoints);
        //Debug.Log(min);
        for (int i = min; i < m_PointCloud.Count; i++)
        {
            GameObject point = m_PointCloud[i];
            List<GameObject> closePoints = GetClosePoints(point, m_PointCloud);
            //Debug.Log("ClosePoints");
            //Debug.Log(closePoints.Count);
            if (closePoints.Count > 0)
            {
                // connect
                foreach (GameObject otherPt in closePoints)
                {
                    GameObject line = Instantiate(m_LinePrefab);
                    LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, point.transform.position);
                    lineRenderer.SetPosition(1, otherPt.transform.position);
                    
                    m_LineConnections.Add(line);
                }
            }
        }
        /*
        foreach (GameObject point in m_PointCloud)
        {
            List<GameObject> closePoints = GetClosePoints(point, m_PointCloud);
            //Debug.Log("ClosePoints");
            //Debug.Log(closePoints.Count);
            if (closePoints.Count > 0)
            {
                // connect
                foreach (GameObject otherPt in closePoints)
                {
                    GameObject line = Instantiate(m_LinePrefab);
                    LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, point.transform.position);
                    lineRenderer.SetPosition(1, otherPt.transform.position);
                    
                    m_LineConnections.Add(line);
                }
            }
        }
        */
    }

    private List<GameObject> GetClosePoints(GameObject targetPoint, List<GameObject> allPoints)
    {
        List<GameObject> closePoints = new List<GameObject>();
        IOrderedEnumerable<GameObject> orderedPoints = allPoints.OrderBy(pt => (Vector3.Distance(targetPoint.transform.position, pt.transform.position)));
        foreach (GameObject point in orderedPoints)
        {
            float distance = Vector3.Distance(targetPoint.transform.position, point.transform.position);
            if (distance < m_MaxDistance)
            {
                // keep if in range
                if (closePoints.Count < m_NumConnection) closePoints.Add(point);
                else return closePoints;
            }
        }

        return closePoints;
    }

    public void SavePointsToCSV()
    {
        WriteCSV();
    }

    public void ErasePoints()
    {
        foreach (GameObject point in m_PointCloud)
        {
            Destroy(point);
        }

        foreach (GameObject line in m_LineConnections)
        {
            Destroy(line);
        }

        m_LineConnections.Clear();
        m_PointCloud.Clear();
    }
    
    IEnumerator SaveDataAfterDelay()
    {
        yield return new WaitForSeconds(15f);
        WriteCSV();
    }

    private void WriteCSV()
    {
        TextWriter textWriter;
        if (m_OculusLocal)
        {
            textWriter = new StreamWriter(Application.persistentDataPath + $"PointCloud.csv", false);
        }
        else
        {
            textWriter = new StreamWriter("D:/PointCloud.csv", false);

        }
        //Debug.Log("Saved");
        //string outputDirectory = "D:/" + m_Filename;
        foreach (GameObject point in m_PointCloud)
        {
            Vector3 position = point.transform.position;
            textWriter.WriteLine(position.x + "," + position.y + "," + position.z);
        }
        textWriter.Close();
    }
    
    public void WriteIDsToCSV(int largeID, int smallID, int nestingID)
    {
        TextWriter textWriter;
        if (m_OculusLocal)
        {
            textWriter = new StreamWriter(Application.persistentDataPath + $"EggloID.csv", false);
        }
        else
        {
            textWriter = new StreamWriter("D:/EggloID.csv", false);

        }
        
        textWriter.WriteLine(largeID);
        textWriter.WriteLine(smallID);
        textWriter.WriteLine(nestingID);
        textWriter.Close();
    }
}

using UnityEngine;
public class GeoMenuController : MonoBehaviour
{
    [SerializeField] private GameObject m_TargetMenuLarge;
    [SerializeField] private GameObject m_TargetMenuSmall;
    [SerializeField] private GameObject m_NestingMenu;
    [SerializeField] private DataStructure m_DataStructure;

    private bool m_LargeMenuActive;
    private bool m_SmallMenuActive;
    private bool m_NestingMenuActive;

    private int m_LargeID = 0;
    private int m_SmallID = 4;
    private int m_NestingID = 0;

    [SerializeField] private Material m_ActiveMat;
    [SerializeField] private Material m_DefaultMat;
    private void Start()
    {
        m_TargetMenuLarge.SetActive(false);
        m_TargetMenuSmall.SetActive(false);
        m_NestingMenu.SetActive(false);
    }

    private void Update()
    {
        // small menu
        if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch) && !m_TargetMenuSmall.activeSelf)
        {
            SpawnMenuAtHand(m_TargetMenuSmall);
            Debug.Log("1Activate");
            //m_SmallMenuActive = true;
        }
        else if (!OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            HideMenu(m_TargetMenuSmall);
            //Debug.Log("1Deactivate");
            //m_SmallMenuActive = false;
        }
        
        // large menu
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch) && !m_TargetMenuLarge.activeSelf)
        {
            SpawnMenuAtHand(m_TargetMenuLarge);
            Debug.Log("2Activate");
            //m_LargeMenuActive = true;
        }
        else if (!OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            HideMenu(m_TargetMenuLarge);
            //m_LargeMenuActive = false;
            //Debug.Log("2Deactivate");

        }
        
        // nesting menu
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch) && !m_NestingMenu.activeSelf)
        {
            SpawnMenuAtHand(m_NestingMenu);
            Debug.Log("NestingActivate");
        }
        else if (!OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            HideMenu(m_NestingMenu);
        }
    }

    private void SpawnMenuAtHand(GameObject menu)
    {
        menu.SetActive(true);
        menu.transform.position = transform.position;
        menu.transform.rotation = transform.rotation;
        GameObject[] icons = GameObject.FindGameObjectsWithTag("GeoMenu");

        foreach (GameObject icon in icons)
        {
            icon.GetComponent<MeshRenderer>().material = m_DefaultMat;
        }
    }

    private void HideMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("GeoMenu"))
        {
            if (m_TargetMenuLarge.activeSelf)
            {
                m_LargeID = other.gameObject.GetComponent<GeoID>().m_GeoID;
                other.gameObject.GetComponent<MeshRenderer>().material = m_ActiveMat;
            }
            if (m_TargetMenuSmall.activeSelf)
            {
                m_SmallID = other.gameObject.GetComponent<GeoID>().m_GeoID;
                other.gameObject.GetComponent<MeshRenderer>().material = m_ActiveMat;
            }
            if (m_NestingMenu.activeSelf)
            {
                m_NestingID = other.gameObject.GetComponent<GeoID>().m_GeoID;
                other.gameObject.GetComponent<MeshRenderer>().material = m_ActiveMat;
            }
            m_DataStructure.WriteIDsToCSV(m_LargeID, m_SmallID, m_NestingID);
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("GeoMenu"))
        {
            other.gameObject.GetComponent<MeshRenderer>().material = m_DefaultMat;
        }
    }

    
}

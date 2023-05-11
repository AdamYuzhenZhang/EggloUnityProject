using UnityEngine;

public class SystemMenuController : MonoBehaviour
{
    [SerializeField] private GameObject m_SystemMenu;
    private bool m_SystemMenuActive;
    [SerializeField] private Material m_ActiveMat;
    [SerializeField] private Material m_DefaultMat;

    [SerializeField] private ImportOBJ m_ImportOBJ;
    [SerializeField] private DataStructure m_DataStructure;
    
    private int m_ActionID;
    private bool m_ActionSelected;
    private void Start()
    {
        m_SystemMenu.SetActive(false);
    }
    
    private void Update()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch) && !m_SystemMenu.activeSelf)
        {
            SpawnMenuAtHand(m_SystemMenu);
        }
        else if (!OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            HideMenu(m_SystemMenu);
            if (m_ActionSelected)
            {
                TakeAction(m_ActionID);
            }
        }
    }

    private void TakeAction(int id)
    {
        switch (id)
        {
            case 0:
                // do nothing
                break;
            case 1:
                // import obj
                m_ImportOBJ.ImportOBJModel();
                break;
            case 2:
                // clear obj
                m_ImportOBJ.ClearOBJModel();
                break;
            case 3:
                // clear points
                m_DataStructure.ErasePoints();
                break;
            case 4:
                // restart app
                m_ImportOBJ.RestartEverything();
                break;
        }

        m_ActionSelected = false;
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
            if (m_SystemMenu.activeSelf)
            {
                m_ActionID = other.gameObject.GetComponent<GeoID>().m_GeoID;
                other.gameObject.GetComponent<MeshRenderer>().material = m_ActiveMat;
                m_ActionSelected = true;
            }
        }
    }
    
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("GeoMenu"))
        {
            other.gameObject.GetComponent<MeshRenderer>().material = m_DefaultMat;
            m_ActionID = 0;
        }
    }
}

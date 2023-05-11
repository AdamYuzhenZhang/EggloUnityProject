using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dummiesman;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text;
using Oculus.Interaction.Input;

public class ImportOBJ : MonoBehaviour
{
    //[SerializeField] private GameObject justLoadedObject;
    [SerializeField] private GameObject theJustLoadedObject;
    [SerializeField] private List<GameObject> all3DModels;
    [SerializeField] private TextMesh m_DebugMessage;
    [SerializeField] private Material m_Material;
    [SerializeField] private Vector3 m_ObjScale = Vector3.one;
    [SerializeField] private Vector3 m_ObjRotation = Vector3.zero;
    [SerializeField] private GameObject m_Attractor;
    [SerializeField] private GameObject m_Attractor2;

    //[SerializeField] private FingerTracker m_FingerTracker;
    private bool m_PositionObj;

    public void RestartEverything()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ImportOBJModel()
    {
        //ClearOBJModel();
        //make www
        var www = new WWW("https://people.sc.fsu.edu/~jburkardt/data/obj/lamp.obj");
        while (!www.isDone)
            System.Threading.Thread.Sleep(1);
        m_DebugMessage.text = "Make WWW";
        //create stream and load
        var textStream = new MemoryStream(Encoding.UTF8.GetBytes(www.text));
        m_DebugMessage.text = "TextStream";
        
        string objPath = Application.persistentDataPath + $"Obj.obj";
        //string objPath = "D:/filesObj.obj";

        //loadedObject = new OBJLoader().Load(textStream);
        GameObject justLoadedObject = new OBJLoader().Load(objPath);
        Debug.Log("Loaded");
        justLoadedObject.transform.localScale = m_ObjScale;
        Debug.Log("Scale Changed");
        justLoadedObject.transform.rotation = Quaternion.Euler(m_ObjRotation.x, m_ObjRotation.y, m_ObjRotation.z);
        Debug.Log("Rotation Changed");
        //justLoadedObject.GetComponent<MeshRenderer>().material = m_Material;
        m_DebugMessage.text = objPath;
        Debug.Log("Message Changed");
        SetLocationOfLastOBJ(justLoadedObject);
        all3DModels.Add(justLoadedObject);
        Debug.Log("Obj Added");
        //new MTLLoader();
        //m_DebugMessage.text = (loadedObject == null).ToString();
        //loadedObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Specular"));


        //m_DebugMessage.text = loadedObject.ToString();
        //var loadedMtl = new MTLLoader().Load(objPath);
        
        // move the object for alignment
        
    }

    public void ClearOBJModel()
    {
        Destroy(theJustLoadedObject);
        m_DebugMessage.text = "OBJ Model Cleared";
    }

    public void SetLocationOfLastOBJ(GameObject justLoadedObject)
    {
        justLoadedObject.transform.position = m_Attractor.transform.position;
        justLoadedObject.transform.SetParent(m_Attractor.transform);
        Debug.Log("Parent Changed");
        m_PositionObj = true;
        m_Attractor.GetComponent<AttractorController>().m_StopDraw = true;
        m_Attractor2.GetComponent<AttractorController>().m_StopDraw = true;
        theJustLoadedObject = justLoadedObject;
    }

    private void Update()
    {
        if (m_PositionObj)
        {
            //m_FingerTracker.CheckPinching = true;
            //if (m_FingerTracker.PinchedForOneSecond)
            
            // release if trigger is pressed
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                // release
                Debug.Log("Release");
                theJustLoadedObject.transform.SetParent(null); 
                //m_FingerTracker.CheckPinching = false;
                m_PositionObj = false;
                StartCoroutine(delayAndDraw());
            }
        }

        /*

        // call functions
        // import obj
        if (OVRInput.Get(OVRInput.Button.Two, OVRInput.Controller.LTouch))
        {
            Debug.Log("OBJ Imported");
            ImportOBJModel();
        }

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch))
        {
            Debug.Log("OBJ Cleared");
            ClearOBJModel();
        }
        */
    }

    IEnumerator delayAndDraw()
    {
        yield return new WaitForSeconds(1);
        m_Attractor.GetComponent<AttractorController>().m_StopDraw = false;
        m_Attractor2.GetComponent<AttractorController>().m_StopDraw = false;
    }
}

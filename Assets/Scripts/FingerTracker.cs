using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FingerTracker : MonoBehaviour
{
    [SerializeField] private GameObject handToTrack;
    private OVRSkeleton ovrSkeleton;
    private OVRBone indexBone;
    private OVRBone thumbBone;

    private AttractorController m_Attractor;
    public bool PinchedForOneSecond;
    public bool CheckPinching;
    private float pinchStart;
    private float pinchDuration;
    private bool isPinching;
    [SerializeField] private float m_PinchThreshold = 0.01f;
    [SerializeField] private GameObject m_PinchIndicator;
    [SerializeField] private Material m_PinchMaterial;
    
    void Start()
    {
        m_Attractor = GetComponent<AttractorController>();
        StartCoroutine(SetUpBone());
        m_PinchIndicator.SetActive(false);
    }

    IEnumerator SetUpBone()
    {
        ovrSkeleton = handToTrack.GetComponent<OVRSkeleton>();
        while (ovrSkeleton.Bones.Count == 0)
        {
            yield return null;
        }

        indexBone = ovrSkeleton.Bones.Where(b => b.Id == OVRSkeleton.BoneId.Hand_IndexTip).SingleOrDefault();
        Debug.Log("IndexBone");
        Debug.Log(indexBone);
        
        thumbBone = ovrSkeleton.Bones.Where(b => b.Id == OVRSkeleton.BoneId.Hand_ThumbTip).SingleOrDefault();
        Debug.Log("ThumbBone");
        Debug.Log(thumbBone);
    }

    private void Update()
    {
        if (indexBone != null && thumbBone != null)
        {
            transform.position = (indexBone.Transform.position + thumbBone.Transform.position) / 2;
            m_Attractor.SetRadius(Vector3.Distance(indexBone.Transform.position,thumbBone.Transform.position));
            
            // check pinching status
            if (CheckPinching && Vector3.Distance(indexBone.Transform.position, thumbBone.Transform.position) < m_PinchThreshold)
            {
                if (!isPinching)
                {
                    // start pinching
                    isPinching = true;
                    pinchStart = Time.time;
                    pinchDuration = 0f;
                    PinchedForOneSecond = false;
                    m_PinchIndicator.SetActive(true);
                    m_PinchIndicator.transform.localScale = Vector3.one;
                }
                else
                {
                    // continue pinching
                    pinchDuration = Time.time - pinchStart;
                    pinchDuration = Mathf.Clamp(pinchDuration, 0f, 1f);
                    m_PinchIndicator.transform.localScale = Vector3.one * (1 - pinchDuration);
                    m_PinchMaterial.color = new Color(1f, 1-pinchDuration, 1-pinchDuration, pinchDuration);
                    if (pinchDuration >= 1f)
                    {
                        PinchedForOneSecond = true;
                        m_PinchIndicator.SetActive(false);
                    }
                }
            }
            else
            {
                isPinching = false;
                PinchedForOneSecond = false;
                m_PinchIndicator.SetActive(false);
            }
        }
    }
}

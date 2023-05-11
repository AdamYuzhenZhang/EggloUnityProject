using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IndexToolDriver : MonoBehaviour
{
    [SerializeField] private GameObject handToTrack;
    private OVRSkeleton ovrSkeleton;
    private OVRBone indexBone;
    
    void Start()
    {
        StartCoroutine(SetUpBone());
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
    }
    
    private void Update()
    {
        if (indexBone != null)
        {
            transform.position = indexBone.Transform.position;
        }
    }
}

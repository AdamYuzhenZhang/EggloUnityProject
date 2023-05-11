using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AirtableApiClient;

public class AirTable : MonoBehaviour
{
    [SerializeField] private string baseId;
    [SerializeField] private string appKey;
    
    
    string offset = null;
    string errorMessage = null;
    //List<AirtableRecord> records = new List<AirtableRecord>();

    //private AirtableBase airtableBase;

    private void Start()
    {
        //airtableBase = new AirtableBase(appKey, baseId);
    }
}

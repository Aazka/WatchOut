using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Sphere"))
        {
            other.gameObject.transform.parent.gameObject.GetComponent<CollectableObjectController>().Drop();
        }
    }
}

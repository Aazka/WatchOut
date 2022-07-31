using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectController : MonoBehaviour
{
    PlayerManager playerManager;
    GameObject sphere;
    Rigidbody rb;
    private void Start()
    {
        playerManager = PlayerManager.instance;
        sphere = this.gameObject.transform.GetChild(0).gameObject;
        if(this.gameObject.GetComponent<Rigidbody>()==null)
        {
            this.gameObject.AddComponent<Rigidbody>();
            rb = this.gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }
           
        this.gameObject.GetComponent<Renderer>().material= playerManager.collisionMaterial;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("collectableOBJ"))
        {
            if(!playerManager.collisionGO.Contains(collision.gameObject))
            {
                collision.gameObject.tag = "CollectedObj";
                collision.transform.parent = playerManager.collectedPoolTransform.transform;
                playerManager.collisionGO.Add(collision.gameObject);
                collision.gameObject.AddComponent<CollectableObjectController>();
            }
        }
        if(collision.gameObject.CompareTag("Obstricle"))
        {
            playerManager.collisionGO.Remove(this.gameObject);
            this.gameObject.SetActive(false);
            var v = Instantiate(playerManager.particles, this.transform);
            v.transform.parent = null;
            Destroy(v, 2f);
            Destroy(this.gameObject,3);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Finish"))
        {
            if(!playerManager.isFinished)
            {
                playerManager.isFinished = true;
                playerManager.CallSphere();
            }
        }
    }
    public void SphereOn()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        sphere.gameObject.GetComponent<Renderer>().material = playerManager.collisionMaterial;
    }
    public void Drop()
    {
        sphere.layer = 6;
        sphere.GetComponent<SphereCollider>().isTrigger = false;
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().useGravity = true;
    }
}

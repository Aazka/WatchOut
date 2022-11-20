using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayer : MonoBehaviour
{
    Material playerMaterial;
    PlayerManager playerManager;
    Rigidbody rb;
    bool isGrounded;
    private void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        playerManager = PlayerManager.instance;
        playerMaterial = this.GetComponent<Renderer>().material;
        playerMaterial = playerManager.collisionMaterial[PlayerPrefs.GetInt("Skin", 0)];
        playerManager.collisionGO.Add(this.gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            Ground();
        }
    }
    void Ground()
    {
        isGrounded = true;
        playerManager.playerState = PlayerManager.PlayerState.Move;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(this, 1);
    }
}

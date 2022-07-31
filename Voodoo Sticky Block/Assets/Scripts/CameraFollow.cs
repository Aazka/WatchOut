using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed;
    [SerializeField] Vector3 offset;
    public Transform target;
    PlayerManager playerManager;
    Vector3 desiredPos,smoothPos;
    private void Start()
    {
        playerManager = PlayerManager.instance;
    }
    private void LateUpdate()
    {
        if(!playerManager.isFinished)
        {
            desiredPos = target.position + offset;
            smoothPos = Vector3.Lerp(this.transform.position, desiredPos, smoothSpeed);
            transform.position = new Vector3(transform.position.x, transform.position.y, smoothPos.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerManager playerManager;
    [SerializeField] float moveSpeed;
    [SerializeField] float controlSpeed;
    [SerializeField] bool isTouch = false;
    [SerializeField] float minX, maxX;
    float touchPos;
    Vector3 direction;
  
    private void Start()
    {
        playerManager = PlayerManager.instance;
    }
    private void Update()
    {
        GetInput();
    }
    private void FixedUpdate()
    {
        if(playerManager.playerState==PlayerManager.PlayerState.Move)
        {
            this.transform.position += Vector3.forward * moveSpeed * Time.fixedDeltaTime;
        }
        if(isTouch)
        {
            touchPos += Input.GetAxis("Mouse X") * controlSpeed * Time.fixedDeltaTime;
            touchPos = Mathf.Clamp(touchPos,minX,maxX);
        }
        this.transform.position = new Vector3(touchPos, this.transform.position.y, this.transform.position.z);
    }
    void GetInput()
    {
        if(Input.GetMouseButton(0))
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }
    }
}

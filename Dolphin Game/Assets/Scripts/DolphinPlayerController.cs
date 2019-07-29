using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinPlayerController : SmallWhaleControllerBase
{
	public GameObject bubblePrefab;
	int bubbleHash = Animator.StringToHash("Bubble");
    private bool bubbleSheduled = false;
    public float bubbleDelay = 1f;
    private float timeLeftUntilBubble = 0;
  
  	public float bubbleThrustPower = 1f;

  
  
    void Start()
    {
        base.Init();
    }

    protected override void Update()
    {
        if (playerController.CurrentPlayerController != this)
            return;

		 bool bButtonPressed = Input.GetButtonUp("B Button");

        if (bButtonPressed)
        {
            animator.SetTrigger(bubbleHash);
            SheduleBubble(); 

        }
		
		if (bubbleSheduled)
        {
            timeLeftUntilBubble -= Time.deltaTime;

            if (timeLeftUntilBubble < 0)
            {
ShotBubble();
	
                bubbleSheduled = false;
            }
        }
		
        base.Update();
    }

    private void SheduleBubble()
    {
        bubbleSheduled = true;
        timeLeftUntilBubble = bubbleDelay;
    }
	
	    private void ShotBubble()
    {
        var bubble = Instantiate(bubblePrefab);
		bubble.transform.position = EccoOrigin.transform.position;
				var bubbleRigidBody = bubble.GetComponent<Rigidbody>();
				bubbleRigidBody.velocity = transform.forward * bubbleThrustPower;
    }
					
}

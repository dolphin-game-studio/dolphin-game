using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Angler : MonoBehaviour
{
    [SerializeField] private GameObject sharkToFollow;

    [SerializeField] private float roationSpeed;

    [SerializeField] private float moveSpeed;

    [SerializeField] private Vector3 offsetToShark;


    void Start()
    {
        SharkToFollow = sharkToFollow;
    }
    bool followsShark;
    bool followsConsciousHai;
    bool followsSharkPlayer;
    bool followedHaiGotUnconscious;
    bool followsHai;

    public GameObject SharkToFollow
    {
        get { return sharkToFollow; }
        set
        {
            Debug.Log("SharkToFollow");

            if (sharkToFollow == null)
            {
                Debug.Log("1");

                sharkToFollow = value;
            }
            else if (value == null)
            {
                Debug.Log("2");
                sharkToFollow = null;
            }

            followsShark = sharkToFollow != null;

            var hai = followsShark ? sharkToFollow.GetComponent<Hai>() : null;
            followsHai = followsShark && sharkToFollow.GetComponent<Hai>() != null;

            followsConsciousHai = followsHai && hai.Conscious;
            followedHaiGotUnconscious = followsHai && !hai.Conscious;

            followsSharkPlayer = followsShark && sharkToFollow.GetComponent<SharkPlayerController>() != null;
        }
    }

    void Update()
    {               
        if (followsConsciousHai || followsSharkPlayer)
        {
            RotateToDesiredRotation();
            MoveToDesiredPosition();
        }

        if (followsHai &&  followedHaiGotUnconscious) {
            SharkToFollow = null;
        }
    }

    private void RotateToDesiredRotation()
    {
        var desiredRotation = sharkToFollow.transform.rotation;

        if (transform.rotation != desiredRotation)
        {
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, roationSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }

    private void MoveToDesiredPosition()
    {
        var desiredPosition = sharkToFollow.transform.position + offsetToShark;

        if (transform.position != desiredPosition)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            transform.position = pos;
        }

    }
}

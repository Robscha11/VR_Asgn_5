using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class GoGoScript : MonoBehaviour
{
    private GameObject scene = null;
    private GameObject rightHandController;
    private XRController rightXRController;

    public float threshhold = 0.35f;
    private GameObject head;
    private GameObject leftHand;
    private GameObject rightHand;
    private CollisionDetector leftDetector;
    private CollisionDetector rightDetector;
    private GameObject leftHandCenter;
    private GameObject rightHandCenter;
    private GameObject rightHandColliderProxy;

    private GameObject selectedObject;

    private bool gripButtonLF = false;


    // Start is called before the first frame update
    void Awake()
    {
        scene = GameObject.Find("Scene");
        rightHandController = GameObject.Find("RightHand Controller");
        rightXRController = rightHandController.GetComponent<XRController>();

        head = transform.Find("Camera Offset/Main Camera").gameObject;
        leftHand = transform.Find("Camera Offset/LeftHand Controller/HandLeft").gameObject;
        rightHand = transform.Find("Camera Offset/RightHand Controller/HandRight").gameObject;
        leftHandCenter = transform.Find("Camera Offset/LeftHand Controller/LeftCenter").gameObject;
        rightHandCenter = transform.Find("Camera Offset/RightHand Controller/RightCenter").gameObject;

        rightHandColliderProxy = GameObject.Find("HandColliderProxy");
        rightDetector = rightHandColliderProxy.GetComponent<CollisionDetector>();
    }

    // Update is called once per frame
    void Update()
    {
        // YOUR CODE - BEGIN
        // GoGo behavior
        Vector3 relativPos = new Vector3(head.transform.position.x - rightHandCenter.transform.position.x, 0, head.transform.position.z - rightHandCenter.transform.position.z);
        var distMCtoRHC = relativPos.magnitude;
        Debug.Log(distMCtoRHC);

        rightHand.transform.rotation = rightHandController.transform.rotation;

        if (distMCtoRHC < threshhold)
        {
            rightHand.transform.position = rightHandController.transform.position;
            rightHandColliderProxy.transform.position = rightHand.transform.position;
        } else
        {
            float k = 10f;
            var d = k * Mathf.Pow((distMCtoRHC - threshhold),2.0f);
            rightHand.transform.position = rightHandController.transform.position + rightHandController.transform.TransformDirection(Vector3.forward) * d;// + new Vector3(0, 0, d);
            rightHandColliderProxy.transform.position = rightHand.transform.position;
        }
        
        // YOUR CODE - END   

        // mapping: grip button (middle finger)
        bool gripButton = false;
        rightXRController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out gripButton);
        //Debug.Log("middle finger rocker: " + gripButton);

        if (gripButton != gripButtonLF) // state changed
        {
            if (gripButton) // up (false->true)
            {
                if (rightDetector.collided && selectedObject == null)
                {
                    Debug.Log("Collided with Object: " + rightDetector.collidedObject.name);
                    // YOUR CODE - BEGIN
                    SelectObject(rightDetector.collidedObject);
                    // YOUR CODE - END   
                }
            }
            else // down (true->false)
            {
                if (selectedObject != null)
                {
                    DeselectObject();
                }
            }
        }
        gripButtonLF = gripButton;

    }

    private void SelectObject(GameObject go)
    {
        // YOUR CODE - BEGIN
        selectedObject = go;
        selectedObject.transform.SetParent(rightHand.transform, true); // worldPositionStays = true
        // YOUR CODE - END  
    }

    private void DeselectObject()
    {
        // YOUR CODE - BEGIN
        selectedObject.transform.SetParent(scene.transform, true); // worldPositionStays = true
        selectedObject = null;
        // YOUR CODE - END  
    }

    private void OnDisable()
    {
        rightHand.transform.position = rightHandCenter.transform.position;
        leftHand.transform.position = leftHandCenter.transform.position;
        rightHandColliderProxy.GetComponent<BoxCollider>().enabled = false;
        rightHandColliderProxy.SetActive(false);
    }

    private void OnEnable()
    {
        //rightHandCenter.transform.position = rightHandCenter.transform.parent.position;
        rightHand.transform.position = rightHandCenter.transform.position;
        leftHand.transform.position = leftHandCenter.transform.position;
        rightHandColliderProxy.GetComponent<BoxCollider>().enabled = true;
        rightHandColliderProxy.SetActive(true);
    }
}

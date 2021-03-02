using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Argelaguet : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject rightHandController;

    private GameObject selectionRayGO;
    private LineRenderer selectionRayLR;

    private GameObject feedbackRayGO;
    private LineRenderer feedbackRayLR;

    private GameObject rightRayIntersectionSphere;

    private RaycastHit rightHit;
    public LayerMask myLayerMask;


    private void Awake()
    {
        mainCamera = GameObject.Find("Main Camera");
        rightHandController = GameObject.Find("RightHand Controller");

        if (selectionRayGO == null)
        {
            selectionRayGO = new GameObject();
            selectionRayGO.name = "Selection Ray";
            selectionRayGO.transform.SetParent(rightHandController.transform, false);

            selectionRayLR = selectionRayGO.AddComponent<LineRenderer>() as LineRenderer;
            selectionRayLR.startWidth = 0.01f;
            selectionRayLR.positionCount = 2; // two points (one line segment)
            selectionRayLR.enabled = false;
            selectionRayLR.material.color = Color.white;
        }

        if (feedbackRayGO == null)
        {
            feedbackRayGO = new GameObject();
            feedbackRayGO.name = "Feedback Ray";
            feedbackRayGO.transform.SetParent(rightHandController.transform, false);

            feedbackRayLR = feedbackRayGO.AddComponent<LineRenderer>() as LineRenderer;
            feedbackRayLR.startWidth = 0.01f;
            feedbackRayLR.positionCount = 2; // two points (one line segment)
            feedbackRayLR.enabled = false;
        }

        if (rightRayIntersectionSphere == null)
        {
            // geometry for intersection visualization
            rightRayIntersectionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //rightRayIntersectionSphere.transform.parent = this.gameObject.transform;
            rightRayIntersectionSphere.name = "Right Ray Intersection Sphere";
            rightRayIntersectionSphere.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            rightRayIntersectionSphere.GetComponent<MeshRenderer>().material.color = Color.yellow;
            rightRayIntersectionSphere.GetComponent<SphereCollider>().enabled = false; // disable for picking ?!
            rightRayIntersectionSphere.SetActive(false); // hide
        }
    }


    // Update is called once per frame
    void Update()
    {
        // ----------------- Argelaguet selection technique -----------------

        // YOUR CODE - BEGIN
        if (Physics.Raycast(mainCamera.transform.position, rightHandController.transform.TransformDirection(Vector3.forward), out rightHit, Mathf.Infinity, myLayerMask))
        {
            OnEnable();
            Debug.Log("Did Hit");
            // update ray visualization
            selectionRayLR.SetPosition(0, mainCamera.transform.position);
            selectionRayLR.SetPosition(1, rightHit.point);

            feedbackRayLR.SetPosition(0, rightHandController.transform.position);
            feedbackRayLR.SetPosition(1, rightHit.point);

            // update intersection sphere visualization
            rightRayIntersectionSphere.SetActive(true); // show
            rightRayIntersectionSphere.transform.position = rightHit.point;
        }
        else // ray does not intersect with objects
        {
            OnEnable();
            // update ray visualization
            selectionRayLR.SetPosition(0, mainCamera.transform.position);
            selectionRayLR.SetPosition(1, mainCamera.transform.position + rightHandController.transform.TransformDirection(Vector3.forward) * 1000);

            feedbackRayLR.SetPosition(0, rightHandController.transform.position);
            feedbackRayLR.SetPosition(1, rightHandController.transform.position + rightHandController.transform.TransformDirection(Vector3.forward) * 1000);


            // update intersection sphere visualization
            rightRayIntersectionSphere.SetActive(false); // hide
        }

        // visualize feedback ray
        // YOUR CODE - END
    }

    private void OnDisable()
    {
        if (selectionRayLR != null) selectionRayLR.enabled = false;
        if (feedbackRayLR != null) feedbackRayLR.enabled = false;
        if (rightRayIntersectionSphere != null) rightRayIntersectionSphere.SetActive(false);
    }

    private void OnEnable()
    {
        if (selectionRayLR != null) selectionRayLR.enabled = true;
        if (feedbackRayLR != null) feedbackRayLR.enabled = true;
    }
}

/*comments about Argelaguet selection technique
 * it's useful to point at objects that are behind the feedback ray of the hand
 * it needs some time to get used to it how objects are selected
 * the objects that you can see fully can be selected in every position and hence the advantage is that what you see you can select and you are not constrained to your hands view.
 * no need to move your head to select different objects in the scene is also a nice feature that helps preventing neck tension
 * with the visible feedback ray it's easier to control the selection ray in the scene
 * 
 * drawbacks:
 * for us it's more intuitiv and faster to select with the hand pointer and not thinking about the distance between hand and controller to select sth.
 * it's 
 */ 

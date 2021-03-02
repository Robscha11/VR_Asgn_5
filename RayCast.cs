using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public LineRenderer rayRenderer;

    // changeable Ray Parameters
    public float rayWidth = 0.01f;
    public float rayVelocity = 700;
    public int segmentCount = 70;
    public float segmentScale = 0.3f;
    public Material rayMaterial;

    private Color validRayColor = Color.blue;
    private Color invalidRayColor = Color.red;


    //store information about hit & give access via properties
    private Collider _hitObject;
    public Collider hitObject { get { return _hitObject; } }
    // HitVector
    private Vector3 _hitVector;
    public Vector3 hitVector { get { return _hitVector; } }
    // Normal of the Hit
    private Vector3 _hitNormal;
    public Vector3 hitNormal {get{return _hitNormal;}}
    public bool useNormals = true;

    //Activate the ray
    public bool ray_active = false;
    public bool ray_update = true;
    public LayerMask myLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        addLineRenderer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   if (ray_update)
        
            if (ray_active) { simulateRayPath(); }
            else { rayRenderer.positionCount = 0; }
        

    }

    void simulateRayPath()
    {
        Vector3[] segments = new Vector3[segmentCount];

        //start of the jumping ray at the position of the object this script is attached to
        segments[0] = transform.position;

        // initial velocity
        Vector3 segVelocity = transform.forward * rayVelocity * Time.deltaTime;

        //reset hitobject
        _hitObject = null;

        // calculate Raycast
        for (int i = 1; i < segmentCount; i++)
        {

            if (_hitObject != null)
            {
                segments[i] = _hitVector;
                continue;
            }
            // Time to traverse one segment of segScale; scale/length if length not 0; 0 else
            float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0;

            //add velocity for current segments timestep
            segVelocity = segVelocity + Physics.gravity * segTime;

            //Check for hit with a physics object

            RaycastHit hit;
            if (Physics.Raycast(segments[i - 1], segVelocity, out hit, segmentScale, myLayerMask))
            {

               
                //remember hitobject
                _hitObject = hit.collider;

                if(useNormals) _hitNormal = GetMeshColliderNormal(hit);

                {
                    // set next position to position where object hit occured
                    segments[i] = segments[i - 1] + segVelocity.normalized * hit.distance;
                    //correct ending velocity for interrupted path
                    segVelocity = segVelocity - Physics.gravity * (segmentScale - hit.distance) / segVelocity.magnitude;
                    //save Postion of Collision
                    _hitVector = segments[i];
                    
                }
            }
            else
            {
                segments[i] = segments[i - 1] + segVelocity * segTime;
            }
           
        }
        rayRenderer.positionCount = segmentCount;
        for (int i = 0; i < segmentCount; i++) rayRenderer.SetPosition(i, segments[i]);

    }

    public void setRayMaterial(Material material)
    {
        rayRenderer.material = material;

    }

    public void setRayColor(Color color)
    {
        rayRenderer.material.color = color;
    }

    public void rayValid(bool valid)
    {
        if (valid) setRayColor(validRayColor);
        else setRayColor(invalidRayColor);
    }

    void addLineRenderer()
    {
        rayRenderer = gameObject.AddComponent<LineRenderer>();
        rayRenderer.startWidth = rayWidth;
        rayRenderer.material = rayMaterial;
    }


    private Vector3 GetMeshColliderNormal(RaycastHit hit)
    {
        MeshCollider collider = (MeshCollider)hit.collider;
        Mesh mesh = collider.sharedMesh;
        Vector3[] normals = mesh.normals;
        int[] triangles = mesh.triangles;

        Vector3 n0 = normals[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 n1 = normals[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 n2 = normals[triangles[hit.triangleIndex * 3 + 2]];
        Vector3 baryCenter = hit.barycentricCoordinate;
        Vector3 interpolatedNormal = n0 * baryCenter.x + n1 * baryCenter.y + n2 * baryCenter.z;
        interpolatedNormal.Normalize();
        interpolatedNormal = hit.transform.TransformDirection(interpolatedNormal);
        return interpolatedNormal;


    }

}

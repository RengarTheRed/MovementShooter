using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class AIPerception : MonoBehaviour
{
    //drawing the cone mesh
    public float distance = 10;
    public float angle = 30;
    public float height = 1;
    public Color meshColor = Color.red;

    private Mesh _mesh;

    //Checking colliding objects
    public int scanFrequency = 30;
    private int count;
    private float scanInterval;
    private float scanTimer;
    public LayerMask layersToScan;
    private Collider[] _collidersHit = new Collider[50];
    public List<GameObject> _objectsHit = new List<GameObject>();
    public LayerMask blockingLayers;
    public Transform perceptionOrigin;

    public bool _bSeePlayer = false;

    //Clear Timer
    private float clearTimer = 3;

    private void Start()
    {
        scanInterval = 1 / scanFrequency;
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(perceptionOrigin.position, distance, _collidersHit, layersToScan, QueryTriggerInteraction.Collide);
        _objectsHit.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = _collidersHit[i].gameObject;
            if (IsInSight(obj))
            {
                _objectsHit.Add(obj);
                ReportPlayerSighting(obj);
            }
            else
            {
                StartCoroutine(ClearSight(clearTimer));
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = perceptionOrigin.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest-origin;

        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, perceptionOrigin.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, blockingLayers))
        {
            return false;
        }
        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segments = 10;
        int numTriangles = (segments *4) + 4;
        int numVertices = numTriangles *3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCentre = perceptionOrigin.localPosition;
        //Vector3 bottomLeft = Quaternion.Euler(perceptionOrigin.localPosition.x, perceptionOrigin.localPosition.y - angle, perceptionOrigin.localPosition.z) * Vector3.forward * distance;
        //Vector3 bottomRight = Quaternion.Euler(perceptionOrigin.localPosition.x, perceptionOrigin.localPosition.y + angle, perceptionOrigin.localPosition.z) * Vector3.forward * distance;
        
        //Vector3 bottomCentre = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCentre = bottomCentre + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;

        int vert = 0;
        
        //left
        vertices[vert++] = bottomCentre;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCentre;
        vertices[vert++] = bottomCentre;
        
        //right
        vertices[vert++] = bottomCentre;
        vertices[vert++] = topCentre;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCentre;

        float currentAngle = -angle;
        float deltaAngle = (angle*2)/segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle+deltaAngle, 0) * Vector3.forward * distance;
            
            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;
            
            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            //top
            vertices[vert++] = topCentre;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
        
            //bottom
            vertices[vert++] = bottomCentre;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            
            
            currentAngle += deltaAngle;
        }

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        scanInterval = 1 / scanFrequency;
    }

    //Sets blackboard variables for see player
    private void ReportPlayerSighting(GameObject obj)
    {
        Debug.Log(("Player has been sighted"));
        _bSeePlayer = true;
    }

    //Sets blackboard variables for noise report
    public void HearNoise(Vector3 soundLocation)
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent.CalculatePath(soundLocation, agent.path))
        {
            
        }
        else
        {
            Debug.Log("Can't get path to sound");
        }
    }

    private IEnumerator ClearSight(float duration)
    {
        yield return new WaitForSeconds(duration);
        
    }

    private void OnDrawGizmos()
    {
        if (_mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(_mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(_collidersHit[i].transform.position, .2f);
        }

        Gizmos.color = Color.green;
        if (_objectsHit.Count > 0)
        {
            foreach (var obj in _objectsHit)
            {
                Gizmos.DrawSphere(obj.transform.position, .2f);
            }
        }
    }
}

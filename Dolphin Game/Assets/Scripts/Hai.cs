﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hai : MonoBehaviour
{
    [Range(1, 3)]
    public int rank = 1;

    public GameObject[] armbands;

    AnimationCurve viewPanAnimationCurve;
    private float viewPanAnimationTime;
    public CapsuleCollider Collider;

    [Range(0, 360)]
    public float viewAngle;

    public float timeToRegainConsciousness;

    private float timeSpendStunned = 0;


    public float ViewRadius
    {
        get
        {
            if (visibleTargets.Count == 0)
            {
                return viewRadiusWhenNotSuspicious;
            }
            else
            {
                return viewRadiusWhenSuspicious;
            }
        }
    }

    private bool _stunned = false;

    public bool Stunned
    {
        get { return _stunned; }
        set
        {
            if (_stunned != value)
            {
                _stunned = value;

                if (Stunned)
                {
                    stunnedParticleSystem.Play();
                    viewMesh.Clear();
                }
                else
                {
                    stunnedParticleSystem.Stop();
                }
            }
        }
    }

    private bool _knockedOut = false;

    public bool KnockedOut
    {
        get { return _knockedOut; }
        set
        {
            if (_knockedOut != value)
            {
                _knockedOut = value;

                if (KnockedOut)
                {
                    Stunned = false;

                    knockedParticleSystem.Play();

                    viewMesh.Clear();
                }
            }
        }
    }

    public bool Conscious => !Stunned && !KnockedOut;

    [Range(1, 100)]
    public float viewRadiusWhenNotSuspicious = 30;

    [Range(1, 100)]
    public float viewRadiusWhenSuspicious = 35;



    [Range(-180, 180)]
    public float viewPanAngle1;
    [Range(-180, 180)]
    public float viewPanAngle2;


    public ParticleSystem stunnedParticleSystem;
    public ParticleSystem knockedParticleSystem;



    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public List<GameObject> visibleTargets = new List<GameObject>();

    public bool IsAlarmed => visibleTargets.Count > 0;
    public bool IsNotAlarmed => visibleTargets.Count == 0;

    public int Rank
    {
        get
        {
            return rank;
        }
        set
        {
            if (rank != 0)
                armbands[rank - 1].SetActive(false);

            rank = value;

            if (rank != 0)
                armbands[rank - 1].SetActive(true);
        }
    }

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;

    Quaternion currentViewPanRotation;


    public float panDuration = 1.5f;


    void Start()
    {
        Rank = rank;

        Collider = GetComponent<CapsuleCollider>();
        if (Collider == null)
        {
            Debug.LogError("collider is not set.");
        }

        viewPanAnimationCurve = AnimationCurve.EaseInOut(0, viewPanAngle1, panDuration, viewPanAngle2);

        if (stunnedParticleSystem == null)
        {
            Debug.LogError("unconsciousParticleSystem is not set.");
        }

        if (knockedParticleSystem == null)
        {
            Debug.LogError("knockedParticleSystem is not set.");
        }

        viewMesh = new Mesh
        {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;
    }




    void LateUpdate()
    {
        if (Conscious)
        {
            UpdateViewDirection();

            DrawFieldOfView();

            FindVisibleTargets();
        }



        if (Stunned)
        {
            RegainConsciousness();
        }


    }

    private void RegainConsciousness()
    {
        timeSpendStunned += Time.deltaTime;

        if (timeSpendStunned > timeToRegainConsciousness)
        {
            Stunned = false;
            timeSpendStunned = 0;
        }
    }

    float currentViewPanAngle;

    private void UpdateViewDirection()
    {
        var angles = transform.eulerAngles;
        if (visibleTargets.Count == 0)
        {
            viewPanAnimationTime += Time.deltaTime;

            var wayBack = viewPanAnimationTime % (panDuration * 2) > panDuration;

            var pan = viewPanAnimationTime % (panDuration);

            if (wayBack)
            {
                pan = panDuration - pan;
            }

            currentViewPanAngle = viewPanAnimationCurve.Evaluate(pan);

            currentViewPanRotation = Quaternion.Euler(angles.x + currentViewPanAngle, angles.y > 180 ? -90 : 90, angles.z);

        }
        else
        {
            var visiblePlayer = visibleTargets[0];
            Vector3 dirToTarget = (visiblePlayer.transform.position - transform.position).normalized;
            transform.forward = dirToTarget;

            currentViewPanRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y > 180 ? -90 : 90, transform.eulerAngles.z);

        }



    }

    RayPlayerController spottedRay;

    void FindVisibleTargets()
    {

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);
        visibleTargets.Clear();

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Collider target = targetsInViewRadius[i];
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;

            if (Vector3.Angle(currentViewPanRotation * Vector3.forward, dirToTarget) < viewAngle / 2)
            {

                float dstToTarget = Vector3.Distance(transform.position, target.transform.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                {
                    RayPlayerController ray = target.GetComponent<RayPlayerController>();
                    if (ray != null)
                    {

                        bool isNotNearGround = !Physics.Raycast(transform.position, dirToTarget, dstToTarget * 1.2f, obstacleMask);
                        bool isMoving = ray.Rigidbody.velocity.magnitude > 1;

                        if (spottedRay != null || isNotNearGround || isMoving)
                        {
                            visibleTargets.Add(target.gameObject);
                            spottedRay = ray;
                        }
                    }
                    else
                    {
                        visibleTargets.Add(target.gameObject);
                    }
                }
            }
            else
            {
                spottedRay = null;

            }
        }
    }

    void DrawFieldOfView()
    {

        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();

        var startAngle = viewAngle / 2;

        for (int i = 0; i <= stepCount; i++)
        {
            var lookRotation = Quaternion.Euler(currentViewPanRotation.eulerAngles.x - startAngle + stepAngleSize * i, currentViewPanRotation.eulerAngles.y, currentViewPanRotation.eulerAngles.z);

            ViewCastInfo newViewCast = ViewCast(lookRotation * Vector3.forward);

            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }


    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        Vector3 minAngle = minViewCast.dir;
        Vector3 maxAngle = maxViewCast.dir;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            Vector3 dir = (minAngle + maxAngle).normalized;
            ViewCastInfo newViewCast = ViewCast(dir);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = dir;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = dir;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(Vector3 dir)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, ViewRadius, obstacleMask))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, dir);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * ViewRadius, ViewRadius, dir);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.x;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public Vector3 dir;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, Vector3 _dir)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            dir = _dir;
        }
    }

    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }



}

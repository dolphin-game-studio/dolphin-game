using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

[ExecuteInEditMode]
public class ScannerEffectDemo : MonoBehaviour
{
    public AudioSource[] dolphinSounds;

    ScanDistance[] scanDistances = new ScanDistance[10];
    int currentScanDistancesIndex = 0;


    public Transform ScannerOrigin;
    public Material EffectMaterial;
    public float ScanDistance;

    private Camera _camera;

    // Demo Code
    bool _scanning;
    Scannable[] _scannables= new Scannable[0];

    void Start()
    {
        _scannables = FindObjectsOfType<Scannable>();

        for (int i = 0; i < scanDistances.Length; i++)
        {
            scanDistances[i] = new ScanDistance();
        }
    }

    void Update()
    {
        for (int i = 0; i < scanDistances.Length; i++)
        {
            if (scanDistances[i].Scanning) {

            }
            scanDistances[i].Distance += Time.deltaTime * 50;
        }

        if (_scanning)
        {
            ScanDistance += Time.deltaTime * 50;
            foreach (Scannable s in _scannables)
            {
                if (Vector3.Distance(ScannerOrigin.position, s.transform.position) <= ScanDistance)
                    s.Ping();
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _scanning = true;
            ScanDistance = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //AudioSource dolphinSound = dolphinSounds[Random.Range(0, dolphinSounds.Length)];
            //dolphinSound.Play();
            _scanning = true;
            ScanDistance = 0;

            scanDistances[currentScanDistancesIndex % scanDistances.Length].Scanning = true;
            scanDistances[currentScanDistancesIndex % scanDistances.Length].Distance = 0;
            currentScanDistancesIndex = (currentScanDistancesIndex + 1) % scanDistances.Length;

        }
    }
    // End Demo Code

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;

    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Debug.Log("OnRenderImage");
        EffectMaterial.SetVector("_WorldSpaceScannerPos", ScannerOrigin.position);


        float[] scanDistancesArray = new float[100];
        for (int i = 0; i < scanDistances.Length && i < scanDistancesArray.Length; i++)
        {
            scanDistancesArray[i] = scanDistances[i].Distance;
        }

        EffectMaterial.SetFloatArray("_ScanDistances", scanDistancesArray);
        EffectMaterial.SetFloat("_ScanDistance", ScanDistance);

        RaycastCornerBlit(src, dst, EffectMaterial);
    }



    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }
}

public class ScanDistance
{
    float distance = 0;
    bool scanning = false; 

    public float Distance { get => distance; set => distance = value; }
    public bool Scanning { get => scanning; set => scanning = value; }
}
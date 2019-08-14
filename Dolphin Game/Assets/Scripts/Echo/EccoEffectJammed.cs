using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Rendering;
using System.Collections.Generic;

[ExecuteInEditMode]
public class EccoEffectJammed : MonoBehaviour
{
    Echo[] echos = new Echo[100];

    public EccoEffectJammed()
    {
        for (int i = 0; i < echos.Length; i++)
        {
            echos[i] = new Echo();
        }
    }

    private PlayerController playerController;

    public AudioSource[] dolphinSounds;

    int currentScanDistancesIndex = 0;


    //Demo Code public Transform ScannerOrigin;
    public Material EffectMaterial;
    //Demo Code public float ScanDistance;

    private Camera _camera;

    // Demo Code
    // bool _scanning;
    Findable[] findables = new Findable[0];

    public void StartEcho(Echo echo)
    {
        echos[currentScanDistancesIndex % echos.Length] = echo;
        echo.Scanning = true;

        currentScanDistancesIndex = (currentScanDistancesIndex + 1) % echos.Length;
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        findables = FindObjectsOfType<Findable>();
    }

    void Update()
    {
        for (int i = 0; i < echos.Length; i++)
        {
            if (echos[i].Scanning)
            {
                echos[i].Distance += Time.deltaTime * 50;
                if (echos[i].Type == EchoType.Echo && echos[i].Distance < 250)
                {
                    foreach (Findable findable in findables)
                    {
                        if (Vector3.Distance(echos[i].Origin, findable.transform.position) <= echos[i].Distance)
                        {
                            findable.FoundByEcho(echos[i]);
                        }
                    }
                }

            }
        }





        //if (Input.GetButtonDown("X Button"))
        //{
        //AudioSource dolphinSound = dolphinSounds[Random.Range(0, dolphinSounds.Length)];
        //dolphinSound.Play();

        //Demo Code _scanning = true;
        //Demo Code ScanDistance = 0;



        //}
    }
    // End Demo Code

    void OnEnable()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;

    }

    Transform eccoOrigin = null;

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {




        float[] echoDistancesArray = new float[100];
        float[] echoTypesArray = new float[100];
        Vector4[] echoOriginsArray = new Vector4[100];

        for (int i = 0; i < echos.Length && i < echoDistancesArray.Length; i++)
        {
            echoDistancesArray[i] = echos[i].Distance;
            echoOriginsArray[i] = echos[i].Origin;

            switch (echos[i].Type)
            {
                case EchoType.Echo:
                    echoTypesArray[i] = 0f;

                    break;
                case EchoType.JammedEcho:
                    echoTypesArray[i] = 1f;

                    break;
                case EchoType.HackEcho:
                    echoTypesArray[i] = 2f;

                    break;
                default: break;

            }

        }

        EffectMaterial.SetVectorArray("_EchoOrigins", echoOriginsArray);
        EffectMaterial.SetFloatArray("_EchoDistances", echoDistancesArray);
        EffectMaterial.SetFloatArray("_EchoTypes", echoTypesArray);


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

 
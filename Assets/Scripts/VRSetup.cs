using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class VRSetup : MonoBehaviour
{
    public bool VREnabled;
    public GameObject VRPrefab;

    private bool vrEnabled = false;
    private bool vrRunning = false;
    private GameObject vrRig;
    private GameObject pancakeCamera;

    void Start()
    {
        vrRunning = XRGeneralSettings.Instance.Manager.isInitializationComplete;
        pancakeCamera = FindObjectOfType<Camera>().gameObject;

        checkForUpdate();
    }

    void Update()
    {
        checkForUpdate();
    }

    void checkForUpdate()
    {
        if (VREnabled != vrEnabled)
            updateVR();
    }

    void updateVR()
    {
        vrEnabled = VREnabled;

        if (vrEnabled && !vrRunning)
        {
            //XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
            StartCoroutine(StartVR());

            vrRunning = true;
        }
        if (!vrEnabled && vrRunning)
        {
            if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
            {
                XRGeneralSettings.Instance.Manager.StopSubsystems();
                XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            }   
            if (Application.isEditor)
                DestroyImmediate(vrRig);
            else
                Destroy(vrRig);
            pancakeCamera.SetActive(true);
            vrRunning = false;
        }
    }

    public IEnumerator StartVR()
    {
        pancakeCamera.SetActive(false);
        vrRig = Instantiate(VRPrefab);
        
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }

	void OnDestroy()
	{
        if (XRGeneralSettings.Instance.Manager.isInitializationComplete)
        {
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        }
	}
}

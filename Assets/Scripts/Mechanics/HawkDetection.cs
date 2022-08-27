using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkDetection : GameBehaviour
{
    public GameObject hawkBody;
    public GameObject detectionCircle;
    public bool isGrowing;
    private bool rayCasting;
    public Vector3 minScale;
    public Vector3 maxScale;
    float lerpTime = 0.3f;

    public float timer;
    readonly float maxTimer = 8f;
    public LayerMask mask;

    private void Start()
    {
        detectionCircle.transform.localPosition = new Vector3(0, 0, 0);
        detectionCircle.GetComponent<MeshRenderer>().enabled = false;

        isGrowing = false;
        timer = maxTimer;
        rayCasting = false;
    }
    private void Update()
    {

        timer = Mathf.Clamp(timer, 0, maxTimer);
        if (isGrowing)
        {
            detectionCircle.transform.localScale = Vector3.Lerp(detectionCircle.transform.localScale, maxScale,
                lerpTime * Time.deltaTime);
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                rayCasting = false;
                
                GM.RespawnPlayer();
                hawkBody.transform.localPosition = new Vector3(0, 0, 0);
                detectionCircle.transform.localPosition = new Vector3(0, 0, 0);
                isGrowing = false;

                timer = maxTimer;
            }
        }
        if (!isGrowing)
        {
            timer += Time.deltaTime;
            detectionCircle.transform.localScale = minScale;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            rayCasting = true;
            Debug.Log("israycasting");
            //isGrowing = true;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hawkBody.transform.position = new Vector3(TPM.transform.position.x, hawkBody.transform.position.y, TPM.transform.position.z);
            RaycastHit hit;
            Ray hawkRay = new Ray(hawkBody.transform.position, Vector3.down);

            if (rayCasting)
            {
                if (Physics.Raycast(hawkRay, out hit, 200, mask))
                {
                    if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        detectionCircle.GetComponent<MeshRenderer>().enabled = true;
                        Debug.DrawLine(hawkRay.origin, hit.point, Color.red);
                        detectionCircle.transform.position = new Vector3(other.transform.position.x, TPM.groundCheck.transform.position.y -0.01f, other.transform.position.z);
                        isGrowing = true;
                    }
                    else
                    {
                        detectionCircle.GetComponent<MeshRenderer>().enabled = false;
                        Debug.DrawLine(hawkRay.origin, hawkRay.origin * 100, Color.green);
                        isGrowing = false;
                    }
                }
                
            }
        }
        

                    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isGrowing = false;
            hawkBody.transform.localPosition = new Vector3(0, 0, 0);
            detectionCircle.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}

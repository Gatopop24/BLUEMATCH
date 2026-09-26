using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private Vector3 grapplePoint;
    [SerializeField] private float maxDistance = 1000f; //erase if not used
    [SerializeField] private int inputAction;
    [SerializeField] private bool isHooked;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip ShootSound;
    private SpringJoint joint;
    public LayerMask canBeHooked;
    public Transform tip;
    public Transform cam;
    public Transform player;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if(InputController.Instance.GetButton(inputAction))
        {
            if (!isHooked)
            {
                ShootGrapple();
            }
        }
        else
        {
            if(isHooked)
            {
                StopGrapple();
            }
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }

    private void ShootGrapple()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, canBeHooked))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.3f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
            isHooked = true;
            player.GetComponent<PlayerController>().isSwinging = true;
            PlayAudio.PlayClip(audioSource, ShootSound);
        }
    }

    private void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
        isHooked = false;
        player.GetComponent<PlayerController>().isSwinging = false;
    }

    private void DrawRope()
    {
        if(!joint)
        {
            return;
        }
        lineRenderer.SetPosition(0, tip.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }
}

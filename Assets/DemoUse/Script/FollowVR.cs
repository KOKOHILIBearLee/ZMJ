using UnityEngine;
using System.Collections;

public class FollowVR : MonoBehaviour
{
    public GameObject followTarget;
    public Vector3 relativePosition;
    public Vector3 relativeRotate;
    // Update is called once per frame
    void Update()
    {
        if (followTarget == null)
            return;
        Ray rayx = new Ray(followTarget.transform.position, followTarget.transform.right);
        Ray rayy = new Ray(followTarget.transform.position, followTarget.transform.up);
        Ray rayz = new Ray(followTarget.transform.position, followTarget.transform.forward);

        Vector3 xdi = rayx.GetPoint(relativePosition.x) - followTarget.transform.position;
        Vector3 ydi = rayy.GetPoint(relativePosition.y) - followTarget.transform.position;
        Vector3 zdi = rayz.GetPoint(relativePosition.z) - followTarget.transform.position;

        this.transform.position = followTarget.transform.position + xdi + ydi + zdi;

        this.transform.rotation = followTarget.transform.rotation * Quaternion.Euler(relativeRotate);
    }
}

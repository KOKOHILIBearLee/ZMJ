using UnityEngine;
using System.Collections;

public class ZMJMap : MonoBehaviour
{
    RectTransform trans;

    public Transform startPosition;
    public Transform endPosition;
    public Transform targetPosition;

    private void Awake()
    {
        trans = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector3 position)
    {
        Vector3 result = position - startPosition.position;
        Vector3 maxsize = endPosition.position - startPosition.position;
        Vector2 actualSize = new Vector2(maxsize.x, maxsize.z);
        Vector2 anchor = new Vector2(Mathf.Abs(result.x / actualSize.x), Mathf.Abs(result.z / actualSize.y));
        trans.anchorMax = anchor;
        trans.anchorMin = anchor;
    }

    private void OnGUI()
    {
      //  GUI.Label(new Rect(0, 0, 500, 500), trans.anchorMax + "  " + trans.anchorMin);
    }

    private void Update()
    {
        try
        {
            SetPosition(targetPosition.position);
        }
        catch (System.Exception e) { }
    }
}

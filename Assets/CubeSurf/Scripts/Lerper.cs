using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lerper : MonoBehaviour
{
    [SerializeField]
    public IProgress progressInterface;
    public Transform startPoint;
    public Transform targetPoint;
    public bool position = true;
    public bool rotation = true;
    public LerpingType type = LerpingType.LINEAR;

    private bool isSelf = false;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        if(  startPoint == transform)
        {
            isSelf = true;
        }

        if (isSelf) {
            startPosition = startPoint.localPosition;
            startRotation = startPoint.localRotation;
        }
    }

    void Update()
    {
        float p = progressInterface.GetProgress(type);

        if (isSelf)
        {
            if (position)
            {
                transform.position = Vector3.Lerp(transform.parent.TransformPoint(startPosition), targetPoint.position, p);
            }
            if (rotation)
            {
                transform.rotation = Quaternion.Lerp(transform.parent.rotation * startRotation, targetPoint.rotation, p);
            }
        }
        else
        {
            if (position)
            {
                transform.position = Vector3.Lerp(startPoint.position, targetPoint.position, p);
            }
            if (rotation)
            {
                transform.rotation = Quaternion.Lerp(startPoint.rotation, targetPoint.rotation, p);
            }
        }
    }
}

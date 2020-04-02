using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainSpriteLineRenderer : MonoBehaviour
{
    public Transform origin;
    public Transform weapon;
    public LineRenderer lineRenderer;

    void Update()
    {
        lineRenderer.SetPosition(0,origin.position);
        lineRenderer.SetPosition(1,weapon.position);
    }
}

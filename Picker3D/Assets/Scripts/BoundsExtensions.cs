using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions 
{
    public static Vector3 RandomPointInBounds(this Bounds bounds, bool centerY = false)
    {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
          (centerY ? bounds.center.y : UnityEngine.Random.Range(bounds.min.y, bounds.max.y)),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}

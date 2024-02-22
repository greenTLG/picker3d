using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPreview : MonoBehaviour
{
    Rigidbody rb;
    List<Collider> colliders = new List<Collider>();
    Renderer[] renderers;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            if (rb.isKinematic)
            {
                rb = null;
            }
            else
            {
                rb.isKinematic = true;
            }
        }
        colliders.AddRange(GetComponentsInChildren<Collider>());
        colliders.RemoveAll(x => x.isTrigger);
        colliders.ForEach(x => x.isTrigger = true);


    }

    private void OnDestroy()
    {
        colliders.ForEach(x => x.isTrigger = false);
        if (rb != null)
        {
            rb.isKinematic = false;
        }

    }
}

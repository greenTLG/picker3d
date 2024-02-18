﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    [SerializeField] Rigidbody ownRb;
    [SerializeField] float explosionDelay = 1;
    bool isCollected = false;

    public void WhenCollected()
    {
        if (isCollected)
            return;
        isCollected = true;
        Invoke("Explode", explosionDelay);
    }

    void Explode()
    {
        ParticleManager.Instance.Play("PlayerObjExplosion", transform.position);
        gameObject.SetActive(false);
    }
}

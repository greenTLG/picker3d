using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjectController : MonoBehaviour
{
    [SerializeField] Rigidbody ownRb;
    [SerializeField] GameObject ownVisual;
    [SerializeField] Rigidbody[] pieceRbs = new Rigidbody[0];
    Vector3[] localPositions_pieces, localEulerAngles_pieces;

    private void Awake()
    {
        localPositions_pieces = new Vector3[pieceRbs.Length];
        localEulerAngles_pieces = new Vector3[pieceRbs.Length];

        for (int i = 0; i < pieceRbs.Length; i++)
        {
            localPositions_pieces[i] = pieceRbs[i].transform.localPosition;
            localEulerAngles_pieces[i] = pieceRbs[i].transform.localEulerAngles;
        }
    }

    public void WhenCollected()
    {
        ownRb.isKinematic = true;
        ownVisual.SetActive(false);
        for (int i = 0; i < pieceRbs.Length; i++)
        {
            pieceRbs[i].gameObject.SetActive(true);
            pieceRbs[i].transform.DOScale(0, 0.5f).SetEase(Ease.InBack).SetDelay(0.5f);
        }
    }


}

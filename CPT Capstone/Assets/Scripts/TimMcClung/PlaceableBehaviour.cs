using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceableBehaviour : MonoBehaviour
{
    public bool isPlaced { get; private set; }

    public abstract void OnPreview();
    public abstract bool OnPlace();
    protected abstract bool CanPlace();
    protected abstract void SetPosition(RaycastHit hitInfo);

    public void Place()
    {
        if (OnPlace())
        {
            isPlaced = true;
        }
    }

}

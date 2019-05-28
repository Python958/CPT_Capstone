using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableGun : PlaceableBehaviour
{
    public Color canPlaceColor;
    public Color cantPlaceColor;

    //AMS script array
    public MonoBehaviour[] aiScripts;

    Renderer myRenderer;
    Material originalMaterial;
    Material temporaryMaterial;
    Collider myCollider;

    // allows the object to preview by making it a different color as well as turning the collider off

    void Awake()
    {
        myRenderer = gameObject.GetComponent<Renderer>();
       // originalMaterial = myRenderer.sharedMaterial;
        SetupTemporaryMaterial();
      //  myRenderer.material = temporaryMaterial;

        myCollider = gameObject.GetComponent<Collider>();
        //myCollider.enabled = false;
        SetPosition(GetPlacementPosition());
    }

    //Sets the temporary material color for the object being able to be placed vs not being able to be placed.

    public override void OnPreview()
    {
        if (isPlaced) return;

        if (CanPlace())
        {
           //temporaryMaterial.color = canPlaceColor;
        }
        else
        {
           //temporaryMaterial.color = cantPlaceColor;
        }

        SetPosition(GetPlacementPosition());
    }

    // checks to see if the postion is valid for placement

    protected override bool CanPlace()
    {
        if (GetPlacementPosition().collider != null) return true;
        return false;
    }

    // checks to see if the postion is valid for placement

    public override bool OnPlace()
    {
        RaycastHit hitInfo = GetPlacementPosition();
        if (hitInfo.collider != null)
        {
            SetPosition(hitInfo);

            //myRenderer.material = originalMaterial;

           // myCollider.enabled = true;

            //AMS Addition
            for (int i = 0; i < aiScripts.Length; i++)
            {
                aiScripts[i].enabled = true;
            }

            return true;
        }
        return false;
    }

    protected override void SetPosition(RaycastHit hitInfo)
    {
        if (hitInfo.collider != null)
        {
            transform.position = hitInfo.point;
        }
    }

    RaycastHit GetPlacementPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        Physics.Raycast(ray, out hitInfo);
        return hitInfo;
    }

    void SetupTemporaryMaterial()
    {
        //Creates standard shader with transparency

        temporaryMaterial = new Material(Shader.Find("Standard"));
        temporaryMaterial.SetFloat("_Mode", 3);
        temporaryMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        temporaryMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        temporaryMaterial.SetInt("_ZWrite", 0);
        temporaryMaterial.DisableKeyword("_ALPHATEST_ON");
        temporaryMaterial.DisableKeyword("_ALPHABLEND_ON");
        temporaryMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        temporaryMaterial.renderQueue = 3000;
    }
}


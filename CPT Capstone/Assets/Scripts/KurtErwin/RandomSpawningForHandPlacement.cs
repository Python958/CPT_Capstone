using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RandomSpawningForHandPlacement : MonoBehaviour
{
    
    private Collider colliderBox;
    public GameObject[] decorateObject1;
    public float decorateObject1Spacing;        //should be how often you want the object to spawn
    public float decorateObject1ChanceToSpawn;  //0 to 1 how often it should spawn in a space

    public void BuildItems()
    {
        var colliderBox = gameObject.GetComponent<Collider>();
        if(colliderBox != null)
        {
            var size = colliderBox.bounds.size;

            if (decorateObject1 != null)
            {
                var iterateX = Mathf.Round(size.x / decorateObject1Spacing);
                var iterateZ = Mathf.Round(size.z / decorateObject1Spacing);

                for(var x = 0; x < iterateX; x++)
                {
                    for (var z = 0; z < iterateZ; z++)
                    {
                        if(Random.Range(0f,1f) < decorateObject1ChanceToSpawn)
                        {
                            var decorateToUse = decorateObject1[Random.Range((int)0, decorateObject1.Length)];
                            var newDecorate = PrefabUtility.InstantiatePrefab(decorateToUse) as GameObject;
                            var xOffset = Random.Range(0f, decorateObject1Spacing);
                            var yOffset = Random.Range(0f, decorateObject1Spacing);

                            var setPos = new Vector3(transform.position.x - size.x / 2f + (decorateObject1Spacing * x) + xOffset, transform.position.y, transform.position.z - size.z / 2f + (decorateObject1Spacing * z) + yOffset);
                            newDecorate.transform.position = setPos;
                            newDecorate.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                        }
                    }
                }
            }
        }
    }
}

[CustomEditor(typeof(RandomSpawningForHandPlacement))]
public class RandomSpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandomSpawningForHandPlacement myScript = (RandomSpawningForHandPlacement)target;
        if (GUILayout.Button("Build Object"))
        {
            myScript.BuildItems();
        }
    }
}


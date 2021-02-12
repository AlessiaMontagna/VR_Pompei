using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ArrayModifier : MonoBehaviour
{
    public int CountX;
    public int CountY;
    public GameObject TheObject;
    void OnValidate()
    {
        Apply();

    }

    private void Apply()
    {
        if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (TheObject == null) return;
        Renderer renderer = TheObject.GetComponent<Renderer>();
        if(renderer != null)
        {
            foreach(Transform t in transform)
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(t.gameObject);
                };
            }
        }

        float lastX = 0;
        float lastZ = 0;

        for (int i=0; i<CountY; i++)
        {
            for(int j=0; j <CountX; j++)
            {
                Vector3 pos = new Vector3(transform.localPosition.x + lastX, transform.localPosition.y, transform.localPosition.z + lastZ);
                GameObject go = (GameObject)Instantiate(TheObject, pos, Quaternion.identity, transform) as GameObject;
                go.name = TheObject.name + "_" + i + "_" + j;
                lastX -= renderer.bounds.size.x;
            }
            lastX = 0;
            lastZ += renderer.bounds.size.z;
        }
    }
}

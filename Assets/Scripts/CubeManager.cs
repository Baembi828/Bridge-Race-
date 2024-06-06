using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeManager : MonoBehaviour
{
    public GameObject player;
    public GameObject sp;
    public GameObject ep;
    public GameObject father;
    GameObject collected;
    public int count = 0;

    public Stack<GameObject> cubeStack = new Stack<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cube")
        {
            collected = other.gameObject;
            cubeStack.Push(collected);
            collected.transform.SetParent(player.transform);
            collected.transform.localPosition = sp.transform.localPosition;
            collected.transform.localRotation = Quaternion.identity;
        }

        if (cubeStack.Count > 1)
        {
            Transform previousObject = cubeStack.Peek().transform;
            collected.transform.SetParent(previousObject);

            Vector3 localPositionOffset = new Vector3(0f, previousObject.GetComponent<Renderer>().bounds.size.y, 0f);

            int stackHeight = cubeStack.Count - 1;
            collected.transform.localPosition = sp.transform.localPosition + localPositionOffset * stackHeight;

        }

        count++;
        Debug.Log(count);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log("1 time");
            cubeStack.Pop();
            collected.transform.SetParent(father.transform);
            collected.transform.localPosition = ep.transform.localPosition;
            collected.transform.localRotation = Quaternion.identity;

            if (cubeStack.Count < count)
            {
                cubeStack.Pop();
                collected.transform.SetParent(father.transform);
                collected.transform.localPosition = ep.transform.localPosition + collected.transform.localPosition;
            }
        }
    }
}
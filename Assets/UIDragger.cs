using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UIDragger : MonoBehaviour 
{

    public const string DRAGGABLE_TAG = "UIDraggable";
    private bool dragging = false;  

    private Vector2 originalPosition;
    private Transform objectToDrag;
    private Image objectToDragImage;
    private int originalIndex;
    private int replacementIndex;

    GameObject DigitalGood;

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    #region Monobehaviour API

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            objectToDrag = GetDraggableTransformUnderMouse();

            if (objectToDrag != null)
            {
                dragging = true;
                originalIndex = objectToDrag.GetSiblingIndex();

                objectToDrag.transform.parent.transform.parent.GetComponent<ScrollRect>().enabled = false;
            }

            
        }

        if (dragging)
        {

            objectToDrag.position = Input.mousePosition;

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (objectToDrag != null)
            {
                var objectToReplace = GetDraggableTransformUnderMouse();

                replacementIndex = objectToReplace.GetSiblingIndex();

                if (objectToReplace != objectToDrag)
                {
                    objectToDrag.SetSiblingIndex(replacementIndex);
                    objectToReplace.SetSiblingIndex(originalIndex);
                    //SetItemIndex(originalIndex, replacementIndex);
                    SetItemIndex(replacementIndex, originalIndex);


                }
                else
                {
                    objectToDrag.SetSiblingIndex(originalIndex);
                }

                objectToReplace.transform.parent.transform.parent.GetComponent<ScrollRect>().enabled = true;
                objectToDrag = null;
            }

            dragging = false;
        }
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;

        for (int i = 0; i <= hitObjects.Count - 1; i++)
        {
            //Get the Code Panel child
            if (hitObjects[i].gameObject.name == "Digital Good(Clone)")
            {
                DigitalGood = hitObjects[i].gameObject;
                break;
            }
        }

        return DigitalGood;

    }

    private Transform GetDraggableTransformUnderMouse()
    {
        var clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && clickedObject.tag == DRAGGABLE_TAG)
        {
            return clickedObject.transform;
        }

        return null;
    }

    private void SetItemIndex(int oldIndex, int newIndex)
    {
        string json = ((TextAsset)Resources.Load("Data/digitals")).text;
        JSONNode node = JSON.Parse(json);

        Debug.Log("OLD: " + oldIndex + ", NEW: " + newIndex);

        node[oldIndex]["Index"].AsInt = newIndex;
        node[newIndex]["Index"].AsInt = oldIndex;

        Debug.Log(node);

        File.WriteAllText("Assets/Apollo/Examples/Resources/Data/digitals.txt", node.ToString());
    }

    #endregion
}

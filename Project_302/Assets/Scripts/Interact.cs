using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private GameObject item;
    public PlayerCamera playerCamera;
    public Rigidbody itemRB;
    public float distance;
    public float throwFoce = 250f;
    public Transform holdPos;
    public GameObject interactIcon;
    Vector3 originalPos;
    public bool onInteract = false;
    GameObject interactable;

    public void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, distance))
        {
            if (hit.transform.tag == "Item" && !onInteract)
            {
                interactIcon.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    itemRB.isKinematic = true;
                    itemRB.useGravity = true;
                    interactable = hit.transform.gameObject;
                    originalPos = hit.transform.position;
                    StartCoroutine(PickupItem());
                    onInteract = true;
                }
            }
            else
            {
                interactIcon.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Mouse1) && onInteract)
        {
            StartCoroutine(DropItem());
            onInteract = false;
        }

        if (onInteract)
        {

            interactable.transform.position = Vector3.Lerp(interactable.transform.position, holdPos.position, 0.4f);

            if (Input.GetKeyDown(KeyCode.E) && onInteract)
            {
                ThrowItem();
                
            }


            if (Input.GetKey(KeyCode.R))
            {
                if (playerCamera != null)
                    playerCamera.enabled = false;

                holdPos.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 350f);
            }
            else
            {
                if (playerCamera != null)
                    playerCamera.enabled = true;
            }
        }
    }

    IEnumerator PickupItem()
    {
        yield return new WaitForSeconds(0.2f);
        interactable.transform.SetParent(holdPos);
    }

    IEnumerator DropItem()
    {
       if(interactable != null)
        {
            interactable.transform.SetParent(null);

            Rigidbody itemRB = interactable.GetComponent<Rigidbody>();
            if(itemRB != null)
            {
                itemRB.isKinematic = false;
                itemRB.useGravity = true;
            }

            interactable = null;
            onInteract = false;
            yield return new WaitForSeconds(0f);
        }
        
    }


    void ThrowItem()
    {
        interactable.transform.SetParent(null);

        Rigidbody itemrb = interactable.transform.GetComponent<Rigidbody>();
        if (itemrb != null)
        {
            itemRB.isKinematic = false;
            itemRB.AddForce(transform.forward * throwFoce);
           
        }

        interactable = null;
        onInteract = false;
    }
}

    
    


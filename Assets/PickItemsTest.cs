using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PickItemsTest : NetworkBehaviour
{
    private Transform itemPosition;

    private bool picked = false;
    private bool onRange = false;

    private NetworkObject networkObjectInRange;
    private NetworkObject networkObjectPicked;
    private void Start()
    {
        if (!IsOwner) return;
        itemPosition = transform.parent.GetChild(2).transform;
    }
    private void OnTriggerStay(Collider other)
    {
        if (!IsOwner) return;
        if(other.tag == "Pickable" && !picked)
        {
            onRange = true;
            networkObjectInRange = other.GetComponent<NetworkObject>();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsOwner) return;
        if (other.tag == "Pickable")
        {
            networkObjectInRange = null;
            onRange = false;
        }
    }

    public void Update()
    {
        if (!IsOwner) return;
        if(onRange && !picked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                picked = true;
                networkObjectInRange.GetComponent<ItemPicked>().SetPositionServerRpc(itemPosition.position, false);
                networkObjectInRange.GetComponent<ItemPicked>().SetParentServerRpc(transform.parent.gameObject, true);
                networkObjectPicked = networkObjectInRange;
            }
        }
        else if(picked)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                picked = false;
                var pos = networkObjectPicked.transform.position;
                pos.y = 0f;
                networkObjectPicked.GetComponent<ItemPicked>().SetPositionServerRpc(pos, true);
                networkObjectPicked.GetComponent<ItemPicked>().SetParentServerRpc(transform.parent.gameObject,false);
                networkObjectPicked = null;
            }
        }
    }





}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ItemPicked : NetworkBehaviour
{

    [ServerRpc(RequireOwnership =false)]
    public void SetPositionServerRpc(Vector3 newPosition, bool gravityOn)
    {
        transform.position = newPosition;
        transform.GetComponent<Rigidbody>().useGravity = gravityOn;
        transform.GetComponent<Rigidbody>().velocity = Vector3.zero;
        transform.GetComponent<Rigidbody>().isKinematic = !gravityOn;
        SetPositionClientRpc(newPosition);

    }

    [ClientRpc]
    private void SetPositionClientRpc(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    [ServerRpc(RequireOwnership =false)]
    public void SetParentServerRpc(NetworkObjectReference parentGameObject, bool parentOn)
    {
        if (!parentGameObject.TryGet(out NetworkObject networkObject))
        {
            Debug.Log("error");
        }
        if (parentOn)
            transform.GetComponent<NetworkObject>().TrySetParent(networkObject);
        else
            transform.parent = null;
    }

}

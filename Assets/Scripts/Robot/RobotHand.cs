using UnityEngine;

public class RobotHand : MonoBehaviour
{
    private GameObject touchingItem;
    private GameObject holdingItem;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Grabbable"))
        {
            touchingItem = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == touchingItem)
        {
            touchingItem = null;
        }
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (holdingItem != null)
            {
                LetGoObject();
            }
            else if (touchingItem != null)
            {
                GrabObject();
            }
        }
    }

    private void GrabObject()
    {
        holdingItem = touchingItem;

        holdingItem.transform.SetParent(transform);

        Rigidbody rb = holdingItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    private void LetGoObject()
    {
        holdingItem.transform.SetParent(null);

        Rigidbody rb = holdingItem.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        holdingItem = null;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    private bool _triggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "TableTrigger")
        {
            ArchivingController controller = other.transform.parent.GetComponent<ArchivingController>();
            controller.isTriggered = true;
            controller.OnTriggerChange();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "TableTrigger")
        {
            ArchivingController controller = other.transform.parent.GetComponent<ArchivingController>();
            controller.isTriggered = false;
            controller.OnTriggerChange();
        }
    }
}

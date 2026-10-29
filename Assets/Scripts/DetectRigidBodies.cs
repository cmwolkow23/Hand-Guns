using System.Collections.Generic;
using UnityEngine;

public class DetectRigidBodies : MonoBehaviour
{
    private class Contact
    {
        public Rigidbody rigidbody;
        public int count;
    }

    private readonly Dictionary<Collider, Contact> contacts =
        new Dictionary<Collider, Contact>();

    private readonly HashSet<Rigidbody> rigidBodies =
        new HashSet<Rigidbody>();

    private Vector3 previousPosition;
    private Quaternion previousRotation;

    private void OnEnable()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;
    }

    private void OnDisable()
    {
        contacts.Clear();
        rigidBodies.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        Rigidbody body = collision.rigidbody;

        if (collider == null || body == null)
            return;

        if (contacts.TryGetValue(collider, out Contact contact))
        {
            contact.count++;
            return;
        }

        contacts.Add(collider, new Contact
        {
            rigidbody = body,
            count = 1
        });

        rigidBodies.Add(body);
    }

    private void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;

        if (collider == null ||
            !contacts.TryGetValue(collider, out Contact contact))
            return;

        contact.count--;

        if (contact.count > 0)
            return;

        Rigidbody body = contact.rigidbody;

        contacts.Remove(collider);

        // Don't remove the Rigidbody if another collider
        // belonging to it is still touching the helper.
        foreach (Contact remaining in contacts.Values)
        {
            if (remaining.rigidbody == body)
                return;
        }

        rigidBodies.Remove(body);
    }

    private void FixedUpdate()
    {
        Vector3 currentPosition = transform.position;
        Quaternion currentRotation = transform.rotation;

        Vector3 positionDelta =
            currentPosition - previousPosition;

        Quaternion rotationDelta =
            currentRotation * Quaternion.Inverse(previousRotation);

        foreach (Rigidbody body in rigidBodies)
        {
            if (body == null)
                continue;

            /*
             * Calculate where the Rigidbody is relative
             * to the helper's previous world position.
             */
            Vector3 offset =
                body.position - previousPosition;

            /*
             * Rotate that offset around the helper's
             * world-space pivot.
             */
            Vector3 rotatedOffset =
                rotationDelta * offset;

            /*
             * Apply the helper's translation and rotation.
             */
            Vector3 newPosition =
                currentPosition + rotatedOffset;

            body.MovePosition(newPosition);

            body.MoveRotation(
                rotationDelta * body.rotation
            );
        }

        previousPosition = currentPosition;
        previousRotation = currentRotation;

        RemoveInvalidBodies();
    }

    private void RemoveInvalidBodies()
    {
        List<Rigidbody> invalidBodies = null;

        foreach (Rigidbody body in rigidBodies)
        {
            if (body != null)
                continue;

            if (invalidBodies == null)
                invalidBodies = new List<Rigidbody>();

            invalidBodies.Add(body);
        }

        if (invalidBodies == null)
            return;

        foreach (Rigidbody body in invalidBodies)
            rigidBodies.Remove(body);
    }
}
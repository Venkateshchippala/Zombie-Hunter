using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{

    [SerializeField] private Transform lower;
    [SerializeField] private Transform upper;

    /// <summary>
    /// Finds the normalized position of a world point relative to the bounds defined by "lower" and "upper".
    /// </summary>
    /// <param name="position">The world position to normalize.</param>
    /// <returns>A Vector2 with normalized X and Y coordinates.</returns>
    public Vector2 FindNormalizedPosition(Vector3 position)
    {
        float xPosition = Mathf.InverseLerp(lower.position.x, upper.position.x, position.x);
        float yPosition = Mathf.InverseLerp(lower.position.z, upper.position.z, position.z);
        return new Vector2(xPosition, yPosition);
    }

}
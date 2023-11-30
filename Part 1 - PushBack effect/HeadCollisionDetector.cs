using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionDetector : MonoBehaviour
{
    [SerializeField, Range(0, 0.5f)]
    private float _detectionDelay = 0.05f;
    [SerializeField]
    private float _detectionDistance = 0.2f;
    [SerializeField]
    private LayerMask _detectionLayers;
    public List<RaycastHit> DetectedColliderHits { get; private set; }

    private float _currentTime = 0;

    private List<RaycastHit> PreformDetection
    (Vector3 position, float distance, LayerMask mask)
    {
        List<RaycastHit> detectedHits = new();

        List<Vector3> directions
            = new() { transform.forward, transform.right, -transform.right };

        RaycastHit hit;
        foreach (var dir in directions)
        {
            if (Physics.Raycast(position, dir, out hit, distance, mask))
            {
                detectedHits.Add(hit);
            }
        }
        return detectedHits;
    }

    private void Start()
    {
        DetectedColliderHits = PreformDetection(transform.position,
           _detectionDistance, _detectionLayers);
    }
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _detectionDelay)
        {
            _currentTime = 0;
            DetectedColliderHits = PreformDetection(transform.position,
                _detectionDistance, _detectionLayers);
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying == false)
            return;
        Color c = Color.green;
        c.a = 0.5f;
        if (DetectedColliderHits.Count > 0)
        {
            c = Color.red;
            c.a = 0.5f;
        }

        Gizmos.color = c;
        Gizmos.DrawWireSphere(transform.position, _detectionDistance);

        List<Vector3> directions = new() { transform.forward, transform.right, -transform.right };
        Gizmos.color = Color.magenta;
        foreach (var dir in directions)
        {
            Gizmos.DrawRay(transform.position, dir);
        }
    }
}

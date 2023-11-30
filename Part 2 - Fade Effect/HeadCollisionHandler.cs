using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private HeadCollisionDetector _detector;
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    public float pushBackStrength = 1.0f;
    [SerializeField]
    private FadeEffect _blackScreenFade;

    public bool IsClimbing
    {
        get;
        set;
    }

    private Vector3 CalculatePushBackDirection(List<RaycastHit> colliderHits)
    {
        Vector3 combinedNormal = Vector3.zero;
        foreach (RaycastHit hitPoint in colliderHits)
        {
            combinedNormal +=
                new Vector3(hitPoint.normal.x, 0, hitPoint.normal.z); ;
        }
        return combinedNormal;
    }

    private void Update()
    {
        if (_detector.InsideCollider && IsClimbing)
        {
            _blackScreenFade.Fade(true);
            return;
        }
        if (_detector.DetectedColliderHits.Count <= 0)
        {
            _blackScreenFade.Fade(false);
            return;
        }
        if (IsClimbing)
        {
            _blackScreenFade.Fade(true);
            return;
        }
        Vector3 pushBackDirection
            = CalculatePushBackDirection(_detector.DetectedColliderHits);

        Debug.DrawRay(transform.position, pushBackDirection.normalized, Color.magenta);

        _characterController
            .Move(pushBackDirection.normalized * pushBackStrength * Time.deltaTime);
    }
}

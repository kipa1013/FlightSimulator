using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheel : XRBaseInteractable
{
    [SerializeField] private Transform wheelTransform;

    public UnityEvent<float> OnWheelRotated;

    private float currentAngle = 0.0f;

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        //Debug.Log("Hover");
        base.OnHoverEntered(args);
        
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        currentAngle = FindWheelAngle();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        currentAngle = FindWheelAngle();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                RotateWheel();
                TranslateWheel();

            }
                
        }
    }

   
    private void RotateWheel()
    {
        // Convert that direction to an angle, then rotation
        float totalAngle = FindWheelAngle();
        //Debug.Log("totalAngle:"+totalAngle);
        // Apply difference in angle to wheel
        float angleDifference = currentAngle - totalAngle;
        //TODO: Maximale Drehung begrenzen
        if (!RotationValid(angleDifference))
        {
            return;
        }

        wheelTransform.Rotate(transform.forward, -angleDifference, Space.World);
        OnWheelRotated?.Invoke(angleDifference);
        currentAngle = totalAngle;
        
            
        // Store angle for next process
    }

    private float FindWheelAngle()
    {
        float totalAngle = 0;

        // Combine directions of current interactors
        foreach (IXRSelectInteractor interactor in interactorsSelecting)
        {
            Vector2 direction = FindLocalPoint(interactor.transform.position);
            totalAngle += ConvertToAngle(direction) * FindRotationSensitivity();
        }

        return totalAngle;
    }

    

    private Vector2 FindLocalPoint(Vector3 position)
    {
        // Convert the hand positions to local, so we can find the angle easier
        return transform.InverseTransformPoint(position).normalized;
    }

    private float ConvertToAngle(Vector2 direction)
    {
        // Use a consistent up direction to find the angle
        return Vector2.SignedAngle(Vector2.up, direction);
    }

    private float FindRotationSensitivity()
    {
        // Use a smaller rotation sensitivity with two hands
        return 1.0f / interactorsSelecting.Count;
    }

    public float GetRotationNormalized()
    {
        /*Debug.Log("Local Rotation:"+wheelTransform.localRotation);
        Debug.Log("global Rotation:"+this.wheelTransform.rotation);
        Debug.Log("global EulerAngles:"+this.wheelTransform.eulerAngles);
        Debug.Log("local EulerAngles:"+this.wheelTransform.localEulerAngles);*/
        float degrees = wheelTransform.localEulerAngles.z;
        if (degrees > 180)
        {
            degrees= -(360f - degrees);
        }

        var normalized = degrees/180;
        //Debug.Log(normalized);
        return normalized;

    }
    //@Paul
    public bool RotationValid(float angleDifference)
    {
        var rotation = GetRotationNormalized();
        
        var degrees = wheelTransform.localEulerAngles.z + (-angleDifference);
        if (degrees>180)
        {
            degrees= -(360f - degrees);
        }

        var normalized = degrees/180;
        //Debug.Log(normalized);
        return Mathf.Sign(normalized)==Mathf.Sign(rotation)||Mathf.Abs(rotation)<0.5;
    }

    #region push

    //@Paul Kilger
    private float FindNewWheelPosition()
    {
        float interactorPositionZ = interactorsSelecting[0].transform.position.z;
        float totalZ = interactorPositionZ-wheelTransform.transform.position.z;
        //Debug.Log("relative Position of interactor in z direction:"+ Mathf.Clamp(totalZ, 0f, 0.40f));
        Debug.Log("wheelTransform local z:" + wheelTransform.transform.localPosition.z);
        //Debug.Log(wheelTransform.localPosition);
        return Mathf.Clamp(totalZ, -0.2f, 0.3f);
    }

    private float FindWheelTranslation()
    {
        return wheelTransform.transform.localPosition.z - FindNewWheelPosition();
    }
    private void TranslateWheel()
    {
        var vector = Vector3.zero;
        vector.z = FindNewWheelPosition();
        Debug.Log("Vector:"+vector);
        wheelTransform.localPosition = vector;
        //wheelTransform.Translate(vector, Space.Self);
        //wheelTransform.SetPositionAndRotation(vector,wheelTransform.rotation);
        //wheelTransform.Translate(0,0,-FindWheelTranslation(),Space.Self);
    }

    #endregion
    
}
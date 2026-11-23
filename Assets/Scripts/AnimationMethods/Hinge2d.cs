using UnityEngine;

[System.Serializable]
public class Hinge2d
{
    public float maxAngle;
    public float minAngle;
    private float currentAngle = 0.0f;

    public Hinge2d(float maxAngle, float minAngle) { 
    
     this.maxAngle = maxAngle;
     this.minAngle = minAngle;
    }

    public float AngleConstraint(float angle) {

        if (angle < minAngle) return minAngle;
        if(angle > maxAngle) return maxAngle;

        return angle;
        
    }

    public float GetRotation(float targetAngle) { 
    
        float constrainedAngle= AngleConstraint(targetAngle);
        float deltaAngle = constrainedAngle - currentAngle;
        currentAngle = constrainedAngle;
        return deltaAngle;
    }


}

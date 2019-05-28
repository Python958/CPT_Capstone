using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_StaticFunctions : MonoBehaviour
{
    //first-order intercept using absolute target position
    public static Vector3 LeadTargetPosition(Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
    {
        Vector3 targetRelativePosition = targetPosition - shooterPosition;
        Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;

        float time = LeadTargetTime(shotSpeed, targetRelativePosition, targetRelativeVelocity);
        return (targetPosition + time * (targetRelativeVelocity));
    }

    //first-order intercept using relative target position
    public static float LeadTargetTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f) { return (0f); }

        float a = velocitySquared - shotSpeed * shotSpeed;

        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude / ( 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition) );
            return Mathf.Max(t, 0f);
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        var rightTime = 0f;
        if (determinant > 0f)
        {
            var square = Mathf.Sqrt(determinant);
            float time1 = (-b + square) / (2f * a);
            float time2 = (-b - square) / (2f * a);

            rightTime = (time1 > 0f ? (time2 > 0f ? Mathf.Min(time1, time2) : time1) : Mathf.Max(time2, 0f));
        }

        return (rightTime);
    }

    //logistic function
    public static float LogisticFunction(float value)
    {

        var returnValue = 0f;

        var a = 1f;
        var c = 1f;
        var k = 7f;
        var b = Mathf.Exp(-k);

        returnValue = c / (1 + Mathf.Pow(a * b, value-.5f));

        return (returnValue);

    }// 0 to 1 or else expect close to 1 or zero
}

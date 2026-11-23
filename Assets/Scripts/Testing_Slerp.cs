using UnityEngine;

public class Testing_Slerp : MonoBehaviour
{

    public Transform objetoA;

    private float time = 0.0f;
    public float dt = 0.02f;

    public float angleRotation = Mathf.PI / 2;
    public Vector3 RotationAxis = new Vector3(0.0f, 0.0f, 1.0f);

    MyQuaternion initialQuaternion;

    Vector3 initialPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialQuaternion = MyQuaternion.IdentityQuaternion();
        initialPosition = objetoA.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if (time < 1.0f)
        {
            //MyQuaternion currentQuaternion = Interpolation.SLERP(initialQuaternion, finalQuaternion, time);
            MyQuaternion currentQuaternion = Interpolation.SLERP(initialQuaternion, angleRotation, RotationAxis, time);
            Vector3 pA = Rotation3D.QuaternionRot(initialPosition, currentQuaternion);

            objetoA.position = pA;

            time = Mathf.Min(time + dt, 1.0f);


        }
        
        /* Vector3 pA = Rotation3D.QuaternionRot(initialPosition, finalQuaternion);
        objetoA.position = pA;
        */
    }

}
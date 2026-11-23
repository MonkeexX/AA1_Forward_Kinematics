using UnityEngine;

public class Simulation : MonoBehaviour
{

    public Transform objectA;
    public Transform objectB;
    public Transform objectC;
    public Transform objectD;



    public float time = 0.0f;
    public float dt = 0.1f;

    private Vector2 initialPositionA;
    private Vector2 initialPositionB;
    private Vector2 initialPositionC;
    private Vector2 initialPositionD;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        initialPositionA = objectA.position;
        initialPositionB = objectB.position;
        initialPositionC = objectC.position;
        initialPositionD = objectD.position;

    }

    // Update is called once per frame
    void Update()
    {
        float angle = time;

        Vector2 pA = Rotation2D.MatrixRot(initialPositionA, angle);
        Vector2 pB = Rotation2D.ComplexRot(initialPositionB, angle);
        Vector3 pC = Rotation3D.MatrixRot(initialPositionC, angle, "z");

        Vector3 axis = new Vector3(0.0f, 0.0f, 1.0f);
        Vector3 pD = Rotation3D.QuaternionRot(initialPositionD, angle, axis);

        objectA.position = new Vector3(pA.x, pA.y, 0);
        objectB.position = new Vector3(pB.x, pB.y, 0);
        objectC.position = pC;
        objectD.position = pD;

        time += dt;
        
    } 
}

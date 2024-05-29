using UnityEngine;

public class platformMovement : MonoBehaviour
{

    float dirY;
    float speed = 3f;
    
    SliderJoint2D sliderJoin;
    JointMotor2D motorTop = new JointMotor2D { motorSpeed = -2, maxMotorTorque = 10000 };
    JointMotor2D motorDawn = new JointMotor2D { motorSpeed = 2, maxMotorTorque = 10000 };

    [SerializeField] bool fallen = true;


    private void Start()
    {
        sliderJoin = GetComponent<SliderJoint2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < -19f)
        {
            sliderJoin.motor = motorTop;
        }
        else if (transform.position.y > -12.7f)
        {
            sliderJoin.motor = motorDawn;
        }
    }
}

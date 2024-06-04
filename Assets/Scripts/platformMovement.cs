using UnityEngine;

public class platformMovement : MonoBehaviour
{
    
    SliderJoint2D sliderJoin;
    JointMotor2D motorTop;
    //JointMotor2D motorDawn = new JointMotor2D { motorSpeed = 2, maxMotorTorque = 10000 };

    //[SerializeField] bool fallen = true;


    private void Start()
    {
        sliderJoin = GetComponent<SliderJoint2D>();
        motorTop = sliderJoin.motor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("υσι");
        if (collision.tag == "stopper")
        {
            motorTop.motorSpeed *= -1;
            sliderJoin.motor = motorTop;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}

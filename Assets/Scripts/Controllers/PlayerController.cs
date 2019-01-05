using UnityEngine;

[RequireComponent(typeof(AttackManager))]
[RequireComponent(typeof(MotionController))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public bool IsGrounded { get { return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y); } }

    private Animator an;
    private AttackManager am;
    private MotionController mc;
    private float jumpLimitSeconds = 0.2f;
    private float jumpLimitTimer = 0; 
    

    private void Awake()
    {
        an = GetComponent<Animator>();
        am = GetComponent<AttackManager>();
        mc = GetComponent<MotionController>();
    }
    private void LateUpdate()
    {
        mc.ApplyMovement();
        mc.ResetVelocity();

        if (jumpLimitTimer > 0) jumpLimitTimer -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        an.SetBool("Walking", false);
        if (IsGrounded)
        {
            an.SetBool("Jumping", false);
            Debug.Log("Ground!");
        }
    }

    /// <summary>
    /// Checks a <see cref="InputData"/> struct and determines what actions to make based on the data contained.
    /// </summary>
    /// <param name="data"></param>
    public void ReadInput(InputData data)
    {
        if (data.buttons[0]) am.LightAttack();
        if (data.buttons[1]) am.Throw();
        
        // Jump
        if (data.axes[0] > 0.5 && IsGrounded && jumpLimitTimer <= 0)
        {
            jumpLimitTimer = jumpLimitSeconds;
            mc.UpdateVelocity(new Vector3(mc.XVel, mc.Motion.JumpHeight, 0));
            an.SetBool("Jumping", true);
        }
        // Walk
        if (data.axes[1] !=  0)
        {            
            mc.UpdateVelocity(
                new Vector3(
                    data.axes[1] * mc.Motion.Speed,
                    mc.YVel, 0));
            an.SetBool("Walking", true);
        }
    }

   
}
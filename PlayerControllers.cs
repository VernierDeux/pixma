using UnityEngine;
using System.Collections;

public class PlayerControllers : MonoBehaviour {

    public float speed = 6.0F;
    public float jumpValue = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    void Update()
    {
        ///  CharacterController >> GameObject
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)///ground?
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //Debug.Log("Input.GetAxis = " + moveDirection);
            /// local coord >> world space
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= speed;
            if (Input.GetButton("Jump"))///space!
                /// Y vect
                moveDirection.y = jumpValue;

        }
        /// min grav
        //Debug.Log("moveDirection.y = " + moveDirection.y + " / gravity * Time.deltaTime = " + gravity * Time.deltaTime);
        moveDirection.y -= gravity * Time.deltaTime;

        controller.Move(moveDirection * Time.deltaTime);
    }


}

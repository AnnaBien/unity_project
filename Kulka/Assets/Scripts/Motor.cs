using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Poruszanie sie kulki, boost, joystick*/

public class Motor : MonoBehaviour {

    public float moveSpeed = 5.0f;
    public float drag = 0.5f; //flor effecting
    public float terminalRotationSpeed = 25.0f;
    public VirtualJoystick moveJoystick;
    

    public float boostSpeed = 5.0f;
    public float boostCooldown = 2.0f;          //mozesz boostowac co 2 sekundy
    private float lastBoost;


    private Rigidbody controller;
    private Transform camTransform;

    private static Motor instance;
    public static Motor Instance { get { return instance; } }

    public bool movement;

    private void Start() {
    lastBoost = Time.time - boostCooldown;

        instance = this;
        movement = false;

        controller = GetComponent<Rigidbody>();
        controller.maxAngularVelocity = terminalRotationSpeed;
        controller.drag = drag;

        camTransform = Camera.main.transform;
    }

    private void Update() {
        //------------------------------------------------------------------Keyboard input
        Vector3 dir = Vector3.zero;     //Kierunek poruszania sie

        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (dir.magnitude > 1)          //nie przyspieszaj
            dir.Normalize();
        //---------------------------------------------------------

        if (moveJoystick.InputDirection != Vector3.zero) {//jezeli jest ruch na joysticku
            dir = moveJoystick.InputDirection;
            movement = true;
        }
        else
            movement = false;

        //Obrot kamery w odpowiednim kierunku
        Vector3 rotatedDir = camTransform.TransformDirection(dir);
        rotatedDir = new Vector3(rotatedDir.x, 0, rotatedDir.z);  //wyzerowanie osi y
        rotatedDir = rotatedDir.normalized * dir.magnitude;

        controller.AddForce(rotatedDir * moveSpeed);
    }

    public void Boost() {
        if (Time.time - lastBoost > boostCooldown) {
            lastBoost = Time.time;
            controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
            
        }
    }

    public void BoostTile() {
        controller.AddForce(controller.velocity.normalized * boostSpeed, ForceMode.VelocityChange);
        controller.AddForceAtPosition(new Vector3(0, 10, 0), controller.transform.position, ForceMode.VelocityChange);
    }
}

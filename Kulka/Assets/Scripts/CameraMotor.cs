using System.Collections;
using UnityEngine;

/*Kamera podaza za playerem*/

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;        //kulka

    private Vector3 offset;
    private float distance = 5.0f;
    private float yOffset = 3.5f;

    private Vector3 desiredPosition;
    private float smoothSpeed = 7.5f;

    private Vector2 touchPosition;
    private float swipeResistance = 400.0f;
    //public VirtualJoystick moveJoystick;

    private void Start() {
        offset = new Vector3(0, yOffset, -1f * distance);
    }

    private void Update() {
   
        /*Funkcjonalność zastąpiona przyciskami do rotacji kamery------------------------------
        if (Input.GetKeyDown(KeyCode.LeftArrow))    //wywolywanie obrotow kamery
            SlideCamera(true);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            SlideCamera(false);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            touchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
            float swipeForce = touchPosition.x - Input.mousePosition.x;         //swiping left-right
            //Debug.Log(moveJoystick.InputDirection);
            if(Mathf.Abs(swipeForce) > swipeResistance && Motor.Instance.movement == false) {       //movement - nie zmieniać kamery jeśli był ruszany joystick
                if (swipeForce < 0) 
                    SlideCamera(true); //przesuwam w prawo --> obrot w lewo         
                else
                    SlideCamera(false);
            }
        }--------------------------------------------------------------------------------*/
    }

    private void FixedUpdate() {        //Szybszy Update
        desiredPosition = lookAt.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(lookAt.position + Vector3.up);
    }

    public void SlideCamera(bool left) {       //jesli nie w lewo to defaultowo w prawo
        if (left)                              //obroty kamery
            offset = Quaternion.Euler(0, -90, 0) * offset;       //obrot w lewo
        else
            offset = Quaternion.Euler(0, 90, 0) * offset;      //obrot w prawo
    }
}

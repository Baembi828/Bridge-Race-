using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform jsback;
    RectTransform jsfront;
    public Animator animator;

    Transform cube;
    public float speed = 5f;
    float radius;
    float inputMagnitude;

    Vector3 vecmove;
    bool touch = false;

    public void Start()
    {
        jsback = transform.Find("JoystickBack").GetComponent<RectTransform>();
        jsfront = transform.Find("JoystickBack/Joystick").GetComponent<RectTransform>();
        cube = GameObject.Find("stickman").transform;
        radius = jsback.rect.width * 0.5f;
    }

    public void Update()
    {
        if (touch)
        {
            cube.position += vecmove;
            animator.SetFloat("Input Magnitude", 1);
        }
    }

    void OnTouch(Vector2 vectouch)
    {
        Vector2 vec = new Vector2(vectouch.x - jsback.position.x, vectouch.y - jsback.position.y);

        vec = Vector2.ClampMagnitude(vec, radius);
        jsfront.localPosition = vec;

        float fsqr = (jsback.position - jsfront.position).sqrMagnitude / (radius * radius);
        Vector2 vecNormal = vec.normalized;
        vecmove = new Vector3(vecNormal.x * speed * Time.deltaTime * fsqr, 0f, vecNormal.y * speed * Time.deltaTime * fsqr);
        cube.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        touch = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnTouch(eventData.position);
        touch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        jsfront.localPosition = Vector3.zero;
        touch = false;
        animator.SetFloat("Input Magnitude", 0);
    }
}

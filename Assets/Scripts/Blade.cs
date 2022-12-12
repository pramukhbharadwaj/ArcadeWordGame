using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;
    private Collider bladeCollider;
    private TrailRenderer bladeTrail;
    private bool slicing;

    public float minSliceVelocity = 0.01f;

    public Vector3 direction {get; private set;}
    public float sliceForce = 5f;

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        if (SceneManager.GetActiveScene().name != "Level6" || SceneManager.GetActiveScene().name != "Level6-Tutorial")
            bladeCollider.enabled = false;
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        
    }

    private void onEnable()
    {
        if (SceneManager.GetActiveScene().name != "Level6" || SceneManager.GetActiveScene().name != "Level6-Tutorial")
            StopSlicing();
        else
            StartSlicing();

    }

    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name != "Level6" || SceneManager.GetActiveScene().name != "Level6-Tutorial")
            StopSlicing();
    }

    // Update is called once per frame
    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level6" || SceneManager.GetActiveScene().name != "Level6-Tutorial")
        {
            if(Input.GetMouseButtonDown(0)) {
                StartSlicing();
            }
            else if(Input.GetMouseButtonUp(0)) {
                StopSlicing();
            }
            else if(slicing)
            {
                ContinueSlicing();
            }
        }
        else
        {
            ContinueSlicing();
        }
    }
    
    public void EnableBladeTrail()
    {
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        bladeTrail.enabled = true;
    }

    public void DisableBladeTrail()
    {
        bladeTrail = GetComponentInChildren<TrailRenderer>();
        bladeTrail.enabled = false;
        bladeTrail.Clear();
    }

    private void StartSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z * -1));

        transform.position = newPosition;

        slicing = true;

        if (SceneManager.GetActiveScene().name == "Level6" || SceneManager.GetActiveScene().name == "Level6-Tutorial")
            bladeCollider.enabled = true;

        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.transform.position.z * -1));
        direction = newPosition-transform.position;

        if (SceneManager.GetActiveScene().name != "Level6" || SceneManager.GetActiveScene().name != "Level6-Tutorial")
        {
            float velocity = direction.magnitude/Time.deltaTime;
            bladeCollider.enabled = velocity > minSliceVelocity;
        }

        transform.position = newPosition;
    }
}

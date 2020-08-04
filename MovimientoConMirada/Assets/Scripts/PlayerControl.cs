using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour
{

    public LayerMask teleportLayer;

    private TeleportControl _currentPoint = null;
    private TeleportControl _oldPoint = null;
    public GameObject _lastHited;


    // Update is called once per frame
    void Update()
    {
        RaycastHit _hit;


        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out _hit, Mathf.Infinity, teleportLayer))
        {
            _hit.collider.GetComponentInParent<TeleportControl>().LoadingTeleport(this);
            Debug.DrawLine(Camera.main.transform.position, _hit.point, Color.green); 
            _lastHited = _hit.collider.gameObject;
        }
        else
        {
            if(_lastHited != null)
            {
                //_lastHited.GetComponentInParent<TeleportControl>().ReloadTeleport(this);
                _lastHited.transform.parent.GetComponent<TeleportControl>().ReloadTeleport(this);
            }
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 100, Color.red);
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void SetCurrentPoint(TeleportControl point)
    {

        if (_currentPoint != null)
        {
            _oldPoint = _currentPoint;
            _oldPoint.gameObject.SetActive(true);
            _oldPoint.RestoreValues();
        }
        _currentPoint = point;

    }
}

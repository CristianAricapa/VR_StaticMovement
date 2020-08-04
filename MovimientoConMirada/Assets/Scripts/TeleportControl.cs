using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Enums; // Me permite NO escribir ENUMS antes de TYPEOFTELEPORT


[RequireComponent(typeof(LookToCam))]
public class TeleportControl : MonoBehaviour
{

    public typeOfTeleport type;



    private GameObject[] _childs;
    private float _counter, _reloadCounter;
    private Material _planeMaterial;
    private Image _backgroundImage;

    private Quaternion _originalRotation;
    private bool _isAwake = false;

    private void Awake()
    {
        _originalRotation = transform.rotation;

        Init();

        _isAwake = true;
    }
    private void Start()
    {
        _planeMaterial = _childs[(int)typeOfTeleport.Ring].GetComponentInChildren<MeshRenderer>().material;
        _backgroundImage = _childs[(int)typeOfTeleport.Tread].transform.GetChild(0).GetComponent<Image>();

        _backgroundImage.fillAmount = 1 - _counter;
        _planeMaterial.SetFloat("_Cutoff", _counter);

    }



    private void Init()
    {
        _counter = Constants.MIN_CUTOFF_VALUE;


        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        _childs = new GameObject[transform.childCount];

        _childs[(int)typeOfTeleport.Ring] = GetComponentInChildren<MeshRenderer>().gameObject;    
        _childs[(int)typeOfTeleport.Tread] = GetComponentInChildren<Canvas>().gameObject;         

        _childs[(int)typeOfTeleport.Ring].SetActive(type == typeOfTeleport.Ring);           
        _childs[(int)typeOfTeleport.Tread].SetActive(type == typeOfTeleport.Tread);          

        GetComponent<LookToCam>().enabled = (type == typeOfTeleport.Ring);                       


    }

    private void RestoreOrientation()
    {
        if (_isAwake)
            transform.rotation = _originalRotation;

    }

    private void OnValidate()
    {
        Init();
        RestoreOrientation();
    }



    public void RestoreValues()
    {
        Init();
        transform.rotation = _originalRotation;

        _backgroundImage.fillAmount = 1 - _counter;
        _planeMaterial.SetFloat("_Cutoff", _counter);
    }

    public void LoadingTeleport(PlayerControl player)
    {
        _counter += Time.deltaTime / Constants.TIME_TO_TELEPORT;

        if (_counter > Constants.MAXIM_CUTOFF_VALUE)
        {
            _counter = Constants.MAXIM_CUTOFF_VALUE;
            player.transform.position = transform.position;
            player.transform.rotation = _originalRotation;

            player.SetCurrentPoint(this);
            player._lastHited = null;
            gameObject.SetActive(false);
        }

        _reloadCounter = _counter;

        switch (type)
        {
            case typeOfTeleport.Ring:
                _planeMaterial.SetFloat("_Cutoff", _counter);
                break;
            case typeOfTeleport.Tread:

                _backgroundImage.fillAmount = 1 - _counter;
                break;

        }
    }

    public void ReloadTeleport(PlayerControl player)
    {
        _reloadCounter -= Time.deltaTime / Constants.TIME_TO_TELEPORT;
        if (_reloadCounter < Constants.MIN_CUTOFF_VALUE)
        {
            _reloadCounter = Constants.MIN_CUTOFF_VALUE;
        }

        _counter = _reloadCounter;
        switch (type)
        {
            case typeOfTeleport.Ring:
                _planeMaterial.SetFloat("_Cutoff", _reloadCounter);
                break;
            case typeOfTeleport.Tread:
                _backgroundImage.fillAmount = 1 - _reloadCounter;
                break;
        }
    }
}

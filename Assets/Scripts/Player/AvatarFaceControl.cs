using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class AvatarFaceControl : MonoBehaviour
{
    public Material AvatarFace;

    [SerializeField] Texture _defaultTexture;
    
    bool _isDefault = true;


    #region >>>>>>> Hanjun added
    string _faceGoPath = "Space_Suit/Tpose_/Man_Suit/Face";
    GameObject _faceGo;
    Material _faceMat;

    PhotonView _view;
    #endregion

    private void Start() {

        _view = GetComponentInParent<PhotonView>();
        _faceGo = transform.Find(_faceGoPath).gameObject;
        _faceMat = _faceGo.GetComponent<Renderer>().material;
        // assign a copy of the material so we don't change the material attached to the prefab.
        _faceMat = new Material(_faceMat);
        _faceGo.GetComponent<Renderer>().material = _faceMat;
        

        if (_faceMat is null)
        {
            Debug.LogError("AvatarFaceControl/ Cannot fetch face material");
            return;
        }
        // from now on AvatarFace references the copied material
        AvatarFace = _faceMat;
        AvatarFace.SetTexture("_MainTex", _defaultTexture); 
    }



    private void Update()
    {
        if (!_view.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) InvokeShowFace(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) InvokeShowFace(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) InvokeShowFace(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) InvokeShowFace(3);
    }

    public void ChangeFace(int faceIndex) 
    {
        AvatarFace.SetTexture("_MainTex", AvatarFaceManagement.s_avatarTextureList[faceIndex]);

        if (AvatarFaceManagement.s_avatarTextureList[faceIndex].name.Equals("happy"))
            _isDefault = true;
        else
            _isDefault = false;
    }

    public void InvokeShowFace(int index)
    {
        int imageIndex = AvatarFaceManagement.s_favList[index].GetImageIndex();
        StartCoroutine(ShowFaceForSeconds(imageIndex));

        if (!_view.IsMine) return;
        SyncChangeFace(imageIndex);
    }

    #region >>>>>> Netcode
    void SyncChangeFace(int imageIndex)
    {
        // propagate change to other players
        Debug.Log("AvatarFaceControl/ syncing face image#" + imageIndex);
        _view.RPC("SetSyncedFace", RpcTarget.Others, imageIndex);
    }

    // positively DO NOT DELETE (although it has 0 references, it is called by rpc.)
    [PunRPC] 
    public void SetSyncedFace(int imageIndex, PhotonMessageInfo info)
    {
        Debug.Log($"AvatarFaceControl/ Start RPC face image#{imageIndex} by {info.Sender.NickName}");
        StartCoroutine(ShowFaceForSeconds(imageIndex));
    }
    #endregion

    IEnumerator ShowFaceForSeconds(int index)
    {
        if (!_isDefault) yield break;

        ChangeFace(index);

        yield return new WaitForSeconds(10f);

        ChangeFace(AvatarFaceManagement.DefaultIndex);
    }
}
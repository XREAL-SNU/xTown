using Photon.Pun;
using UnityEngine;



namespace StarterAssets
{
    public class ThirdPersonControllerMulti : ThirdPersonController
	{
		private PhotonView _view;

		protected sealed override void Awake()
		{
			
			if (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnected)
            {
				base.Awake();
				return;
			}

			_view = GetComponent<PhotonView>();
            if (_view.IsMine)
            {
				base.Awake();
			}
		}

		protected sealed override void Start()
		{
			if (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnected)
			{
				base.Start();
				return;
			}
			if (_view.IsMine)
			{
				base.Start();
			}
		}

		protected sealed override void Update()
		{
			if (!PhotonNetwork.InRoom || !PhotonNetwork.IsConnected)
			{
				base.Update();
				return;
			}
			if (_view.IsMine)
			{
				base.Update();
			}
		}

        private void OnDisable()
        {
			Debug.Log("Disabled controller, destroying character from scene.");
			Destroy(this.gameObject);
        }

    }
}
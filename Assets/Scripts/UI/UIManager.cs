using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XReal.XTown.UI {


    public class UIManager : MonoBehaviour
    {
        // singleton, and UIManager must persist across scenes. right? I guess...
        static UIManager _UIManager;
        public static UIManager UI
        {
            get => _UIManager;
        }
        void Awake()
        {
            if (_UIManager == null)
            {
                _UIManager = this;
            }
            else if (_UIManager != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        // sorting order
        int _order = 10;

        // references to the two types of UI.
        Stack<UIPopup> _popupStack = new Stack<UIPopup>();
        UIScene _sceneUI = null;

        // used for sorting order of UI.
        public GameObject Root
        {
            get
            {
                GameObject root = GameObject.Find("@UI_Root");
                if (root == null) root = new GameObject { name = "@UI_Root" };

                return root;
            }
        }

        // displaying UIcanvas 'go'.
        public void ShowCanvas(GameObject go, bool sort = true)
        {
            Canvas canvas = go.GetComponent<Canvas>();
            if(canvas is null)
            {
                Debug.LogError("SetCanvas's first argument requires component canvas but is missing!");
                return;
            }

            canvas.overrideSorting = true;
            // set the sorting order of UI canvas.
            if (sort)
            {
                canvas.sortingOrder = _order++;
            }
            else
            {
                canvas.sortingOrder = 0;
            }
        }

        // prefab UI instantiation. path: Resources/UI/(Scene or Popup)/name
        public string UIPathPrefix = "UI/";
        // SceneUI: full screen
        public T ShowSceneUI<T>(string name = null) where T: UIScene
        {
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
            GameObject go = Instantiate(Resources.Load<GameObject>(UIPathPrefix + $"Scene/{name}"));
            T sceneUI = go.GetComponent<T>();
            if(sceneUI is null)
            {
                Debug.LogError("UIManager/ failed to show canvas: check if script is attached");
                return null;
            }
            // set current sceneUI to active one.
            _sceneUI = sceneUI;
            // set hierarchy.
            go.transform.SetParent(Root.transform);
            return sceneUI;
        }

        // popup UIs. They need closing methods as well.
        public T ShowPopupUI<T>(string name = null) where T: UIPopup
        {
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;
            GameObject go = Instantiate(Resources.Load<GameObject>(UIPathPrefix + $"Popup/{name}"));
            T popup = go.GetComponent<T>();
            if(popup is null)
            {
                Debug.LogError("UIManager/ failed to open popup: check if script is attached.");
                return null;
            }
            Debug.Log($"show popup: {name}");

            // add active popup to stack
            _popupStack.Push(popup);
            // set hierarchy.
            go.transform.SetParent(Root.transform);
            return popup;
        }

        public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
        {
            if (string.IsNullOrEmpty(name)) name = typeof(T).Name;

            GameObject go = Instantiate(Resources.Load<GameObject>(UIPathPrefix + $"SubItem/{name}"));
            
            if (parent is null)
            {
                Debug.LogError("UIManager/ failed to open subitem: check if script is attached.");
                return null;
            }
            if (parent != null) go.transform.SetParent(parent);

            return UIUtils.GetOrAddComponent<T>(go);
        }

        public void ClosePopupUI()
        {
            if (_popupStack.Count == 0) return;
            UIPopup popup = _popupStack.Pop();
            Destroy(popup.gameObject);
            _order--;
        }


        // safety: specify popup to close
        public void ClosePopupUI(UIPopup popup)
        {
            if (_popupStack.Count == 0) return;
            if(_popupStack.Peek() != popup)
            {
                Debug.LogError("UIManager/ Close popup failed!");
                return;
            }
            ClosePopupUI();
        }


        // close all
        public void CloseAllPopupUI()
        {
            while (_popupStack.Count > 0) ClosePopupUI();
        }
    }

    
}
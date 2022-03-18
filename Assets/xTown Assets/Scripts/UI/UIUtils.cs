using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace XReal.XTown.UI
{
    public class UIUtils
    {
        /// <summary>
        /// utility for searching through UI element's hierarchy.
        /// </summary>
        
        // serach for component T(ex. Button, Image, Toggle..) in go with name UIname
        public static T FindUIChild<T>(GameObject go, string UIname = null, bool searchGrandChildren = false) where T : UnityEngine.Object
        {
            if (!searchGrandChildren)
            { // just the immediate children
                for (int i = 0; i < go.transform.childCount; ++i)
                {
                    Transform transform = go.transform.GetChild(i);
                    // if UIname is provided for search,
                    if (string.IsNullOrEmpty(UIname) || transform.name == UIname)
                    {
                        T component = transform.GetComponent<T>();
                        if (component != null) return component;
                    }
                }
            }
            else
            {
                foreach (T component in go.GetComponentsInChildren<T>())
                {
                    if (string.IsNullOrEmpty(UIname) || component.name == UIname)
                    {
                        return component;
                    }
                }
            }

            Debug.Log($"UIUtils/ FindUIChild failed : {UIname}");
            return null;
        }


        // search for gameObject, not attached component T of it.
        public static GameObject FindUIChild(GameObject go, string name = null, bool searchGrandChildren = false)
        {
            Transform transform = FindUIChild<Transform>(go, name, searchGrandChildren);
            if (transform == null) return null;
            return transform.gameObject;
        }

        public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
        {
            T component = go.GetComponent<T>();
            if (component == null)
                component = go.AddComponent<T>();
            return component;
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UIManagement
{
    public class UIConfig : MonoBehaviour
    {
        private static UIConfig instance;
        // 预制件路径字典
        private readonly Dictionary<string, string> pathDict;
        // 预制件字典
        private readonly Dictionary<string, GameObject> prefabDict;
        // 已加载界面字典
        public Dictionary<string, BasePanel> panelDict;
        private Transform uiRoot;
        // 挂载根节点
        public Transform UIRoot
        {
            get
            {
                if (uiRoot == null)
                {
                    uiRoot = GameObject.Find("Canvas")
                            ? GameObject.Find("Canvas").transform
                            : new GameObject("Canvas").transform;
                }
                return uiRoot;
            }
        }

        public static UIConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType(typeof(UIConfig)) as UIConfig;
                }
                return instance;
            }
        }

        private UIConfig()
        {
            // 初始化单例
            instance = this;
            // 初始化路径字典
            pathDict = new() {
                {UIConst.UserPanel, "Menu/UserPanel"},
                {UIConst.CreateUserPanel, "Menu/CreateUserPanel"},
            };
            // 初始化预制件字典
            prefabDict = new();
            // 初始化界面字典
            panelDict = new();
        }

        // 打开指定界面
        public BasePanel OpenPanel(string name)
        {
            // 判定界面是否已经打开
            if (panelDict.TryGetValue(name, out BasePanel panel))
            {
                Debug.LogWarning("界面已打开");
                return panel;
            }
            // 判定界面路径是否存在
            if (pathDict.TryGetValue(name, out string path))
            {
                Debug.LogError("界面不存在或未配置界面路径");
                return null;
            }
            // 加载预制件
            if (!prefabDict.TryGetValue(name, out GameObject panelPrefab))
            {
                string tgtPath = "Prefab/Panel/" + path;
                panelPrefab = Resources.Load(tgtPath) as GameObject;
                prefabDict.Add(name, panelPrefab);
            }
            // 打开界面并加入缓存
            GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
            panel = panelObject.GetComponent<BasePanel>();
            panel.OpenPanel(name);
            panelDict.Add(name, panel);
            return panel;
        }

        public bool ClosePanel(string name)
        {
            // 检测界面是否存在
            if (!panelDict.TryGetValue(name, out BasePanel panel))
            {
                Debug.LogError("界面未打开或界面不存在:" + name);
                return false;
            }
            // 关闭界面并删除缓存
            panel.ClosePanel();
            return true;
        }
    }

    public class UIConst
    {
        public const string UserPanel = "UserPanel";
        public const string CreateUserPanel = "CreateUserPanel";
    }
}

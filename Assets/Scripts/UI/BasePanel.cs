using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIManagement;

public class BasePanel : MonoBehaviour
{

    protected new string name;
    public string Name { get => name; }
    protected bool isRemove = false;
    public bool IsRemove { get => isRemove; }
    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }
    public virtual void ClosePanel()
    {
        isRemove = true;
        gameObject.SetActive(false);
        Destroy(gameObject);
        // 移除缓存
        if (UIConfig.Instance.panelDict.ContainsKey(name))
        {
            UIConfig.Instance.panelDict.Remove(name);
        }
    }
}

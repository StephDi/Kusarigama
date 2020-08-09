using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectKeyFragment : MonoBehaviour
{
    [SerializeField] private List<KeyFragment.KeyType> keyList;
    [SerializeField] private Image[] keys;
    [SerializeField] private Image SecretKeyFull;

    private void Awake()
    {
        keyList = new List<KeyFragment.KeyType>();
    }
    public void AddKey(KeyFragment.KeyType keyType)
    {
        keyList.Add(keyType);
        UpdateKeyFragmentUI();
    }

    public bool ContainsKey(KeyFragment.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter(Collider other)
    {
        KeyFragment key = other.GetComponent<KeyFragment>();
        if (key != null)
        {
            FindObjectOfType<AudioManager>().Play("ItemPickUp");
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }
    }

    private void UpdateKeyFragmentUI()
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            KeyFragment.KeyType keyType = keyList[i];
            switch (keyType)
            {
                default:
                case KeyFragment.KeyType.L1: keys[0].gameObject.SetActive(true); break;
                case KeyFragment.KeyType.L2: keys[1].gameObject.SetActive(true); break;
                case KeyFragment.KeyType.L3: keys[2].gameObject.SetActive(true); break;
                case KeyFragment.KeyType.R1: keys[3].gameObject.SetActive(true); break;
                case KeyFragment.KeyType.R2: keys[4].gameObject.SetActive(true); break;
                case KeyFragment.KeyType.R3: keys[5].gameObject.SetActive(true); break;
            }
        }
        if (keyList.Count >= 4)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i].gameObject.SetActive(false);
            }
            SecretKeyFull.gameObject.SetActive(true);
        }
    }
}

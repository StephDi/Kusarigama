using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFragment : MonoBehaviour
{
    [SerializeField] private KeyType keyType;
    public enum KeyType
    {
        L1,L2,L3,R1,R2,R3
    }

    public KeyType GetKeyType()
    {
        return keyType;
    }
}

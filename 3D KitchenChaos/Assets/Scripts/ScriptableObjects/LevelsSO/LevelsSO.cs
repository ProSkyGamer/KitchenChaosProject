using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelsSO : ScriptableObject
{
    public Loader.Scene sceneToLoad;
    public LevelSettingSO levelSettings;
}

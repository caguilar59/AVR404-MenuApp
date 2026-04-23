using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq;

[System.Serializable]
public class InteractionModeDictionary : SerializableDictionaryBase<string, GameObject> { }

public class InteractionController : Singleton<InteractionController>
{
    [SerializeField] private InteractionModeDictionary interactionModes;
    [SerializeField] private string initialMode = "Title";
    [SerializeField] private bool alwaysStartAtTitle = true;

    private GameObject currentMode;

    protected override void Awake()
    {
        base.Awake();
        ResetAllModes();
    }

    private void ResetAllModes()
    {
        foreach(GameObject mode in interactionModes.Values)
        {
            mode.SetActive(false);
        }
    }

    public static void EnableMode(string name)
    {
        Instance?._EnableMode(name);
    }

    private void _EnableMode(string name)
    {
        GameObject modeObject;
        if (TryGetModeObject(name, out modeObject))
        {
            StartCoroutine(ChangeMode(modeObject));
        }
        else
        {
            Debug.LogError("undefined mode named " + name);
        }

    }

    private bool TryGetModeObject(string name, out GameObject modeObject)
    {
        if (interactionModes.TryGetValue(name, out modeObject))
            return true;

        string withSuffix = name.EndsWith(" Mode") ? name : $"{name} Mode";
        if (interactionModes.TryGetValue(withSuffix, out modeObject))
            return true;

        string withoutSuffix = name.EndsWith(" Mode") ? name.Substring(0, name.Length - " Mode".Length) : name;
        if (interactionModes.TryGetValue(withoutSuffix, out modeObject))
            return true;

        modeObject = interactionModes
            .FirstOrDefault(pair => string.Equals(pair.Key, name, System.StringComparison.OrdinalIgnoreCase)).Value;
        if (modeObject != null)
            return true;

        modeObject = interactionModes
            .FirstOrDefault(pair => string.Equals(pair.Key, withSuffix, System.StringComparison.OrdinalIgnoreCase)).Value;
        return modeObject != null;
    }

    private IEnumerator ChangeMode(GameObject mode)
    {
        if (mode == currentMode)
            yield break;
        if (currentMode)
        {
            currentMode.SetActive(false);
            yield return null;
        }

        currentMode = mode;
        mode.SetActive(true);
    }
    private void Start()
    {
        if (alwaysStartAtTitle)
        {
            if (TryGetModeObject("Title", out GameObject titleModeObject))
            {
                StartCoroutine(ChangeMode(titleModeObject));
                return;
            }

            if (TryGetModeObject("Title Mode", out titleModeObject))
            {
                StartCoroutine(ChangeMode(titleModeObject));
                return;
            }
        }

        if (TryGetModeObject(initialMode, out GameObject initialModeObject))
        {
            StartCoroutine(ChangeMode(initialModeObject));
            return;
        }

        if (TryGetModeObject("Title", out GameObject fallbackTitleModeObject))
        {
            StartCoroutine(ChangeMode(fallbackTitleModeObject));
            return;
        }

        if (TryGetModeObject("Title Mode", out GameObject fallbackTitleModeObjectWithSuffix))
        {
            StartCoroutine(ChangeMode(fallbackTitleModeObjectWithSuffix));
            return;
        }

        Debug.LogError("No startup interaction mode was found.");
    }
}

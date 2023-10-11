using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchAndDropdown : MonoBehaviour
{
    public GameObject starDetailCanvas;
    public InputField searchInputField;
    public Dropdown resultsDropdown;
    // private bool shouldUpdateInputField = false;

    private void Start()
    {
        searchInputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        resultsDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnInputFieldEndEdit(string searchText)
    {
        resultsDropdown.ClearOptions();

        if (string.IsNullOrEmpty(searchText))
        {
            resultsDropdown.Hide();
            return;
        }

        List<string> matchingNames = new List<string>();

        foreach (Transform child in transform)
        {
            if (child.name.Contains(searchText))
            {
                matchingNames.Add(child.name);
            }
        }

        if (matchingNames.Count > 0)
        {
            resultsDropdown.AddOptions(matchingNames);
            resultsDropdown.value = -1;
            resultsDropdown.Show();
        }
        else
        {
            resultsDropdown.Hide();
        }
    }


    public void OnSearchTextChanged(string searchText)
    {
        resultsDropdown.ClearOptions();
        // shouldUpdateInputField = false;

        if (string.IsNullOrEmpty(searchText))
        {
            resultsDropdown.Hide();
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(searchInputField.gameObject);
            return;
        }

        List<string> matchingNames = new List<string>();

        foreach (Transform child in transform)
        {
            if (child.name.Contains(searchText))
            {
                matchingNames.Add(child.name);
            }
        }

        if (matchingNames.Count > 0)
        {
            resultsDropdown.AddOptions(matchingNames);
            resultsDropdown.value = -1;  // This prevents auto-selecting the first entry
            resultsDropdown.Show();
            // shouldUpdateInputField = true;
        }
        else
        {
            resultsDropdown.Hide();
        }
    }

    public void OnDropdownValueChanged(int index)
    {
        // if (shouldUpdateInputField)
        // {
        //     searchInputField.text = resultsDropdown.options[index].text;
        // }
        searchInputField.text = resultsDropdown.options[index].text;
        resultsDropdown.Hide();
        //StartCoroutine(ReactivateInputField());
    }

    private IEnumerator ReactivateInputField()
    {
        yield return new WaitForSeconds(0.2f);
        searchInputField.ActivateInputField();
    }

    public void OnDropdownItemClicked()
    {
        searchInputField.text = resultsDropdown.options[resultsDropdown.value].text;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(searchInputField.gameObject);
    }

    public void OnSearchButtonPressed()
    {
        string searchText = searchInputField.text.Trim();
        if (string.IsNullOrEmpty(searchText))
        {
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.name == searchText)
            {
                StarDetailComponent starDetailComponent = child.GetComponent<StarDetailComponent>();
                if (starDetailComponent != null)
                {
                    starDetailComponent.UITest();
                    UIOpen();
                    return;
                }
            }
        }

        Debug.LogWarning("No matching child object found!");
    }

    private void UIOpen()
    {
        NewStartTransform.Instance.AllControl = false;
        starDetailCanvas.SetActive(true);
    }
}

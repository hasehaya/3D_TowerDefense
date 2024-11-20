using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class FacilitySelectPresenter :MonoBehaviour
{
    [SerializeField] Transform availableFacilityParent;
    [SerializeField] Transform selectedFacilityParent;
    [SerializeField] GameObject facilityCellPrefab;
    [SerializeField] GameObject popupObj;
    [SerializeField] Button playBtn;
    [SerializeField] Button backBtn;
    SceneLoader.SceneName selectedSceneName;

    private List<FacilityParameter> selectedFacilities = new List<FacilityParameter>();

    private void Start()
    {
        var availableFacilities = ScriptableObjectManager.Instance.GetFacilityParameterArray();
        foreach (var facility in availableFacilities)
        {
            var viewObj = Instantiate(facilityCellPrefab, availableFacilityParent);
            var view = viewObj.GetComponent<FacilityCellView>();
            view.SetIcon(facility.icon);
            view.SetButtonAction(() => OnClickFacilityButton(facility, viewObj));
        }

        popupObj.SetActive(false);

        backBtn.onClick.AddListener(() => popupObj.SetActive(false));
        playBtn.onClick.AddListener(() => StartCoroutine(SceneLoader.Instance.LoadScene(selectedSceneName)));
    }

    public void Initialize(SceneLoader.SceneName sceneName)
    {
        selectedSceneName = sceneName;
        popupObj.SetActive(true);
    }

    void OnClickFacilityButton(FacilityParameter facility, GameObject viewObj)
    {
        if (viewObj.transform.parent == availableFacilityParent)
        {
            if (selectedFacilities.Count < 5)
            {
                viewObj.transform.SetParent(selectedFacilityParent);
                selectedFacilities.Add(facility);
            }
            else
            {
                Debug.Log("選択可能な施設は5つまでです。");
            }
        }
        else if (viewObj.transform.parent == selectedFacilityParent)
        {
            viewObj.transform.SetParent(availableFacilityParent);
            selectedFacilities.Remove(facility);
        }
    }
}

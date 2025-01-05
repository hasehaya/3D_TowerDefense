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
    Facility.Type mustFacility;

    private List<FacilityParameter> selectedFacilities = new List<FacilityParameter>();
    private Dictionary<FacilityParameter, GameObject> facilityToViewMap = new Dictionary<FacilityParameter, GameObject>();

    private void Start()
    {
        SetAvailableFacilityList();

        popupObj.SetActive(false);

        backBtn.onClick.AddListener(() => popupObj.SetActive(false));
        playBtn.onClick.AddListener(LoadStage);
    }

    void SetAvailableFacilityList()
    {
        var availableFacilities = SaveDataManager.Instance.SaveData.GetAvailableFacilityTypeList();
        var facilityies = ScriptableObjectManager.Instance.GetFacilityParameterArray();
        foreach (var facility in facilityies)
        {
            if (!availableFacilities.Contains(facility.type))
            {
                continue;
            }
            var viewObj = Instantiate(facilityCellPrefab, availableFacilityParent);
            var view = viewObj.GetComponent<FacilityCellView>();
            view.SetIcon(facility.icon);
            view.SetButtonAction(() => OnClickFacilityButton(facility, viewObj));

            facilityToViewMap.Add(facility, viewObj);
        }
    }

    void LoadStage()
    {
        SharedSceneData.AvailableFacilityTypes = selectedFacilities.ConvertAll(facility => facility.type);
        StartCoroutine(SceneLoader.Instance.LoadScene(selectedSceneName));
    }

    public void Initialize(StageData stageData)
    {
        selectedSceneName = stageData.sceneName;
        mustFacility = stageData.mustFacilityType;

        for (int i = selectedFacilityParent.childCount - 1; i >= 0; i--)
        {
            var child = selectedFacilityParent.GetChild(i);
            child.SetParent(availableFacilityParent);
        }

        selectedFacilities.Clear();

        FacilityParameter mustFacilityParameter = null;

        foreach (var facilityParam in facilityToViewMap.Keys)
        {
            if (facilityParam.type == mustFacility)
            {
                mustFacilityParameter = facilityParam;
                break;
            }
        }

        if (mustFacilityParameter != null)
        {
            var mustFacilityViewObj = facilityToViewMap[mustFacilityParameter];

            mustFacilityViewObj.transform.SetParent(selectedFacilityParent);
            selectedFacilities.Add(mustFacilityParameter);

            var view = mustFacilityViewObj.GetComponent<FacilityCellView>();
            view.SetInteractable(false);
        }

        popupObj.SetActive(true);
    }

    void OnClickFacilityButton(FacilityParameter facility, GameObject viewObj)
    {
        if (facility.type == mustFacility)
        {
            return;
        }

        if (viewObj.transform.parent == availableFacilityParent)
        {
            if (selectedFacilities.Count < 5)
            {
                viewObj.transform.SetParent(selectedFacilityParent);
                selectedFacilities.Add(facility);
            }
        }
        else if (viewObj.transform.parent == selectedFacilityParent)
        {
            viewObj.transform.SetParent(availableFacilityParent);
            selectedFacilities.Remove(facility);
        }
    }
}

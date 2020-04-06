using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FarmerEventManager : MonoBehaviour
{
    public GameObject[] farmerEventPosTab;
    public GameObject[] farmersForEventPrefabsTab;
    public float timeToFarmerEvent;
    public List<GameObject> farmersFromEventTab = new List<GameObject>();
    public TextMeshProUGUI rewardText;
    public GameObject farmerEventPanelCanv;
    public TutorialManager tutorialManager;

    public Button acceptBttn, revardBttn;

    public GameObject farmerEventWindow;
    // Start is called before the first frame update
    void Start()
    { 
        tutorialManager = FindObjectOfType<TutorialManager>();
        acceptBttn.onClick.AddListener(()=> 
        {
            CoreManager.Instance.audioManager.PlaySound(CoreManager.Instance.audioManager.audioClipsTab[17]);
            AcceptBttn();
        });
        revardBttn.onClick.AddListener(()=> {

            RewardAcceptBttn(); 
        
        });

    }

    public void StartFarmerEvent()
    {
        if (!CoreManager.Instance.tutorial)
        {
            Debug.Log("farmer");
            ResetFarmer(); 
        }
    }

    public void StartNewFarmerEventAfterAfterGameStart(float time)
    {
        Sequence startNewFarmEventAfterGameStart = DOTween.Sequence();

        startNewFarmEventAfterGameStart.AppendInterval(time).AppendCallback(() =>
        {         
            farmerEventPosTab = GameObject.FindGameObjectsWithTag("FarmerEvent");
            int i = Random.Range(0, farmerEventPosTab.Length);
            int j = Random.Range(0, farmersForEventPrefabsTab.Length);
            farmersFromEventTab[0] = Instantiate(farmersForEventPrefabsTab[j], farmerEventPosTab[i].transform.position, Quaternion.Euler(0, 0, -90), parent: farmerEventPosTab[i].transform);
            EventArrowsManager.instance.InstantiateArrow(farmerEventPosTab[i].transform.position, EventArrowsManager.instance.farmerEventArrowPrefab, EventArrowsManager.instance.farmerEventArrowList);
        });
    }

    private void SetNewFarmerEvent()
    {
        farmerEventPosTab = GameObject.FindGameObjectsWithTag("FarmerEvent");

        Sequence setNewFarmerEvent = DOTween.Sequence();

        setNewFarmerEvent.AppendInterval(timeToFarmerEvent)
            .AppendCallback(()=> {
                int i = Random.Range(0, farmerEventPosTab.Length);
                int j = Random.Range(0, farmersForEventPrefabsTab.Length);
                farmersFromEventTab[0] = Instantiate(farmersForEventPrefabsTab[j], farmerEventPosTab[i].transform.position, Quaternion.Euler(0, 0, -90), parent: farmerEventPosTab[i].transform);
                EventArrowsManager.instance.InstantiateArrow(farmerEventPosTab[i].transform.position, EventArrowsManager.instance.farmerEventArrowPrefab, EventArrowsManager.instance.farmerEventArrowList);
            });

    }

    public void OpenFarmerCanvPanel()
    {
        revardBttn.gameObject.SetActive(AdsManager.Instance.IsReadyRewardedVideo());

       
        //rewardText.text = "Thanks for your help!\nTake it:  " + MoneyFormat.Default(earnedMoneyFromEvent) + " as a reward.";
      //  rewardText.text = MoneyFormat.Default(earnedMoneyFromEvent);

        farmerEventWindow.transform.DOScale(0f, 0f).OnComplete(() =>
        {
            farmerEventPanelCanv.SetActive(true);
            farmerEventWindow.transform.DOScale(1f, 0.3f);
        });
    }

    void AcceptBttn()
    {
        farmerEventPanelCanv.SetActive(false);

        if (CoreManager.Instance.missionsManager.missionsPanelUIList.Exists(x => x.missionType == ItemsMissions.MissionType.farmer))
        {
            CoreManager.Instance.missionsManager.AddPoint(CoreManager.Instance.missionsManager.missionsPanelUIList[CoreManager.Instance.missionsManager.missionsPanelUIList.FindIndex(x => x.missionType == ItemsMissions.MissionType.farmer)].missionId);
        }

        ResetFarmer();    

        CoreManager.Instance.idleManager.GameData.Player.Tokens += 1;
        CoreManager.Instance.fortuneWheelManager.fortuneWheelTokens.TokenDisplayRefresh();

    }



    void RewardAcceptBttn()
    {     
        AdsManager.Instance.ShowRewardedVideo("token", 2);
        farmerEventPanelCanv.SetActive(false);
        if (CoreManager.Instance.missionsManager.missionsPanelUIList.Exists(x => x.missionType == ItemsMissions.MissionType.farmer))
        {
            CoreManager.Instance.missionsManager.AddPoint(CoreManager.Instance.missionsManager.missionsPanelUIList[CoreManager.Instance.missionsManager.missionsPanelUIList.FindIndex(x => x.missionType == ItemsMissions.MissionType.farmer)].missionId);
        }
        ResetFarmer();
    }
    

   // private GameObject[] farmersLive
    public void ResetFarmer()
    {
        Sequence farmerEvSeq = DOTween.Sequence();
        Sequence farmSeq = DOTween.Sequence();

        farmSeq.AppendCallback(() =>
        {

            if(farmersFromEventTab.Count > 0)
            {
                for(int i = 0; i <farmersFromEventTab.Count; i++)
                {
                    farmersFromEventTab[i].gameObject.GetComponent<FarmerEventBounceItem>().KillSeq();
                    
                    Destroy(farmersFromEventTab[i].gameObject);
                }
                EventArrowsManager.instance.DestroyArrows(EventArrowsManager.instance.farmerEventArrowList);
            } 
        })
        .AppendInterval(0.01f)
        .AppendCallback(()=> {
            if (farmersFromEventTab[0] != null)
            {
                farmersFromEventTab[0].gameObject.GetComponent<FarmerEventBounceItem>().KillSeq();
                EventArrowsManager.instance.DestroyArrows(EventArrowsManager.instance.farmerEventArrowList);
                Destroy(farmersFromEventTab[0].gameObject);
            }

            if (farmersFromEventTab.Capacity > 1)
            {
                for(int i=0; i< farmersFromEventTab.Count; i++)
                {
                    farmersFromEventTab[i].gameObject.GetComponent<FarmerEventBounceItem>().KillSeq();
                    EventArrowsManager.instance.DestroyArrows(EventArrowsManager.instance.farmerEventArrowList);
                    Destroy(farmersFromEventTab[i].gameObject);
                }
            }

        }).AppendInterval(0.025f).OnComplete(() => {
            SetNewFarmerEvent();
        });             
    }
}

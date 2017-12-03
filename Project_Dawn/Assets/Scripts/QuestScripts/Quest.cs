using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum QuestObjective { KILL, GATHER, REACH, EXPLORE, UNDEF };
public class Quest : MonoBehaviour
{
  QuestObjective myObjective = QuestObjective.UNDEF;
  bool isUnlocked, isComplete, isPrimary;
  public string QuestName, QuestDescription;
  public GameObject myHUDEntry;
  protected Text titleText, descriptionText, progressText;
  protected float fadeValue = 1;
  protected int frameCounter = 0;
  bool deleteCoroutineStarted = false;

  public bool IsUnlocked
  {
    get
    {
      return isUnlocked;
    }

    set
    {
      isUnlocked = value;
    }
  }

  public bool IsComplete {
    get {
      return isComplete;
    }

    set {
      isComplete = value;
    }
  }

  public bool IsPrimary {
    get{
      return isPrimary;
    }

    set {
      isPrimary = value;
    }
  }

  public QuestObjective MyObjective{
    get {
      return myObjective;
    }

    protected set {
      myObjective = value;
    }
  }

  public virtual void UnlockQuest(){
    IsUnlocked = true;
    if (myHUDEntry == null){
      myHUDEntry = GameObject.Find("QuestHUD").GetComponent<QuestManager>().QuestEntryPrefab;
    }
    myHUDEntry = Instantiate(myHUDEntry, QuestManager.Instance.transform.Find("Column").transform, false);
    titleText = myHUDEntry.transform.Find("Title").GetComponent<Text>();
    descriptionText = myHUDEntry.transform.Find("Context/Description").GetComponent<Text>();
    progressText = myHUDEntry.transform.Find("Context/Progress").GetComponent<Text>();
    titleText.text = QuestName;
    descriptionText.text = QuestDescription;
  }

  public virtual void UpdateProgress(){
    
  }

  public virtual void Initialize() { }

  public void QuestComplete(){
    myHUDEntry.GetComponent<Image>().color = Color.yellow;
    descriptionText.fontStyle = FontStyle.Italic;
    descriptionText.text = "You have completed this objective!";
    progressText.text = "";
    isComplete = true;
  }

  public virtual void Update()
  {
    if (!IsUnlocked)
    {
      return;
    }

    if (isComplete && fadeValue >= 0)
    {
      myHUDEntry.GetComponent<CanvasGroup>().alpha = fadeValue;
      fadeValue -= Time.deltaTime;
    }
    if (isComplete && fadeValue < 0 && !deleteCoroutineStarted){
      frameCounter++;
      if (frameCounter >= 3){
        QuestManager.Instance.RemoveQuest(this);
        Destroy(myHUDEntry);
      }
    }
  }
}

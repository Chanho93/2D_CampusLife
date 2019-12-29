using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

namespace HJ.Quest
{
    public class QuestUI :  IQuestUI
    {
        public QuestStatus ChangeQuest(QuestStatus status)
        {
            return status;
        }

        public void HideQuest()
        {
            GameObject questbox = QuestManager.Instance.ui_QuestBox;

            if (questbox != null)
            {
                QuestManager.Instance.ui_QuestBox.SetActive(false);
            }
        }

        public void ShowQuest()
        {
            GameObject questbox = QuestManager.Instance.ui_QuestBox;

            if(questbox != null)
            {
                QuestManager.Instance.ui_QuestBox.SetActive(true);
            }
            
        }
    }
}


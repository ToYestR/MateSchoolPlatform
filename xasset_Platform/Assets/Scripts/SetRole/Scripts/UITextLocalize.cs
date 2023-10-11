using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SmipleLocalization
{
    public class UITextLocalize : MonoBehaviour
    {
        private Text mText;
        [SerializeField]
        private TextKey textKey;

        private void Awake()
        {
            this.mText = base.GetComponent<Text>();
        }

        private void Start()
        {
            LocalizationSys.ChangeCurLanguageTypeEvent += OnChangeLanguage;
            this.OnChangeLanguage();
        }

        private void OnDestroy()
        {
            LocalizationSys.ChangeCurLanguageTypeEvent -= OnChangeLanguage;
        }

        private void OnChangeLanguage()
        {
            this.StaticSet();
        }

        private void StaticSet()
        {
            this.mText.text = LocalizationSys.Localize(textKey);
        }
    }
}

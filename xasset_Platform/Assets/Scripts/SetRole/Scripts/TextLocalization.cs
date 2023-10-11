using System;
using System.Collections.Generic;
using UnityEngine;

namespace SmipleLocalization
{
    [Serializable]
    public class TextLocalization
    {
        [SerializeField]
        private string keyName;//当前文本类型名称，主要是方便编辑器上查看
        public TextKey curTextKey;//该文本类型
        public List<ItemData> itemDatas = new List<ItemData>();//当前文本类型对应的语言本地化文本信息

        //构造函数：初始化
        public TextLocalization(string _textKey, params string[] language)
        {
            this.keyName = _textKey;
            this.curTextKey = (TextKey)Enum.Parse(typeof(TextKey), _textKey);
            for (int i = 0; i < language.Length; i++)
            {
                ItemData cnData = new ItemData((LanguageType)i, language[i]);
                itemDatas.Add(cnData);
            }
        }

        //根据语言类型查找对应语言文本信息
        public ItemData GetType(LanguageType t)
        {
            for (int i = 0; i < this.itemDatas.Count; i++)
            {
                if (this.itemDatas[i].languageType == t)
                {
                    return this.itemDatas[i];
                }
            }
            throw new KeyNotFoundException("Language not found: " + t);
        }
    }
}

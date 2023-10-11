using UnityEngine;
using System;
using System.Collections.Generic;


namespace SmipleLocalization
{
    public class LocalizationData : ScriptableObject
    {
        public List<TextLocalization> localizationTxts = new List<TextLocalization>();

        /// <summary>
        /// 根据TextKey获取对应的所有语言类型信息
        /// </summary>
        /// <param name="k">需要本地化的文本</param>
        public TextLocalization GetTextInfo(TextKey k)
        {
            for (int i = 0; i < this.localizationTxts.Count; i++)
            {
                if (this.localizationTxts[i].curTextKey == k)
                {
                    return this.localizationTxts[i];
                }
            }
            //return null;
            throw new KeyNotFoundException("TextKey not found: " + k);
        }
    }

}

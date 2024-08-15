using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace RTLTMPro
{
    [ExecuteInEditMode]
    public class RTLTextMeshPro3D : TextMeshPro
    {
        // ReSharper disable once InconsistentNaming
#if TMP_VERSION_2_1_0_OR_NEWER
        public override string text
#else
        public new string text
#endif
        {
            get { return base.text; }
            set
            {
                if (originalText == value)
                    return;

                originalText = value;

                UpdateText();
            }
        }

        public string OriginalText
        {
            get { return originalText; }
        }

        public bool PreserveNumbers
        {
            get { return preserveNumbers; }
            set
            {
                if (preserveNumbers == value)
                    return;

                preserveNumbers = value;
                havePropertiesChanged = true;
            }
        }

        public bool Farsi
        {
            get { return farsi; }
            set
            {
                if (farsi == value)
                    return;

                farsi = value;
                havePropertiesChanged = true;
            }
        }

        public bool FixTags
        {
            get { return fixTags; }
            set
            {
                if (fixTags == value)
                    return;

                fixTags = value;
                havePropertiesChanged = true;
            }
        }

        private static bool ForceFix
        {
            get => LocalizationSettings.SelectedLocale != null && LocalizationSettings.SelectedLocale.Identifier.CultureInfo.TextInfo.IsRightToLeft;
        }

        [SerializeField] protected bool preserveNumbers;

        [SerializeField] protected bool farsi = true;

        [SerializeField] [TextArea(3, 10)] protected string originalText;

        [SerializeField] protected bool fixTags = true;

        [SerializeField] protected bool forceFix;

        protected readonly FastStringBuilder finalText = new FastStringBuilder(RTLSupport.DefaultBufferSize);

        protected string resultOfLastProcess = null;

        protected void Update()
        {
            if (havePropertiesChanged)
            {
                UpdateText();
            }
        }

        public void UpdateText()
        {
            if (originalText == null)
                originalText = "";

            if (!ForceFix)
            {
                isRightToLeftText = false;
                base.text = GetChunkFixedText(originalText);
            } 
            else if (originalText != resultOfLastProcess)  // If originalText == resultOfLastProcess, we're trying to process a string for a second time
            {
                isRightToLeftText = true;
                base.text = GetFixedText(originalText);

                resultOfLastProcess = base.text;            // Store the processed string
            }

            havePropertiesChanged = true;
        }

        private string GetChunkFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            FastStringBuilder recombinedString = new FastStringBuilder("");
            List<string> chunks = TextUtils.SplitLtrRtlChunks(input);

            FastStringBuilder arChunkFixer = new FastStringBuilder(RTLSupport.DefaultBufferSize);

            // Loop over chunks. Fix RTL and just append LTR
            for (int i = 0; i < chunks.Count; i++)
            {
                if (TextUtils.IsRTLInput(chunks[i]))
                {
                    arChunkFixer.Clear();

                    // Fix the Arabic block of text
                    RTLSupport.FixRTL(chunks[i], arChunkFixer, farsi, fixTags, preserveNumbers);

                    recombinedString += arChunkFixer.ToString();
                }
                else
                {
                    recombinedString += chunks[i]; // If LTR chunk, just append
                }
            }

            return recombinedString;
        }

        private string GetFixedText(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            finalText.Clear();
            RTLSupport.FixRTL(input, finalText, farsi, fixTags, preserveNumbers);
            finalText.Reverse();
            return finalText.ToString();
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace RTLTMPro
{
    [ExecuteInEditMode]
    public class RTLTextMeshPro : TextMeshProUGUI
    {
        [SerializeField] protected bool preserveNumbers = true;
        [SerializeField] protected bool farsi;
        [SerializeField][TextArea(3, 10)] protected string originalText;
        [SerializeField] protected bool fixTags = true;

        private bool isInputField;

        private string resultOfLastProcess = null;
        private readonly FastStringBuilder output = new(RTLSupport.DefaultBufferSize);

        private bool IsRightToLeftLocale => LocalizationSettings.SelectedLocale != null && LocalizationSettings.SelectedLocale.Identifier.CultureInfo.TextInfo.IsRightToLeft;

        protected override void Awake()
        {
            base.Awake();

            var inputField = GetComponentInParent<TMP_InputField>();
            isInputField = inputField != null && inputField.textComponent == this;
        }

        protected void Update()
        {
            if (!havePropertiesChanged) return;

            UpdateText();
        }

        public void UpdateText()
        {
            originalText = string.IsNullOrEmpty(originalText) ? string.Empty : originalText;

            bool isRightToLeft =
                (isInputField && TextUtils.IsRTLInput(originalText)) ||
                (IsRightToLeftLocale && TextUtils.ContainsRTLCharacter(originalText));

            bool process = isRightToLeftText != isRightToLeft || originalText != resultOfLastProcess;
            isRightToLeftText = isRightToLeft;

            if (!process) return;

            RTLSupport.FixText(originalText, output, isRightToLeftText, farsi, fixTags, preserveNumbers);

            resultOfLastProcess = base.text = output.ToString();
            havePropertiesChanged = true;
        }

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
    }
}
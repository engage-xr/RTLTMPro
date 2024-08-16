using NUnit.Framework;

namespace RTLTMPro.Tests
{
    public class EngageOrderingTests
    {
        [TestCase("فيديو 360؟ انتقل إلى غرفة 360", "ﻓﯿﺪﯾﻮ 063؟ ﺍﻧﺘﻘﻞ ﺇﻟﻰ ﻏﺮﻓﺔ 063")]
        [TestCase("فيديو 360.", "ﻓﯿﺪﯾﻮ 063.")]
        [TestCase("فيديو (360)", "ﻓﯿﺪﯾﻮ )063(")]
        [TestCase("إحضار جميع المستخدمين إلى \"سطح المريخ (المريخ)\"", "ﺇﺣﻀﺎﺭ ﺟﻤﯿﻊ ﺍﻟﻤﺴﺘﺨﺪﻣﯿﻦ ﺇﻟﻰ \"ﺳﻄﺢ ﺍﻟﻤﺮﯾﺦ )ﺍﻟﻤﺮﯾﺦ(\"")]
        [TestCase("\"(المريخ)\"", "\")ﺍﻟﻤﺮﯾﺦ(\"")]
        [TestCase("(\"المريخ\").", ")\"ﺍﻟﻤﺮﯾﺦ\"(.")]
        [TestCase("\"المريخ\".", "\"ﺍﻟﻤﺮﯾﺦ\".")]
        [TestCase("(المريخ).", ")ﺍﻟﻤﺮﯾﺦ(.")]
        [TestCase("هل أنت متأكد أنك تريد حذف \"موسيقىaaa\"؟", "ﻫﻞ ﺃﻧﺖ ﻣﺘﺄﻛﺪ ﺃﻧﻚ ﺗﺮﯾﺪ ﺣﺬﻑ \"ﻣﻮﺳﯿﻘﻰaaa\"؟")]
        [TestCase("هل أنت متأكد أنك تريد حذف \"aaaموسيقى\"؟", "ﻫﻞ ﺃﻧﺖ ﻣﺘﺄﻛﺪ ﺃﻧﻚ ﺗﺮﯾﺪ ﺣﺬﻑ \"aaaﻣﻮﺳﯿﻘﻰ\"؟")]
        [TestCase("انتقل إلى http://www.google.com/test?mode=true", "ﺍﻧﺘﻘﻞ ﺇﻟﻰ eurt=edom?tset/moc.elgoog.www//:ptth")]
        public void CharacterOrderRTL(string input, string expected)
        {
            AssertTextFix(input, expected, true);
        }

        [TestCase("A recording is in progress by عبد. Your voice, actions, and movements might be recorded.", "A recording is in progress by ﺪﺒﻋ. Your voice, actions, and movements might be recorded.")]
        [TestCase("عبد is recording.", "ﺪﺒﻋ is recording.")]
        [TestCase("Bring all users to قاعة المحاضرات", "Bring all users to ﺕﺍﺮﺿﺎﺤﻤﻟﺍ ﺔﻋﺎﻗ")]
        public void RTLTextInLTRStrings(string input, string expected)
        {
            AssertTextFix(input, expected, false);
        }

        [TestCase("François speaks Español", "François speaks Español")]
        public void NonEnglishCharacters(string input, string expected)
        {
            AssertTextFix(input, expected, false);
        }

        private void AssertTextFix(string input, string expected, bool isRightToLeft)
        {
            var output = new FastStringBuilder(RTLSupport.DefaultBufferSize);
            RTLSupport.FixText(input, output, isRightToLeft, true, true, true);

            Assert.AreEqual(expected, output.ToString());
        }
    }
}

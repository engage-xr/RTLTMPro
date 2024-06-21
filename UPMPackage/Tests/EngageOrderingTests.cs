using System.Text;
using NUnit.Framework;
using RTLTMPro;

using System;
using UnityEngine;

namespace RTLTMPro.Tests
{
    public class EngageOrderingTests
    {
        // Code to log the encoded output
        /*
            foreach (char c in result)
            {
                Debug.Log("\\u" + ((int)c).ToString("X4"));
            }
        */

        // Tests need to replicate what happens in RTLTextMeshPro.GetFixedText()
        //  If you change the code in that method, make the same changes to this function
        protected string simulatedUpdateText(string text, bool Farsi, bool FixTags, bool PreserveNumbers)
        {
            var output = new FastStringBuilder(RTLSupport.DefaultBufferSize);
            RTLSupport.FixRTL(text, output, Farsi, FixTags, PreserveNumbers);
            output.Reverse();

            return output.ToString();
        }

        [Test]
        public void NumberOrder_MidEnd()
        {
            // Arrange
            const string input = "فيديو 360؟ انتقل إلى غرفة 360";
            const string encodedExpected = "\uFED3\uFBFF\uFEAA\uFBFE\uFEEE\u0020\u0030\u0036\u0033\u061F\u0020\uFE8D\uFEE7\uFE98\uFED8\uFEDE\u0020\uFE87\uFEDF\uFEF0\u0020\uFECF\uFEAE\uFED3\uFE94\u0020\u0030\u0036\u0033";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void NumberOrder_FollowedByPunctuation()
        {
            // Arrange
            const string input = "فيديو 360.";
            const string encodedExpected = "\uFED3\uFBFF\uFEAA\uFBFE\uFEEE\u0020\u0030\u0036\u0033\u002E";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void NumberOrder_Control_Brackets()
        {
            // Arrange
            const string input = "فيديو (360)";
            const string encodedExpected = "\uFED3\uFBFF\uFEAA\uFBFE\uFEEE\u0020\u0029\u0030\u0036\u0033\u0028";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void QuotesOrder_AfterBracket()
        {
            // Arrange
            const string input = "إحضار جميع المستخدمين إلى \"سطح المريخ (المريخ)\"";
            const string encodedExpected = "\uFE87\uFEA3\uFEC0\uFE8E\uFEAD\u0020\uFE9F\uFEE4\uFBFF\uFECA\u0020\uFE8D\uFEDF\uFEE4\uFEB4\uFE98\uFEA8\uFEAA\uFEE3\uFBFF\uFEE6\u0020\uFE87\uFEDF\uFEF0\u0020\u0022\uFEB3\uFEC4\uFEA2\u0020\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0020\u0029\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0028\u0022";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void QuotesOrder_OutsideBracket()
        {
            // Arrange
            const string input = "\"(المريخ)\"";
            const string encodedExpected = "\u0022\u0029\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0028\u0022";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void QuotesOrder_InsideBracket()
        {
            // Arrange
            const string input = "(\"المريخ\").";
            const string encodedExpected = "\u0029\u0022\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0022\u0028\u002E";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void QuotesOrder_PeriodAfter()
        {
            // Arrange
            const string input = "\"المريخ\".";
            const string encodedExpected = "\u0022\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0022\u002E";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void Brackets_PeriodAfter()
        {
            // Arrange
            const string input = "(المريخ).";
            const string encodedExpected = "\u0029\uFE8D\uFEDF\uFEE4\uFEAE\uFBFE\uFEA6\u0028\u002E";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        // Use of RTLTextMeshPro fails due to missing TMPro references that I can't resolve
        /*
        [Test]
        public void DirectTest()
        {
            // Arrange
            var text = new FastStringBuilder("فيديو 360؟ انتقل إلى غرفة 360");

            RTLTextMeshPro rtlTMP = new RTLTextMeshPro();
            rtlTMP.text = text;
            rtlTMP.PreserveNumbers = true;
            rtlTMP.Farsi = true;
            rtlTMP.FixTags = true;
            rtlTMP.ForceFix = false;

            // Act
            //string result = rtlTMP.UpdateText();

            // Assert

        }
        */
    }
}

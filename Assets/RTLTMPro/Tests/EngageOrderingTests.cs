using System.Text;
using NUnit.Framework;
using RTLTMPro;

using System;
using UnityEngine;

namespace RTLTMPro.Tests
{
    public class EngageOrderingTests
    {
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

        [Test]
        public void QuotesOrder_LatinAfterArabic()
        {
            // Arrange
            const string input = "هل أنت متأكد أنك تريد حذف \"موسيقىaaa\"؟";
            const string encodedExpected = "\uFEEB\uFEDE\u0020\uFE83\uFEE7\uFE96\u0020\uFEE3\uFE98\uFE84\uFEDB\uFEAA\u0020\uFE83\uFEE7\uFEDA\u0020\uFE97\uFEAE\uFBFE\uFEAA\u0020\uFEA3\uFEAC\uFED1\u0020\u0022\uFEE3\uFEEE\uFEB3\uFBFF\uFED8\uFEF0\u0061\u0061\u0061\u0022\u061F";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }

        [Test]
        public void QuotesOrder_LatinBeforeArabic()
        {
            // Arrange
            const string input = "هل أنت متأكد أنك تريد حذف \"aaaموسيقى\"؟";
            const string encodedExpected = "\uFEEB\uFEDE\u0020\uFE83\uFEE7\uFE96\u0020\uFEE3\uFE98\uFE84\uFEDB\uFEAA\u0020\uFE83\uFEE7\uFEDA\u0020\uFE97\uFEAE\uFBFE\uFEAA\u0020\uFEA3\uFEAC\uFED1\u0020\u0022\u0061\u0061\u0061\uFEE3\uFEEE\uFEB3\uFBFF\uFED8\uFEF0\u0022\u061F";

            // Act
            string result = simulatedUpdateText(input, true, true, true);

            // Assert
            Assert.AreEqual(encodedExpected, result);
        }
    }
}

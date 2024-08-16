using System.Collections.Generic;

namespace RTLTMPro
{
    public static class RTLSupport
    {
        public const int DefaultBufferSize = 2048;

        private static readonly FastStringBuilder inputBuilder = new(DefaultBufferSize);
        private static readonly FastStringBuilder glyphFixerOutput = new(DefaultBufferSize);

        public static void FixText(string input, FastStringBuilder output, bool isRightToLeft, bool farsi = true, bool fixTextTags = true, bool preserveNumbers = false)
        {
            output.Clear();

            if (string.IsNullOrEmpty(input)) return;

            if (isRightToLeft)
            {
                FixRTL(input, output, farsi, fixTextTags, preserveNumbers);
                output.Reverse();
            }
            else
            {
                FixRTLChunked(input, output);
            }
        }

        /// <summary>
        ///     Fixes the provided string
        /// </summary>
        /// <param name="input">Text to fix</param>
        /// <param name="output">Fixed text</param>
        /// <param name="fixTextTags"></param>
        /// <param name="preserveNumbers"></param>
        /// <param name="farsi"></param>
        /// <returns>Fixed text</returns>
        internal static void FixRTL(string input, FastStringBuilder output, bool farsi = true, bool fixTextTags = true, bool preserveNumbers = false)
        {
            inputBuilder.SetValue(input);
            TashkeelFixer.RemoveTashkeel(inputBuilder);
            // The shape of the letters in shapeFixedLetters is fixed according to their position in word. But the flow of the text is not fixed.
            GlyphFixer.Fix(inputBuilder, glyphFixerOutput, preserveNumbers, farsi, fixTextTags);
            //Restore tashkeel to their places.
            TashkeelFixer.RestoreTashkeel(glyphFixerOutput);

            TashkeelFixer.FixShaddaCombinations(glyphFixerOutput);
            // Fix flow of the text and put the result in FinalLetters field
            LigatureFixer.Fix(glyphFixerOutput, output, farsi, fixTextTags, preserveNumbers);
            if (fixTextTags)
            {
                RichTextFixer.Fix(output);
            }
            inputBuilder.Clear();
        }

        internal static void FixRTLChunked(string input, FastStringBuilder output, bool farsi = true, bool fixTextTags = true, bool preserveNumbers = false)
        {
            List<string> chunks = TextUtils.SplitLtrRtlChunks(input);

            FastStringBuilder arChunkFixer = new FastStringBuilder(RTLSupport.DefaultBufferSize);

            // Loop over chunks. Fix RTL and just append LTR
            for (int i = 0; i < chunks.Count; i++)
            {
                if (TextUtils.IsRTLInput(chunks[i]))
                {
                    arChunkFixer.Clear();

                    // Fix the Arabic block of text
                    FixRTL(chunks[i], arChunkFixer, farsi, fixTextTags, preserveNumbers);

                    output += arChunkFixer.ToString();
                }
                else
                {
                    output += chunks[i]; // If LTR chunk, just append
                }
            }
        }
    }
}
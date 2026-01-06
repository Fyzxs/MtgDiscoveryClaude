import { useMemo } from 'react';

interface TextDistribution {
  top: string;
  left: string;
  right: string;
}

interface UseTextDistributionOptions {
  text: string;
  maxTopWidth: number;
  fontSize: number;
  fontFamily?: string;
  fontWeight?: number | string;
}

/**
 * Custom hook to distribute text across top, left, and right sections
 * using Canvas API for accurate text measurement.
 *
 * Reading Order: LEFT → TOP → RIGHT (reconstructs original sentence)
 * - LEFT: First words (vertical, reads bottom-to-top)
 * - TOP: Middle words (horizontal, as many as fit)
 * - RIGHT: Last words (vertical, reads top-to-bottom)
 *
 * Algorithm:
 * 1. Measure maximum words that fit in TOP
 * 2. Calculate remaining words to split between LEFT and RIGHT
 * 3. Distribute so reading LEFT → TOP → RIGHT gives original sentence
 * 4. ALWAYS split on word boundaries (spaces), never mid-word
 */
export const useTextDistribution = ({
  text,
  maxTopWidth,
  fontSize,
  fontFamily = 'Roboto, Helvetica, Arial, sans-serif',
  fontWeight = 600,
}: UseTextDistributionOptions): TextDistribution => {
  return useMemo(() => {
    const words = text.trim().split(/\s+/);

    if (words.length === 0) {
      return { top: '', left: '', right: '' };
    }

    // Create canvas for text measurement
    const canvas = document.createElement('canvas');
    const context = canvas.getContext('2d');

    if (context === null) {
      // Fallback if canvas context not available - prioritize top
      const topWordCount = Math.ceil(words.length * 0.6);
      const remainingCount = words.length - topWordCount;
      const leftCount = Math.ceil(remainingCount / 2);
      const rightCount = remainingCount - leftCount;

      return {
        left: words.slice(0, leftCount).join(' '),
        top: words.slice(leftCount, leftCount + topWordCount).join(' '),
        right: words.slice(leftCount + topWordCount).join(' '),
      };
    }

    context.font = `${fontWeight} ${fontSize}px ${fontFamily}`;

    // Find maximum words that can fit in the top section
    // We need to test different "middle" sections of the text
    let maxTopWordCount = 0;

    // First, find how many consecutive words can fit in top (starting from any position)
    for (let startIdx = 0; startIdx < words.length; startIdx++) {
      for (let endIdx = startIdx + 1; endIdx <= words.length; endIdx++) {
        const testText = words.slice(startIdx, endIdx).join(' ');
        const metrics = context.measureText(testText);

        if (metrics.width <= maxTopWidth) {
          const wordCount = endIdx - startIdx;
          if (wordCount > maxTopWordCount) {
            maxTopWordCount = wordCount;
          }
        } else {
          break; // No point testing longer sequences from this start
        }
      }
    }

    // If all words fit on top, put them there
    if (maxTopWordCount >= words.length) {
      return {
        top: text,
        left: '',
        right: '',
      };
    }

    // Calculate how to distribute remaining words between left and right
    const remainingCount = words.length - maxTopWordCount;
    const leftCount = Math.ceil(remainingCount / 2);
    const rightCount = remainingCount - leftCount;

    // Distribute: LEFT gets first words, TOP gets middle, RIGHT gets last
    // Reading order: LEFT → TOP → RIGHT = original sentence
    return {
      left: words.slice(0, leftCount).join(' '),
      top: words.slice(leftCount, leftCount + maxTopWordCount).join(' '),
      right: words.slice(leftCount + maxTopWordCount).join(' '),
    };
  }, [text, maxTopWidth, fontSize, fontFamily, fontWeight]);
};

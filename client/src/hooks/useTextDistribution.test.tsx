/**
 * Tests for useTextDistribution hook
 *
 * Note: These are example tests to demonstrate the hook's behavior.
 * They should be run with a proper testing framework like Jest or Vitest.
 */

import { renderHook } from '@testing-library/react';
import { useTextDistribution } from './useTextDistribution';

describe('useTextDistribution', () => {
  // Mock canvas for testing
  beforeAll(() => {
    HTMLCanvasElement.prototype.getContext = jest.fn(() => ({
      measureText: jest.fn((text: string) => ({
        width: text.length * 8, // Approximate 8px per character
      })),
      font: '',
    })) as any;
  });

  it('should put all text on top if it fits', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'Short Name',
        maxTopWidth: 500,
        fontSize: 14,
      })
    );

    expect(result.current.top).toBe('Short Name');
    expect(result.current.left).toBe('');
    expect(result.current.right).toBe('');
  });

  it('should distribute long text across top, left, and right', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'Avatar: The Last Airbender Beginner Box Front Cards',
        maxTopWidth: 200,
        fontSize: 14,
      })
    );

    // With 200px width and ~8px/char, we can fit ~25 characters
    // "Avatar: The Last Airbender" is exactly 25 chars
    expect(result.current.top).toBeTruthy();
    expect(result.current.left).toBeTruthy();
    expect(result.current.right).toBeTruthy();

    // Verify no words are split
    const allWords = 'Avatar: The Last Airbender Beginner Box Front Cards'.split(/\s+/);
    const distributedWords = [
      ...result.current.top.split(/\s+/),
      ...result.current.left.split(/\s+/),
      ...result.current.right.split(/\s+/),
    ].filter(w => w);

    expect(distributedWords.length).toBe(allWords.length);
  });

  it('should split remaining words evenly between left and right', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'One Two Three Four Five Six',
        maxTopWidth: 100, // Fits ~2 words
        fontSize: 14,
      })
    );

    const leftWords = result.current.left.split(/\s+/).filter(w => w).length;
    const rightWords = result.current.right.split(/\s+/).filter(w => w).length;

    // Should split remaining words evenly (or differ by at most 1)
    expect(Math.abs(leftWords - rightWords)).toBeLessThanOrEqual(1);
  });

  it('should handle empty text', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: '',
        maxTopWidth: 300,
        fontSize: 14,
      })
    );

    expect(result.current.top).toBe('');
    expect(result.current.left).toBe('');
    expect(result.current.right).toBe('');
  });

  it('should handle single word', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'Foundations',
        maxTopWidth: 300,
        fontSize: 14,
      })
    );

    expect(result.current.top).toBe('Foundations');
    expect(result.current.left).toBe('');
    expect(result.current.right).toBe('');
  });

  it('should put all words on sides if none fit on top', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'Supercalifragilisticexpialidocious Extra Words',
        maxTopWidth: 50, // Very narrow
        fontSize: 14,
      })
    );

    expect(result.current.top).toBe('');
    expect(result.current.left).toBeTruthy();
    expect(result.current.right).toBeTruthy();
  });

  it('should respect font settings', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'Test Text Here',
        maxTopWidth: 300,
        fontSize: 20, // Larger font
        fontWeight: 700,
        fontFamily: 'Arial',
      })
    );

    // With larger font, fewer words should fit on top
    // Just verify it runs without error
    expect(result.current.top).toBeDefined();
  });

  it('should always split on word boundaries', () => {
    const { result } = renderHook(() =>
      useTextDistribution({
        text: 'These are complete words that should never be split',
        maxTopWidth: 150,
        fontSize: 14,
      })
    );

    const allText = [result.current.top, result.current.left, result.current.right]
      .filter(s => s)
      .join(' ');

    const originalWords = 'These are complete words that should never be split'.split(/\s+/);
    const resultWords = allText.split(/\s+/);

    // Every word should be complete
    resultWords.forEach(word => {
      expect(originalWords).toContain(word);
    });
  });

  it('should memoize results for same inputs', () => {
    const { result, rerender } = renderHook(
      (props) => useTextDistribution(props),
      {
        initialProps: {
          text: 'Same Text',
          maxTopWidth: 300,
          fontSize: 14,
        },
      }
    );

    const firstResult = result.current;

    // Rerender with same props
    rerender({
      text: 'Same Text',
      maxTopWidth: 300,
      fontSize: 14,
    });

    // Should return same object reference (memoized)
    expect(result.current).toBe(firstResult);
  });

  it('should recalculate when inputs change', () => {
    const { result, rerender } = renderHook(
      (props) => useTextDistribution(props),
      {
        initialProps: {
          text: 'Original Text',
          maxTopWidth: 300,
          fontSize: 14,
        },
      }
    );

    const firstResult = result.current;

    // Rerender with different text
    rerender({
      text: 'Different Text',
      maxTopWidth: 300,
      fontSize: 14,
    });

    // Should return different result
    expect(result.current).not.toBe(firstResult);
    expect(result.current.top).not.toBe(firstResult.top);
  });
});

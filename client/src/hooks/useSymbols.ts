import { useTranslation } from 'react-i18next';
import type { CardFinish, CardSpecial } from '../types/collection';

/**
 * Hook for culturally-aware symbol and emoji usage
 * Replaces hardcoded emojis with localized symbols
 */
export function useSymbols() {
  const { t } = useTranslation('symbols');

  return {
    /**
     * Get the appropriate symbol for a card finish type
     */
    getFinishSymbol: (finish: CardFinish): string => {
      return t(`finish.${finish}`, { defaultValue: '🔹' });
    },

    /**
     * Get the appropriate symbol for a special card designation
     */
    getSpecialSymbol: (special: CardSpecial): string => {
      return t(`special.${special}`, { defaultValue: '📜' });
    },

    /**
     * Get collection status symbol
     */
    getStatusSymbol: (status: 'collected' | 'notCollected' | 'partial'): string => {
      return t(`status.${status}`, { defaultValue: status === 'collected' ? '✅' : status === 'partial' ? '🔄' : '⭕' });
    },

    /**
     * Get all finish symbols for display
     */
    getAllFinishSymbols: () => {
      return {
        nonfoil: t('finish.nonfoil', { defaultValue: '🔹' }),
        foil: t('finish.foil', { defaultValue: '✨' }),
        etched: t('finish.etched', { defaultValue: '⚡' })
      };
    },

    /**
     * Get all special symbols for display
     */
    getAllSpecialSymbols: () => {
      return {
        artistProof: t('special.artistProof', { defaultValue: '📜' }),
        signed: t('special.signed', { defaultValue: '✍️' }),
        altered: t('special.altered', { defaultValue: '🎨' })
      };
    }
  };
}

/**
 * Hook for finish type labels and symbols
 */
export function useFinishDisplay() {
  const { t: tCards } = useTranslation('cards');
  const { getFinishSymbol } = useSymbols();

  return {
    getSymbol: (finish: CardFinish) => getFinishSymbol(finish),
    getLabel: (finish: CardFinish) => tCards(`finish.${finish}`),
    getLabelWithSymbol: (finish: CardFinish) => {
      const symbol = getFinishSymbol(finish);
      const label = tCards(`finish.${finish}`);
      return `${symbol} ${label}`;
    }
  };
}

/**
 * Hook for special card type labels and symbols
 */
export function useSpecialDisplay() {
  const { t: tCards } = useTranslation('cards');
  const { getSpecialSymbol } = useSymbols();

  return {
    getSymbol: (special: CardSpecial) => getSpecialSymbol(special),
    getLabel: (special: CardSpecial) => tCards(`special.${special}`),
    getLabelWithSymbol: (special: CardSpecial) => {
      const symbol = getSpecialSymbol(special);
      const label = tCards(`special.${special}`);
      return `${symbol} ${label}`;
    }
  };
}
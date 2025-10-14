import {
  formatFinishText,
  formatPromoText,
  formatFrameEffectText,
  getFinishColor
} from '../badgeFormatters';

describe('badgeFormatters', () => {
  describe('formatFinishText', () => {
    it('should format foil correctly', () => {
      expect(formatFinishText('foil')).toBe('Foil');
      expect(formatFinishText('FOIL')).toBe('Foil');
    });

    it('should format etched correctly', () => {
      expect(formatFinishText('etched')).toBe('Etched');
    });

    it('should format nonfoil correctly', () => {
      expect(formatFinishText('nonfoil')).toBe('Non-Foil');
    });

    it('should format glossy correctly', () => {
      expect(formatFinishText('glossy')).toBe('Glossy');
    });

    it('should format galaxyfoil correctly', () => {
      expect(formatFinishText('galaxyfoil')).toBe('Galaxy Foil');
    });

    it('should return original text for unknown finish', () => {
      expect(formatFinishText('unknown')).toBe('unknown');
    });
  });

  describe('formatPromoText', () => {
    it('should format prerelease correctly', () => {
      expect(formatPromoText('prerelease')).toBe('Pre-Release');
    });

    it('should format promobundle correctly', () => {
      expect(formatPromoText('promobundle')).toBe('Promo Bundle');
    });

    it('should format buyabox correctly', () => {
      expect(formatPromoText('buyabox')).toBe('Buy-a-Box');
    });

    it('should format fnm correctly', () => {
      expect(formatPromoText('fnm')).toBe('FNM');
    });

    it('should format judge_gift correctly', () => {
      expect(formatPromoText('judge_gift')).toBe('Judge Gift');
    });

    it('should format serialized correctly', () => {
      expect(formatPromoText('serialized')).toBe('Serialized');
    });

    it('should format surgefoil correctly', () => {
      expect(formatPromoText('surgefoil')).toBe('Surge Foil');
    });

    it('should format promopack correctly', () => {
      expect(formatPromoText('promopack')).toBe('Promo Pack');
    });

    it('should handle FF + Roman numerals pattern', () => {
      expect(formatPromoText('ffxiv')).toBe('FF XIV');
      expect(formatPromoText('ffvii')).toBe('FF VII');
      expect(formatPromoText('ffxvi')).toBe('FF XVI');
    });

    it('should capitalize unknown promo types', () => {
      expect(formatPromoText('custom_promo')).toBe('Custom Promo');
      expect(formatPromoText('special')).toBe('Special');
    });

    it('should handle underscores in unknown types', () => {
      expect(formatPromoText('special_edition_promo')).toBe('Special Edition Promo');
    });
  });

  describe('formatFrameEffectText', () => {
    it('should format legendary correctly', () => {
      expect(formatFrameEffectText('legendary')).toBe('Legendary');
    });

    it('should format showcase correctly', () => {
      expect(formatFrameEffectText('showcase')).toBe('Showcase');
    });

    it('should format extendedart correctly', () => {
      expect(formatFrameEffectText('extendedart')).toBe('Extended Art');
    });

    it('should format fullart correctly', () => {
      expect(formatFrameEffectText('fullart')).toBe('Full Art');
    });

    it('should format textless correctly', () => {
      expect(formatFrameEffectText('textless')).toBe('Textless');
    });

    it('should format miracle correctly', () => {
      expect(formatFrameEffectText('miracle')).toBe('Miracle');
    });

    it('should format nyxtouched correctly', () => {
      expect(formatFrameEffectText('nyxtouched')).toBe('Nyxtouched');
    });

    it('should format devoid correctly', () => {
      expect(formatFrameEffectText('devoid')).toBe('Devoid');
    });

    it('should format colorshifted correctly', () => {
      expect(formatFrameEffectText('colorshifted')).toBe('Colorshifted');
    });

    it('should format sunmoondfc correctly', () => {
      expect(formatFrameEffectText('sunmoondfc')).toBe('Sun/Moon DFC');
    });

    it('should format compasslanddfc correctly', () => {
      expect(formatFrameEffectText('compasslanddfc')).toBe('Compass Land DFC');
    });

    it('should format originpwdfc correctly', () => {
      expect(formatFrameEffectText('originpwdfc')).toBe('Origin PW DFC');
    });

    it('should format shatteredglass correctly', () => {
      expect(formatFrameEffectText('shatteredglass')).toBe('Shattered Glass');
    });

    it('should capitalize unknown frame effects', () => {
      expect(formatFrameEffectText('custom_frame')).toBe('Custom Frame');
    });
  });

  describe('getFinishColor', () => {
    it('should return primary for foil', () => {
      expect(getFinishColor('foil')).toBe('primary');
      expect(getFinishColor('FOIL')).toBe('primary');
    });

    it('should return secondary for etched', () => {
      expect(getFinishColor('etched')).toBe('secondary');
    });

    it('should return info for nonfoil', () => {
      expect(getFinishColor('nonfoil')).toBe('info');
    });

    it('should return default for unknown finish', () => {
      expect(getFinishColor('unknown')).toBe('default');
    });
  });

  describe('edge cases', () => {
    it('should handle empty strings', () => {
      expect(formatFinishText('')).toBe('');
      expect(formatPromoText('')).toBe('');
      expect(formatFrameEffectText('')).toBe('');
      expect(getFinishColor('')).toBe('default');
    });

    it('should handle mixed case inputs', () => {
      expect(formatFinishText('FoIl')).toBe('Foil');
      expect(formatPromoText('PreRelease')).toBe('Pre-Release');
      expect(formatFrameEffectText('ShowCase')).toBe('Showcase');
    });

    it('should handle whitespace', () => {
      expect(formatPromoText('  prerelease  ')).toBe('  prerelease  '); // Preserves input
    });
  });
});

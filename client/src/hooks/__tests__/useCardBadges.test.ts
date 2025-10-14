import { renderHook } from '@testing-library/react';
import { useCardBadges } from '../useCardBadges';

describe('useCardBadges', () => {
  describe('finish badges', () => {
    it('should return foil badge when card is foil only', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false
        })
      );

      expect(result.current).toHaveLength(1);
      expect(result.current[0]).toMatchObject({
        type: 'finish',
        label: 'foil',
        value: 'foil'
      });
    });

    it('should return both foil and nonfoil badges for hybrid cards', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: true,
          etched: false
        })
      );

      expect(result.current).toHaveLength(2);
      expect(result.current.map(b => b.label)).toContain('nonfoil');
      expect(result.current.map(b => b.label)).toContain('foil');
    });

    it('should return etched badge when card has etched finish', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: false,
          etched: true
        })
      );

      expect(result.current).toHaveLength(1);
      expect(result.current[0]).toMatchObject({
        type: 'finish',
        label: 'etched',
        value: 'etched'
      });
    });

    it('should not show foil badge when card has special foil promo', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          promoTypes: ['surgefoil']
        })
      );

      // Should not include foil badge because surgefoil is a special foil type
      const hasFoilBadge = result.current.some(b => b.label === 'foil');
      expect(hasFoilBadge).toBe(false);
    });
  });

  describe('promo badges', () => {
    it('should return promo type badges', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          promoTypes: ['prerelease', 'promopack']
        })
      );

      const promoBadges = result.current.filter(b => b.type === 'promo');
      expect(promoBadges).toHaveLength(2);
      expect(promoBadges.map(b => b.label)).toContain('prerelease');
      expect(promoBadges.map(b => b.label)).toContain('promopack');
    });

    it('should filter out excluded promo types', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          promoTypes: ['boosterfun', 'prerelease'],
          excludePromoTypes: ['boosterfun']
        })
      );

      const promoBadges = result.current.filter(b => b.type === 'promo');
      expect(promoBadges).toHaveLength(1);
      expect(promoBadges[0].label).toBe('prerelease');
    });

    it('should show generic promo badge when isPromo is true but no specific types', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          isPromo: true,
          promoTypes: []
        })
      );

      const promoBadges = result.current.filter(b => b.type === 'promo');
      expect(promoBadges).toHaveLength(1);
      expect(promoBadges[0].label).toBe('promo');
    });
  });

  describe('serialized cards', () => {
    it('should show only serialized badge when card is serialized', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: true,
          etched: true,
          promoTypes: ['serialized', 'prerelease'],
          frameEffects: ['showcase', 'extendedart']
        })
      );

      // Should only show serialized badge, no finish, frame, or other promo badges
      expect(result.current).toHaveLength(1);
      expect(result.current[0]).toMatchObject({
        type: 'promo',
        label: 'serialized',
        value: 'serialized'
      });
    });
  });

  describe('frame effect badges', () => {
    it('should return frame effect badges', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          frameEffects: ['showcase', 'extendedart']
        })
      );

      const frameBadges = result.current.filter(b => b.type === 'frame');
      expect(frameBadges).toHaveLength(2);
      expect(frameBadges.map(b => b.label)).toContain('showcase');
      expect(frameBadges.map(b => b.label)).toContain('extendedart');
    });

    it('should filter out excluded frame effects', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          frameEffects: ['legendary', 'showcase'],
          excludeFrameEffects: ['legendary']
        })
      );

      const frameBadges = result.current.filter(b => b.type === 'frame');
      expect(frameBadges).toHaveLength(1);
      expect(frameBadges[0].label).toBe('showcase');
    });
  });

  describe('digital badge', () => {
    it('should return digital badge when card is digital', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: true,
          etched: false,
          digital: true
        })
      );

      const digitalBadge = result.current.find(b => b.type === 'digital');
      expect(digitalBadge).toBeDefined();
      expect(digitalBadge?.label).toBe('digital');
    });
  });

  describe('badge combinations', () => {
    it('should return multiple badge types correctly ordered', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: true,
          etched: false,
          digital: true,
          promoTypes: ['prerelease'],
          frameEffects: ['showcase']
        })
      );

      expect(result.current.length).toBeGreaterThan(1);

      // Check types are present
      const types = result.current.map(b => b.type);
      expect(types).toContain('finish'); // nonfoil and/or foil
      expect(types).toContain('digital');
      expect(types).toContain('promo');
      expect(types).toContain('frame');
    });
  });

  describe('edge cases', () => {
    it('should handle null frameEffects', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          frameEffects: null
        })
      );

      expect(result.current).toHaveLength(1);
      expect(result.current[0].label).toBe('foil');
    });

    it('should handle undefined arrays', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          promoTypes: undefined,
          frameEffects: undefined
        })
      );

      expect(result.current).toHaveLength(1);
      expect(result.current[0].label).toBe('foil');
    });

    it('should return empty array when no badges apply', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: false,
          nonfoil: false,
          etched: false
        })
      );

      expect(result.current).toHaveLength(0);
    });
  });

  describe('special foil detection', () => {
    it('should detect galaxyfoil as special foil', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          promoTypes: ['galaxyfoil']
        })
      );

      // Should not show regular foil badge
      const hasFoilBadge = result.current.some(b => b.label === 'foil');
      expect(hasFoilBadge).toBe(false);

      // Should show galaxyfoil promo badge
      const hasGalaxyFoil = result.current.some(b => b.label === 'galaxyfoil');
      expect(hasGalaxyFoil).toBe(true);
    });

    it('should detect ripplefoil as special foil', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          promoTypes: ['ripplefoil']
        })
      );

      const hasFoilBadge = result.current.some(b => b.label === 'foil');
      expect(hasFoilBadge).toBe(false);
    });

    it('should detect oilslick as special foil', () => {
      const { result } = renderHook(() =>
        useCardBadges({
          foil: true,
          nonfoil: false,
          etched: false,
          promoTypes: ['oilslick']
        })
      );

      const hasFoilBadge = result.current.some(b => b.label === 'foil');
      expect(hasFoilBadge).toBe(false);
    });
  });
});

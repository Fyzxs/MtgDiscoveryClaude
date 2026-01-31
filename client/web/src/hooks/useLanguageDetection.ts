import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { SUPPORTED_LANGUAGES } from '../i18n/config';

export interface LanguageDetectionResult {
  detectedLanguage: string;
  isSupported: boolean;
  confidence: 'high' | 'medium' | 'low';
  source: 'localStorage' | 'navigator' | 'htmlTag' | 'fallback';
}

/**
 * Hook to provide language detection information and first-time user experience
 */
export function useLanguageDetection() {
  const { i18n } = useTranslation();
  const [detectionResult, setDetectionResult] = useState<LanguageDetectionResult | null>(null);
  const [showLanguagePrompt, setShowLanguagePrompt] = useState(false);

  useEffect(() => {
    // Check if this is the first visit
    const hasVisited = localStorage.getItem('mtg-discovery-visited');
    const savedLanguage = localStorage.getItem('mtg-discovery-language');

    // Detect browser language
    const browserLanguage = navigator.language.split('-')[0]; // Get language without region
    const isSupported = SUPPORTED_LANGUAGES.some(lang => lang.code === browserLanguage);

    // Determine detection source and confidence
    let source: LanguageDetectionResult['source'] = 'fallback';
    let confidence: LanguageDetectionResult['confidence'] = 'low';

    if (savedLanguage) {
      source = 'localStorage';
      confidence = 'high';
    } else if (browserLanguage && isSupported) {
      source = 'navigator';
      confidence = 'high';
    } else if (browserLanguage) {
      source = 'navigator';
      confidence = 'medium';
    } else {
      const htmlLang = document.documentElement.lang;
      if (htmlLang) {
        source = 'htmlTag';
        confidence = 'low';
      }
    }

    setDetectionResult({
      detectedLanguage: browserLanguage || i18n.language,
      isSupported,
      confidence,
      source
    });

    // Show language prompt for first-time users with unsupported browser language
    if (!hasVisited && !savedLanguage && browserLanguage && !isSupported) {
      setShowLanguagePrompt(true);
    }

    // Mark as visited
    if (!hasVisited) {
      localStorage.setItem('mtg-discovery-visited', 'true');
    }
  }, [i18n.language]);

  const dismissLanguagePrompt = () => {
    setShowLanguagePrompt(false);
  };

  const selectLanguage = (languageCode: string) => {
    i18n.changeLanguage(languageCode);
    setShowLanguagePrompt(false);
  };

  return {
    detectionResult,
    showLanguagePrompt,
    dismissLanguagePrompt,
    selectLanguage,
    supportedLanguages: SUPPORTED_LANGUAGES
  };
}
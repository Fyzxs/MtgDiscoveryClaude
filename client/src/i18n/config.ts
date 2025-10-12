import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import LanguageDetector from 'i18next-browser-languagedetector';
import Backend from 'i18next-http-backend';

// Translation namespaces
export const NAMESPACES = [
  'common',
  'cards',
  'sets',
  'collection',
  'navigation',
  'errors',
  'symbols'
] as const;

export type Namespace = typeof NAMESPACES[number];

// Supported languages
export const SUPPORTED_LANGUAGES = [
  { code: 'en', name: 'English', nativeName: 'English' },
  { code: 'es', name: 'Spanish', nativeName: 'Español' },
  { code: 'fr', name: 'French', nativeName: 'Français' },
  { code: 'de', name: 'German', nativeName: 'Deutsch' },
  { code: 'ja', name: 'Japanese', nativeName: '日本語' },
  { code: 'pt', name: 'Portuguese', nativeName: 'Português' }
] as const;

export type LanguageCode = typeof SUPPORTED_LANGUAGES[number]['code'];

// Initialize i18next
i18n
  .use(Backend)
  .use(LanguageDetector)
  .use(initReactI18next)
  .init({
    // Default language
    fallbackLng: 'en',

    // Supported languages
    supportedLngs: SUPPORTED_LANGUAGES.map(lang => lang.code),

    // Namespaces
    ns: NAMESPACES,
    defaultNS: 'common',

    // Detection options
    detection: {
      order: ['localStorage', 'navigator', 'htmlTag'],
      caches: ['localStorage'],
      lookupLocalStorage: 'mtg-discovery-language'
    },

    // Backend options for loading translations
    backend: {
      loadPath: '/locales/{{lng}}/{{ns}}.json',
      addPath: '/locales/{{lng}}/{{ns}}.missing.json'
    },

    // Interpolation options
    interpolation: {
      escapeValue: false // React already escapes values
    },

    // React options
    react: {
      useSuspense: true
    },

    // Development options
    debug: process.env.NODE_ENV === 'development',

    // Handle missing keys
    saveMissing: process.env.NODE_ENV === 'development',

    // Pluralization
    pluralSeparator: '_',

    // Context separator for variations
    contextSeparator: '_'
  });

export default i18n;
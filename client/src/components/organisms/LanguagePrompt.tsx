import React from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Box,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Chip
} from '@mui/material';
import { useLanguageDetection } from '../../hooks/useLanguageDetection';

/**
 * First-time user language selection prompt
 * Shows when user's browser language isn't supported
 */
export const LanguagePrompt: React.FC = () => {
  const {
    detectionResult,
    showLanguagePrompt,
    dismissLanguagePrompt,
    selectLanguage,
    supportedLanguages
  } = useLanguageDetection();

  if (!showLanguagePrompt || !detectionResult) {
    return null;
  }

  const getFlagEmoji = (countryCode: string): string => {
    const flags: Record<string, string> = {
      'en': '🇺🇸',
      'es': '🇪🇸',
      'fr': '🇫🇷',
      'de': '🇩🇪',
      'ja': '🇯🇵',
      'pt': '🇧🇷'
    };
    return flags[countryCode] || '🌐';
  };

  const getRecommendedLanguage = () => {
    // Try to find a close match for unsupported languages
    const browserLang = detectionResult.detectedLanguage;

    // Language mappings for close alternatives
    const languageMappings: Record<string, string> = {
      'pt-br': 'pt',
      'pt-pt': 'pt',
      'en-gb': 'en',
      'en-us': 'en',
      'es-mx': 'es',
      'es-ar': 'es',
      'fr-ca': 'fr'
    };

    return languageMappings[browserLang] || 'en';
  };

  const recommendedLang = getRecommendedLanguage();

  return (
    <Dialog
      open={showLanguagePrompt}
      onClose={dismissLanguagePrompt}
      maxWidth="sm"
      fullWidth
      aria-labelledby="language-selection-title"
    >
      <DialogTitle id="language-selection-title">
        Welcome to MTG Discovery
      </DialogTitle>

      <DialogContent>
        <Box sx={{ mb: 3 }}>
          <Typography variant="body1" gutterBottom>
            We detected your browser language as <strong>{detectionResult.detectedLanguage}</strong>,
            but we don't have a translation for it yet.
          </Typography>

          <Typography variant="body2" color="text.secondary">
            Please choose your preferred language:
          </Typography>
        </Box>

        {/* Recommended Language */}
        <Box sx={{ mb: 2 }}>
          <Typography variant="subtitle2" gutterBottom>
            Recommended:
          </Typography>
          <Button
            variant="outlined"
            startIcon={<span style={{ fontSize: '1.2em' }}>{getFlagEmoji(recommendedLang)}</span>}
            onClick={() => selectLanguage(recommendedLang)}
            fullWidth
            sx={{ justifyContent: 'flex-start', mb: 2 }}
          >
            {supportedLanguages.find(lang => lang.code === recommendedLang)?.nativeName}
            <Chip
              label="Recommended"
              size="small"
              color="primary"
              sx={{ ml: 'auto' }}
            />
          </Button>
        </Box>

        {/* All Available Languages */}
        <Typography variant="subtitle2" gutterBottom>
          Other options:
        </Typography>

        <List dense>
          {supportedLanguages
            .filter(lang => lang.code !== recommendedLang)
            .map((language) => (
            <ListItem key={language.code} disablePadding>
              <ListItemButton onClick={() => selectLanguage(language.code)}>
                <ListItemIcon sx={{ minWidth: 'auto', mr: 2 }}>
                  <span style={{ fontSize: '1.2em' }}>
                    {getFlagEmoji(language.code)}
                  </span>
                </ListItemIcon>
                <ListItemText
                  primary={language.nativeName}
                  secondary={language.name !== language.nativeName ? language.name : undefined}
                />
              </ListItemButton>
            </ListItem>
          ))}
        </List>

        <Box sx={{ mt: 2, p: 2, bgcolor: 'background.default', borderRadius: 1 }}>
          <Typography variant="caption" color="text.secondary">
            You can change your language preference anytime using the language switcher
            in the header or footer.
          </Typography>
        </Box>
      </DialogContent>

      <DialogActions>
        <Button onClick={dismissLanguagePrompt} color="inherit">
          Use English for now
        </Button>
      </DialogActions>
    </Dialog>
  );
};
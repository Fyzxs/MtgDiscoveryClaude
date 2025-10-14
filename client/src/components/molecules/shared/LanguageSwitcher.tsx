import React from 'react';
import {
  FormControl,
  Select,
  MenuItem,
  Box,
  Typography,
  ListItemIcon,
  ListItemText,
  type SelectChangeEvent
} from '@mui/material';
import { useTranslation } from 'react-i18next';
import { SUPPORTED_LANGUAGES, type LanguageCode } from '../../../i18n/config';

interface LanguageSwitcherProps {
  variant?: 'standard' | 'outlined' | 'filled';
  size?: 'small' | 'medium';
  showLabel?: boolean;
  compact?: boolean;
}

export const LanguageSwitcher: React.FC<LanguageSwitcherProps> = ({
  variant = 'outlined',
  size = 'small',
  showLabel = false,
  compact = false
}) => {
  const { i18n } = useTranslation();

  const handleLanguageChange = (event: SelectChangeEvent<string>) => {
    const newLanguage = event.target.value as LanguageCode;
    i18n.changeLanguage(newLanguage);
  };

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

  if (compact) {
    return (
      <FormControl size="small" sx={{ minWidth: 'auto' }}>
        <Select
          value={i18n.language}
          onChange={handleLanguageChange}
          displayEmpty
          variant="standard"
          disableUnderline
          sx={{
            '& .MuiSelect-select': {
              display: 'flex',
              alignItems: 'center',
              padding: '4px 8px',
              fontSize: '1.2em',
              border: 'none',
              '&:focus': {
                backgroundColor: 'transparent',
              }
            },
            '& .MuiSelect-icon': {
              display: 'none'
            }
          }}
        >
          {SUPPORTED_LANGUAGES.map((language) => (
            <MenuItem key={language.code} value={language.code}>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <span style={{ fontSize: '1.2em' }}>
                  {getFlagEmoji(language.code)}
                </span>
                <Typography variant="body2">
                  {language.nativeName}
                </Typography>
              </Box>
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    );
  }

  return (
    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
      {showLabel && (
        <Typography variant="body2" color="text.secondary">
          Language:
        </Typography>
      )}

      <FormControl variant={variant} size={size} sx={{ minWidth: 120 }}>
        <Select
          value={i18n.language}
          onChange={handleLanguageChange}
          displayEmpty
          sx={{
            '& .MuiSelect-select': {
              display: 'flex',
              alignItems: 'center',
              gap: 1
            }
          }}
        >
          {SUPPORTED_LANGUAGES.map((language) => (
            <MenuItem key={language.code} value={language.code}>
              <ListItemIcon sx={{ minWidth: 'auto', mr: 1 }}>
                <span style={{ fontSize: '1.2em' }}>
                  {getFlagEmoji(language.code)}
                </span>
              </ListItemIcon>
              <ListItemText
                primary={language.nativeName}
                secondary={language.name !== language.nativeName ? language.name : undefined}
                secondaryTypographyProps={{
                  variant: 'caption',
                  color: 'text.disabled'
                }}
              />
            </MenuItem>
          ))}
        </Select>
      </FormControl>
    </Box>
  );
};
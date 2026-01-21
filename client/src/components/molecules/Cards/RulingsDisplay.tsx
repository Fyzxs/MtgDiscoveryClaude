import React, { useState, useEffect } from 'react';
import { logger } from '../../../utils/logger';
import { Box, Typography } from '@mui/material';
import { ExpandableSection } from '../../molecules/shared/ExpandableSection';
import { LoadingContainer, ErrorAlert } from '../../atoms';
import { formatRulingDate } from '../../../utils/dateFormatters';
import { fetchWithRetry, globalLoadingManager } from '../../../utils/networkErrorHandler';

interface Ruling {
  object: string;
  oracle_id?: string;
  source: string;
  published_at: string;
  comment: string;
}

interface RulingsResponse {
  object: string;
  has_more: boolean;
  data: Ruling[];
}

interface RulingsDisplayProps {
  rulingsUri?: string;
}

export const RulingsDisplay: React.FC<RulingsDisplayProps> = ({ rulingsUri }) => {
  const [rulings, setRulings] = useState<Ruling[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [expanded, setExpanded] = useState(true); // Start expanded

  useEffect(() => {
    if (!rulingsUri) {
      setRulings([]);
      return;
    }

    const fetchRulings = async () => {
      const loadingKey = `rulings-${rulingsUri}`;
      setLoading(true);
      setError(null);
      globalLoadingManager.setLoading(loadingKey, true);
      
      try {
        const data: RulingsResponse = await fetchWithRetry<RulingsResponse>(rulingsUri, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
          },
          retries: 2,
          retryDelay: 1000,
          timeout: 8000,
          onRetry: (attemptNumber, error) => {
            logger.debug(`Retrying rulings fetch (attempt ${attemptNumber}):`, error.message);
          }
        });
        
        // Sort rulings by date (oldest first)
        const sortedRulings = (data.data || []).sort((a, b) => {
          const dateA = new Date(a.published_at).getTime();
          const dateB = new Date(b.published_at).getTime();
          return dateA - dateB; // Oldest first
        });
        setRulings(sortedRulings);
      } catch (err) {
        logger.error('Error fetching rulings:', err);
        const networkError = err as Error & { userMessage?: string };
        if (networkError.userMessage) {
          setError(networkError.userMessage);
        } else {
          setError('Failed to load rulings. Please try again later.');
        }
        setRulings([]);
      } finally {
        setLoading(false);
        globalLoadingManager.setLoading(loadingKey, false);
      }
    };

    fetchRulings();
  }, [rulingsUri]);

  // Don't render anything if no rulings URI or no rulings
  if (!rulingsUri || (rulings.length === 0 && !loading && !error)) {
    return null;
  }


  return (
    <ExpandableSection
      title="Rulings"
      count={rulings.length}
      isLoading={loading}
      isError={Boolean(error)}
      expanded={expanded}
      onExpandedChange={setExpanded}
    >
      {loading && (
        <LoadingContainer size="small" />
      )}

      {error && (
        <ErrorAlert message={error} />
      )}

      {!loading && !error && rulings.length > 0 && (
        <Box sx={{ pl: 2 }}>
          {rulings.map((ruling, index) => (
            <Box key={index} sx={{ mb: 2.5 }}>
              <Typography 
                variant="caption" 
                sx={{ 
                  color: 'primary.main',
                  fontWeight: 600,
                  display: 'block',
                  mb: 0.5
                }}
              >
                {formatRulingDate(ruling.published_at)}
              </Typography>
              <Typography 
                variant="body2" 
                sx={{ 
                  color: 'text.secondary',
                  lineHeight: 1.6
                }}
              >
                {ruling.comment}
              </Typography>
            </Box>
          ))}
        </Box>
      )}

      {!loading && !error && rulings.length === 0 && (
        <Typography variant="body2" color="text.secondary" sx={{ pl: 2 }}>
          No rulings available for this card.
        </Typography>
      )}
    </ExpandableSection>
  );
};
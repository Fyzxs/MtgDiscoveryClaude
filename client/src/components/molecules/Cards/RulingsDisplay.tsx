import React, { useState, useEffect } from 'react';
import { Box, Typography } from '@mui/material';
import { ExpandableSection } from '../../molecules/shared/ExpandableSection';
import { LoadingContainer } from '../../atoms/shared/LoadingContainer';
import { ErrorAlert } from '../../atoms/shared/ErrorAlert';

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
      setLoading(true);
      setError(null);
      
      try {
        // Use CORS proxy if needed or fetch directly
        // Note: Scryfall API supports CORS, so direct fetch should work
        const response = await fetch(rulingsUri, {
          method: 'GET',
          headers: {
            'Accept': 'application/json',
          }
        });
        
        if (!response.ok) {
          throw new Error(`Failed to fetch rulings: ${response.statusText}`);
        }
        
        const data: RulingsResponse = await response.json();
        setRulings(data.data || []);
      } catch (err) {
        console.error('Error fetching rulings:', err);
        // Check if it's a CORS error
        if (err instanceof TypeError && err.message.includes('fetch')) {
          setError('Unable to load rulings due to network restrictions');
        } else {
          setError('Failed to load rulings');
        }
        setRulings([]);
      } finally {
        setLoading(false);
      }
    };

    fetchRulings();
  }, [rulingsUri]);

  // Don't render anything if no rulings URI or no rulings
  if (!rulingsUri || (rulings.length === 0 && !loading && !error)) {
    return null;
  }

  const formatDate = (dateString: string): string => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', { 
      year: 'numeric', 
      month: '2-digit', 
      day: '2-digit' 
    });
  };

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
                {formatDate(ruling.published_at)}
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
import React from 'react';
import { Box, Container, Typography, Divider } from '../../atoms';
import { useTheme } from '../../atoms';

export const Footer: React.FC = () => {
  const currentYear = new Date().getFullYear();
  const theme = useTheme();
  
  return (
    <Box 
      component="footer" 
      sx={{ 
        mt: 'auto',
        backgroundColor: 'background.paper',
        borderTop: '1px solid',
        borderColor: 'divider',
        py: 4
      }}
    >
      <Container maxWidth="lg">
        <Box sx={{ textAlign: 'center' }}>
          <Typography 
            variant="h6" 
            gutterBottom 
            sx={{ 
              fontWeight: 'bold',
              background: theme.mtg.gradients.footer,
              backgroundClip: 'text',
              WebkitBackgroundClip: 'text',
              WebkitTextFillColor: 'transparent',
              mb: 3
            }}
          >
            MtgDiscovery
          </Typography>
          
          <Divider sx={{ mb: 3 }} />
          
          <Typography 
            variant="body2" 
            color="text.secondary" 
            paragraph
            sx={{ 
              maxWidth: '800px', 
              mx: 'auto',
              lineHeight: 1.7
            }}
          >
            MtgDiscovery is unofficial Fan Content permitted under the Wizards of the Coast Fan Content Policy. 
            The literal and graphical information presented on this site about Magic: The Gathering, 
            including card images and mana symbols, is copyright Wizards of the Coast, LLC. 
            MtgDiscovery is not produced by or endorsed by Wizards of the Coast.
          </Typography>

          <Typography 
            variant="body2" 
            color="text.secondary" 
            paragraph
            sx={{ 
              maxWidth: '800px', 
              mx: 'auto',
              lineHeight: 1.7
            }}
          >
            Data provided by Scryfall, LLC. MtgDiscovery is not produced by or endorsed by Scryfall.
            Card prices and promotional offers represent daily estimates and/or market values provided by affiliates. 
            No guarantee is made for any price information. See stores for final prices and details.
          </Typography>

          <Typography 
            variant="body2" 
            color="text.secondary" 
            sx={{ 
              maxWidth: '800px', 
              mx: 'auto',
              lineHeight: 1.7
            }}
          >
            All other content © {currentYear} MtgDiscovery. 
            Portions of the materials used are property of Wizards of the Coast. ©Wizards of the Coast LLC.
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};
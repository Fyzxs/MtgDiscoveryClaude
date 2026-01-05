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
            variant="body2"
            color="text.secondary"
            paragraph
            sx={{
              maxWidth: '800px',
              mx: 'auto',
              lineHeight: 1.7
            }}
          >
            MtgDiscovery is unofficial Fan Content permitted under the Fan Content Policy.
            Not approved/endorsed by Wizards.
            Portions of the materials used are property of Wizards of the Coast. ©Wizards of the Coast LLC.
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};
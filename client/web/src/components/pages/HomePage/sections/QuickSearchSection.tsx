import { useNavigate } from 'react-router-dom';
import { Box, Typography, Stack } from '../../../atoms';
import { AppButton } from '../../../molecules/shared/AppButton';

export function QuickSearchSection() {
  const navigate = useNavigate();

  return (
    <section aria-labelledby="quick-links-heading">
      <Typography
        id="quick-links-heading"
        variant="h2"
        sx={{
          position: 'absolute',
          width: 1,
          height: 1,
          overflow: 'hidden',
          clip: 'rect(0, 0, 0, 0)',
        }}
      >
        Quick Links
      </Typography>

      <Box
        sx={{
          px: { xs: 2, md: 4 },
          py: { xs: 3, md: 4 },
          mx: 'auto',
          maxWidth: 600,
        }}
      >
        <Stack
          direction={{ xs: 'column', sm: 'row' }}
          spacing={2}
          justifyContent="center"
        >
          <AppButton
            variant="outlined"
            size="large"
            onClick={() => navigate('/search/cards')}
            sx={{ flex: { sm: 1 }, maxWidth: { sm: 180 } }}
          >
            Search Cards
          </AppButton>
          <AppButton
            variant="outlined"
            size="large"
            onClick={() => navigate('/sets')}
            sx={{ flex: { sm: 1 }, maxWidth: { sm: 180 } }}
          >
            Browse Sets
          </AppButton>
          <AppButton
            variant="outlined"
            size="large"
            onClick={() => navigate('/search/artists')}
            sx={{ flex: { sm: 1 }, maxWidth: { sm: 180 } }}
          >
            Search Artists
          </AppButton>
        </Stack>
      </Box>
    </section>
  );
}

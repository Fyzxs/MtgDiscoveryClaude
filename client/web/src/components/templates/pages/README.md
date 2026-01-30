# Page Templates

This directory contains page-level templates that provide consistent layout structures for common page types in the MTG Discovery application.

## BrowseTemplate

A reusable template for browse/listing pages that display collections of items (cards, sets, artists, etc.) with optional filtering, search, and pagination capabilities.

### Usage

```tsx
import { BrowseTemplate } from '../components/templates/pages';
import { FilterPanel } from '../components/organisms/filters/FilterPanel';
import { ResultsSummary } from '../components/molecules/shared/ResultsSummary';

<BrowseTemplate
  header={<Typography variant="h3">All Sets</Typography>}
  filters={<FilterPanel config={filterConfig} />}
  summary={<ResultsSummary current={10} total={617} label="sets" />}
  content={<ResponsiveGrid>{setCards}</ResponsiveGrid>}
  pagination={<Pagination count={10} />}
/>
```

### Template Structure

```
┌─────────────────────────────┐
│         Header              │ (optional)
├─────────────────────────────┤
│ ┌─────────┐ ┌─────────────┐ │
│ │ Sidebar │ │   Filters   │ │ (both optional)
│ │         │ ├─────────────┤ │
│ │         │ │   Summary   │ │ (optional)
│ │         │ ├─────────────┤ │
│ │         │ │   Content   │ │ (required)
│ │         │ ├─────────────┤ │
│ │         │ │ Pagination  │ │ (optional)
│ └─────────┘ └─────────────┘ │
└─────────────────────────────┘
```

### Props

- **header**: Page title, hero section, or any header content
- **filters**: FilterPanel or custom filtering interface
- **summary**: ResultsSummary or custom result counts/info
- **content**: Main grid/list of items (required)
- **pagination**: Pagination controls
- **sidebar**: Optional sidebar for additional filters or info
- **maxWidth**: Container max width (defaults to full width)
- **containerPadding**: Override default container spacing
- **containerSx**: Additional container styles

### Layout Behavior

- **Responsive**: Automatically adapts to mobile/desktop layouts
- **Sidebar**: On mobile, sidebar appears below content. On desktop, it's positioned to the left
- **Flexible**: All sections except content are optional
- **Semantic**: Uses proper ARIA labels and semantic HTML structure
- **Centered**: Header, filters, and pagination are centered by default

### Examples

See `BrowseTemplate.examples.tsx` for complete usage examples including:
- Basic browse page (AllSetsPage style)
- Browse with sidebar layout
- Minimal template usage

### Best Practices

1. **Content is Required**: Always provide the main content area
2. **Consistent Spacing**: Use default spacing unless customization is needed
3. **Responsive Components**: Ensure all child components are responsive
4. **Accessibility**: Include proper ARIA labels in filter and content components
5. **Performance**: Use React.memo for expensive child components

## SearchTemplate

A reusable template for search result pages that provides a focused search interface with prominent search input, optional filters, results display, and comprehensive state handling.

### Usage

```tsx
import { SearchTemplate } from '../components/templates/pages';

<SearchTemplate
  searchInput={
    <TextField
      placeholder="Search cards..."
      value={searchTerm}
      onChange={handleSearch}
      InputProps={{
        startAdornment: <SearchIcon />,
        endAdornment: loading && <CircularProgress size={20} />
      }}
    />
  }
  resultsSummary={
    <Typography>Found {results.length} cards matching "{searchTerm}"</Typography>
  }
  resultsContent={
    <ResponsiveGrid>
      {results.map(card => <CardDisplay key={card.id} card={card} />)}
    </ResponsiveGrid>
  }
  emptyState={
    <Typography>No cards found matching "{searchTerm}"</Typography>
  }
  isLoading={loading}
  isEmpty={results.length === 0}
/>
```

### Template Structure

```
┌─────────────────────────────┐
│      Search Input           │ (required, prominent)
├─────────────────────────────┤
│    Advanced Filters         │ (optional, collapsible)
├─────────────────────────────┤
│    Results Summary          │ (optional, search context)
├─────────────────────────────┤
│     Quick Filters           │ (optional, refinement)
├─────────────────────────────┤
│     Search Results          │ (required)
│        OR Loading           │
│        OR Empty State       │
│        OR Initial State     │
├─────────────────────────────┤
│      Pagination             │ (optional)
└─────────────────────────────┘
```

### Key Differences from BrowseTemplate

- **Search-First**: Search input is the primary interaction point
- **State Management**: Built-in handling for loading, empty, and initial states
- **Advanced Filters**: Optional and typically collapsible to keep search prominent
- **Context-Aware**: Results summary includes search context
- **Quick Refinement**: Optional quick filters for search refinement

### Props

- **searchInput**: Primary search input component (required)
- **advancedFilters**: Collapsible advanced filtering options (optional)
- **resultsSummary**: Search results summary with context (optional)
- **quickFilters**: Quick filter chips/buttons for refinement (optional)
- **resultsContent**: Main search results display (required)
- **loadingState**: Loading indicator component (optional)
- **emptyState**: No results found component (optional)
- **pagination**: Pagination controls (optional)
- **isLoading**: Boolean to control loading state display
- **isEmpty**: Boolean to control empty state display
- **showInitialState**: Boolean to show initial/instruction state
- **maxWidth**: Container max width (defaults to 'md' for search focus)
- **containerPadding**: Override default container spacing
- **containerSx**: Additional container styles

### State Management

The SearchTemplate handles different display states:

1. **Initial State** (`showInitialState: true`): Shows instructions or initial content
2. **Loading State** (`isLoading: true`): Shows loading indicator
3. **Empty State** (`isEmpty: true, isLoading: false`): Shows no results message
4. **Results State** (`!isLoading, !isEmpty, !showInitialState`): Shows search results

### Layout Behavior

- **Search Focus**: Search input is prominently positioned and always visible
- **Progressive Disclosure**: Advanced filters can be collapsed to reduce cognitive load
- **Responsive**: Adapts to mobile/desktop with appropriate spacing
- **State Transitions**: Smooth transitions between loading, empty, and results states
- **Accessibility**: Proper ARIA labels and semantic structure for search interfaces

### Examples

See `SearchTemplate.examples.tsx` for complete usage examples including:
- Card search with advanced filters and quick refinement
- Artist search with simplified interface
- Minimal search implementation

### Best Practices

1. **Search Input Focus**: Make search input prominent and accessible
2. **State Handling**: Provide meaningful loading and empty states
3. **Progressive Enhancement**: Start simple, add advanced features as needed
4. **Search Context**: Include search terms in results summaries
5. **Quick Actions**: Provide quick filters for common refinements
6. **Performance**: Debounce search input and memoize expensive results

## DetailTemplate

A reusable template for detail view pages that provides a comprehensive layout structure for individual item pages like card details, set details, artist details, etc.

### Usage

```tsx
import { DetailTemplate } from '../components/templates/pages';

<DetailTemplate
  breadcrumb={
    <Breadcrumbs>
      <Link to="/sets">Sets</Link>
      <Typography color="text.primary">Dominaria United</Typography>
    </Breadcrumbs>
  }
  header={
    <Box>
      <Typography variant="h2">Dominaria United</Typography>
      <Typography variant="subtitle1">Standard Legal Set</Typography>
    </Box>
  }
  heroSection={
    <Box sx={{ display: 'flex', gap: 3 }}>
      <SetIcon code="DMU" size="large" />
      <Box>
        <Typography>Release Date: September 9, 2022</Typography>
        <Typography>281 Cards</Typography>
      </Box>
    </Box>
  }
  mainContent={
    <Tabs value={tab} onChange={setTab}>
      <Tab label="Cards" />
      <Tab label="Statistics" />
    </Tabs>
  }
  sidebar={
    <Box>
      <Typography variant="h6">Quick Stats</Typography>
      <List>...</List>
    </Box>
  }
  relatedContent={
    <Box>
      <Typography variant="h6">Related Sets</Typography>
      <Grid>...</Grid>
    </Box>
  }
  actions={
    <>
      <Button variant="contained">View All Cards</Button>
      <Button variant="outlined">Track Set</Button>
    </>
  }
  layout="sidebar"
/>
```

### Template Structure

```
┌─────────────────────────────┐
│    Breadcrumb Navigation    │ (optional)
├─────────────────────────────┤
│      Main Header            │ (required)
├─────────────────────────────┤
│      Hero Section           │ (optional)
├─────────────────────────────┤
│ ┌─────────────┐ ┌─────────┐ │
│ │ Main Content│ │ Sidebar │ │ (sidebar optional)
│ │             │ │         │ │
│ │   (Tabs,    │ │(Related │ │
│ │ Sections,   │ │ Items,  │ │
│ │ Details)    │ │ Quick   │ │
│ │             │ │Actions) │ │
│ └─────────────┘ └─────────┘ │
├─────────────────────────────┤
│    Related Content          │ (optional)
├─────────────────────────────┤
│      Actions Area           │ (optional)
└─────────────────────────────┘
```

### Key Features

- **Flexible Layout**: Single column or sidebar layout options
- **Responsive Design**: Sidebar moves to bottom on mobile (configurable)
- **Hero Section**: Prominent area for main image and key stats
- **Tab Organization**: Main content area perfect for tabbed sections
- **Related Content**: Dedicated area for recommendations and similar items
- **Action Areas**: Consistent placement for user interaction buttons
- **Breadcrumb Navigation**: Context-aware navigation for user orientation

### Props

- **breadcrumb**: Breadcrumb navigation for context and back navigation (optional)
- **header**: Main header with title, subtitle, and primary information (required)
- **heroSection**: Hero area for main image, key stats, and primary information (optional)
- **mainContent**: Main content area with tabs, sections, or detailed information (required)
- **sidebar**: Sidebar content for related items, quick actions, or metadata (optional)
- **relatedContent**: Related content section for similar items or recommendations (optional)
- **actions**: Actions area for save, share, collection actions, etc. (optional)
- **maxWidth**: Container max width (defaults to full width)
- **containerPadding**: Override default container spacing
- **containerSx**: Additional container styles
- **layout**: Layout mode - 'single' or 'sidebar' (defaults to 'single')
- **mobileSidebar**: Whether to show sidebar on mobile (defaults to false, moves to bottom)

### Layout Modes

#### Single Column Layout (`layout="single"`)
- Main content takes full width
- Sidebar is ignored
- Perfect for simple detail pages without auxiliary content

#### Sidebar Layout (`layout="sidebar"`)
- Main content and sidebar positioned side by side on desktop
- Sidebar moves below main content on mobile (unless `mobileSidebar=true`)
- Ideal for complex detail pages with related information

### Layout Behavior

- **Responsive**: Automatically adapts layout based on screen size
- **Mobile-First**: Optimized for mobile viewing with desktop enhancements
- **Flexible Sections**: All sections except header and mainContent are optional
- **Semantic Structure**: Uses proper HTML5 semantic elements (header, main, aside, nav, section)
- **Accessibility**: Includes ARIA labels and proper navigation structure

### Common Use Cases

#### Card Detail Pages
- **Breadcrumb**: Card search → Lightning Bolt
- **Header**: Card name and basic info
- **Hero**: Card image and quick stats
- **Main Content**: Tabs for printings, rules text, price history
- **Sidebar**: Legality, collection status, quick actions
- **Related**: Similar cards, other versions
- **Actions**: Add to collection, share, export

#### Set Detail Pages
- **Breadcrumb**: Sets → Dominaria United
- **Header**: Set name and description
- **Hero**: Set symbol and key information
- **Main Content**: Cards grid with filtering/sorting
- **Sidebar**: Set statistics, collection progress
- **Related**: Other sets in block/cycle
- **Actions**: Track set, view spoiler, share

#### Artist Detail Pages
- **Breadcrumb**: Artists → Rebecca Guay
- **Header**: Artist name and description
- **Hero**: Artist photo and statistics
- **Main Content**: Artwork gallery with filters
- **Sidebar**: Popular cards, art style tags
- **Related**: Similar artists
- **Actions**: Follow artist, view all artwork

### Examples

See `DetailTemplate.examples.tsx` for complete usage examples including:
- Card detail page with full feature set
- Set detail page with statistics sidebar
- Artist detail page with gallery and related content

### Best Practices

1. **Header Clarity**: Make the main subject clear and prominent
2. **Hero Usage**: Use hero section for the most important visual/stats
3. **Content Organization**: Use tabs or sections for organizing detailed content
4. **Sidebar Purpose**: Keep sidebar focused on quick actions and related info
5. **Mobile Experience**: Consider mobile layout when designing sidebar content
6. **Breadcrumb Context**: Provide clear navigation context for user orientation
7. **Action Accessibility**: Ensure all action buttons are keyboard accessible
8. **Related Content**: Include relevant recommendations to encourage exploration
9. **Performance**: Lazy load related content and use React.memo for expensive components
10. **Consistency**: Follow established patterns from other detail pages in the application
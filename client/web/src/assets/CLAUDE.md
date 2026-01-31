# Assets Folder

## Purpose
Static assets (images, icons, SVG files) used throughout the application. Currently minimal — only React logo SVG.

## Organization

```
assets/
  └─ react.svg          (Static SVG files)
```

## Naming
- Use kebab-case for asset filenames: `logo.svg`, `icon-card.svg`
- Descriptive names that indicate the asset's purpose

## Usage
Import assets directly in components:

```typescript
import reactLogo from '@/assets/react.svg'
```

Then reference in JSX:

```tsx
<img src={reactLogo} alt="React" />
```

## Guidelines

- ✓ Keep file sizes small (optimize SVGs, use WebP for images where possible)
- ✓ Always provide `alt` text for images
- ✓ SVG files should be clean and properly formatted
- ✓ Use this folder ONLY for static assets, not dynamically generated content

## When Not to Use

- **Component styles**: Use `/styles/` instead
- **Icon components**: Create icon components in `/components/atoms/` instead
- **Fonts**: Configure via theme or CSS imports
- **Generated images**: These should be fetched from backend

See: `assets/` directory for current asset examples.

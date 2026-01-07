import React, { useMemo, useRef, useEffect } from 'react';
import { Box } from '../../atoms';
import { PageTab } from '../../atoms/Binder';

interface BinderPageTabsProps {
  /** Current page number (1-indexed) */
  currentPage: number;
  /** Total number of pages */
  totalPages: number;
  /** Callback when a tab is clicked */
  onPageSelect: (page: number) => void;
  /** Whether in book mode (affects click behavior) */
  bookMode?: boolean;
}

/** Fixed spacing between tabs in pixels */
const TAB_HEIGHT = 26;

/**
 * Tab navigation for binder pages.
 * Tabs stack from top, wrapping to additional columns for large sets.
 */
export const BinderPageTabs: React.FC<BinderPageTabsProps> = ({
  currentPage,
  totalPages,
  onPageSelect,
  bookMode = false
}) => {
  // In book mode, clicking a tab navigates to the spread containing that page
  const handleTabClick = (pageNum: number) => {
    if (bookMode && pageNum > 1) {
      // Spreads are: [1], [2,3], [4,5], [6,7], etc.
      // Navigate to the even page of the spread
      const spreadStart = pageNum % 2 === 0 ? pageNum : pageNum - 1;
      onPageSelect(spreadStart);
    } else {
      onPageSelect(pageNum);
    }
  };

  // Determine which pages are currently displayed
  const lastDisplayedPage = useMemo(() => {
    if (bookMode && currentPage > 1) {
      // In book mode, spreads are [1], [2,3], [4,5], etc.
      const first = currentPage % 2 === 0 ? currentPage : currentPage - 1;
      return Math.min(first + 1, totalPages);
    }
    return currentPage;
  }, [currentPage, totalPages, bookMode]);

  // Left side: pages already viewed (before current)
  // Right side: pages not yet viewed (after current)
  const { leftTabs, rightTabs } = useMemo(() => {
    const left: number[] = [];
    const right: number[] = [];

    for (let i = 1; i <= totalPages; i++) {
      if (i <= lastDisplayedPage) {
        left.push(i);
      } else {
        right.push(i);
      }
    }

    return { leftTabs: left, rightTabs: right };
  }, [totalPages, lastDisplayedPage]);

  // Simple vertical position for single column
  const getTabTopOffset = (index: number) => index * TAB_HEIGHT;

  // Ref for left scroll container to auto-scroll to active tab
  const leftScrollRef = useRef<HTMLDivElement>(null);

  // Scroll left container to show active tab when page changes
  useEffect(() => {
    if (leftScrollRef.current && leftTabs.length > 0) {
      // Active tab is at the bottom of the left list
      const activeTabIndex = leftTabs.length - 1;
      const activeTabTop = activeTabIndex * TAB_HEIGHT;
      const containerHeight = leftScrollRef.current.clientHeight;

      // Scroll so active tab is visible (centered if possible)
      const scrollTarget = Math.max(0, activeTabTop - containerHeight / 2 + TAB_HEIGHT / 2);
      leftScrollRef.current.scrollTo({
        top: scrollTarget,
        behavior: 'smooth'
      });
    }
  }, [currentPage, leftTabs.length]);

  // Common scrollable container styles
  // Height matches binder page aspect ratio (3:4) at various widths
  const scrollContainerSx = {
    width: 32,
    maxHeight: { xs: 480, sm: 600, md: 733, lg: 800 },
    overflowY: 'auto',
    overflowX: 'hidden',
    flexShrink: 0,
    alignSelf: 'flex-start',
    // Hide scrollbar but keep functionality
    scrollbarWidth: 'none',
    '&::-webkit-scrollbar': { display: 'none' }
  };

  // Render left tabs (pages already viewed)
  const leftTabsElement = leftTabs.length > 0 ? (
    <Box ref={leftScrollRef} sx={scrollContainerSx}>
      <Box sx={{ position: 'relative', height: leftTabs.length * TAB_HEIGHT }}>
        {leftTabs.map((pageNum, index) => (
          <PageTab
            key={`left-${pageNum}`}
            pageNumber={pageNum}
            isActive={pageNum === currentPage || (bookMode && pageNum >= lastDisplayedPage - 1 && pageNum <= lastDisplayedPage)}
            onClick={() => handleTabClick(pageNum)}
            side="left"
            topOffset={getTabTopOffset(index)}
          />
        ))}
      </Box>
    </Box>
  ) : null;

  // Render right tabs (pages not yet viewed)
  const rightTabsElement = rightTabs.length > 0 ? (
    <Box sx={scrollContainerSx}>
      <Box sx={{ position: 'relative', height: rightTabs.length * TAB_HEIGHT }}>
        {rightTabs.map((pageNum, index) => (
          <PageTab
            key={`right-${pageNum}`}
            pageNumber={pageNum}
            isActive={false}
            onClick={() => handleTabClick(pageNum)}
            side="right"
            topOffset={getTabTopOffset(index)}
          />
        ))}
      </Box>
    </Box>
  ) : null;

  return { leftTabs: leftTabsElement, rightTabs: rightTabsElement };
};

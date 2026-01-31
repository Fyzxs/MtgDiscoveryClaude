/**
 * Date formatting utilities for consistent date display across the application
 */

/**
 * Formats a date string to 'MMM yyyy' format (e.g., 'Jan 2024')
 * Used for release dates in card overlays and metadata
 */
export const formatReleaseDate = (dateString?: string | null): string => {
  if (!dateString) return '';
  
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return '';
    
    return date.toLocaleDateString('en-US', { 
      month: 'short', 
      year: 'numeric' 
    });
  } catch {
    return '';
  }
};

/**
 * Formats a date string to 'MM/DD/YYYY' format
 * Used for ruling dates and other detailed date displays
 */
export const formatRulingDate = (dateString?: string | null): string => {
  if (!dateString) return '';
  
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return '';
    
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit'
    });
  } catch {
    return '';
  }
};

/**
 * Formats a date string to 'MMMM DD, YYYY' format (e.g., 'January 15, 2024')
 * Used for full date displays
 */
export const formatFullDate = (dateString?: string | null): string => {
  if (!dateString) return '';
  
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return '';
    
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  } catch {
    return '';
  }
};

/**
 * Formats a date string to a relative time format (e.g., '2 days ago', 'in 3 months')
 * Useful for showing how recent something is
 */
export const formatRelativeDate = (dateString?: string | null): string => {
  if (!dateString) return '';
  
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return '';
    
    const now = new Date();
    const diffTime = now.getTime() - date.getTime();
    const diffDays = Math.floor(diffTime / (1000 * 60 * 60 * 24));
    
    if (diffDays === 0) return 'Today';
    if (diffDays === 1) return 'Yesterday';
    if (diffDays === -1) return 'Tomorrow';
    if (diffDays > 0 && diffDays < 7) return `${diffDays} days ago`;
    if (diffDays > 0 && diffDays < 30) return `${Math.floor(diffDays / 7)} weeks ago`;
    if (diffDays > 0 && diffDays < 365) return `${Math.floor(diffDays / 30)} months ago`;
    if (diffDays < 0 && diffDays > -7) return `in ${Math.abs(diffDays)} days`;
    if (diffDays < 0 && diffDays > -30) return `in ${Math.floor(Math.abs(diffDays) / 7)} weeks`;
    if (diffDays < 0 && diffDays > -365) return `in ${Math.floor(Math.abs(diffDays) / 30)} months`;
    
    return formatReleaseDate(dateString);
  } catch {
    return '';
  }
};

/**
 * Parses a date string and returns a Date object
 * Returns null if the date is invalid
 */
export const parseDate = (dateString?: string | null): Date | null => {
  if (!dateString) return null;
  
  try {
    const date = new Date(dateString);
    if (isNaN(date.getTime())) return null;
    return date;
  } catch {
    return null;
  }
};

/**
 * Checks if a date string represents a valid date
 */
export const isValidDate = (dateString?: string | null): boolean => {
  return parseDate(dateString) !== null;
};
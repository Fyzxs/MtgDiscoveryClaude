import React from 'react';

interface CardProps {
  children: React.ReactNode;
  className?: string;
  variant?: 'default' | 'elevated' | 'outlined';
  padding?: 'none' | 'sm' | 'md' | 'lg';
}

export const Card: React.FC<CardProps> = ({ 
  children, 
  className = '', 
  variant = 'default',
  padding = 'md'
}) => {
  const baseClasses = 'bg-white dark:bg-gray-900 transition-all duration-200';
  
  const variantClasses = {
    default: 'border border-gray-200 dark:border-gray-800 rounded-lg',
    elevated: 'border border-gray-200 dark:border-gray-800 rounded-lg shadow-lg hover:shadow-xl',
    outlined: 'border-2 border-gray-300 dark:border-gray-700 rounded-lg'
  };

  const paddingClasses = {
    none: '',
    sm: 'p-4',
    md: 'p-6',
    lg: 'p-8'
  };

  return (
    <div className={`${baseClasses} ${variantClasses[variant]} ${paddingClasses[padding]} ${className}`}>
      {children}
    </div>
  );
};
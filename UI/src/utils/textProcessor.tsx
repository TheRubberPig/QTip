import React from 'react';

const EMAIL_REGEX = /([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})/g;

export const highlightPii = (text: string): React.ReactNode[] => {
  const parts = text.split(EMAIL_REGEX);
  
  return parts.map((part, i): React.ReactNode => {
    
    if (part.match(EMAIL_REGEX)) {
      return (
        <span key={i} className="relative group cursor-help pointer-events-auto"> 
          
          <span className="border-b-2 border-red-500 border-dashed decoration-wavy">
            {part}
          </span>
          
          {/* Tooltip */}
          <span className="absolute top-full left-1/2 -translate-x-1/2 mt-1 px-2 py-1 bg-gray-800 text-white text-xs rounded opacity-0 group-hover:opacity-100 whitespace-nowrap pointer-events-none z-50">
            PII - Email Address
          </span>
        </span>
      );
    }
    // Return regular text wrapped in a span to maintain layout parity
    return <span key={i}>{part}</span>;
  });
};
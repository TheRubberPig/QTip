import React from 'react';
import { highlightPii } from '../utils/textProcessor';

interface SecureInputProps {
  value: string;
  onChange: (val: string) => void;
}

export const SecureInput: React.FC<SecureInputProps> = ({ value, onChange }) => {
  return (
    <div className="relative w-full h-64 border rounded-md bg-gray-50 overflow-hidden font-mono text-base">
        {/* Input */}
      <textarea
        className="absolute inset-0 w-full h-full p-4 bg-transparent text-transparent caret-gray-800 resize-none focus:outline-none z-0 leading-relaxed"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        spellCheck="false"
        placeholder="Enter text with emails..."
      />

      {/* Highlight */}
      <div className="absolute inset-0 p-4 whitespace-pre-wrap break-words pointer-events-none z-10 leading-relaxed text-gray-800">
        {highlightPii(value)}
      </div>
      
    </div>
  );
};
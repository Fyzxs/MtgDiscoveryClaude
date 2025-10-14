import Box from '../Box';

interface ManaSymbolProps {
  symbol: string;
  size?: 'small' | 'medium' | 'large';
  className?: string;
}

export const ManaSymbol = ({ 
  symbol, 
  size = 'medium',
  className = '' 
}: ManaSymbolProps) => {
  const getSizeValue = () => {
    switch (size) {
      case 'small': return '1rem';
      case 'medium': return '1.25rem';
      case 'large': return '1.5rem';
      default: return '1.25rem';
    }
  };

  const getSymbolClass = (s: string): string => {
    // Remove braces and convert to lowercase for class names
    const sym = s.replace(/{|}/g, '').toLowerCase();
    
    // Handle special symbols
    switch (sym) {
      // Basic mana
      case 'w': return 'ms-w';
      case 'u': return 'ms-u';
      case 'b': return 'ms-b';
      case 'r': return 'ms-r';
      case 'g': return 'ms-g';
      case 'c': return 'ms-c';
      
      // Generic mana
      case '0': return 'ms-0';
      case '1': return 'ms-1';
      case '2': return 'ms-2';
      case '3': return 'ms-3';
      case '4': return 'ms-4';
      case '5': return 'ms-5';
      case '6': return 'ms-6';
      case '7': return 'ms-7';
      case '8': return 'ms-8';
      case '9': return 'ms-9';
      case '10': return 'ms-10';
      case '11': return 'ms-11';
      case '12': return 'ms-12';
      case '13': return 'ms-13';
      case '14': return 'ms-14';
      case '15': return 'ms-15';
      case '16': return 'ms-16';
      case '17': return 'ms-17';
      case '18': return 'ms-18';
      case '19': return 'ms-19';
      case '20': return 'ms-20';
      case 'x': return 'ms-x';
      case 'y': return 'ms-y';
      case 'z': return 'ms-z';
      
      // Hybrid mana
      case 'w/u': case 'wu': return 'ms-wu';
      case 'w/b': case 'wb': return 'ms-wb';
      case 'u/b': case 'ub': return 'ms-ub';
      case 'u/r': case 'ur': return 'ms-ur';
      case 'b/r': case 'br': return 'ms-br';
      case 'b/g': case 'bg': return 'ms-bg';
      case 'r/g': case 'rg': return 'ms-rg';
      case 'r/w': case 'rw': return 'ms-rw';
      case 'g/w': case 'gw': return 'ms-gw';
      case 'g/u': case 'gu': return 'ms-gu';
      
      // Phyrexian mana
      case 'w/p': case 'wp': return 'ms-wp';
      case 'u/p': case 'up': return 'ms-up';
      case 'b/p': case 'bp': return 'ms-bp';
      case 'r/p': case 'rp': return 'ms-rp';
      case 'g/p': case 'gp': return 'ms-gp';
      case 'p': return 'ms-p';
      
      // Hybrid phyrexian
      case 'w/u/p': case 'wup': return 'ms-wup';
      case 'w/b/p': case 'wbp': return 'ms-wbp';
      case 'u/b/p': case 'ubp': return 'ms-ubp';
      case 'u/r/p': case 'urp': return 'ms-urp';
      case 'b/r/p': case 'brp': return 'ms-brp';
      case 'b/g/p': case 'bgp': return 'ms-bgp';
      case 'r/g/p': case 'rgp': return 'ms-rgp';
      case 'r/w/p': case 'rwp': return 'ms-rwp';
      case 'g/w/p': case 'gwp': return 'ms-gwp';
      case 'g/u/p': case 'gup': return 'ms-gup';
      
      // Generic hybrid
      case '2/w': case '2w': return 'ms-2w';
      case '2/u': case '2u': return 'ms-2u';
      case '2/b': case '2b': return 'ms-2b';
      case '2/r': case '2r': return 'ms-2r';
      case '2/g': case '2g': return 'ms-2g';
      
      // Snow mana
      case 's': return 'ms-s';
      
      // Other symbols
      case 't': return 'ms-tap';
      case 'q': return 'ms-untap';
      case 'e': return 'ms-e';
      case 'infinity': return 'ms-infinity';
      case 'half': case '½': return 'ms-half';
      case '100': return 'ms-100';
      case '1000000': return 'ms-1000000';
      
      default:
        // If it's a number, use the number class
        if (!isNaN(Number(sym))) {
          return `ms-${sym}`;
        }
        // Default to generic
        return 'ms-x';
    }
  };

  const sizeValue = getSizeValue();
  const symbolClass = getSymbolClass(symbol);

  return (
    <Box
      component="i"
      className={`ms ms-cost ${symbolClass} ${className}`}
      sx={{
        fontSize: sizeValue,
        lineHeight: 1,
        display: 'inline-block',
        verticalAlign: 'middle'
      }}
      title={`Mana: ${symbol.replace(/{|}/g, '')}`}
    />
  );
};
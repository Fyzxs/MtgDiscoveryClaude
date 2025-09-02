import React, { useState } from 'react';
import { useLazyQuery } from '@apollo/client/react';
import { GET_CARDS_BY_IDS } from '../../graphql/queries/cards';
import type { Card, CardContext } from '../../types/card';

// Import all components
import { RarityBadge } from '../atoms/Cards/RarityBadge';
import { ManaSymbol } from '../atoms/Cards/ManaSymbol';
import { CardImage } from '../atoms/Cards/CardImage';
import { PriceDisplay } from '../atoms/shared/PriceDisplay';
import { ExternalLinkIcon } from '../atoms/shared/ExternalLinkIcon';
import { CollectorNumber } from '../atoms/Cards/CollectorNumber';
import { SetIcon } from '../atoms/Sets/SetIcon';

import { ManaCost } from '../molecules/Cards/ManaCost';
import { ArtistInfo } from '../molecules/Cards/ArtistInfo';
import { CardLinks } from '../molecules/Cards/CardLinks';
import { CollectorInfo } from '../molecules/Cards/CollectorInfo';
import { CardMetadata } from '../molecules/Cards/CardMetadata';

import { CardDisplay } from '../organisms/CardDisplayResponsive';
import { CardCompact } from '../organisms/CardCompactResponsive';

export const CardDemo: React.FC = () => {
  const [cardId, setCardId] = useState('d5c83259-9b90-47c2-b48e-c7d78519e792');
  const [inputValue, setInputValue] = useState(cardId);
  const [context, setContext] = useState<CardContext>({
    isOnSetPage: false,
    isOnArtistPage: false,
    isOnCardPage: false,
    showCollectorInfo: true
  });

  // Mock card data for immediate display
  const mockCard: Card = {
    id: 'd5c83259-9b90-47c2-b48e-c7d78519e792',
    name: 'Master of Arms',
    manaCost: '{2}{W}',
    typeLine: 'Creature — Human Soldier',
    oracleText: 'First strike\n{1}{W}: Tap target creature blocking Master of Arms.',
    power: '2',
    toughness: '2',
    rarity: 'uncommon',
    setCode: 'WTH',
    setName: 'Weatherlight',
    collectorNumber: '20',
    artist: 'Dan Frazier',
    artistIds: ['059bba56-5feb-42e4-8c2e-e2f1e6ba11f9'],
    releasedAt: '2024-08-01',
    prices: {
      usd: '0.19',
      usdFoil: null,
      usdEtched: null,
      eur: '0.15',
      eurFoil: null,
      tix: null
    },
    imageUris: {
      small: 'https://cards.scryfall.io/small/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.jpg',
      normal: 'https://cards.scryfall.io/normal/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.jpg',
      large: 'https://cards.scryfall.io/large/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.jpg',
      png: 'https://cards.scryfall.io/png/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.png',
      artCrop: 'https://cards.scryfall.io/art_crop/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.jpg',
      borderCrop: 'https://cards.scryfall.io/border_crop/front/d/5/d5c83259-9b90-47c2-b48e-c7d78519e792.jpg'
    },
    scryfallUri: 'https://scryfall.com/card/wth/20/master-of-arms',
    purchaseUris: {
      tcgplayer: 'https://tcgplayer.com/product/1234',
      cardmarket: 'https://cardmarket.com/product/5678',
      cardhoarder: null
    },
    relatedUris: {
      gatherer: 'https://gatherer.wizards.com/pages/card/details.aspx?multiverseid=1234',
      tcgplayerInfiniteArticles: '',
      tcgplayerInfiniteDecks: '',
      edhrec: 'https://edhrec.com/cards/master-of-arms'
    }
  };

  const [getCards, { loading, error, data }] = useLazyQuery(GET_CARDS_BY_IDS);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setCardId(inputValue);
    getCards({ variables: { ids: { cardIds: [inputValue] } } });
  };

  const card = (data as any)?.cardsById?.data?.[0] || mockCard;

  // Navigation handlers
  const handleCardClick = () => {
    // In a real app, this would use React Router or Next.js routing
  };

  const handleSetClick = () => {
    // In a real app, this would use React Router or Next.js routing
  };

  const handleArtistClick = () => {
    // In a real app, this would use React Router or Next.js routing
  };

  return (
    <div className="min-h-screen bg-gray-950 text-white p-4 sm:p-6 lg:p-8">
      <div className="max-w-7xl mx-auto space-y-8">
        <div>
          <h1 className="text-4xl font-bold mb-2">MTG Card Component Demo</h1>
          <p className="text-gray-400">
            Interactive demonstration of all card display components at atomic, molecular, and organism levels
          </p>
        </div>

        {/* Card ID Input */}
        <div className="bg-gray-900 rounded-lg p-6">
          <form onSubmit={handleSubmit} className="flex gap-4">
            <input
              type="text"
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              placeholder="Enter card ID (e.g., d5c83259-9b90-47c2-b48e-c7d78519e792)"
              className="flex-1 px-4 py-2 bg-gray-800 border border-gray-700 rounded-lg text-white placeholder-gray-500 focus:outline-none focus:border-blue-500"
            />
            <button
              type="submit"
              disabled={loading}
              className="px-6 py-2 bg-blue-600 hover:bg-blue-700 disabled:bg-gray-700 rounded-lg font-semibold transition-colors"
            >
              {loading ? 'Loading...' : 'Load Card'}
            </button>
          </form>
          {error && (
            <p className="mt-2 text-red-500 text-sm">Error loading card: {error.message}</p>
          )}
        </div>

        {/* Context Controls */}
        <div className="bg-gray-900 rounded-lg p-6">
          <h2 className="text-xl font-semibold mb-4">Context Settings</h2>
          <div className="grid grid-cols-2 md:grid-cols-4 gap-4">
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={context.isOnSetPage}
                onChange={(e) => setContext({...context, isOnSetPage: e.target.checked})}
                className="rounded border-gray-600 bg-gray-700 text-blue-600"
              />
              <span className="text-sm">On Set Page</span>
            </label>
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={context.isOnArtistPage}
                onChange={(e) => setContext({...context, isOnArtistPage: e.target.checked})}
                className="rounded border-gray-600 bg-gray-700 text-blue-600"
              />
              <span className="text-sm">On Artist Page</span>
            </label>
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={context.isOnCardPage}
                onChange={(e) => setContext({...context, isOnCardPage: e.target.checked})}
                className="rounded border-gray-600 bg-gray-700 text-blue-600"
              />
              <span className="text-sm">On Card Page</span>
            </label>
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                checked={context.showCollectorInfo}
                onChange={(e) => setContext({...context, showCollectorInfo: e.target.checked})}
                className="rounded border-gray-600 bg-gray-700 text-blue-600"
              />
              <span className="text-sm">Show Collector Info</span>
            </label>
          </div>
        </div>

        {/* Atoms Section */}
        <div className="bg-gray-900 rounded-lg p-4 sm:p-6">
          <h2 className="text-xl sm:text-2xl font-semibold mb-4 sm:mb-6">Atoms</h2>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4 sm:gap-6">
            {/* Rarity Badge */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">RarityBadge</h3>
              <div className="flex gap-2">
                <RarityBadge rarity="common" />
                <RarityBadge rarity="uncommon" />
                <RarityBadge rarity="rare" />
                <RarityBadge rarity="mythic" />
              </div>
            </div>

            {/* Mana Symbols */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">ManaSymbol</h3>
              <div className="flex gap-2">
                <ManaSymbol symbol="{W}" />
                <ManaSymbol symbol="{U}" />
                <ManaSymbol symbol="{B}" />
                <ManaSymbol symbol="{R}" />
                <ManaSymbol symbol="{G}" />
                <ManaSymbol symbol="{2}" />
              </div>
            </div>

            {/* Price Display */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">PriceDisplay</h3>
              <div className="space-y-1">
                <PriceDisplay price="2.50" />
                <PriceDisplay price="15.00" />
                <PriceDisplay price="125.00" />
              </div>
            </div>

            {/* Collector Number */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CollectorNumber</h3>
              <CollectorNumber number={card.collectorNumber} setCode={card.setCode} />
            </div>

            {/* External Links */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">ExternalLinkIcon</h3>
              <div className="flex gap-2">
                <ExternalLinkIcon type="scryfall" url={card.scryfallUri} />
                <ExternalLinkIcon type="tcgplayer" url={card.purchaseUris?.tcgplayer} />
                <ExternalLinkIcon type="cardmarket" url={card.purchaseUris?.cardmarket} />
              </div>
            </div>

            {/* Card Image Small */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CardImage (small)</h3>
              <CardImage imageUris={card.imageUris} cardName={card.name} size="small" />
            </div>

            {/* Set Icon */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">SetIcon</h3>
              <div className="flex gap-2 items-center">
                <SetIcon setCode="lea" size="small" />
                <SetIcon setCode="wth" size="medium" rarity="uncommon" />
                <SetIcon setCode="mh2" size="large" rarity="rare" />
                <SetIcon setCode="neo" size="xlarge" rarity="mythic" />
              </div>
            </div>
          </div>
        </div>

        {/* Molecules Section */}
        <div className="bg-gray-900 rounded-lg p-6">
          <h2 className="text-2xl font-semibold mb-6">Molecules</h2>
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            {/* Mana Cost */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">ManaCost</h3>
              <ManaCost manaCost={card.manaCost} />
            </div>

            {/* Artist Info */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">ArtistInfo</h3>
              <ArtistInfo 
                artist={card.artist} 
                artistIds={card.artistIds} 
                context={context}
                onArtistClick={handleArtistClick}
              />
            </div>

            {/* Card Links */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CardLinks</h3>
              <CardLinks
                scryfallUri={card.scryfallUri}
                purchaseUris={card.purchaseUris}
                relatedUris={card.relatedUris}
                cardName={card.name}
              />
            </div>

            {/* Collector Info */}
            <div className="bg-gray-800 p-4 rounded-lg">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CollectorInfo</h3>
              <CollectorInfo
                collectorNumber={card.collectorNumber}
                setCode={card.setCode}
                rarity={card.rarity}
              />
            </div>

            {/* Card Metadata */}
            <div className="bg-gray-800 p-4 rounded-lg col-span-2">
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CardMetadata</h3>
              <CardMetadata
                name={card.name}
                cardId={card.id}
                typeLine={card.typeLine}
                setName={card.setName}
                setCode={card.setCode}
                rarity={card.rarity}
                releasedAt={card.releasedAt}
                context={context}
                onCardClick={handleCardClick}
                onSetClick={handleSetClick}
              />
            </div>
          </div>
        </div>

        {/* Organisms Section */}
        <div className="bg-gray-900 rounded-lg p-6">
          <h2 className="text-2xl font-semibold mb-6">Organisms</h2>
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
            {/* Full Card Display */}
            <div>
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CardDisplay (Full)</h3>
              <CardDisplay 
                card={card} 
                context={context}
                onCardClick={handleCardClick}
                onSetClick={handleSetClick}
                onArtistClick={handleArtistClick}
              />
            </div>

            {/* Compact Card Display */}
            <div>
              <h3 className="text-sm font-semibold text-gray-400 mb-3">CardCompact (Grid View)</h3>
              <div className="max-w-sm">
                <CardCompact 
                  card={card} 
                  context={context}
                  onCardClick={handleCardClick}
                  onSetClick={handleSetClick}
                  onArtistClick={handleArtistClick}
                />
              </div>
            </div>
          </div>
        </div>

        {/* Multiple Cards Grid Demo */}
        <div className="bg-gray-900 rounded-lg p-4 sm:p-6">
          <h2 className="text-xl sm:text-2xl font-semibold mb-4 sm:mb-6">Grid Layout Demo</h2>
          <p className="text-sm text-gray-400 mb-4">Responsive grid: 1 column on mobile, 2 on tablet, 3 on desktop</p>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3 sm:gap-4">
            {[1, 2, 3].map((i) => (
              <CardCompact
                key={i}
                card={{
                  ...card,
                  rarity: i === 1 ? 'rare' : i === 2 ? 'mythic' : 'uncommon',
                  setCode: i === 1 ? 'mh2' : i === 2 ? 'neo' : 'wth',
                  prices: { ...card.prices, usd: i === 1 ? '131.78' : i === 2 ? '25.50' : '2.96' }
                }}
                context={context}
                onCardClick={handleCardClick}
                onSetClick={handleSetClick}
                onArtistClick={handleArtistClick}
              />
            ))}
          </div>
        </div>
      </div>
    </div>
  );
};
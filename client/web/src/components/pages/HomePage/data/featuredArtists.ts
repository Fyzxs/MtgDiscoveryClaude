export interface FeaturedArtistData {
  name: string;
  description: string;
  cardCount: number;
  sampleCardNames: string[];
  featuredCardName: string;
}

export const featuredArtists: FeaturedArtistData[] = [
  {
    name: 'Magali Villeneuve',
    description: 'Known for planeswalkers and iconic characters',
    cardCount: 150,
    sampleCardNames: [
      'Chandra, Torch of Defiance',
      'Narset, Enlightened Master',
      'Marchesa, the Black Rose',
      'Liliana, the Last Hope',
    ],
    featuredCardName: 'Chandra, Torch of Defiance',
  },
  {
    name: 'Seb McKinnon',
    description: 'Known for ethereal, dreamlike art style',
    cardCount: 100,
    sampleCardNames: ['Bedevil', 'Soulherder', 'Archon of Absolution', 'Damnation (STA)'],
    featuredCardName: 'Soulherder',
  },
  {
    name: 'Johannes Voss',
    description: 'Known for enchantments and angels',
    cardCount: 120,
    sampleCardNames: [
      'Gift of Orzhova',
      'Anguished Unmaking',
      'Restoration Angel',
      'Karlov of the Ghost Council',
    ],
    featuredCardName: 'Gift of Orzhova',
  },
  {
    name: 'Anson Maddocks',
    description: 'Original MTG artist known for iconic early creatures',
    cardCount: 55,
    sampleCardNames: [
      'Hurloon Minotaur',
      'Hypnotic Specter',
      'Sengir Vampire',
      'Animate Dead',
    ],
    featuredCardName: 'Hurloon Minotaur',
  },
  {
    name: 'Liz Danforth',
    description: 'Pioneer fantasy artist with distinctive Fallen Empires work',
    cardCount: 35,
    sampleCardNames: [
      'Hymn to Tourach',
      'Essence Vortex',
      'Orcish Veteran',
      'Elvish Hunter',
    ],
    featuredCardName: 'Hymn to Tourach',
  },
  {
    name: 'Julie Baroh',
    description: 'Original Alpha artist known for classic white cards',
    cardCount: 18,
    sampleCardNames: [
      'Veteran Bodyguard',
      'Personal Incarnation',
      'Purelace',
      'Thoughtlace',
    ],
    featuredCardName: 'Veteran Bodyguard',
  },
];

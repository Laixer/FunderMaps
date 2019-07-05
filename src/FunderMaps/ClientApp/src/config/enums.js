
/**
 * Configuration of Enums
 */

export const typeOptions = [ 
  {
    text: 'Aanvullend onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Monitoring',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Notitie',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Quickscan',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Onbekend',
    color: 'white',
    bgColor: '#3D5372'
  },
  {
    text: 'Sloopgrens onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Second opinion',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Archief onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Bouwkundig onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Funderingsadvies',
    color: 'white',
    bgColor: '#3D5372'
  },
  {
    text: 'Inspectieput',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Funderings onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Grondwater onderzoek',
    color: 'white',
    bgColor: '#3D5372'
  }
]

export const statusOptions = [ 
  {
    text: 'Todo',
    label: 'Nog te beoordelen',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Pending',
    label: 'In bewerking',
    bgColor: '#F5A623'
  }, 
  {
    text: 'Done',
    label: 'Afgerond',
    bgColor: '#28CC8B'
  }, 
  {
    text: 'Discarded',
    label: 'Afgevallen',
    bgColor: '#3D5372'
  }, 
  {
    text: 'PendingReview',
    label: 'Gecontroleerd',
    bgColor: '#3D5372'
  }, 
  {
    text: 'Rejected',
    label: 'Afgekeurd',
    bgColor: '#FF4E4E'
  } 
]

export const accessOptions = [ 
  {
    value: 'Public',
    label: 'Openbaar'
  }, 
  {
    value: 'Private',
    label: 'Gesloten'
  } 
]

export const foundationTypeOptions = [
  {
    value: 'Wood',
    text: 'Hout'
  }, 
  {
    value: 'Concrete',
    text: 'Beton'
  },
  {
    value: 'NoPile',
    text: 'Niet onderheid'
  },
  {
    value: 'WoodCharger',
    text: 'Hout met oplanger'
  },
  {
    value: 'WeightedPile',
    text: 'Verzwaardepuntpaal'
  },
  {
    value: 'Combined',
    text: 'Gecombineerd'
  },
  {
    value: 'SteelPile',
    text: 'Stalen buispalen'
  },
  {
    value: 'Other',
    text: 'Overig'
  },
  {
    value: 'Unknown',
    text: 'Onbekend'
  },
  {
    value: 'NoPileMasonry',
    text: 'Niet onderheid: gemetseld'
  },
  {
    value: 'NoPileStrips',
    text: 'Niet onderheid: stroken fundering'
  },
  {
    value: 'NoPileBearingFloor',
    text: 'Niet onderheid: fundering met dragen vloer'
  },
  {
    value: 'NoPileConcreteFloor',
    text: 'Niet onderheid: dragende betonvloer'
  },
  {
    value: 'NoPileSlit',
    text: 'Niet onderheid: slieten'
  },
  {
    value: 'WoodAmsterdam',
    text: 'Hout: Amsterdam fundering'
  },
  {
    value: 'WoodRotterdam',
    text: 'Hout: Rotterdam fundering'
  }
]

export const foundationQualityOptions = [
  {
    value: 'Bad',
    text: 'Slecht'
  }, 
  {
    value: 'Mediocre',
    text: 'Matig'
  }, 
  {
    value: 'Tolerable',
    text: 'Redelijk'
  },
  {
    value: 'Good',
    text: 'Goed'
  },
  {
    value: 'MediocreGood',
    text: 'Matig tot goed'
  },
  {
    value: 'MediocreBad',
    text: 'Matig tot slecht'
  }
]

export const substructureOptions = [
  {
    value: 'Cellar',
    text: 'Kelder'
  }, 
  { 
    value: 'Basement',
    text: 'Souterrain'
  }, 
  {
    value: 'Crawlspace',
    text: 'Kruipruimte'
  }, 
  { 
    value: 'None',
    text: 'Geen'
  }
]

export const foundationDamageCauseOptions = [
  {
    value: 'Drainage', 
    text: 'Bemaling'
  },
  {
    value: 'ConstructionFlaw',
    text: 'Constructieve fouten'
  },
  {
    value: 'Drystand',
    text: 'Droogstand (schimmels)'
  },
  {
    value: 'Overcharge',
    text: 'Overbelasting'
  },
  {
    value: 'OverchargeNegativeCling',
    text: 'Overbelasting (negatieve kleef)'
  },
  {
    value: 'NegativeCling',
    text: 'Negatieve kleef'
  },
  {
    value: 'BioInfection',
    text: 'Bacteriele aantasting'
  },
  {
    value: 'Unknown',
    text: 'Niet vermeld'
  },
  {
    value: 'FungusInfection',
    text: 'Bacterien en schimmels aantasting'
  }
]

export const enforcementTermOptions = [
  {
    text: '0-5 jaar',
    value: 'Term0_5'
  }, 
  {
    text: '5-10 jaar',
    value: 'Term5_10'
  }, 
  {
    text: '10-20 jaar',
    value: 'Term10_20'
  }, 
  {
    text: '5 jaar',
    value: 'Term5'
  }, 
  {
    text: '10 jaar',
    value: 'Term10'
  }, 
  {
    text: '15 jaar',
    value: 'Term15'
  }, 
  {
    text: '20 jaar',
    value: 'Term20'
  }, 
  {
    text: '25 jaar',
    value: 'Term25'
  }, 
  {
    text: '30 jaar',
    value: 'Term30'
  }
]

export const BaseMeasurementLevelOptions = [
  {
    value: 'NAP',
    text: 'NAP (Nederland)'
  }, 
  {
    value: 'TAW (België)',
    text: 'TAW'
  },
  {
    value: 'NN (Duitsland)',
    text: 'NN'
  }
]

export const FoundationRecoveryEvidenceTypeOptions = [
  {
    value: 'Permit',
    text: 'Vergunning'
  },
  {
    value: 'FoundationReport',
    text: 'Funderingsonderzoek'
  },
  {
    value: 'ArchiveReport',
    text: 'Archiefonderzoek'
  },
  {
    value: 'OwnerEvidence',
    text: 'Bewijs Eigenaar'
  }
]

export const FoundationRecoveryType = [
  {
    value: 'Table',
    text: 'Hersteld met tafelconstructie'
  },
  {
    value: 'BeamOnPile',
    text: 'Hersteld met randbalken op nieuwe palen'
  },
  {
    value: 'PileLowering',
    text: 'Paalkopverlaging'
  },
  {
    value: 'PileInWall',
    text: 'Hersteld met uit muren gedrukte palen'
  },
  {
    value: 'Injection',
    text: 'Verstevigen van de ondergrond door een injectie met kunsthars'
  },
  {
    value: 'Unknown',
    text: 'Onbekend'
  }
]

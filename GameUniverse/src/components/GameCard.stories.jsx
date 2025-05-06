import { GameCard } from './GameCard';

export default {
    title: 'Components/GameCard', 
    component: GameCard, 
};

export const Default = () => <GameCard title="Cyberpunk" genre="RPG" />;
export const Shooter = () => <GameCard title="Call of Duty" genre="Shooter" />;
export const Strategy = () => <GameCard title="Civilization" genre="Strategy" />;

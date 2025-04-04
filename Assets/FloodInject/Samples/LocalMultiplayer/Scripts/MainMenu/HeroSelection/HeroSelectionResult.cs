public record HeroSelectionResult(HeroSelectionResult.PlayerSelection[] PlayerSelections)
{
    public record PlayerSelection(int PlayerIndex, HeroTemplate SelectedHeroTemplate)
    {
        public int PlayerIndex { get; } = PlayerIndex;
        public HeroTemplate SelectedHeroTemplate { get; } = SelectedHeroTemplate;
    }

    public PlayerSelection[] PlayerSelections { get; } = PlayerSelections;
}
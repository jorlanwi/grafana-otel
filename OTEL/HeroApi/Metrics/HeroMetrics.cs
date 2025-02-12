using System.Diagnostics.Metrics;

namespace HeroApi.Metrics;

public class HeroMetrics
{
    private Counter<int> HerosReadCounter { get; }
    private Counter<int> HeroReadCounter { get; }
    private Counter<int> HeroAddedCounter { get; }
    private Counter<int> HeroDeletedCounter { get; }
    private Counter<int> HeroUpdatedCounter { get; }
    private UpDownCounter<int> TotalHeroUpDownCounter { get; }

    public HeroMetrics(IMeterFactory meterFactory, HeroTelemetry heroTelemetry)
    {
        var meter = meterFactory.Create(heroTelemetry.MeterName);

        HerosReadCounter = meter.CreateCounter<int>("heros-read", "Hero");
        HeroReadCounter = meter.CreateCounter<int>("hero-read", "Hero");
        HeroAddedCounter = meter.CreateCounter<int>("hero-added", "Hero");
        HeroDeletedCounter = meter.CreateCounter<int>("hero-deleted", "Hero");
        HeroUpdatedCounter = meter.CreateCounter<int>("hero-updated", "Hero");
        TotalHeroUpDownCounter = meter.CreateUpDownCounter<int>("total-Hero", "Hero");
    }

    public void ReadHeros() => HerosReadCounter.Add(1);
    public void ReadHero() => HeroReadCounter.Add(1);

    public void AddHero() {
        HeroAddedCounter.Add(1);
        TotalHeroUpDownCounter.Add(1);
    }
    public void DeleteHero()
    {
        HeroDeletedCounter.Add(1);
        TotalHeroUpDownCounter.Add(-1);
    }
    public void UpdateHero() => HeroUpdatedCounter.Add(1);
}

using MyWorkout.Api.Models;

namespace MyWorkout.Api.Data;

// Výchozí cviky, které se vloží do databáze při vytvoření tabulek.
// Hodnocení a zdroj se doplní ve Správě aplikace.
public static class ExerciseSeed
{
    public static readonly Exercise[] All =
    {
        new() { Id = 1, Name = "Bench press", MuscleGroup = "Hrudník", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Leh na rovné lavici, chodidla na zemi. Činku spouštěj kontrolovaně ke spodní části hrudníku a vytlač ji zpět nad ramena." },
        new() { Id = 2, Name = "Tlaky s jednoručkami na šikmé lavici", MuscleGroup = "Hrudník", Equipment = "Jednoručky", Difficulty = "Začátečník",
            Description = "Lavice skloněná asi na 30 stupňů. Jednoručky spouštěj k horní části hrudníku a tlač je nahoru nad ramena." },
        new() { Id = 3, Name = "Kliky", MuscleGroup = "Hrudník", Equipment = "Vlastní váha", Difficulty = "Začátečník",
            Description = "Dlaně o něco šíř než ramena, tělo v jedné linii od hlavy po paty. Spouštěj hrudník k zemi a vytlač se zpět." },
        new() { Id = 4, Name = "Kliky na bradlech", MuscleGroup = "Triceps", Equipment = "Vlastní váha", Difficulty = "Mírně pokročilý",
            Description = "Opora na bradlech s propnutými pažemi. Pokrč lokty, spusť se dolů a vytlač se zpět nahoru." },
        new() { Id = 5, Name = "Francouzský tlak", MuscleGroup = "Triceps", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Leh na lavici, činka nad hlavou. Pokrčením loktů ji spusť k čelu a propnutím loktů vrať zpět, lokty drž na místě." },
        new() { Id = 6, Name = "Dřep", MuscleGroup = "Nohy", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Činka na horní části zad, chodidla na šířku ramen. Jdi dolů do dřepu s rovnými zády a vytlač se zpět nahoru." },
        new() { Id = 7, Name = "Rumunský mrtvý tah", MuscleGroup = "Nohy", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Kolena lehce pokrčená, záda rovná. Předkloň se v kyčlích a spouštěj činku podél nohou, dokud cítíš protažení zadní strany stehen." },
        new() { Id = 8, Name = "Výpady", MuscleGroup = "Nohy", Equipment = "Jednoručky", Difficulty = "Začátečník",
            Description = "Udělej dlouhý krok vpřed a pokrč obě kolena, zadní koleno míří k zemi. Odraz se zpět do stoje a vystřídej nohy." },
        new() { Id = 9, Name = "Mrtvý tah", MuscleGroup = "Záda", Equipment = "Velká činka", Difficulty = "Pokročilý",
            Description = "Činka na zemi nad středem chodidla. S rovnými zády ji zvedni do stoje prací nohou a kyčlí a stejnou cestou ji polož zpět." },
        new() { Id = 10, Name = "Shyby", MuscleGroup = "Záda", Equipment = "Vlastní váha", Difficulty = "Mírně pokročilý",
            Description = "Vis na hrazdě nadhmatem. Přitáhni se bradou nad hrazdu a kontrolovaně se spusť do visu." },
        new() { Id = 11, Name = "Přítahy v předklonu", MuscleGroup = "Záda", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Předklon s rovnými zády, činka v natažených pažích. Přitahuj ji k břichu a lopatky stahuj k sobě." },
        new() { Id = 12, Name = "Stahování kladky", MuscleGroup = "Záda", Equipment = "Stroj", Difficulty = "Začátečník",
            Description = "Vsedě uchop tyč kladky na šířku ramen a stahuj ji k horní části hrudníku, pak ji pomalu pusť zpět." },
        new() { Id = 13, Name = "Tlaky nad hlavu", MuscleGroup = "Ramena", Equipment = "Velká činka", Difficulty = "Mírně pokročilý",
            Description = "Ve stoje drž činku u horní části hrudníku. Vytlač ji nad hlavu do propnutých paží a spusť zpět." },
        new() { Id = 14, Name = "Upažování s jednoručkami", MuscleGroup = "Ramena", Equipment = "Jednoručky", Difficulty = "Začátečník",
            Description = "Ve stoje zvedej jednoručky lehce pokrčenými pažemi do stran do výšky ramen a pomalu je spouštěj." },
        new() { Id = 15, Name = "Bicepsový zdvih", MuscleGroup = "Biceps", Equipment = "Jednoručky", Difficulty = "Začátečník",
            Description = "Ve stoje, lokty u těla. Ohýbej paže a zvedej jednoručky k ramenům, pak je pomalu spusť." },
        new() { Id = 16, Name = "Zkracovačky", MuscleGroup = "Břicho", Equipment = "Vlastní váha", Difficulty = "Začátečník",
            Description = "Leh na zádech, kolena pokrčená. Zvedni hlavu a ramena od země stažením břicha a pomalu se vrať zpět." },
    };
}

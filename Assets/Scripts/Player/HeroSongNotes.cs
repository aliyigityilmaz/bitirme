using UnityEngine;

/// <summary>
/// Bu statik sınıf, her kahramana (Hero) özgü 6 notalı şarkı dizilerini sağlar.
/// Her notanın değeri, o round’da slider’ın dolma süresini (ve dolayısıyla input timing’ini) belirler.
/// Böylece her kahramanın oyunda farklı bir ritmi ve tonu olur.
/// </summary>
public static class HeroSongNotes
{
    /// <summary>
    /// Verilen kahramanın id’sine göre özel şarkı notalarını döndürür.
    /// Eğer hero null ya da tanımlı değilse, varsayılan nota dizisini döndürür.
    /// </summary>
    public static float[] GetSongNotesForHero(Hero hero)
    {
        if (hero == null)
        {
            Debug.LogError("HeroSongNotes.GetSongNotesForHero: Gelen hero nesnesi null, varsayılan notalar kullanılıyor.");
            return GetDefaultSongNotes();
        }

        switch (hero.id)
        {
            case 1:
                return GetSongNotesForHero1();
            case 2:
                return GetSongNotesForHero2();
            case 3:
                return GetSongNotesForHero3();
            case 4:
                return GetSongNotesForHero4();
            default:
                Debug.LogWarning("HeroSongNotes: Kahraman id " + hero.id + " için özel şarkı notası tanımlanmamış, varsayılan notalar kullanılıyor.");
                return GetDefaultSongNotes();
        }
    }

    /// <summary>
    /// Kahraman 1 için 6 notalı özel şarkı dizisi.
    /// </summary>
    private static float[] GetSongNotesForHero1()
    {
        return new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
    }

    /// <summary>
    /// Kahraman 2 için 6 notalı özel şarkı dizisi.
    /// </summary>
    private static float[] GetSongNotesForHero2()
    {
        return new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
    }

    /// <summary>
    /// Kahraman 3 için 6 notalı özel şarkı dizisi.
    /// </summary>
    private static float[] GetSongNotesForHero3()
    {
        return new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
    }

    /// <summary>
    /// Kahraman 4 için 6 notalı özel şarkı dizisi.
    /// </summary>
    private static float[] GetSongNotesForHero4()
    {
        return new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
    }

    /// <summary>
    /// Tanımlanmamış kahramanlar için kullanılacak varsayılan şarkı notaları.
    /// </summary>
    private static float[] GetDefaultSongNotes()
    {
        return new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
    }
}

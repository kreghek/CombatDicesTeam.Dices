namespace CombatDicesTeam.Dices;

/// <summary>
/// Auxiliary extensions of the service for working with dice.
/// </summary>
public static class DiceExtensions
{
    /// <summary>
    /// Getting a random number in the specified range [min, max].
    /// </summary>
    /// <param name="dice"> The dice used for a roll. </param>
    /// <param name="min"> Minimum value of the dice. </param>
    /// <param name="max"> Maximum value of the dice. </param>
    /// <returns> Returns a random number in the specified range [min, max]. </returns>
    public static int Roll(this IDice dice, int min, int max)
    {
        if (min > max)
        {
            throw new ArgumentException($"Max value {max} can't be least min {min}.",
                nameof(max));
        }

        if (dice == null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (min == max)
        {
            return min;
        }

        var range = max - min;
        var roll = dice.Roll(range + 1);

        return roll - 1 + min;
    }

    public static int Roll2D6(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(6) + dice.Roll(6);
    }

    /// <summary>
    /// Rolls a random index from a set.
    /// </summary>
    /// <typeparam name="T"> The type of the set's elements. </typeparam>
    /// <param name="dice"> The dice on the basis of which to make a random choice. </param>
    /// <param name="list"> The list of elements from which to select an element. </param>
    /// <returns> Random index from the list. </returns>
    public static int RollArrayIndex<T>(this IDice dice, IList<T> list)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        var rollIndex = dice.Roll(0, list.Count - 1);
        return rollIndex;
    }

    public static int RollD100(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(100);
    }

    public static int RollD3(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(3);
    }

    public static int RollD6(this IDice dice)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        return dice.Roll(6);
    }

    /// <summary>
    /// Rolls a random item from a set.
    /// </summary>
    /// <typeparam name="T"> The type of the set's elements. </typeparam>
    /// <param name="dice"> The dice on the basis of which to make a random choice. </param>
    /// <param name="list"> The list of elements from which to select an element. </param>
    /// <returns> Random item from the list. </returns>
    public static T RollFromList<T>(this IDice dice, IReadOnlyList<T> list)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count == 1)
        {
            return list[0];
        }

        var rollIndex = dice.Roll(0, list.Count - 1);
        var item = list[rollIndex];
        return item;
    }

    /// <summary>
    /// Rolls multiple random items from a set.
    /// </summary>
    /// <typeparam name="T"> The type of the set's elements. </typeparam>
    /// <param name="dice"> The dice on the basis of which to make a random choice. </param>
    /// <param name="list"> The list of elements from which to select an element. </param>
    /// <param name="count"> Count of the elements to select. </param>
    /// <returns> Set of random items from the list. </returns>
    public static IEnumerable<T> RollFromList<T>(this IDice dice, IList<T> list, int count)
    {
        if (dice is null)
        {
            throw new ArgumentNullException(nameof(dice));
        }

        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (list.Count < count)
        {
            throw new ArgumentException(
                $"The requested count {count} must be bigger or equal that list length {list.Count}.",
                nameof(count));
        }

        var openList = new List<T>(list);

        for (var i = 0; i < count; i++)
        {
            var rolledItem = dice.RollFromList(openList);

            yield return rolledItem;

            openList.Remove(rolledItem);
        }
    }

    public static IReadOnlyList<T> ShuffleList<T>(this IDice dice, IReadOnlyList<T> source)
    {
        var resultList = new List<T>();
        var openList = new List<T>(source);

        for (var i = 0; i < source.Count; i++)
        {
            var rolledItem = dice.RollFromList(openList);

            resultList.Add(rolledItem);
            openList.Remove(rolledItem);
        }

        return resultList;
    }
}
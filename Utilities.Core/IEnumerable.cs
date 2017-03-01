﻿/* See the LICENSE.txt file in the root folder for license details. */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace Utilities.Core
{
  public static class IEnumerableUtils
  {
    /* .Net 2.0 introduced a List<T>.ForEach method, but there is no comparable
        IEnumerable<T>.ForEach.  Lots of other types implement IEnumerable,
        so this method comes in handy in places besides List<T>. */
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
      sequence.Name("sequence").NotNull();
      action.Name("action").NotNull();

      foreach (T item in sequence)
        action(item);
    }

    public static void ForEachI<T>(this IEnumerable<T> sequence, Action<T, Int32> action)
    {
      sequence.Name("sequence").NotNull();
      action.Name("action").NotNull();

      var i = 0;
      foreach (T item in sequence)
        action(item, i++);
    }

    public static Boolean ContainsCI(this IEnumerable<String> values, String searchValue)
    {
      values.Name("values").NotNull();
      searchValue.Name("searchValue").NotNull().NotEmpty();

      return values.Any(s => s.EqualsCI(searchValue));
    }

    public static String Join(this IEnumerable<Char> values)
    {
      return String.Join("", values);
    }

    public static String Join(this IEnumerable<String> values, String separator)
    {
      values.Name("values").NotNull();
      separator.Name("separator").NotNull();

      return String.Join(separator, values);
    }

    public static String Join(this IEnumerable<String> values)
    {
      values.Name("values").NotNull();

      return values.Join(Environment.NewLine);
    }

    public static String JoinAndIndent(this IEnumerable<String> values, String separator, Int32 indent)
    {
      values.Name("values").NotNull();
      indent.Name("indent").GreaterThan(0);

      var indentString = " ".Repeat(indent);
      return values.Select(v => v.Indent(indent)).Join(separator);
    }

    public static String JoinAndIndent(this IEnumerable<String> values, Int32 indent)
    {
      return values.JoinAndIndent(Environment.NewLine, indent);
    }

    public static IEnumerable<String> Lines(this TextReader textReader)
    {
      textReader.Name("textreader").NotNull();

      String line = null;
      while ((line = textReader.ReadLine()) != null)
        yield return line;
    }

    /// <summmary>
    /// Return true if an IEnumerable&lt;T&gt; is null or contains no elements.
    /// </summmary>
    public static Boolean IsNullOrEmpty<T>(this IEnumerable<T> items)
    {
      return ((items == null) || !items.Any());
    }

    public static IEnumerable<T> Tail<T>(this IEnumerable<T> items)
    {
      return (items == null) ? null : items.Skip(1).Take(items.Count() - 1);
    }

    // IEnumerable has a Sum method, but not a Product method.
    public static BigInteger Product(this IEnumerable<Int32> ints)
    {
      ints.Name("ints").NotNull();

      return ints.Aggregate((BigInteger) 1, (acc, next) => acc * next);
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> items, Int32 numOfParts)
    {
      var i = 0;
      return items.GroupBy(x => i++ % numOfParts);
    }

    private static readonly Random _random = new Random();

    public static IList<T> Randomize<T>(this IList<T> list)
    {
      for (var currentIndex = list.Count() - 1; currentIndex >= 1; currentIndex--)
      {
        var randomIndex = _random.Next(currentIndex + 1);
        T value = list[randomIndex];
        list[randomIndex] = list[currentIndex];
        list[currentIndex] = value;
      }

      return list;
    }
  }
}

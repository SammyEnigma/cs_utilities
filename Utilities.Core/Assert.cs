﻿/* See UNLICENSE.txt file for license details. */

using System;

namespace Utilities.Core
{
  [Flags]
  public enum StringAssertion
  {
    None = 0x1,
    NotNull = 0x2,
    NotOnlyWhitespace = 0x4,
    NotZeroLength = 0x8,
    All = NotNull | NotOnlyWhitespace | NotZeroLength
  }

  /*

    A small collection of extension methods that reduces the code needed to
    check strings for null-ness, whitespace only and length.

    A common idiom in C# methods is using one or more if/then statements to check parameter(s) for validity.

      public String GetFileContents(String filename)
      {
        if (filename == null)
          throw new ArgumentNullException("filename cannot be null.");
              
        if (filename.Trim().Length == 0)
          throw new ArgumentException("filename cannot be empty.");
              
        // more code here...
      }

    Those if/then statements are ripe for abstraction.
    A more pleasant way to express the above logic might be something like this:

      public String GetFileContents(String filename)
      {
        filename.Check("filename", StringAssertion.NotNull | StringAssertion.NotZeroLength);
              
        // more code here...
      }

    Or even shorter, since checking a string for null-ness and length is so common:

      public String GetFileContents(String filename)
      {
        filename.Check("filename");
              
        // more code here...
      }

    That's what this class provides - extension methods on .Net's string type
    that reduce many of those if/then statements to simple function calls.
  
    One thing to note is these string extension methods will work if the string is null and the null has a string type.
  
      // The following three lines of code are equivalent.
          
      AssertUtils.Check(null, "s");
          
      ((String) null).Check("s");
          
      String s = null; s.Check("s");
          
      // The compiler will emit an error if an untyped null is used.
      null.Check("s");

    
  // Examples ///////////////////////////////////////////////////////////////

    String s = null;
        
    // The default is to apply StringAssertion.All.
    // An ArgumentNullException will be raised because s is null.
    s.Check("s");

    s = "";

    // An ArgumentException will be raised because s is empty.
    s.Check("s");

    s = "   ";

    // An ArgumentException will be raised because s consists only of whitespace.
    s.Check("s");

    // No exception will be raised because, even though
    // s consists only of whitespace, it has a length
    // greater than zero.
    s.Check("s", StringAssertion.NotNull | StringAssertion.NotZeroLength);

    s = "123";

    // An ArgumentException will be raised.
    // Because StringAssertion.None is specified,
    // s will not be checked for null-ness, zero length or if it consists only of whitespace.
    // Instead, s will be checked to see if its length is 5, which fails because
    // s is only 3 characters long.
    s.Check("s", StringAssertion.None, 5);

    // The last two Int32 parameters are the minimum and
    // maximum allowed length (inclusive) of s.
    // Because s is 3 characters long, no exception will be raised.
    s.Check("s", StringAssertion.None, 3, 5);

    // Other types can be checked for null-ness with the CheckForNull<T> method.
    StreamReader sr = null;
    sr.CheckForNull("sr");

  */

  public static class AssertUtils
  {
    private static void InternalCheckString(Int32 stackFrameLevel, String value, String name, StringAssertion stringAssertion, Int32 minimumLength, Int32 maximumLength)
    {
      name.CheckForNull("name");

      if (!stringAssertion.HasFlag(StringAssertion.None))
      {
        if (stringAssertion.HasFlag(StringAssertion.NotNull))
          value.CheckForNull("value");

        if (stringAssertion.HasFlag(StringAssertion.NotOnlyWhitespace))
          if (String.IsNullOrWhiteSpace(value))
            throw new ArgumentException(String.Format(Properties.Resources.Assert_StringNotWhitespace, name), name);

        /* value has to be non-null before its length can be checked. */
        value.CheckForNull("value");
        if (stringAssertion.HasFlag(StringAssertion.NotZeroLength))
          if (value.Length == 0)
            throw new ArgumentException(String.Format(Properties.Resources.Assert_StringNotZeroLength, name), name);
      }

      if (minimumLength > maximumLength)
        throw new ArgumentException(String.Format(Properties.Resources.Assert_StringInconsistentLengthParameters, minimumLength, maximumLength));

      /* All of the following checks require value to be non-null. */
      value.CheckForNull("value");

      if ((minimumLength == maximumLength) && (value.Length != minimumLength))
        throw new ArgumentException(String.Format(Properties.Resources.Assert_StringLengthsNotEqual, name, value.Length, minimumLength), name);

      if (value.Length < minimumLength)
        throw new ArgumentException(String.Format(Properties.Resources.Assert_StringLengthLessThanMinimum, name, value.Length, minimumLength), name);

      if (value.Length > maximumLength)
        throw new ArgumentException(String.Format(Properties.Resources.Assert_StringLengthGreaterThanMaximum, name, value.Length, maximumLength), name);
    }

    public static void Check(this String value, String name)
    {
      InternalCheckString(3, value, name, StringAssertion.All, 0, Int32.MaxValue);
    }

    public static void Check(this String value, String name, StringAssertion stringAssertion)
    {
      InternalCheckString(3, value, name, stringAssertion, 0, Int32.MaxValue);
    }

    public static void Check(this String value, String name, StringAssertion stringAssertion, Int32 length)
    {
      InternalCheckString(3, value, name, stringAssertion, length, length);
    }

    public static void Check(this String value, String name, StringAssertion stringAssertion, Int32 minimumLength, Int32 maximumLength)
    {
      InternalCheckString(3, value, name, stringAssertion, minimumLength, maximumLength);
    }

    public static void CheckForNull<T>(this T value, String name)
    {
      if (name == null)
        throw new ArgumentNullException("name");

      if (value == null)
        throw new ArgumentNullException(name);
    }
  }
}
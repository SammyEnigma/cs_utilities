﻿using System;

namespace Utilities.Sql.SqlServer
{
  /// <summary>
  /// The text generated by some of the <see cref="Utilities.Sql.Column">Column</see> class's properties can include
  /// a comment indicating whether or not that column is a primary or foreign key.
  /// This enumeration is used to turn that capability on or off.
  /// </summary>
  public enum IncludeKeyIdentificationComment { No, Yes }

  /// <summary>
  /// .Net languages can handle an SQL Server column of type 'xml' in three ways:
  /// as a <see cref="System.String">String</see>, as an <see cref="System.Xml.XmlDocument">XmlDocument</see>,
  /// or as an <see cref="System.Xml.Linq.XDocument">XDocument</see>.
  /// </summary>
  public enum XmlSystem
  {
    /// <summary>
    /// Create code to treat the xml column as a <see cref="System.String">String</see>.
    /// </summary>
    AsString,

    /// <summary>
    /// Create code to treat the xml column as an <see cref="System.Xml.XmlDocument">XmlDocument</see>.
    /// </summary>
    NonLinq_XmlDocument,

    /// <summary>
    /// Create code to treat the xml column as an <see cref="System.Xml.Linq.XDocument">XDocument</see>.
    /// </summary>
    Linq_XDocument
  }

  /// <summary>
  /// XML database columns can optionally have associated XSDs stored in the server that the column can be validated against.
  /// If an XSD is assigned to the XML column, SQL Server will automatically do this validation when an INSERT or UPDATE is done.
  /// <para>But it would be nice to be able to perform this validation on the client before sending the XML to the server.</para>
  /// <para>A value from this enumeration is passed to the Configuration object and allows you to
  /// choose if and where you want client-side XML validation code to be generated.</para>
  /// </summary>
  [Flags]
  public enum XmlValidationLocation
  {
    /// <summary>
    /// No XML validation code will be generated.
    /// </summary>
    None = 0,

    /// <summary>
    /// If the XML column has an associated XSD, XML validation code will be inserted into the property setter generated by the Column.GetClassPropertyDeclaration method.
    /// </summary>
    PropertySetter = 1,

    /// <summary>
    /// If the XML column has an associated XSD, XML validation will be performed when the XML value is assigned to an SqlParameter's Value property.
    /// </summary>
    SqlParameterValueAssignment = 2,

    /// <summary>
    /// If the XML column has an associated XSD, XML validation will be performed in both the property setter and when assigning the XML to an SqlParameter's Value property.
    /// </summary>
    Both = PropertySetter | SqlParameterValueAssignment
  }
}
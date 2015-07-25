﻿using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

class Program
{
    static void Main(string[] args) {
        var settings = new XmlReaderSettings {
             ValidationType = ValidationType.Schema,
        };
        settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessInlineSchema | XmlSchemaValidationFlags.ReportValidationWarnings;
        settings.ValidationEventHandler += validating_ValidationEventHandler;

        using (var stream = File.OpenRead(args[0]))
        using (var reader = XmlReader.Create(stream, settings)) {
            while (reader.Read()) {
            }
        }
    }

    private static void validating_ValidationEventHandler(object sender, ValidationEventArgs e) {
        if (e.Severity == XmlSeverityType.Warning) {
            Console.WriteLine("\tWarning: Matching schema not found. No validation occurred. " + e.Message);
        } else {
            Console.WriteLine("\tValidation error: " + e.Message);
        }
    }
}

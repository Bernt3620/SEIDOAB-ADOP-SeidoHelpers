using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Helpers;

namespace SeidoHelpers;

class Program
{
    static void Main(string[] args)
    {
        #region csSeedGenerator Usage Examples
        Console.WriteLine("csSeedGenerator Usage Examples");

        //Create a generator, inherited from .NET Random
        var rnd = new csSeedGenerator();

        Console.WriteLine("Random Names");
        Console.WriteLine($"Firstname: {rnd.FirstName}");
        Console.WriteLine($"Lastname: {rnd.LastName}");
        Console.WriteLine($"Fullname: {rnd.FullName}");
        Console.WriteLine($"Petname: {rnd.PetName}");

        Console.WriteLine("\nRandom Address");
        var _country = rnd.Country;
        Console.WriteLine($"Streetname: {rnd.StreetAddress(_country)}");
        Console.WriteLine($"City: {rnd.City(_country)}");
        Console.WriteLine($"Zip code: {rnd.ZipCode}");
        Console.WriteLine($"Country: {_country}");

        Console.WriteLine("\nRandom Email and Phone number");
        Console.WriteLine($"Email: {rnd.Email()}");
        Console.WriteLine($"Email for specific name: {rnd.Email("John", "Smith")}");
        Console.WriteLine($"Phone number: {rnd.PhoneNr}");

        Console.WriteLine("\nRandom Quote");
        var _quote = rnd.Quote;
        Console.WriteLine($"Famous Quote: {_quote.Quote}");
        Console.WriteLine($"Author: {_quote.Author}");

        Console.WriteLine("\nBogus Random Latin");
        Console.WriteLine($"Latin paragraph:\n{rnd.LatinParagraph}");
        Console.WriteLine($"\nLatin sentence:\n{rnd.LatinSentence}");
        Console.WriteLine($"\n3 Latin sentences:\n{string.Join(" ", rnd.LatinSentences(3))}");
        Console.WriteLine($"\n10 Latin words:\n{string.Join(", ", rnd.LatinWords(10))}");

        Console.WriteLine("\nRandom Music group and album names");
        Console.WriteLine($"Music group name: {rnd.MusicBandName}");
        Console.WriteLine($"Music album name: {rnd.MusicAlbumName}");

        Console.WriteLine("\nDateAndTime and Bool");
        Console.WriteLine($"This Year: {rnd.DateAndTime()}");
        Console.WriteLine($"Between Years: {rnd.DateAndTime(2000, 2020)}");
        Console.WriteLine($"True or False: {rnd.Bool}");

        Console.WriteLine("\nFrom String, Enum and List");

        Console.WriteLine($"From String: {rnd.FromString("Quick brown fox", " ")}");
        Console.WriteLine($"From Enum {nameof(enGreetings)}: {rnd.FromEnum<enGreetings>()}");

        var f = "Cloudy, Stormy, Rainy, Sunny, Windy";
        List<csWeather> _forecast = new List<csWeather>
        {
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)},
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)},
            new csWeather{ Temp = rnd.NextDecimal(100, 300), Visibility = rnd.FromString(f)}
        };
        Console.WriteLine($"From List {nameof(csWeather)} : {rnd.FromList(_forecast)}");

        Console.WriteLine("\nGenerating a randomly seeded list");
        var _persons = rnd.ToList<csPerson>(10);
        foreach (var item in _persons)
        {
            Console.WriteLine(item);
        }


        Console.WriteLine("\nGenerating a list of unique, randomly seeded, items");
        int _tryNrItems = 1000;
        var _pets = rnd.UniqueItemToList<csPet>(_tryNrItems);
        Console.WriteLine($"Try to generate {_tryNrItems} unique {nameof(csPet)}");
        Console.WriteLine($"{_pets.Count} unique {nameof(csPet)} could be created");
        foreach (var item in _pets)
        {
            Console.WriteLine(item);
        }


        Console.WriteLine("\nPicking unique items from a List");

        var _picklist = "Morning, Evening, Morning, Afternoon, Afternoon".Split(", ");
        var _uniquePicks = rnd.UniqueItemPickFromList<string>(_tryNrItems, _picklist.ToList());
        Console.WriteLine($"Try to pick {_tryNrItems} unique items from {nameof(_picklist)}");
        Console.WriteLine($"{_uniquePicks.Count} unique items could be picked");
        foreach (var item in _uniquePicks)
        {
            Console.WriteLine(item);
        }


        var _AnotherPicklist = rnd.ToList<csPet>(10000);
        var _AnotherUniquePicks = rnd.UniqueItemPickFromList<csPet>(_tryNrItems, _AnotherPicklist);
        Console.WriteLine($"\nTry to pick {_tryNrItems} unique items from {nameof(_AnotherPicklist)}");
        Console.WriteLine($"{_AnotherUniquePicks.Count} unique items could be picked");
        foreach (var item in _AnotherUniquePicks)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine("\nWrite seeds to master-seeds.json file");
        rnd.WriteMasterStream();


        Console.WriteLine("\nRead seeds from master-seeds.json file");
        try
        {
            var rndMySeeds = new csSeedGenerator("master-seeds.json");

            Console.WriteLine("Random Names using master-seeds.json file");
            Console.WriteLine($"Firstname: {rndMySeeds.FirstName}");
            Console.WriteLine($"Lastname: {rndMySeeds.LastName}");
            Console.WriteLine($"Fullname: {rndMySeeds.FullName}");
            Console.WriteLine($"Petname: {rndMySeeds.PetName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Could not read seeds from master-seed.json file");
            Console.WriteLine($"Error {ex.GetType()} {ex.Message}");
        }



        Console.WriteLine("\nRead page 0 of quotes with pagesize 5");
        var quotes = rnd.AllQuotes;  
        var NrOfPages = (int) Math.Ceiling(quotes.Count/5.0);

        foreach (var quote in ReadQuotePages(quotes, 0, 5))
        {
            System.Console.WriteLine(quote.Quote);
        }

        Console.WriteLine($"\nRead page {NrOfPages-1} (final) of quotes with pagesize 5");
        foreach (var quote in ReadQuotePages(quotes, NrOfPages-1, 5))
        {
            System.Console.WriteLine(quote.Quote);
        }

        #endregion


        #region csConsoleInput Usage Example
        /*
        bool _continue = true;
        do
        {
            Console.WriteLine("\n\ncsConsoleInput Usage Example");

            int _intanswer;
            if (!csConsoleInput.TryReadInt32("Enter an integer", -1, 101, out _intanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_intanswer}");

            string _stringanswer = null;
            if (_continue &&
                !csConsoleInput.TryReadString("Enter a string", out _stringanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_stringanswer}");
            
            DateTime _dtanswer = default;
            if (_continue &&
                !csConsoleInput.TryReadDateTime("Enter a date and time", out _dtanswer))
            {
                _continue = false;
                break;
            }
            Console.WriteLine($"You entered {_dtanswer}");
            
        } while (_continue);
        */
        #endregion
        
        Console.WriteLine("\n\nSeidoHelpers Quit");
    }


    //Pagination example
    public static List<csSeededQuote> ReadQuotePages (List<csSeededQuote> quotes, int pageNumber, int pageSize)
        => quotes.Skip(pageNumber * pageSize)
            .Take(pageSize)
            .ToList();

}

public enum enGreetings { Hello, Goodbye, GoodMorning, GoodEvening }
public class csWeather
{
    public decimal Temp { get; set; }
    public string Visibility { get; set; }
    public override string ToString() => $"{Visibility} {Temp} degC";
}

public class csPerson : ISeed<csPerson>
{
    public string FullName { get; set; }
    public DateTime Birthday { get; set; }
    public override string ToString() => $"{FullName} is born on {Birthday:d}";

    #region ISeed implementation to use csSeeGenerator to create random lists
    [JsonIgnore]
    public bool Seeded { get; set; } = false;

    public csPerson Seed(csSeedGenerator rnd)
    {
        FullName = rnd.FullName;
        Birthday = rnd.DateAndTime(1970, 2010);
        return this;
    }
    #endregion
}

public class csPet : ISeed<csPet>, IEquatable<csPet>
{
    public string PetName { get; set; }
    public override string ToString() => $"{PetName}";

    #region ISeed implementation to use csSeeGenerator to create random lists
    [JsonIgnore]
    public bool Seeded { get; set; } = false;

    public csPet Seed(csSeedGenerator rnd)
    {
        PetName = rnd.PetName;
        return this;
    }
    #endregion

    #region implementing IEquatable
    public bool Equals(csPet other) => (other != null) ? (PetName) == (other.PetName) : false;

    public override bool Equals(object obj) => Equals(obj as csPet);
    public override int GetHashCode() => (PetName).GetHashCode();
    #endregion

}




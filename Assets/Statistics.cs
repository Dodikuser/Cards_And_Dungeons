using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class Statistics
{
    public DateTime startData;
    public DateTime stopData;

    public Statistics()
    {
        if (File.Exists("statistics.json")) LoadStatistic();
        else
        {
            MyStatistics = new Dictionary<string, int> {
                {"CountOfRuns", 0 },
                {"CountOfKills", 0 },
                {"CountOfPickUp", 0 },
                {"MaxDungeon", 0 },
                {"TimeSpent", 0 },
                };
        }

    }
    public Dictionary<string, int> MyStatistics { get; set; }

    public void SaveStatistic()
    {
        string json = JsonConvert.SerializeObject(MyStatistics, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText("statistics.json", json);
    }
    public void LoadStatistic()
    {
        if (File.Exists("statistics.json"))
        {
            try
            {
                string jsonContent = File.ReadAllText("statistics.json");
                var dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonContent);
                MyStatistics = dictionary;

            }
            catch (JsonReaderException e)
            {
                Console.WriteLine("Произошла ошибка при разборе JSON: " + e.Message);
            }
        }
    }
    public void TimeCounter()
    {
        stopData = DateTime.Now;

        TimeSpan currentTime = stopData - startData;
        MyStatistics["TimeSpent"] += (int)currentTime.TotalMinutes;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

using UnityEngine.Networking;

using CI.HttpClient;

using ChartAndGraph;

public class RetrieveDataForBarGraph : MonoBehaviour {
  private BarChart barChart;
  public Material[] catMats;
  private HttpClient httpClient;

  public string SensorGroup = "";
  public string BarCommand = "";

  public class XYZSensorData {
    public int timestamp, x, y, z;
  }

  public class MiscSensorData {
    public int timestamp;
    public float temperature, humidity, pressure;
  }

  public class AnomalyAnalysis {
    public float temperature, humidity, pressure, anomaly_score;
  }

  public class StatsAnalysis {
    public int count;
    public float value, average, maximum, minimum, sample_standard_dev;
  }

  // Use this for initialization
  void Start() {
    barChart = GetComponent<BarChart>();
    httpClient = new HttpClient();
    InvokeRepeating("GetSensorData", 2.0f, 2.0f);
  }

  void GetSensorData() {
    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
      return true;
    };

    // valid commands: "realtime:$data-group" and "analytics:null"
    String[] commandParts = BarCommand.Split(new String[] { ":" }, StringSplitOptions.None);
    String mainCmd = commandParts[0];
    String parameter = commandParts[1];

    if (mainCmd.Equals("realtime")) {
      httpClient.GetString(
        new Uri("https://bigdot.herokuapp.com/graph-data/bar/groups/" + parameter),
        delegate (CI.HttpClient.HttpResponseMessage<string> res) {
          if (barChart != null) {
            barChart.DataSource.ClearCategories();
            barChart.DataSource.ClearGroups();

            barChart.DataSource.AddGroup("BarGraphGroup");
            if (parameter != "miscellaneous") {
              barChart.DataSource.AddCategory("x", catMats[0]);
              barChart.DataSource.AddCategory("y", catMats[1]);
              barChart.DataSource.AddCategory("z", catMats[2]);
              barChart.DataSource.AddCategory("w", catMats[0]);
            } else {
              barChart.DataSource.AddCategory("temperature", catMats[0]);
              barChart.DataSource.AddCategory("humidity", catMats[1]);
              barChart.DataSource.AddCategory("pressure", catMats[2]);
            }
          }

          switch (parameter) {
            case "accelerometer":
            case "gyroscope":
            case "magnetometer":
              XYZSensorData XYZData = JsonConvert.DeserializeObject<XYZSensorData>(res.Data);

              if (barChart != null) {
                barChart.DataSource.SetValue("x", "BarGraphGroup", XYZData.x);
                barChart.DataSource.SetValue("y", "BarGraphGroup", XYZData.y);
                barChart.DataSource.SetValue("z", "BarGraphGroup", XYZData.z);
                barChart.DataSource.SetValue("w", "BarGraphGroup", 1);
              }
              break;
            case "miscellaneous":
              MiscSensorData miscData = JsonConvert.DeserializeObject<MiscSensorData>(res.Data);

              if (barChart != null) {
                //kPa,%,C
                barChart.DataSource.SetValue("temperature", "BarGraphGroup", miscData.temperature);
                barChart.DataSource.SetValue("humidity",    "BarGraphGroup", miscData.humidity);
                barChart.DataSource.SetValue("pressure",    "BarGraphGroup", miscData.pressure / 10);
              }
              break;
          }
        }
      );
    } else {
      httpClient.GetString(
        new Uri("https://bigdot.herokuapp.com/graph-data/bar/analysis"),
        delegate (CI.HttpClient.HttpResponseMessage<string> res) {
          if (barChart != null) {
            barChart.DataSource.ClearCategories();
            barChart.DataSource.ClearGroups();

            barChart.DataSource.AddGroup("BarGraphGroup");
            if (res.Data.Contains("anomaly_score")) {
              AnomalyAnalysis analysis = JsonConvert.DeserializeObject<AnomalyAnalysis>(res.Data);

              barChart.DataSource.AddCategory("temperature", catMats[0]);
              barChart.DataSource.AddCategory("humidity", catMats[1]);
              barChart.DataSource.AddCategory("pressure", catMats[2]);
              barChart.DataSource.AddCategory("anomaly score", catMats[0]);

              barChart.DataSource.SetValue("temperature", "BarGraphGroup", analysis.temperature);
              barChart.DataSource.SetValue("humidity", "BarGraphGroup", analysis.humidity);
              barChart.DataSource.SetValue("pressure", "BarGraphGroup", analysis.pressure / 10);
              barChart.DataSource.SetValue("anomaly score", "BarGraphGroup", analysis.pressure / 10);
            } else {
              StatsAnalysis analysis = JsonConvert.DeserializeObject<StatsAnalysis>(res.Data);

              barChart.DataSource.AddCategory("value", catMats[0]);
              barChart.DataSource.AddCategory("count", catMats[1]);
              barChart.DataSource.AddCategory("minimum", catMats[2]);
              barChart.DataSource.AddCategory("maximum", catMats[0]);
              barChart.DataSource.AddCategory("average", catMats[1]);
              barChart.DataSource.AddCategory("sample_standard_dev", catMats[2]);

              barChart.DataSource.SetValue("value", "BarGraphGroup", analysis.value);
              barChart.DataSource.SetValue("count", "BarGraphGroup", analysis.count);
              barChart.DataSource.SetValue("minimum", "BarGraphGroup", analysis.minimum);
              barChart.DataSource.SetValue("maximum", "BarGraphGroup", analysis.maximum);
              barChart.DataSource.SetValue("average", "BarGraphGroup", analysis.average);
              barChart.DataSource.SetValue("sample_standard_dev", "BarGraphGroup", analysis.sample_standard_dev);
            }
          }
        }
      );
    }

    if (barChart != null) {
      barChart.DataSource.AutomaticMaxValue = true;
    }
  }
}

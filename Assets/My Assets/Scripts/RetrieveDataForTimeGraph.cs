using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

using UnityEngine.Networking;

using CI.HttpClient;

using ChartAndGraph;

public class RetrieveDataForTimeGraph : MonoBehaviour {
  private GraphChartBase timeGraph;
  private HttpClient httpClient;

  public string SensorType = "humidity";
  public string TimeFromNow = "1:00:00";

  // Use this for initialization
  void Start () {
    timeGraph = GetComponent<GraphChartBase>();
    httpClient = new HttpClient();
  }

  public class IntPoints {
    public int[] points;
  }

  public class FloatPoints {
    public float[] points;
  }

  public DateTime FromUnixTime(long unixTime) {
    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    return epoch.AddSeconds(unixTime);
  }

  public void GetSensorData() {
    if (SensorType == "" || TimeFromNow == "")
      return;

    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
      return true;
    };

    timeGraph = GetComponent<GraphChartBase>();
    httpClient = new HttpClient();

    httpClient.GetString(
      new Uri("https://bigdot.herokuapp.com/graph-data/time/" + SensorType + "/" + TimeFromNow),
      delegate (CI.HttpClient.HttpResponseMessage<string> res) {
        string category_1 = "Default1";
        string category_2 = "Default2";
        string category_3 = "Default3";

        timeGraph.DataSource.StartBatch();
        timeGraph.DataSource.ClearCategory(category_1);
        timeGraph.DataSource.ClearCategory(category_2);
        timeGraph.DataSource.ClearCategory(category_3);

        string[] miscSensorsArray = { "humidity", "temperature", "pressure" };

        if (miscSensorsArray.Contains(SensorType)) {
          String[] rawData = res.Data.Replace("[", "").Replace("]", "").Split(new String[] { "," }, StringSplitOptions.None);
          int limit = 50;
          if (rawData.Length <= limit) {
            for (int index = 0; index < rawData.Length; index++) {
              timeGraph.DataSource.AddPointToCategory(category_1, index, float.Parse(rawData[index]));
            }
          } else {
            float[] rawValues = new float[limit];
            for (int index = 0; index < rawData.Length; index++) {
              int normalIndex = (int) Math.Floor((float) index / (float) rawData.Length * ((float) limit - 1f));
              rawValues[normalIndex] = float.Parse(rawData[index]);
            }

            for (int index = 0; index < rawValues.Length; index++) {
              if (rawValues[index] != 0)
                timeGraph.DataSource.AddPointToCategory(category_1, index, rawValues[index]);
            }
          }
        } else {
          String[] rawData = res.Data.Replace("[", "").Replace("]", "").Split(new String[] { "," }, StringSplitOptions.None);
          int limit = 50;
          if (rawData.Length <= limit) {
            for (int index = 0; index < rawData.Length; index++) {
              timeGraph.DataSource.AddPointToCategory(category_1, index, float.Parse(rawData[index]));
              timeGraph.DataSource.AddPointToCategory(category_2, index, float.Parse(rawData[index]));
              timeGraph.DataSource.AddPointToCategory(category_3, index, float.Parse(rawData[index]));
            }
          } else {
            int[] rawValuesX = new int[limit];
            int[] rawValuesY = new int[limit];
            int[] rawValuesZ = new int[limit];
            for (int index = 0; index < rawData.Length; index += 3) {
              int normalIndex = (int) Math.Floor((float) index / (float) (rawData.Length / 3f) * ((float) limit / 3f - 1));
              rawValuesX[normalIndex] = int.Parse(rawData[index]);
              rawValuesY[normalIndex] = int.Parse(rawData[index + 1]);
              rawValuesZ[normalIndex] = int.Parse(rawData[index + 2]);
            }

            for (int index = 0; index < limit; index++) {
              if (rawValuesX[index] != 0 && rawValuesY[index] != 0 && rawValuesZ[index] != 0) {
                timeGraph.DataSource.AddPointToCategory(category_1, index, rawValuesX[index]);
                timeGraph.DataSource.AddPointToCategory(category_2, index, rawValuesY[index]);
                timeGraph.DataSource.AddPointToCategory(category_3, index, rawValuesZ[index]);
              }
            }
          }
        }

        timeGraph.DataSource.EndBatch();
        //barChart.DataSource.AutomaticMaxValue = true;
      }
    );
  }
}

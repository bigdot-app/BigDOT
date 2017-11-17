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


/*
IoT(null)=>DynamoDB(dogs and cats)
IoT(null)=>KinesisFirehose(cats and dogs input)
IoT,null,KinesisFirehose,cats and dogs input

KinesisFirehose('cats and dogs input')=>S3('cats and dogs input')
KinesisFirehose named 'cats and dogs input' to S3 named 'cats and dogs input'
Kinesis Firehose named 'cats and dogs input' to Kinesis Analytics named 'cats and dogs app'
Kinesis Analytics named 'cats and dogs app' to Kinesis Firehose named 'cats and dogs output'
Kinesis Firehose named 'cats and dogs output' to S3 named 'cats and dogs output'
Kinesis Firehose named 'cats and dogs output' to Lamdba named 'cats and dogs output processor'
Lamdba named 'cats and dogs output processor' to DynamoDB named 'cats and dogs'
*/

public class DataFlowGenerator : MonoBehaviour {
  public GameObject iot2Database, iot2Kinesis, kinesis2Analytics;
  public TextMesh iotLabel, s3Label, dynamoLabel, streamLabel, analyticsLabel;
  private HttpClient httpClient;

  public class DataflowManifest {
    public bool iot2db;
    public bool iot2stream;
    public bool stream2analytics;
    public string iotThingName;
    public string databaseName;
    public string kinesisStreamName;
    public string kinesisAnalyticsAppName;
  }

  // Use this for initialization
  void Start () {
    httpClient = new HttpClient();
    InvokeRepeating("UpdateDataFlow", 2.0f, 2.0f);
  }

  public void UpdateDataFlow() {
    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
      return true;
    };

    httpClient.GetString(
      new Uri("https://bigdot.herokuapp.com/data-flow"),
      delegate (CI.HttpClient.HttpResponseMessage<string> res) {
        DataflowManifest manifest = JsonConvert.DeserializeObject<DataflowManifest>(res.Data);

        // update object visibility
        iot2Database.SetActive(manifest.iot2db);
        iot2Kinesis.SetActive(manifest.iot2stream);
        kinesis2Analytics.SetActive(manifest.stream2analytics);

        // update object names
        iotLabel.text = manifest.iotThingName;
        s3Label.text = manifest.databaseName;
        dynamoLabel.text = manifest.databaseName;
        streamLabel.text = manifest.kinesisStreamName;
        analyticsLabel.text = manifest.kinesisAnalyticsAppName;
      }
    );
  }
}

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

// AFTER NUMEROUS TRIES AND HOURS OF RESEARCH WE HAVE GOT TO THE POINT WHERE WE GAINED THE FOLLOWING CONCLUSIONS:
// - The main purpose of Unity Engine is game development for multiple platforms. It is indeed a powerful tool but it is not meant for visualizing specific datasets (especially the COVID-19 total cases dataset with dates as time points).
// - It is more common and more effortless to visualize data of this type in specific programming environments (like Python, R or even MS Excel). That mentioned, we have encountered huge problems while trying to find any aid to our issue.
// - We have tried searching internet sites, Unity tutorials and even YouTube tutorials, but found nothing that could make us resolve the given task.
// - The sources were related to creating 2D shader graphs, plotting the usage of computer resources and performing impressive simulations for video games.
// - The only thing we could make out with these information was an unprofessional 3D plot. To keep our examination and project professional we have decided to continue analysis in more fittable and useful environment (in our case - Python).

//------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class CSVPlotter : MonoBehaviour
{
    public string csvPath1 = "path1 here";
    public string csvPath2 = "path2 here";
    public char fieldDelimiter = ',';
    public float plotRange = 100;

    private void Start()
    {
        // Read the CSV files
        string[] lines1 = File.ReadAllLines(csvPath1);
        string[] lines2 = File.ReadAllLines(csvPath2);
        List<float> xAxisValues = new List<float>();
        List<float> yAxisValues = new List<float>();
        List<float> zAxisValues = new List<float>();

        for (int i = 1; i < lines1.Length; ++i)
        {
            string line1 = lines1[i];
            string line2 = lines2[i];
            string[] values1 = line1.Split(fieldDelimiter);
            string[] values2 = line2.Split(fieldDelimiter);

            xAxisValues.Add(float.Parse(values1[0].Substring(0, 1)));
            yAxisValues.Add(float.Parse(values1[1].Replace('.', ',')));
            zAxisValues.Add(float.Parse(values2[1].Replace('.', ',')));
        }

        xAxisValues = NormalizeValues(xAxisValues, plotRange);
        yAxisValues = NormalizeValues(yAxisValues, plotRange);
        zAxisValues = NormalizeValues(zAxisValues, plotRange);

        // Create axes
        CreateAxis(Vector3.zero, Vector3.right * plotRange, Color.red, "X"); // X-axis
        CreateAxis(Vector3.zero, Vector3.up * plotRange, Color.green, "Y"); // Y-axis
        CreateAxis(Vector3.zero, Vector3.forward * plotRange, Color.blue, "Z"); // Z-axis

        for (int i = 0; i < xAxisValues.Count; ++i)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.position = new Vector3(xAxisValues[i], yAxisValues[i], zAxisValues[i]);

            // Set the scale of the sphere to make it bigger
            float scaleFactor = 10.0f;
            sphere.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }

    void CreateAxis(Vector3 start, Vector3 end, Color color, string axisName)
    {
        GameObject axis = new GameObject("Axis");
        LineRenderer lineRenderer = axis.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Standard"));
        lineRenderer.material.color = color;
        lineRenderer.startWidth = 10; // Adjust this value for axes thickness
        lineRenderer.endWidth = 10; // Adjust this value for axes thickness
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // Create axis name
        GameObject axisNameObject = new GameObject("AxisName");
        axisNameObject.transform.position = end;
        axisNameObject.AddComponent<TextMesh>().text = axisName;
        axisNameObject.GetComponent<TextMesh>().anchor = TextAnchor.MiddleCenter;
    }

    static List<float> NormalizeValues(List<float> inputValues, float range = 10)
    {
        float maxValue = inputValues.Max();
        float minValue = inputValues.Min();
        maxValue = maxValue - minValue;

        for (int i = 0; i < inputValues.Count(); ++i)
        {
            inputValues[i] -= minValue;
            inputValues[i] /= maxValue;
            inputValues[i] *= range;
        }
        return inputValues;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
